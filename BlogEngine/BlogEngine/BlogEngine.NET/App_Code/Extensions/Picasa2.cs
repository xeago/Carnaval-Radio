using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using Google.GData.Photos;
using BlogEngine.Core.Web.Extensions;

namespace App_Code.Extensions
{
    public class PicasaAlbum
    {
        public AlbumAccessor Accessor { get; set; }
        public string ThumbNailURl { get; set; }
        public string AlbumUri{ get; set; }
        public List<PicasaPhoto> AlbumContent 
        {
            get { return Picasa2.GetAlbumContent(Accessor.Name); }
        }
    }

    public class PicasaPhoto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageSrc { get; set; }
    }

    /// <summary>
    /// Picasa extension for BlogEngine 2.0
    /// </summary>
    [Extension("Picasa2 Web Albums and SlideShow", "2.0", "<a href=\"http://rtur.net\">Rtur.net</a>")]
    public class Picasa2
    {
        #region Private members
        static protected ExtensionSettings Settings;
        static protected ExtensionSettings Albums;

        const string Tag = "<embed type=\"application/x-shockwave-flash\" src=\"http://picasaweb.google.com/s/c/bin/slideshow.swf\" width=\"{4}\" height=\"{5}\" flashvars=\"host=picasaweb.google.com{3}&RGB=0x000000&feed=http%3A%2F%2Fpicasaweb.google.com%2Fdata%2Ffeed%2Fapi%2Fuser%2F{2}%2Falbumid%2F{0}%3Fkind%3Dphoto%26alt%3Drss%26{1}\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\"></embed>";
        const string Img = "<li><a rel=\"prettyPhoto\" href='{1}'><img src='{0}'></a></li>";
        #endregion

        public Picasa2()
        {
            Post.Serving += PostServing;
            BlogEngine.Core.Page.Serving += PageServing;

            InitSettings();
        }

        static void PageServing(object sender, ServingEventArgs e)
        {
            e.Body = GetBody(e.Body);
        }

        private static void PostServing(object sender, ServingEventArgs e)
        {
            if (e.Location == ServingLocation.PostList
                || e.Location == ServingLocation.SinglePost)
            {
                e.Body = GetBody(e.Body);
            }
        }

        private static string GetBody(string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                const string regShow = @"\[PicasaShow:.*?\]";
                const string regAlbm = @"\[PicasaAlbum:.*?\]";
                string picasa;

                MatchCollection shows = Regex.Matches(body, regShow);
                MatchCollection albums = Regex.Matches(body, regAlbm);

                if (shows.Count > 0)
                {
                    foreach (Match match in shows)
                    {
                        if (match.Length > 11)
                        {
                            string id = match.Value.Substring(12).Replace("]", "");
                            picasa = "<div id=\"PicasaShow\">";
                            picasa += GetSlideShow(id);
                            picasa += "</div>";
                            body = body.Replace(match.Value, picasa);
                        }
                    }
                }

                if (albums.Count > 0)
                {
                    foreach (Match album in albums)
                    {
                        if (album.Length > 12)
                        {
                            string id = album.Value.Substring(13).Replace("]", "");
                            picasa = "<div id=\"PicasaAlbum\">";
                            picasa += GetAlbum(id);
                            picasa += "</div>";
                            body = body.Replace(album.Value, picasa);
                        }
                    }
                }
            }
            return body;
        }

        private static string GetSlideShow(string albumId)
        {
            string s;
            try
            {
                var service = new PicasaService("exampleCo-exampleApp-1");
                string usr = Settings.GetSingleValue("Account") + "@gmail.com";
                string pwd = Settings.GetSingleValue("Password");
                service.setUserCredentials(usr, pwd);

                var query = new AlbumQuery(PicasaQuery.CreatePicasaUri(usr));
                PicasaFeed feed = service.Query(query);

                string id = "";
                string key = "";

                foreach (PicasaEntry entry in feed.Entries)
                {
                    var ac = new AlbumAccessor(entry);

                    if (ac.Name == albumId)
                    {
                        id = ac.Id;
                        string feedUri = entry.FeedUri;
                        if (feedUri.Contains("authkey="))
                        {
                            string authKey = feedUri.Substring(feedUri.IndexOf("authkey=")).Substring(8);
                            key = authKey;
                        }
                    }
                }

                if (key.Length > 0) key = "authkey%3D" + key;

                string user = Settings.GetSingleValue("Account");

                string auto = "";
                if (bool.Parse(Settings.GetSingleValue("AutoPlay")) == false)
                {
                    auto = "&noautoplay=1";
                }

                string width = Settings.GetSingleValue("ShowWidth");
                string height = "96";

                if (int.Parse(width) > 0)
                {
                    height = (int.Parse(width) * 0.74).ToString();
                }
                s = string.Format(Tag, id, key, user, auto, width, height);
            }
            catch (Exception exp)
            {
                s = exp.Message;
            }
            return s;
        }

        public static string GetAlbum(string album)
        {
            string retVal;
            try
            {
                var service = new PicasaService("exampleCo-exampleApp-1");

                string usr = Settings.GetSingleValue("Account") + "@gmail.com";
                string pwd = Settings.GetSingleValue("Password");

                service.setUserCredentials(usr, pwd);

                var query = new PhotoQuery(PicasaQuery.CreatePicasaUri(usr, album));
                PicasaFeed feed = service.Query(query);

                retVal = "<ul id=\"AlbumList\">";
                foreach (PicasaEntry entry in feed.Entries)
                {
                    var firstThumbUrl = entry.Media.Thumbnails[0].Attributes["url"] as string;
                    string thumGrp = "/s" + Settings.GetSingleValue("PicWidth") + "/";

                    if (firstThumbUrl != null) firstThumbUrl = firstThumbUrl.Replace("/s72/", thumGrp);

                    var contentUrl = entry.Media.Content.Attributes["url"] as string;

                    if (contentUrl != null) contentUrl = contentUrl.Substring(0, contentUrl.LastIndexOf("/"));
                    
                    contentUrl += "/s640/" + entry.Title.Text;

                    retVal += string.Format(Img, firstThumbUrl, contentUrl);
                }
                retVal += "</ul>";
            }
            catch (Exception qex)
            {
                retVal = qex.Message;
            }
            return retVal;
        }

        public static List<PicasaPhoto> GetAlbumContent(string album)
        {
            List<PicasaPhoto> retVal = new List<PicasaPhoto>();
            try
            {
                var service = new PicasaService("exampleCo-exampleApp-1");

                string usr = Settings.GetSingleValue("Account") + "@gmail.com";
                string pwd = Settings.GetSingleValue("Password");

                service.setUserCredentials(usr, pwd);

                var query = new PhotoQuery(PicasaQuery.CreatePicasaUri(usr, album));
                PicasaFeed feed = service.Query(query);

                foreach (PicasaEntry entry in feed.Entries)
                {
                    var firstThumbUrl = entry.Media.Thumbnails[0].Attributes["url"] as string;
                    string thumGrp = "/s" + Settings.GetSingleValue("PicWidth") + "/";

                    if (firstThumbUrl != null) firstThumbUrl = firstThumbUrl.Replace("/s72/", thumGrp);

                    var contentUrl = entry.Media.Content.Attributes["url"] as string;

                    if (contentUrl != null) contentUrl = contentUrl.Substring(0, contentUrl.LastIndexOf("/"));

                    contentUrl += "/s640/" + entry.Title.Text;

                    retVal.Add(new PicasaPhoto() { Title = entry.Summary.Text, ImageSrc = contentUrl, Url = firstThumbUrl });
                }
            }
            catch
            {
                retVal = null;
            }
            return retVal;
        }

        public static List<PicasaAlbum> GetAlbums()
        {
            Settings = ExtensionManager.GetSettings("Picasa2");
            if (string.IsNullOrEmpty(Settings.GetSingleValue("Account"))) return null;

            var service = new PicasaService("exampleCo-exampleApp-1");

            string usr = Settings.GetSingleValue("Account") + "@gmail.com";
            string pwd = Settings.GetSingleValue("Password");

            service.setUserCredentials(usr, pwd);

            var query = new AlbumQuery(PicasaQuery.CreatePicasaUri(usr));
            PicasaFeed feed = service.Query(query);

            var albums = new List<PicasaAlbum>();
            foreach (PicasaEntry entry in feed.Entries)
            {
                var ac = new AlbumAccessor(entry);

                // thumbnail
                string albumUri = ((Google.GData.Client.AtomEntry)(entry)).AlternateUri.ToString();
                string firstThumbUrl = entry.Media.Thumbnails[0].Attributes["url"] as string;
                albums.Add(new PicasaAlbum() { Accessor = ac, ThumbNailURl = firstThumbUrl, AlbumUri = albumUri });

            }
            return albums;
        }

        protected void InitSettings()
        {
            var settings = new ExtensionSettings("Picasa2");

            settings.AddParameter("Account");
            settings.AddParameter("Password");
            settings.AddParameter("ShowWidth");
            settings.AddParameter("PicWidth");
            settings.AddParameter("AutoPlay");

            settings.AddValue("Account", "");
            settings.AddValue("Password", "secret");
            settings.AddValue("ShowWidth", "400");
            settings.AddValue("PicWidth", "72");
            settings.AddValue("AutoPlay", true);

            settings.IsScalar = true;
            ExtensionManager.ImportSettings(settings);

            ExtensionManager.SetAdminPage("Picasa2", "~/User controls/Picasa/Admin.aspx");

            Settings = ExtensionManager.GetSettings("Picasa2");
        }
    }
}