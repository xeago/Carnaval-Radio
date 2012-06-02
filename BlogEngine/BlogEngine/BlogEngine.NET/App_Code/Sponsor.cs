using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BlogEngine.Core.Web.Extensions;

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

    

    public string LogoPhysicalPath
    {
        get { return HttpContext.Current.Server.MapPath("../../" + LogoURL); }
    }
    
	public CRSponsor()
	{
        ID = Guid.NewGuid();
	    CreationDate = DateTime.Now;
	}

    public CRSponsor(Guid id, bool IsDelete)
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
        this.SponsorType = (SponsorType) (GSDlib.Utils.NullableInt(dr["SponsorPage_SponsorType"]) ?? 1);

        this.PlayerSwitch = ConvertBoolElseFalse(dr["Player_Switch"]);
        this.PlayerSolid = ConvertBoolElseFalse(dr["Player_Solid"]);
        this.WidgetSwitch = ConvertBoolElseFalse(dr["Widget_Switch"]);
        this.MobileSwitch = ConvertBoolElseFalse(dr["Mobile_Switch"]);
        this.MobileSolid = ConvertBoolElseFalse(dr["Mobile_Solid"]);

        this.MFrequency = (MobileFrequency) (GSDlib.Utils.NullableInt(dr["Mobile_Frequency"]) ?? 1);

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
            settings.AddValue("SponsorPage_SponsorType", ((SponsorType)(GSDlib.Utils.NullableInt(this.SponsorType) ?? 1)).ToString());
            settings.AddValue("Player_Switch", PlayerSwitch.ToString().ToLower());
            settings.AddValue("Player_Solid", PlayerSolid.ToString());
            settings.AddValue("Widget_Switch", WidgetSwitch.ToString());
            settings.AddValue("Mobile_Switch", MobileSwitch.ToString());
            settings.AddValue("Mobile_Solid", MobileSolid.ToString());
            settings.AddValue("Mobile_Frequency", ((MobileFrequency)(GSDlib.Utils.NullableInt(this.MFrequency) ?? 1)).ToString());
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
                switch (parameter.Name)
                {
                    case "Name":
                        parameter.Values[i] = Name;
                        break;
                    case "URL":
                        parameter.Values[i] = Url;
                        break;
                    case "SposnorPage_SponsorType":
                        parameter.Values[i] =
                            ((SponsorType) (GSDlib.Utils.NullableInt(this.SponsorType) ?? 1)).ToString();
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
                        parameter.Values[i] = ((MobileFrequency)(GSDlib.Utils.NullableInt(this.MFrequency) ?? 1)).ToString();
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
            }
        }

        ExtensionManager.SaveSettings("Sponsor", settings);
        return settings.IsKeyValueExists(ID.ToString());
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