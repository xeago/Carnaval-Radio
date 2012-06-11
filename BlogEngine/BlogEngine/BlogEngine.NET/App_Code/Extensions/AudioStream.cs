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
   }





