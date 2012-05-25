using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    public byte[] GetAudioStreamJson()
    {
        Dictionary<string, ManagedExtension> extensions = ExtensionManager.Extensions;
        DataTable table = extensions["AudioStream"].Settings[0].GetDataTable();

        List<AudioStreamObject> audioStreams = new List<AudioStreamObject>();
        foreach (DataRow row in table.Rows)
        {
            AudioStreamObject s = new AudioStreamObject();
            s.Server = (string)row["Server"];
            s.Quality = (string)row["Quality"];
            s.Priority = ((string)row["Priority"]);
            audioStreams.Add(s);
        }

        AudioStreamObject outputStream = audioStreams.Count == 0 ? audioStreams[audioStreams.Count - 1] : new AudioStreamObject("http://TestServer.nl", "Low", "10");

        foreach (AudioStreamObject audioStreamObject in audioStreams)
        {
            if (Convert.ToInt32(outputStream.Priority) > Convert.ToInt32(audioStreamObject.Priority))
            {
                outputStream = audioStreamObject;
            }
        }

        string path = "../App_data/AudioStreams/";
        string filePath = path + "json.json";
        if (!File.Exists(filePath))
        {
            AudioStream.writeOut("100.100.100", "100.100.100", path);
        }
        byte[] output = new byte[0];
        using (FileStream f = File.Open(filePath, FileMode.Open))
        {
            byte[] outputObject = new byte[f.Length];
            f.Read(outputObject, 0, (int)f.Length);
            output = outputObject;
        }


        return output;
    }

}

public class AudioStreamObject
{
    public AudioStreamObject()
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
