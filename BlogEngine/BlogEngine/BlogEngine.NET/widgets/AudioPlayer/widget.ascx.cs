namespace Widgets.AudioPlayer
{
    using System;
    using App_Code.Controls;
    using BlogEngine.Core.Web.Extensions;

    public partial class Widget : WidgetBase
    {
        public string stream;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override bool IsEditable
        {
            get { return true; }
        }

        public override string Name
        {
            get { return "AudioPlayer"; }
        }

        public override void LoadWidget()
        {
            bool s = ExtensionManager.Extensions.ContainsKey("AudioStream");
            if (s)
                stream = ExtensionManager.Extensions["AudioStream"].Settings[0].GetSingleValue("HighStream");
            else
                stream = "http://50.7.241.10:8021/;";
        }
    }
}