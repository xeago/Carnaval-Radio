#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using App_Code.Extensions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;

#endregion

public partial class fotoalbum : BlogBasePage
{
    private bool IE7 = false;
    private string albumID;
    /*
     <link rel="stylesheet" type="text/css" href="css/base/advanced-slider-base.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="css/glossy-curved/black/glossy-curved-black.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="css/lightbox-slider.css" media="screen"/>

<link rel="stylesheet" type="text/css" href="../presentation-assets/presentation.css" media="screen"/>
<!--[if IE 7]><link rel="stylesheet" type="text/css" href="../presentation-assets/presentation-ie7.css" media="screen"/><![endif]-->
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        albumID = Request.QueryString["AlbumID"];
        if(string.IsNullOrEmpty(albumID)){Response.Redirect("foto_albums.aspx");}

        string name = browser.Browser;
        float version = (float) (browser.MajorVersion + browser.MinorVersion);
        IE7 = (name == "IE" && version == 7);

        RegisterStyleSheetInclude(Utils.AbsoluteWebRoot + "Styles/base/advanced-slider-base.css");
        RegisterStyleSheetInclude(Utils.AbsoluteWebRoot +
                                  "Styles/glossy-curved-rounded/orange/glossy-curved-rounded-orange.css");
        RegisterStyleSheetInclude(Utils.AbsoluteWebRoot + "Styles/lightbox-slider.css");

        if (IE7) RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/excanvas.compiled.js");
        RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.advancedSlider.min.js");
        RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.touchSwipe.min.js");
        RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.transition.min.js");
        RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/OnLoadFoto.js");

        PicasaAlbum picasaAlbum = Picasa2.GetAlbums().SingleOrDefault(i => i.Accessor.Id == albumID);
        if (picasaAlbum == null){Response.Redirect("foto_albums.aspx");}

        litTitle.Text = picasaAlbum.Accessor.Name;
        litDesc.Text = picasaAlbum.Accessor.AlbumSummary;
        var sb = new StringBuilder();

        sb.AppendFormat("<div class=\"advanced-slider\" id=\"{0}\">", "lightbox-slider");
        sb.Append("<ul class=\"slides\">");

        //sb.AppendFormat("{0}", picasaAlbum.Accessor.Name);
        //sb.AppendFormat("<a title=\"{1}\" href=\"\"><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" /></a>", picasaAlbum.ThumbNailURl, picasaAlbum.Accessor.Name);

        foreach (var photo in picasaAlbum.AlbumContent)
        {
            sb.Append("<li class=\"slide\">");
            sb.AppendFormat("<img class=\"image\" src=\"{0}\" alt=\"{1}\" title=\"{1}\" />", photo.ImageSrc, photo.Title);
            sb.AppendFormat("<img class=\"thumbnail\" src=\"{0}\" alt=\"{1}\" title=\"{1}\" />", photo.Url, photo.Title);
            sb.AppendFormat("<div class=\"caption\">{0}</div>", photo.Title);
            sb.Append("</li>");
        }

        sb.Append("</ul>");
        sb.Append("</div>");

        litAlbums.Text = sb.ToString();
    }

    /// <summary>
    /// Registers the client script include.
    /// </summary>
    /// <param name="src">The file name.</param>
    private void RegisterClientScriptInclude(string src)
    {
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl();
        si.TagName = "script";
        si.Attributes.Add("type", "text/javascript");
        si.Attributes.Add("src", src);
        this.Page.Header.Controls.Add(si);
        this.Page.Header.Controls.Add(new LiteralControl("\n"));
    }

    /// <summary>
    /// Registers the client script include.
    /// </summary>
    /// <param name="src">The file name.</param>
    private void RegisterStyleSheetInclude(string src)
    {
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl();
        si.TagName = "link";
        si.Attributes.Add("type", "text/css");
        si.Attributes.Add("rel", "stylesheet");
        si.Attributes.Add("media", "screen");
        si.Attributes.Add("href", src);
        this.Page.Header.Controls.Add(si);
        this.Page.Header.Controls.Add(new LiteralControl("\n"));
    }
}