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
        settings.AddParameter("URL", labels.website, 255, false, false, ParameterType.String);

        #region Type Bools

        //Sponsor Page Bools
        settings.AddParameter("SponsorPage_Hoofdsponsor", labels.HoofdSponsor, false, false, ParameterType.String);
        settings.AddParameter("SponsorPage_Sponsor", labels.Sponsor, false, false, ParameterType.String);
        settings.AddParameter("SponsorPage_Subsponsor", labels.SubSponsor, false, false, ParameterType.String);
        settings.AddParameter("SponsorPage_ClubVan50", labels.ClubVan50, false, false, ParameterType.String);
        settings.AddParameter("SponsorPage_VriendenVanCR", labels.VriendenVanCR, false, false, ParameterType.String);

        //Player Bools
        settings.AddParameter("Player_Switch", labels.Switch, false, false, ParameterType.String);
        settings.AddParameter("Player_Solid", labels.Solid, false, false, ParameterType.String);

        //Widget
        settings.AddParameter("Widget_Switch", labels.Switch, false, false, ParameterType.String);

        //Mobiele app
        settings.AddParameter("Mobile", labels.Switch, false, false, ParameterType.String);
        #endregion

        settings.AddParameter("Mobile_Frequency", labels.Switch, false, false, ParameterType.String);

        settings.AddParameter("Logo", labels.sponsorlogo, 255, false, false, ParameterType.String);

        settings.AddParameter("Description", labels.description, int.MaxValue, false, false, ParameterType.String);

        settings.AddParameter("CreationDate", labels.setPublishDate, false, false, ParameterType.String);
        settings.AddParameter("EndDate", labels.endDate, false, false, ParameterType.String);
        settings.AddParameter("Active", labels.active, false, false, ParameterType.String);

        ExtensionManager.ImportSettings(settings);
    }
}





