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
[WebService(Namespace = "/BlogEngine/AudioStreams")]
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
        ExtensionSettings settings = new ExtensionSettings("AudioStream");
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
