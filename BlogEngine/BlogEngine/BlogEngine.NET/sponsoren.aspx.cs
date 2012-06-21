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
        this.Title = Resources.labels.sponsoren;

        System.Web.HttpBrowserCapabilities browser = Request.Browser;
        string name = browser.Browser;
        float version = (float)(browser.MajorVersion + browser.MinorVersion);
        IE7 = (name == "IE" && version == 7);

        StringBuilder sb = new StringBuilder();

        var listSponsors = CRSponsor.GetListOnlyActive();
        foreach (SponsorType sponsorType in Enum.GetValues(typeof(SponsorType)))
        {
            double width = 0;
            double height = 0;
            getTypeSpecificSettings(sponsorType, out width, out height);
            
            var sponsor = new StringBuilder();
            foreach (var crSponsor in listSponsors.Where(i => i.SponsorType == sponsorType))
            {
                string Tag = crSponsor.HasLogo ? string.Format("<img src=\"{0}\" width=\"{2}\" height=\"{3}\" alt=\"{1}\" title=\"{1}\" />", crSponsor.LogoURL, crSponsor.Name, (int)width, (int)height) : crSponsor.Name;

                sponsor.AppendFormat("<li><a target=\"_blank\" href=\"{1}\" title=\"{2}\">{0}</a></li>", Tag, crSponsor.Url, crSponsor.Name);
            }

            if(!string.IsNullOrEmpty(sponsor.ToString()))
            {
                sb.AppendFormat("<div class=\"SponsorTypes {0}\">", sponsorType.ToString());
                sb.AppendFormat("<h3>{0}</h3>", CRSponsor.GetLabelBySponsorType(sponsorType));
                sb.Append("<ul>");
                sb.Append(sponsor.ToString());
                sb.Append("</ul>");
                sb.Append("</div>");
            }
        }
        litSponsors.Text = sb.ToString();
    }

    private static void getTypeSpecificSettings(SponsorType type, out double logoWidth, out double logoHeight)
    {
        logoWidth = 80;
        switch (type)
        {
            case SponsorType.Hoofdsponsor:
                logoWidth = 300;
                break;
            case SponsorType.Sponsor:
                logoWidth = 200;
                break;
            case SponsorType.Subsponsor:
                logoWidth = 150;
                break;
            case SponsorType.ClubVan50:
                logoWidth = 120;
                break;
            default:
                logoWidth = 80;
                break;
        }
        logoHeight = logoWidth / 1.54545454545;
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