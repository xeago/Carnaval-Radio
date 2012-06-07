#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web;
using App_Code.Extensions;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;
using Guestbook;

#endregion

public partial class fotoalbums : BlogBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<PicasaAlbum> albums = Picasa2.GetAlbums();

        var sb = new StringBuilder();
        foreach (var picasaAlbum in albums)
        {
            sb.AppendFormat("{0}", picasaAlbum.Accessor.Name);
            sb.AppendFormat("<a title=\"{1}\" href=\"\"><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" /></a>", picasaAlbum.ThumbNailURl, picasaAlbum.Accessor.Name);
            sb.AppendLine();

            foreach (var photo in picasaAlbum.AlbumContent)
            {
                sb.AppendFormat("<br /><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" /><br />", photo.Url, photo.Title);
                sb.AppendFormat("<br /><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" /><br />", photo.ImageSrc, photo.Title);
            }
        }
        litAlbums.Text = sb.ToString();
    }
}