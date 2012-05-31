using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Summary description for AudioStream
/// </summary>
[Extension("Extension used to give AudioStreams", "1.1", "Nick & Lars")]
public class AudioStream
{
    public AudioStream()
    {
        //Comment.Serving += new EventHandler<ServingEventArgs>(Post_CommentServing);
        ExtensionSettings settings = new ExtensionSettings("AudioStream");


        settings.AddParameter("HighStream", "HighStream.", 500, true, false, ParameterType.String);
        settings.AddParameter("LowStream", "LowStream.", 500, true, false, ParameterType.String);

        // settings.AddValue("HighStream", "Http://HighStreamVoorbeeld.nl");
        //settings.AddValue("LowStream", "Http://lowstreamVoorbeeld.nl");

        settings.IsScalar = true;
        ExtensionManager.ImportSettings(settings);


    }
    #region AudioStreamcreatorstuffvantwan

    public static void writeOut(string high, string low, string path)
    {
        if (!high.ToLower().StartsWith(@"http://")) high = @"http://" + high;
        if (!low.ToLower().StartsWith(@"http://")) low = @"http://" + low;

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        writePls(high, path, "high");
        writePls(low, path, "low");

        writeAsx(high, path, "high");
        writeAsx(low, path, "low");

        writeJson(high, low, path, "json");
    }

    private static void writeJson(string highserver, string lowserver, string path, string file)
    {
        file += ".json";
        using (StreamWriter sw = new StreamWriter(path + file))
        {
            sw.WriteLine("{\"high\":\"" + highserver + "\",\"low\":\"" + lowserver + "\"}");
        }
    }

    private static void writeAsx(string server, string path, string file)
    {
        file += ".asx";
        using (StreamWriter sw = new StreamWriter(path + file))
        {
            sw.WriteLine("<asx version = \"3.0\">" +
                            @"<Title>Carnaval-Radio.nl 2012</Title>" +
                            @"<Author>http://www.carnaval-radio.nl</Author>" +
                            "<MoreInfo href=\"http:////www.carnaval-radio.nl\" />" +
                            "<entry>" +
                                "<ref href=\"http:////" + server + "\" />" +
                                "<Title>Carnaval-Radio.nl 2012</Title> " +
                                @"<Author>http://www.carnaval-radio.nl</Author>" +
                            "</entry>" +
                            "</asx>");

        }
    }

    private static void writePls(string server, string path, string file)
    {
        file += ".pls";
        using (StreamWriter sw = new StreamWriter(path + file))
        {
            sw.WriteLine("[playlist]");
            sw.WriteLine("NumberOfEntries=1");
            sw.WriteLine("File1=" + server);
        }

    }
    #endregion


}





