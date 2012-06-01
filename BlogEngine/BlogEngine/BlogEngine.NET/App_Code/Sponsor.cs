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
    
	public CRSponsor()
	{
        ID = Guid.NewGuid();
	    CreationDate = DateTime.Now;
	}

    public CRSponsor(Guid id)
    {
        //GetSponsor
        var dt = s.GetDataTable();

        var dr = dt.Rows.Cast<DataRow>().SingleOrDefault(i => new Guid(i["ID"].ToString()) == id);
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
        this.EndDate = GSDlib.Utils.NullableDateTime(dr["CreationDate"].ToString());
        this.Active = ConvertBoolElseFalse(dr["Active"]);
    }

    public bool Save()
    {
        ExtensionSettings settings = ExtensionManager.GetSettings("Sponsor");

        foreach (var parameter in settings.Parameters)
        {
            switch (parameter.Name)
            {
                case "Name":
                    parameter.Values[this.index] = Name;
                    break;
                case "URL":
                    parameter.Values[this.index] = Url;
                    break;
                default:
                    break;
            }
        }
        
        ExtensionManager.SaveSettings("Sponsor", settings);
        return settings.IsKeyValueExists(ID.ToString());
    }

    public static List<CRSponsor> GetList()
    {
        return s.GetDataTable().Rows.Cast<DataRow>().Select(dr => new CRSponsor(new Guid(dr["ID"].ToString()))).ToList();
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