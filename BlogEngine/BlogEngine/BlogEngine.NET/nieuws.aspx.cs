#region Using

using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Web;
using System.Linq;
using System.Web.UI.HtmlControls;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using Guestbook;

#endregion

public partial class nieuws : BlogBasePage
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        string query = Request.QueryString["theme"];
        string theme = !string.IsNullOrEmpty(query) ? query : BlogSettings.Instance.Theme;
        this.MasterPageFile = string.Concat(Utils.RelativeWebRoot, "themes/", theme, "/", "CR.master");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = Resources.labels.guestbook;
        string query = Request.QueryString["theme"];
        string theme = !string.IsNullOrEmpty(query) ? query : BlogSettings.Instance.Theme;
        string path = string.Concat(Utils.RelativeWebRoot, "themes/", theme, "/", "PostViewForListLast.ascx");

        var l = Post.Posts;
        foreach (var p in l)
        {
            PostViewBase postView = (PostViewBase)LoadControl(path);
            postView.Post = p;
            postView.ShowExcerpt = true;
            postView.Location = ServingLocation.PostList;
            postView.ID = p.Id.ToString().Replace("-", string.Empty);


            if(p != l.First())
            {
                var hgc = new HtmlGenericControl("div");
                hgc.Attributes.Add("class", "shadow-post");
                crNieuws.Controls.Add(hgc);
            }
            crNieuws.Controls.Add(postView);
        }
    }
}