#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using App_Code.Extensions;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Google.GData.Photos;

#endregion

public partial class sponsoren : BlogBasePage
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
        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string name = browser.Browser;
        float version = (float)(browser.MajorVersion + browser.MinorVersion);
        IE7 = (name == "IE" && version == 7);

        StringBuilder sb = new StringBuilder();

        var listSponsors = CRSponsor.GetList().Where(i => i.Active);
        foreach (SponsorType sponsorType in Enum.GetValues(typeof(SponsorType)))
        {
            sb.Append("<div class=\"SponsorType\">");
            sb.AppendFormat("<h3>{0}</h3>", sponsorType.ToString());
            sb.Append("<ul>");
            
            int width = 0;
            int height = 0;
            GetTypeSpecificSettings(sponsorType, out width, out height);

            foreach (var crSponsor in listSponsors.Where(i => i.SponsorType == sponsorType))
            {
                string Tag = crSponsor.HasLogo ? string.Format("<img src=\"{0}\" width=\"{2}\" height=\"{3}\" alt=\"{1}\" title=\"{1}\" />", crSponsor.LogoURL, crSponsor.Name, width, height) : crSponsor.Name;

                sb.AppendFormat("<li><a href=\"{1}\" title=\"{2}\">{0}</a></li>", Tag, crSponsor.LogoURL, crSponsor.Name);
            }
            sb.Append("</ul>");
            sb.Append("</div>");
        }

        litSponsors.Text = sb.ToString();
    }

    private void GetTypeSpecificSettings(SponsorType type, out int logoWidth, out int logoHeight)
    {
        logoWidth = 300;
        logoHeight = 200;
        switch (type)
        {
            case SponsorType.Hoofdsponsor:
                logoWidth = 300;
                logoHeight = 200;
                break;
            case SponsorType.Sponsor:
                logoWidth = 250;
                logoHeight = 167;
                break;
            case SponsorType.Subsponsor:
                logoWidth = 200;
                logoHeight = 133;
                break;
            case SponsorType.ClubVan50:
                logoWidth = 175;
                logoHeight = 117;
                break;
            default:
                logoWidth = 300;
                logoHeight = 200;
                break;
        }

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