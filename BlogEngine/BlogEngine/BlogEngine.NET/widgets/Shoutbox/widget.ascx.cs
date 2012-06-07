namespace Widgets.Shoutbox
{
    using System;
    using System.Web;
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

        public void ShoutsItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var text = (Label)e.Item.FindControl("lblName");
            var date = (Label)e.Item.FindControl("lblMessage");
            var shout = (Shout)e.Item.DataItem;
            text.Text = shout.Name;
            date.Text = shout.Message;
        }
    }

    internal class Shout
    {
        public string Name;
        public string Message;
    }
}