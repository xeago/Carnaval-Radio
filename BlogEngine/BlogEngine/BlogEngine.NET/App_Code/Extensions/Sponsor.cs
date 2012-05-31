using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;
using Resources;

/// <summary>
/// Summary description for AudioStream
/// </summary>
[Extension("Extension used to set sponsors", "1.0", "Nick")]
public class Sponsor
{
    public Sponsor()
    {
        //Comment.Serving += new EventHandler<ServingEventArgs>(Post_CommentServing);
        var settings = new ExtensionSettings("Sponsor");

        settings.AddParameter("Name", labels.name, 150, true, true, ParameterType.String);
        settings.AddParameter("URL", labels.website, 255);

        #region Type Bools

        //Sponsor Page Bools
        settings.AddParameter("SponsorPage_Hoofdsponsor", labels.HoofdSponsor);
        settings.AddParameter("SponsorPage_Sponsor", labels.Sponsor);
        settings.AddParameter("SponsorPage_Subsponsor", labels.SubSponsor);
        settings.AddParameter("SponsorPage_ClubVan50", labels.ClubVan50);
        settings.AddParameter("SponsorPage_VriendenVanCR", labels.VriendenVanCR);

        //Player Bools
        settings.AddParameter("Player_Switch", labels.Switch);
        settings.AddParameter("Player_Solid", labels.Solid);

        //Widget
        settings.AddParameter("Widget_Switch", labels.Switch);

        //Mobiele app
        settings.AddParameter("Mobile", labels.Switch);
        #endregion

        settings.AddParameter("Mobile_Frequency", labels.Switch);

        settings.AddParameter("Logo", labels.sponsorlogo, 255);

        settings.AddParameter("Description", labels.description, int.MaxValue);

        settings.AddParameter("CreationDate", labels.setPublishDate);
        settings.AddParameter("EndDate", labels.endDate);
        settings.AddParameter("Active", labels.active);

        ExtensionManager.ImportSettings(settings);
    }
}





