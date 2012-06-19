<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<%@ Import Namespace="BlogEngine.Core" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Globalization" %>
<title>{#advanced_dlg.about_title}</title>
<script runat="server">

    public string GetShortenedContent(string content, int pos, string link)
    {
        if (!string.IsNullOrEmpty(Post.Description))
            return Post.Description + "... <a href=\"" + link + "\">Lees meer..</a>";
        string sStripped = Utils.StripHtml(content);
        if (sStripped.Length <= pos) return sStripped;
        else
            return sStripped.Substring(0, pos) + "... <a href=\"" + link + "\">Lees meer..</a>";
    }


    public string getImage(bool ShowExcerpt, string input)
    {
        if (!ShowExcerpt || input == null)
            return "";

        string pain = input;
        string pattern = @"<img(.|\n)+?>";

        Match m = Regex.Match(input, pattern,
                              RegexOptions.IgnoreCase | RegexOptions.Multiline);

        if (m.Success)
        {
            string src = getSrc(m.Value);
            return string.Format("<img class=\"left\" width=\"275\" height=\"155\" {0}  />", src);
        } 
        string path = string.Format("{0}themes/{1}/img/logo.png", Utils.AbsoluteWebRoot, BlogSettings.Instance.GetThemeWithAdjustments(null));
        return string.Format("<img class=\"left\" width=\"275\" height=\"155\" src=\"{0}\"  />", path);
    }


    private string getSrc(string input)
    {
        string pattern = "src=[\'|\"](.+?)[\'|\"]";

        var reImg = new Regex(pattern,
                              RegexOptions.IgnoreCase | RegexOptions.Multiline);

        Match mImg = reImg.Match(input);

        if (mImg.Success)
        {
            return mImg.Value;
        }

        return "";
    }
    
    protected override void OnLoad(EventArgs e)
    {
        var bodyContent = (PlaceHolder) this.FindControl("BodyContent");
        var post = this.Post;
        var body = this.Body;
        if (this.ShowExcerpt)
        {
            string path = string.Format("{0}themes/{1}/img/leesverder.png", Utils.AbsoluteWebRoot, BlogSettings.Instance.GetThemeWithAdjustments(null));
            
            var link = string.Format(" <a class=\"read-more\" href=\"{0}\"><img src=\"{2}\" alt=\"{1}\" title=\"{1}\" /></a>", post.RelativeLink, Utils.Translate("more"), path);

            if (!string.IsNullOrEmpty(post.Description))
            {
                body = "<p>" + post.Description.Replace(Environment.NewLine, "<br />") + "</p>" + link;
            }
            else
            {
                body = Utils.StripHtml(body);
                if (body.Length > 40)
                {
                    body = string.Format("<p>{0}...</p>{1}", body.Trim().Substring(0, 250), link);
                }
            }
        }

        if (bodyContent != null)
        {
            Utils.InjectUserControls(bodyContent, body);
        }
    }

    public override string AdminLinks
    {
        get
        {
            if (Security.IsAuthenticated)
            {

                var postRelativeLink = this.Post.RelativeLink;
                var sb = new StringBuilder();
                sb.Append("<div class=\"adminLinks\">");
                if (this.Post.CanUserEdit)
                {
                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "<a href=\"{0}\">{1}</a> | ",
                        Utils.AbsoluteWebRoot + "admin/Posts/Add_entry.aspx?id=" + this.Post.Id,
                        Utils.Translate("edit"));
                }

                if (this.Post.CanUserDelete)
                {
                    var confirmDelete = string.Format(
                        CultureInfo.InvariantCulture,
                        Utils.Translate("areYouSure"),
                        Utils.Translate("delete").ToLowerInvariant(),
                        Utils.Translate("thePost"));

                    sb.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "<a href=\"#\" onclick=\"if (confirm('{2}')) location.href='{0}?deletepost={1}'\">{3}</a>",
                        postRelativeLink,
                        this.Post.Id,
                        confirmDelete,
                        Utils.Translate("delete"));
                }
                sb.Append("</div>");
                return sb.ToString();
            }
            return string.Empty;
        }
    }

</script>
<div class="xfolkentry postForList" id="post<%=Index%>">
  <div class="title"><h1><a href="<%=Post.RelativeLink%>" class="taggedlink"><%=Server.HtmlEncode(Post.Title)%></a></h1>
  </div>
    
  <div class="image"><%=getImage(true, Post.Content)%></div>
  <div class="text">
      <asp:PlaceHolder ID="BodyContent" runat="server" />
      <%=AdminLinks%>
  </div>

<asp:Panel ID="DontShowForNow" runat="server" Visible="false">
  <div class="footer">
      <span class="pubDate">
          <%=Post.DateCreated.ToString("d. MMMM yyyy HH:mm")%></span>    
    <div class="bookmarks">
        <!-- Share on Twitter and Facebook !-->
    </div>
    
    <%=AdminLinks%>
    

    <%
        if (BlogSettings.Instance.ModerationType == BlogSettings.Moderation.Disqus)
        {%>
    <a rel="nofollow" href="<%=Post.PermaLink%>#disqus_thread"><%=labels.comments%></a>
    <%
        }
        else
        {%>
    <a rel="bookmark" href="<%=Post.PermaLink%>" title="<%=Server.HtmlEncode(Post.Title)%>">Permalink</a>
    <a rel="nofollow" href="<%=Post.RelativeLink%>#comment"><%=labels.comments%> (<%=Post.ApprovedComments.Count%>)</a>   
    <%
        }%>
        <span class="author">by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" +
                              Utils.RemoveIllegalCharacters(Post.Author)%>.aspx">
            <%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName : Post.Author%></a></span>
        <div class="bottom">
            <%=Rating%>
            <p class="tags">Tags:
                <%=TagLinks(", ")%></p>
            <p class="categories">
                <%=CategoryLinks(" | ")%></p>
        </div>

 </div>
    </asp:Panel>
</div>