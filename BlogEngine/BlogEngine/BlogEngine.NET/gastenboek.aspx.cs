#region Using

using System;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Web;
using BlogEngine.Core;
using BlogEngine.Core.Web.Controls;
using Guestbook;

#endregion

public partial class guestbook : BlogBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = Resources.labels.guestbook;
        string query = Request.QueryString["theme"];
        string theme = !string.IsNullOrEmpty(query) ? query : BlogSettings.Instance.Theme;
        string path = string.Concat(Utils.RelativeWebRoot, "themes/", theme, "/", "Guestbook.ascx");

        PostViewBase postView = (BlogEngine.Core.Web.Controls.PostViewBase)LoadControl(path);
        postView.Post = Post.GetPost(Guid.Parse("ab232085-71fe-47ee-866b-7a710f8f9b75"));
        postView.ID = "gastenboek";
        postView.Location = ServingLocation.SinglePost;


        commentView.Post = postView.Post;

        crGuestbook.Controls.Add(postView);
    }
}