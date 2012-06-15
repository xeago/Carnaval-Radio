using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlogEngine.Core.Web.Extensions;

public partial class widgets_AudioPlayer_audioplayer : System.Web.UI.Page
{
    public string stream;
    public string[] streamFiles;

    protected void Page_Load(object sender, EventArgs e)
    {
        streamFiles = new string[2];
        bool s = ExtensionManager.Extensions.ContainsKey("AudioStream");
        if (s)
        {
            stream = ExtensionManager.Extensions["AudioStream"].Settings[0].GetSingleValue("HighStream");
            streamFiles[0] = ExtensionManager.Extensions["AudioStream"].Settings[0].GetSingleValue("PlsFile");
            streamFiles[1] = ExtensionManager.Extensions["AudioStream"].Settings[0].GetSingleValue("AsmxFile");
        }
        else
            stream = "http://50.7.241.10:8021/;";
    }
}