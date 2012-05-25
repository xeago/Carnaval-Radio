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
        string highServer = extensions["AudioStream"].Settings[0].GetSingleValue("HighStream");
        string lowhServer = extensions["AudioStream"].Settings[0].GetSingleValue("LowStream");

        string path = Server.MapPath("..\\AudioStreams\\");
        string filePath = path + "json.json";
            AudioStream.writeOut(highServer,lowhServer, path);
        byte[] output = new byte[0];
        //TODO FIlestream stuff
        using (FileStream f = File.Open(filePath, FileMode.Open))
        {
            byte[] outputObject = new byte[f.Length];
            f.Read(outputObject, 0, (int)f.Length);
            output = outputObject;
        }


        return output;
    }

}


