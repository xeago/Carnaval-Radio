using System.Text;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;

namespace admin.Settings
{
    using System;
    using Resources;
    using App_Code;
    using Page = System.Web.UI.Page;

    public partial class Picasa : Page
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            WebUtils.CheckRightsForAdminSettingsPage(false);

            Page.MaintainScrollPositionOnPostBack = true;
            Page.Title = labels.settings;

            base.OnInit(e);
        }

        static protected ExtensionSettings Settings;

        protected void Page_Load(object sender, EventArgs e)
        {
            // load settings
            Settings = ExtensionManager.GetSettings("Picasa2");

            if (!Page.IsPostBack)
            {
                BindForm();

                try
                {
                    litContent.Text = GetAlbumsTable();
                }
                catch (Exception pEx)
                {
                    litContent.Text = labels.ErrorConnectingPicasa + pEx.Message;
                }
            }
        }

        protected string GetAlbumsTable()
        {
            if (string.IsNullOrEmpty(Settings.GetSingleValue("Account"))) return string.Empty;

            var service = new PicasaService("exampleCo-exampleApp-1");

            string usr = Settings.GetSingleValue("Account") + "@gmail.com";
            string pwd = Settings.GetSingleValue("Password");

            service.setUserCredentials(usr, pwd);

            var sb = new StringBuilder("<table class=\"beTable\">");

            var query = new AlbumQuery(PicasaQuery.CreatePicasaUri(usr));
            PicasaFeed feed = service.Query(query);

            sb.Append(GetHeader());

            var cnt = 0;
            foreach (PicasaEntry entry in feed.Entries)
            {
                var ac = new AlbumAccessor(entry);
                const string cell = "<td>{0}</td>";

                sb.Append(cnt % 2 == 0 ? "<tr>" : "<tr class=\"alt\">");

                // thumbnail
                string albumUri = ((Google.GData.Client.AtomEntry)(entry)).AlternateUri.ToString();
                string firstThumbUrl = entry.Media.Thumbnails[0].Attributes["url"] as string;
                string thumbAncr = string.Format("<a href=\"{2}\"><img src=\"{0}\" alt=\"{1}\" width=\"40\" /></a>",
                             firstThumbUrl, entry.Title.Text, albumUri);
                sb.Append(string.Format(cell, thumbAncr));

                // title
                sb.Append(string.Format(cell, ac.AlbumTitle));

                // description
                sb.Append(string.Format(cell, ac.AlbumSummary + "&nbsp;"));

                // number of photos
                sb.Append(string.Format(cell, ac.NumPhotos));

                // access
                sb.Append(string.Format(cell, ac.Access));

                // extension tag
                string feedUri = entry.FeedUri;
                const string showTag = "[PicasaShow:{0}]";
                const string albmTag = "[PicasaAlbum:{0}]";

                if (ac.Access.ToLower() == "protected")
                {
                    sb.Append(string.Format(cell, "&nbsp;"));
                    sb.Append(string.Format(cell, "&nbsp;"));
                }
                else
                {
                    sb.Append(string.Format(cell, string.Format(showTag, ac.AlbumTitle)));
                    sb.Append(string.Format(cell, string.Format(albmTag, ac.AlbumTitle)));
                }


                sb.Append("</tr>");
                cnt++;
            }

            return sb + "</table>";
        }

        protected string GetHeader()
        {
            var sb = new StringBuilder();
            const string head = "<th>{0}</th>";

            sb.Append("<tr style=\"background-color:#ccc\">");
            sb.Append(string.Format(head, ""));
            sb.Append(string.Format(head, labels.title));
            sb.Append(string.Format(head, labels.description));
            sb.Append(string.Format(head, labels.photos));
            sb.Append(string.Format(head, labels.access));
            sb.Append(string.Format(head, "SlideShow Tag"));
            sb.Append(string.Format(head, "Album Tag"));
            sb.Append("</tr>");

            return sb.ToString();
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (Settings != null)
            {
                Settings.UpdateScalarValue("Account", txtGMail.Text);
                Settings.UpdateScalarValue("Password", txtPwd.Text);
                Settings.UpdateScalarValue("AutoPlay", chkAuto.Checked.ToString());
                Settings.UpdateScalarValue("PicWidth", ddWidth.SelectedValue);
                Settings.UpdateScalarValue("ShowWidth", txtShowWidth.Text);

                ExtensionManager.SaveSettings("Picasa2", Settings);
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void BindForm()
        {
            btnSave.Text = labels.saveSettings;

            ddWidth.Items.Add("72");
            ddWidth.Items.Add("94");
            ddWidth.Items.Add("144");
            ddWidth.Items.Add("160");
            ddWidth.Items.Add("220");
            ddWidth.Items.Add("288");

            if (Settings != null)
            {
                switch (Settings.GetSingleValue("PicWidth"))
                {
                    case "94":
                        ddWidth.Items[1].Selected = true;
                        break;
                    case "144":
                        ddWidth.Items[2].Selected = true;
                        break;
                    case "160":
                        ddWidth.Items[3].Selected = true;
                        break;
                    case "220":
                        ddWidth.Items[4].Selected = true;
                        break;
                    case "288":
                        ddWidth.Items[5].Selected = true;
                        break;
                    default:
                        ddWidth.Items[0].Selected = true;
                        break;
                }

                txtGMail.Text = Settings.GetSingleValue("Account");
                txtPwd.Attributes.Add("value", Settings.GetSingleValue("Password"));
                txtShowWidth.Text = Settings.GetSingleValue("ShowWidth");

                chkAuto.Checked = false;
                if (bool.Parse(Settings.GetSingleValue("AutoPlay")))
                {
                    chkAuto.Checked = true;
                }
            }
        }

    }
}