using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BlogEngine.Core.Web.Controls;
using BlogEngine.Core;

public partial class themes_CarnavalRadio_PostView : BlogEngine.Core.Web.Controls.PostViewBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Location == BlogEngine.Core.ServingLocation.PostList)
        {
            frontpage.Controls.Add(LoadPostView("PostViewForList.ascx", true, ServingLocation.PostList));
        }
        else
            frontpage.Controls.Add(LoadPostView("PostViewFull.ascx", false, ServingLocation.SinglePost));

    }

    private PostViewBase LoadPostView(string useControlNamed, bool showExcerpt, ServingLocation serving)
    {

        string query = Request.QueryString["theme"];
        string theme = !string.IsNullOrEmpty(query) ? query : BlogSettings.Instance.Theme;
        string path = string.Concat(Utils.RelativeWebRoot, "themes/", theme, "/", useControlNamed);
        //Control.MapPath() 

        PostViewBase postView = (BlogEngine.Core.Web.Controls.PostViewBase)LoadControl(path);
        postView.ShowExcerpt = showExcerpt;// true;// BlogSettings.Instance.ShowDescriptionInPostList;
        postView.Post = Post;
        postView.ID = Post.Id.ToString().Replace("-", string.Empty);
        postView.Location = serving;

        return postView;
    }

    /// <summary>
    /// Retrieves the current page index based on the QueryString.
    /// </summary>
    private int GetPageIndex()
    {
        int index = 0;
        if (int.TryParse(Request.QueryString["page"], out index))
            index--;

        return index;
    }

    private int getCurrentPostPos()
    {
        //if (!isPostList)
        //    return -1; 

        int localCounter = 0;
        for (int i = 0; i < BlogEngine.Core.Post.Posts.Count; i++)
        {
            if (Post.Id == BlogEngine.Core.Post.Posts[i].Id)
            {
                localCounter++;
                return localCounter;
            }
            else
            {
                //if (Post.Comments[i].IsApproved || !BlogEngine.Core.BlogSettings.Instance.EnableCommentsModeration)
                //er den visible eller er bruger logget ind skal den tælles med
                if (BlogEngine.Core.Post.Posts[i].IsPublished || Page.User.Identity.IsAuthenticated)
                    localCounter++;
            }
        }
        return -1;
    }
}