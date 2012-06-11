namespace Widgets.Shoutbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Web.UI.WebControls;
    using App_Code.Controls;
    using BlogEngine.Core;

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
            get { return "Shoutbox"; }
        }

        public override void LoadWidget()
        {
        }
    }
}