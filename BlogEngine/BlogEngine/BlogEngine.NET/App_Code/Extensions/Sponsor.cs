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

public enum SponsorType
{
    Hoofdsponsor =1,
    Sponsor,
    Subsponsor,
    ClubVan50,
    VriendenVanCr

}

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

        settings.AddParameter("ID", labels.ID, 150, true, true, ParameterType.String);
        settings.AddParameter("Name", labels.name, 150, true, false, ParameterType.String);
        settings.AddParameter("URL", labels.website, 255);

        //SponsorType Can have values from the SponsorType Enum. The number wil be saved
        settings.AddParameter("SponsorPage_SponsorType", labels.sponsorType);

        #region Type Bools
        //Player Bools
        settings.AddParameter("Player_Switch", labels.Switch);
        settings.AddParameter("Player_Solid", labels.Solid);

        //Widget
        settings.AddParameter("Widget_Switch", labels.widget);

        //Mobiele app
        settings.AddParameter("Mobile_Switch", labels.Switch);
        settings.AddParameter("Mobile_Solid", labels.Solid);
        #endregion

        settings.AddParameter("Mobile_Frequency", labels.Frequency);

        settings.AddParameter("Logo", labels.sponsorlogo, 255);

        settings.AddParameter("Description", labels.description, int.MaxValue);

        settings.AddParameter("CreationDate", labels.setPublishDate);
        settings.AddParameter("EndDate", labels.endDate);
        settings.AddParameter("Active", labels.active);

        ExtensionManager.ImportSettings(settings);
    }
}





