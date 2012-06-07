namespace Widgets.AudioPlayer
{
    using System;
    using App_Code.Controls;
    using BlogEngine.Core.Web.Extensions;

    public partial class Widget : WidgetBase
    {
        public string stream;
        public string[] streamFiles;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override bool IsEditable
        {
            get { return false; }
        }

        public override string Name
        {
            get { return "AudioPlayer"; }
        }

        public override void LoadWidget()
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
}