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
[Extension("Extension used to give AudioStreams","1.0","Nick & Lars")]
public class AudioStream 
{
	public AudioStream()
    {
        //Comment.Serving += new EventHandler<ServingEventArgs>(Post_CommentServing);
        ExtensionSettings settings = new ExtensionSettings("AudioStream");

        settings.AddParameter("Server", "Server.", 254, false,true,ParameterType.String );
        settings.AddValue("Server", "Http://VoorbeeldServer.nl");

        settings.AddParameter("Quality","Qualiteit. Hoog/Laag.",10,true,false,ParameterType.String);
        settings.AddValue("Quality", "quality");

        settings.AddParameter("Priority","Prioriteit, Geeft aan welke stream de standaard stream is. Het Laagste getal zal gebruikt worden.",2,true,false,ParameterType.Integer);
        settings.AddValue("Priority", 10);
       
        ExtensionManager.ImportSettings(settings);
        
	
	}


   
        

}