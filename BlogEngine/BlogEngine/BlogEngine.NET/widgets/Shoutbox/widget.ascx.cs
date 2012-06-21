namespace Widgets.Shoutbox
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            string file = HttpContext.Current.Server.MapPath(@"~/widgets/Shoutbox/shouts.xml");
            if (!File.Exists(file))
            {
                StreamWriter sw = new StreamWriter(file, true);
                sw.Write(@"<?xml version='1.0' encoding='iso-8859-1'?><shouts></shouts>");
                sw.Flush();
            }
        }
    }
}