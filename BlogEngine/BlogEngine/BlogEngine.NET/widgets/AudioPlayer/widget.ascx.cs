namespace Widgets.AudioPlayer
{
    using System;
    using App_Code.Controls;

    public partial class Widget : WidgetBase
    {
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
        }
    }
}