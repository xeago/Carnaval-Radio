using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using BlogEngine.Core;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "AudioStreams")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
[ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public AudioStreamObject GetAudioStreams()
    {
        AudioStream audiostream = new AudioStream();
        ExtensionSettings settings = new ExtensionSettings("AudioStream");
        settings.AddParameter("Server", "Server.", 254, false, true, ParameterType.String);
        settings.AddParameter("Quality", "Qualiteit. Hoog/Laag.", 10, true, false, ParameterType.String);
        settings.AddParameter("Priority", "Prioriteit, Geeft aan welke stream de standaard stream is. Het Laagste getal zal gebruikt worden.", 2, true, false, ParameterType.Integer);
        ExtensionManager.ImportSettings(settings);
        DataTable table = settings.GetDataTable();

        List<AudioStreamObject> audioStreams = new List<AudioStreamObject>();
        foreach (DataRow row in table.Rows)
        {
            AudioStreamObject s = new AudioStreamObject();
            s.Server = (string)row["Server"];
            s.Quality = (string)row["Quality"];
            s.Priority = ((int) row["Priority"]).ToString();           
            audioStreams.Add(s);
        }

        AudioStreamObject output = audioStreams.Count == 0 ? audioStreams[audioStreams.Count-1] : new AudioStreamObject("http://TestServer.nl","Low","10");

        foreach (AudioStreamObject audioStreamObject in audioStreams)
        {
            if (Convert.ToInt32(output.Priority) < Convert.ToInt32(audioStreamObject.Priority))
            {
                output = audioStreamObject;
            }
        }
        return output;
    }

}

public class AudioStreamObject
{
    public AudioStreamObject ()
    {
        
    }
    public AudioStreamObject(string server, string quality, string priority)
    {
        Server = server;
        Quality = quality;
        Priority = priority;
        }
    public string Server;
    public string Quality;
    public string Priority;

}
