#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.UI;
using App_Code.Extensions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;

#endregion

public partial class fotoalbums : BlogBasePage
{
    private bool IE7 = false;
    /*
     <link rel="stylesheet" type="text/css" href="css/base/advanced-slider-base.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="css/glossy-curved/black/glossy-curved-black.css" media="screen"/>
<link rel="stylesheet" type="text/css" href="css/lightbox-slider.css" media="screen"/>

<link rel="stylesheet" type="text/css" href="../presentation-assets/presentation.css" media="screen"/>
<!--[if IE 7]><link rel="stylesheet" type="text/css" href="../presentation-assets/presentation-ie7.css" media="screen"/><![endif]-->
     */
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = Resources.labels.fotos;

        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string name = browser.Browser;
        float version = (float)(browser.MajorVersion + browser.MinorVersion);
        IE7 = (name == "IE" && version == 7);
        
        //RegisterStyleSheetInclude(Utils.AbsoluteWebRoot + "Styles/base/advanced-slider-base.css");
       // RegisterStyleSheetInclude(Utils.AbsoluteWebRoot + "Styles/glossy-curved-rounded/orange/glossy-curved-rounded-orange.css");
        //RegisterStyleSheetInclude(Utils.AbsoluteWebRoot + "Styles/lightbox-slider.css");

        //if (IE7) RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/excanvas.compiled.js");
        //RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.advancedSlider.min.js");
        //RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.touchSwipe.min.js");
        //RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/jquery.transition.min.js");
        //RegisterClientScriptInclude(Utils.AbsoluteWebRoot + "Scripts/fotoalbum/OnLoad.js");

        List<PicasaAlbum> albums = Picasa2.GetAlbums();

        var sb = new StringBuilder();
        sb.AppendFormat("<div class=\"albums\">");
        sb.Append("<ul class=\"albumtiles\">");
        foreach (var picasaAlbum in albums)
        {
            sb.AppendFormat("<li><a title=\"{1}\" href=\"{2}\"><p>{1}</p><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" /></a></li>", 
                picasaAlbum.ThumbNailURl, picasaAlbum.Accessor.Name, string.Format("foto_album.aspx?AlbumID={0}", picasaAlbum.Accessor.Id));
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
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl {TagName = "script"};
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
        var si = new System.Web.UI.HtmlControls.HtmlGenericControl {TagName = "link"};
        si.Attributes.Add("type", "text/css");
        si.Attributes.Add("rel", "stylesheet");
        si.Attributes.Add("media", "screen");
        si.Attributes.Add("href", src);
        this.Page.Header.Controls.Add(si);
        this.Page.Header.Controls.Add(new LiteralControl("\n"));
    }
}