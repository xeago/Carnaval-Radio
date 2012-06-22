using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.Text;
using BlogEngine.Core.Json;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class RecentSongsService : System.Web.Services.WebService {

    public RecentSongsService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public object RecentSongs() {
        var l =recentList();
        var jss = new System.Web.Script.Serialization.JavaScriptSerializer();

        var lijstje = new List<dynamic>();
        foreach (var item in l)
        {
            lijstje.Add(new { song = item });
        }

        return lijstje;
    }

    private static List<string> recentList()
    {
        WebClient c = new WebClient();
        c.Headers.Add(@"User-Agent: Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
        var Settings = ExtensionManager.GetSettings("AudioStream");
        
        var stream = Settings.GetSingleValue("HighStream");
        if (!stream.ToLower().StartsWith("http")) stream = @"http://"+stream;
        if (!stream.EndsWith("/")) stream += "/";
        var a = c.DownloadData(stream+"played.html");
        
        var s = Encoding.UTF8.GetString(a);
        var stringa = new string[] { @"<br><table border=0 cellpadding=2 cellspacing=2><tr><td>Played @</td><td><b>Song Title</b></td></tr><tr>" };

        var stuff = s.Split(stringa, StringSplitOptions.RemoveEmptyEntries)[1];
        var aftertd = stuff.Split(new string[] { @"</td><td>" }, StringSplitOptions.RemoveEmptyEntries);

        var lijstje = new List<string>();
        var nowplaying = aftertd[1].Split(new String[] { @"<td><b" }, StringSplitOptions.RemoveEmptyEntries)[0];
        lijstje.Add(nowplaying);
        for (int i = 2; i < aftertd.Length; i++)
        {
            lijstje.Add(aftertd[i].Split(new string[] { @"</tr>" }, StringSplitOptions.RemoveEmptyEntries)[0]);
        }
        return lijstje;
    }
}
