using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core.Web.Extensions;

/// <summary>
/// Summary description for AudioStream
/// </summary>
[Extension("Extension used to give AudioStreams", "1.0", "Nick & Lars")]
public class AudioStream
{
    public AudioStream()
    {
        //Comment.Serving += new EventHandler<ServingEventArgs>(Post_CommentServing);
        ExtensionSettings settings = new ExtensionSettings("AudioStream");

        settings.AddParameter("Server", "Server.", 254, false, true, ParameterType.String);
        settings.AddValue("Server", "Http://VoorbeeldServer.nl");

        settings.AddParameter("Quality", "Qualiteit. Hoog/Laag.", 10, true, false, ParameterType.String);
        settings.AddValue("Quality", "quality");

        settings.AddParameter("Priority", "Prioriteit, Geeft aan welke stream de standaard stream is. Het Laagste getal zal gebruikt worden.", 2, true, false, ParameterType.Integer);
        settings.AddValue("Priority", 10);

        ExtensionManager.ImportSettings(settings);


    }
    #region AudioStreamcreatorstuffvantwan

    //private static void writeOut(string high, string low, string path)
    //{
    //    if (!high.StartsWith(@"http://")) high=@"http://"+high;
    //    if (!low.StartsWith(@"http://")) low=@"http://"+low;

    //    if (!Directory.Exists(path))
    //        Directory.CreateDirectory(path);
    //    writePls(high, path, "high");
    //    writePls(low, path, "low");

    //    writeAsx(high, path, "high");
    //    writeAsx(high, path, "low");

    //    writeJson(high, low, path, "json");
    //}

    //private static void writeJson(string highserver, string lowserver, string path, string file)
    //{
    //    file += ".json";
    //    using (StreamWriter sw = new StreamWriter(path + file))
    //    {
    //        sw.WriteLine("{\"high\":\"" + highserver + "\",\"low\":\"" + lowserver + "\"}");
    //    }
    //}

    //private static void writeAsx(string server, string path, string file)
    //{
    //    file += ".asx";
    //    using (StreamWriter sw = new StreamWriter(path + file))
    //    {
    //        sw.WriteLine("<asx version = \"3.0\">" +
    //                        @"<Title>Carnaval-Radio.nl 2012</Title>" +
    //                        @"<Author>http://www.carnaval-radio.nl</Author>" +
    //                        "<MoreInfo href=\"http:////www.carnaval-radio.nl\" />" +
    //                        "<entry>" +
    //                            "<ref href=\"http:////" + server + "\" />" +
    //                            "<Title>Carnaval-Radio.nl 2012</Title> " +
    //                            @"<Author>http://www.carnaval-radio.nl</Author>" +
    //                        "</entry>" +
    //                        "</asx>");

    //    }
    //}

    //private static void writePls(string server, string path, string file)
    //{
    //    file +=".pls";
    //    using (StreamWriter sw = new StreamWriter(path + file))
    //    {
    //        sw.WriteLine("[playlist]");
    //        sw.WriteLine("NumberOfEntries=1");
    //        sw.WriteLine("File1=" + server);
    //    }

    //}
    #endregion


}





