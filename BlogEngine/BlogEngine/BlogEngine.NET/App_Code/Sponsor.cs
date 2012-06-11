using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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



            bool shouldUpdateRotateJSON = this.MobileSwitch;
            if (shouldUpdateRotateJSON) CRSponsor.updateJSON(this);

            //else
        }

        ExtensionManager.SaveSettings("Sponsor", settings);
        return settings.IsKeyValueExists(ID.ToString());
    }

    /// <summary>
    /// Updates the JSON files for sponsor. It is smart enough to decide if it should or shouldn't update.
    /// </summary>
    private static void updateJSON() { updateJSON(null); }
    /// <summary>
    /// Updates the JSON files for sponsor. It is smart enough to decide if it should or shouldn't update. Pass null or use the parameterless method for extended smartness.
    /// </summary>
    /// <param name="s">CRSponsor item to base the decision on whether it should or shouldn't update.</param>
    private static void updateJSON(CRSponsor s)
    {
        bool shouldUpdateMobileSwitch = s == null || s.MobileSwitch;
        bool shouldUpdatePlayerSwitch = s == null || s.PlayerSwitch;
        bool shouldUpdateWidgetSwitch = s == null || s.WidgetSwitch;
        bool shouldUpdateMobileSolid = s == null || s.MobileSolid;
        bool shouldUpdatePlayerSolid = s == null || s.PlayerSolid;

        if (shouldUpdateMobileSwitch || shouldUpdatePlayerSwitch || shouldUpdateWidgetSwitch || shouldUpdateMobileSolid || shouldUpdatePlayerSolid)
        {
            var sponsors = GetList().Where(s => s.Active);
            if (shouldUpdateMobileSwitch) updateMobileSwitchJSON(sponsors);
            //if (shouldUpdatePlayerSwitch) updatePlayerSwitchJSON();
            //if (shouldUpdateWidgetSwitch) updateWidgetSwitchJSON();
        }
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
        writeSponsorJSON(mobile, Blog.CurrentInstance.RelativeWebRoot + "rotatingSponsor.json");
    }

    /// <summary>
    /// Writes the supplied list of dynamics to the specified path as json.
    /// </summary>
    /// <param name="obj">Input objects to be written as json.</param>
    /// <param name="path">The output path.</param>
    private static void writeSponsorJSON(List<dynamic> obj,string path)
    {
        var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        var sb = new StringBuilder();
        js.Serialize(obj, sb);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        using (var w = new System.IO.StreamWriter(path))
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
}