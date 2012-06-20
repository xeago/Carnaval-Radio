using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Resources;
using System.Web;
using BlogEngine.Core.Web.Extensions;
using System.Text;
using BlogEngine.Core;
using System.IO;

public enum SponsorType
{
    Hoofdsponsor = 1,
    Sponsor,
    Subsponsor,
    ClubVan50,
    VriendenVanCr
}

public enum MobileFrequency
{
    Often = 1,
    Less
}

/// <summary>
/// Summary description for Sponsor
/// </summary>
public class CRSponsor
{
    private static ExtensionSettings s = ExtensionManager.GetSettings("Sponsor");

    private int index;
    private bool IsEdit = false;
    private DataTable dt;

    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public SponsorType SponsorType { get; set; }
    public string SponsorTypeNaam { get { return CRSponsor.GetLabelBySponsorType(this.SponsorType); } }
    public bool PlayerSwitch { get; set; }
    public bool PlayerSolid { get; set; }
    public bool WidgetSwitch { get; set; }
    public bool MobileSwitch { get; set; }
    public bool MobileSolid { get; set; }
    public MobileFrequency MFrequency { get; set; }
    public string LogoURL { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Active { get; set; }

    public bool HasLogo
    {
        get { return !string.IsNullOrEmpty(LogoURL); }
    }

    public string LogoPhysicalPath
    {
        get { return HttpContext.Current.Server.MapPath("../../" + LogoURL); }
    }

    public CRSponsor()
    {
        ID = Guid.NewGuid();
        CreationDate = DateTime.Now;
    }

    public CRSponsor(Guid id, bool isDelete)
    {
        //GetSponsor
        dt = s.GetDataTable();

        var dr = dt.Rows.Cast<DataRow>().SingleOrDefault(i => Guid.Parse(i["ID"].ToString()) == id);
        if (dr == null) return;

        IsEdit = true;
        this.index = dt.Rows.IndexOf(dr);
        this.ID = id;
        this.Name = dr["Name"].ToString();
        this.Url = dr["URL"].ToString();
        this.SponsorType = (SponsorType)(GSDlib.Utils.NullableInt(dr["SponsorPage_SponsorType"]) ?? 5);

        this.PlayerSwitch = ConvertBoolElseFalse(dr["Player_Switch"]);
        this.PlayerSolid = ConvertBoolElseFalse(dr["Player_Solid"]);
        this.WidgetSwitch = ConvertBoolElseFalse(dr["Widget_Switch"]);
        this.MobileSwitch = ConvertBoolElseFalse(dr["Mobile_Switch"]);
        this.MobileSolid = ConvertBoolElseFalse(dr["Mobile_Solid"]);

        this.MFrequency = (MobileFrequency)(GSDlib.Utils.NullableInt(dr["Mobile_Frequency"]) ?? 5);

        this.LogoURL = dr["Logo"].ToString();
        this.Description = dr["Description"].ToString();
        this.CreationDate = GSDlib.Utils.NullableDateTime(dr["CreationDate"].ToString()) ?? DateTime.MinValue;
        this.EndDate = GSDlib.Utils.NullableDateTime(dr["EndDate"].ToString());
        this.Active = ConvertBoolElseFalse(dr["Active"]);
    }

    public bool Save()
    {
        ExtensionSettings settings = ExtensionManager.GetSettings("Sponsor");
        if (!IsEdit)
        {
            settings.AddValue("ID", ID.ToString());
            settings.AddValue("Name", Name);
            settings.AddValue("URL", Url ?? string.Empty);
            settings.AddValue("SponsorPage_SponsorType", ((int)this.SponsorType).ToString());
            settings.AddValue("Player_Switch", PlayerSwitch.ToString().ToLower());
            settings.AddValue("Player_Solid", PlayerSolid.ToString());
            settings.AddValue("Widget_Switch", WidgetSwitch.ToString());
            settings.AddValue("Mobile_Switch", MobileSwitch.ToString());
            settings.AddValue("Mobile_Solid", MobileSolid.ToString());
            settings.AddValue("Mobile_Frequency", ((int)this.MFrequency).ToString());
            settings.AddValue("Logo", LogoURL ?? string.Empty);
            settings.AddValue("Description", Description ?? string.Empty);
            settings.AddValue("CreationDate", DateTime.Now.ToString());
            settings.AddValue("EndDate", EndDate.ToString());
            settings.AddValue("Active", Active.ToString());
        }
        else
        {
            int i = this.index;
            foreach (var parameter in settings.Parameters)
            {
                #region parameter switch
                switch (parameter.Name)
                {
                    case "Name":
                        parameter.Values[i] = Name;
                        break;
                    case "URL":
                        parameter.Values[i] = Url;
                        break;
                    case "SponsorPage_SponsorType":
                        parameter.Values[i] =
                            ((int)this.SponsorType).ToString();
                        break;
                    case "Player_Switch":
                        parameter.Values[i] = PlayerSwitch.ToString();
                        break;
                    case "Player_Solid":
                        parameter.Values[i] = PlayerSolid.ToString();
                        break;
                    case "Widget_Switch":
                        parameter.Values[i] = WidgetSwitch.ToString();
                        break;
                    case "Mobile_Switch":
                        parameter.Values[i] = MobileSwitch.ToString();
                        break;
                    case "Mobile_Solid":
                        parameter.Values[i] = MobileSolid.ToString();
                        break;
                    case "Mobile_Frequency":
                        parameter.Values[i] = ((int)this.MFrequency).ToString();
                        break;
                    case "Logo":
                        parameter.Values[i] = LogoURL;
                        break;
                    case "Description":
                        parameter.Values[i] = Description;
                        break;
                    //case "CreationDate":
                    //    parameter.Values[i] = CreationDate.ToString();
                    //    break;
                    case "EndDate":
                        parameter.Values[i] = EndDate.HasValue ? EndDate.ToString() : string.Empty;
                        break;
                    case "Active":
                        parameter.Values[i] = Active.ToString();
                        break;
                    default:

                        break;
                }
                #endregion switch parameter
            }

            //else
        }

        ExtensionManager.SaveSettings("Sponsor", settings);

        //save the json
        CRSponsor.updateJSON();

        return settings.IsKeyValueExists(ID.ToString());
    }

    /// <summary>
    /// Writes the supplied list of dynamics to the specified path as json.
    /// </summary>
    /// <param name="obj">Input objects to be written as json.</param>
    /// <param name="path">The output path.</param>
    private static void writeSponsorJSON(dynamic obj, string path, string file)
    {
        var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        var sb = new StringBuilder();
        js.Serialize(obj, sb);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        using (var w = new System.IO.StreamWriter(path + file))
        {
            try
            {
                w.Write(sb);
            }
            catch (IOException e)
            {
                BlogEngine.Core.Utils.Log("writeSponsorJSON()>w.Write", e);
            }
        }
    }

    public bool Delete()
    {
        ExtensionSettings settings = ExtensionManager.GetSettings("Sponsor");
        int i = this.index;
        foreach (var parameter in settings.Parameters)
        {
            parameter.DeleteValue(i);
        }

        ExtensionManager.SaveSettings("Sponsor", settings);

        updateJSON();

        //return if row is deleted
        return dt.Rows.Cast<DataRow>().SingleOrDefault(dr => Guid.Parse(dr["ID"].ToString()) == ID) == null;
    }

    public static List<CRSponsor> GetList()
    {
        return s.GetDataTable().Rows.Cast<DataRow>().Select(dr => new CRSponsor(Guid.Parse(dr["ID"].ToString()), false)).ToList();
    }

    public static List<CRSponsor> GetListOnlyActiveAndAlter()
    {
        var l = GetList();
        var expired = l.Where(i => i.EndDate.HasValue || i.EndDate <= DateTime.Now);
        //if (Security.IsAuthenticated)
        //{
        //    foreach (var item in expired) //fix them on disk aswell
        //    {
        //        item.Active = false;
        //        item.Save();
        //    }
        //}
        var active = l.Where(i => i.Active && (!i.EndDate.HasValue || i.EndDate >= DateTime.Now));

        if (expired.Any()) // update the json if there were any expired
            updateJSON(active);

        return active.ToList();
    }

    public static string GetLabelBySponsorType(SponsorType s)
    {
        var rm = new ResourceManager(typeof(Resources.labels));
        var text = rm.GetString(s.ToString(),System.Globalization.CultureInfo.CreateSpecificCulture("nl"));
        return string.IsNullOrEmpty(text) ? s.ToString() : text;
    }

    private static bool ConvertBoolElseFalse(object a)
    {
        try
        {
            return Convert.ToBoolean(a.ToString());
        }
        catch
        {
            return false;
        }
    }

    private static void updatePlayerSwitchJSON(IEnumerable<CRSponsor> sponsors)
    {
        var playerRotatingSponsors = sponsors.Where(s => s.WidgetSwitch);
        var player = new List<dynamic>();
        foreach (var item in playerRotatingSponsors)
        {
            player.Add(new
            {
                url = item.Url,
                logoUrl = item.LogoURL
            });
        }
        writeSponsorJSON(player, HttpContext.Current.Server.MapPath("~/json/"), "rotatingPlayerSponsor.json");

    }

    /// <summary>
    /// Updates the JSON files for sponsor.
    /// </summary>
    private static void updateJSON() { updateJSON(GetList().Where(s => s.Active)); }
    /// <summary>
    /// Updates the JSON files for sponsor.
    /// </summary>
    private static void updateJSON(IEnumerable<CRSponsor> sponsors)
    {
        updateMobileSwitchJSON(sponsors);
        updateMobileSolidJSON(sponsors);
        updatePlayerSwitchJSON(sponsors);
        //updatePlayerSwitchJSON(sponsors);
        updateWidgetSwitchJSON(sponsors);

    }

    private static void updateWidgetSwitchJSON(IEnumerable<CRSponsor> sponsors)
    {
        var widgetRotatingSponsors = sponsors.Where(s => s.WidgetSwitch);
        var widget = new List<dynamic>();
        foreach (var item in widgetRotatingSponsors)
        {
            widget.Add(new
            {
                url = item.Url,
                logoUrl = item.LogoURL
            });
        }
        writeSponsorJSON(widget, HttpContext.Current.Server.MapPath("~/json/"), "rotatingWidgetSponsor.json");
    }


    /*{
        [{
            'type': 'BLa',
            'order': 0,
            'items' [{
                'name': 'sponsorName',
                'logoUrl': 'htpp://..',
                'description': 'sponsorDescription dkslfds'
            },{
                'name': 'sponsorName',
                'logoUrl': 'htpp://..',
                'description': 'sponsorDescription dkslfds'
            }]
        }]
    }*/
    private static void updateMobileSolidJSON(IEnumerable<CRSponsor> sponsors)
    {
        var mobileSolidSponsors = sponsors.Where(s => s.MobileSolid);
        var sponsorTypen = mobileSolidSponsors.Select(s => s.SponsorType).Distinct();

        var solid = new List<dynamic>();
        foreach (var typen in sponsorTypen)
        {
            var sponsorsPerType = mobileSolidSponsors.Where(s => s.SponsorType == typen);
            var items = new List<dynamic>();
            foreach (var sponsor in sponsorsPerType)
            {
                items.Add(new { name = sponsor.Name, logoUrl = Utils.AbsoluteWebRoot + sponsor.LogoURL, description = sponsor.Description });
            }
            solid.Add(new { order = (int)typen, type = CRSponsor.GetLabelBySponsorType(typen), items = items });
        }
        writeSponsorJSON(solid, HttpContext.Current.Server.MapPath("~/json/"), "solidMobileSponsor.json");
    }

    private static void updateMobileSwitchJSON(IEnumerable<CRSponsor> sponsors)
    {
        var mobileRotatingSponsors = sponsors.Where(s => s.MobileSwitch);
        var mobile = new List<dynamic>();
        foreach (var item in mobileRotatingSponsors)
        {
            mobile.Add(new
            {
                url = item.Url,
                logoUrl = item.LogoURL
            });
        }
        writeSponsorJSON(mobile, HttpContext.Current.Server.MapPath("~/json/"), "rotatingMobileSponsor.json");
    }
}