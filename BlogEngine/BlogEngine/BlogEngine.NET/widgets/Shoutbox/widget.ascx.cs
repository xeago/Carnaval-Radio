namespace Widgets.Shoutbox
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI.WebControls;
    using App_Code.Controls;
    using BlogEngine.Core;

    public partial class Widget : WidgetBase
    {
        private const string ShoutboxCacheKey = "shoutboxCache";
        List<Shout> shouts;
        int pollingInterval = 10000; //in ms

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
            shouts = new List<Shout>();
            var shoutCache = Blog.CurrentInstance.Cache[ShoutboxCacheKey];
            if (shoutCache != null)
            {
                shouts = (List<Shout>)shoutCache; //(shoutCache as List<Shout>);
                this.shoutList.DataSource = shouts;
                this.shoutList.DataBind();
            }
            else
            {
                Blog.CurrentInstance.Cache.Insert(ShoutboxCacheKey, shouts);
            }
        }

        public void submitMessage(object sender, EventArgs e)
        {
            Shout s = new Shout();
            s.Name = tbName.Text;
            s.Message = tbMessage.Text;
            shouts.Add(s);
        }

        public void ShoutsItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var name = (Label)e.Item.FindControl("lblName");
            var text = (Label)e.Item.FindControl("lblMessage");
            var shout = (Shout)e.Item.DataItem;
            name.Text = shout.Name;
            text.Text = shout.Message;
        }
    }

    internal class Shout
    {
        public string Name;
        public string Message;
    }
}