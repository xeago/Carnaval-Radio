<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<title>{#advanced_dlg.about_title}</title>
<script runat="server">
    public string GetShortenedContent(string content, int pos, string link)
    {
        if (Post.Description != null && Post.Description.Length > 0) return Post.Description + "... <a href=\"" + link + "\">[More]</a>";
        string sStripped = BlogEngine.Core.Utils.StripHtml(content);
        if (sStripped.Length <= pos) return sStripped;
        else
            return sStripped.Substring(0, pos) + "... <a href=\"" + link + "\">[More]</a>";
    }

    
    public string getImage(bool ShowExcerpt, string input)
    {

        if (!ShowExcerpt || input == null)
            return "";

        string pain = input;
        string pattern = @"<img(.|\n)+?>";

        System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(input, pattern,

        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);

        if (m.Success)
        {
            string src = getSrc(m.Value);
            string img = string.Format("<img class='left' width=\"150\" {0}  />", src);
            return img;
        }
        else
        {
            return "";
        }
    }



    string getSrc(string input)
    {
        string pattern = "src=[\'|\"](.+?)[\'|\"]";

        System.Text.RegularExpressions.Regex reImg = new System.Text.RegularExpressions.Regex(pattern,
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);

        System.Text.RegularExpressions.Match mImg = reImg.Match(input);

        if (mImg.Success)
        {
            return mImg.Value;
        }

        return "";
    }
    
</script>
<div class="post xfolkentry" id="post<%=Index %>">
  <h1><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Server.HtmlEncode(Post.Title) %></a></h1>
  <span class="author">by <a href="<%=VirtualPathUtility.ToAbsolute("~/") + "author/" + BlogEngine.Core.Utils.RemoveIllegalCharacters(Post.Author) %>.aspx"><%=Post.AuthorProfile != null ? Post.AuthorProfile.DisplayName : Post.Author %></a></span>
  <span class="pubDate"><%=Post.DateCreated.ToString("d. MMMM yyyy HH:mm") %></span>
  
  <div class="textList"><%= getImage(true, Post.Content)%><asp:PlaceHolder ID="BodyContent" runat="server" /></div>
  <div class="bottom">
    <%=Rating %>
    <p class="tags">Tags: <%=TagLinks(", ") %></p>
    <p class="categories"><%=CategoryLinks(" | ") %></p>
  </div>

  <div class="footer">    
    <div class="bookmarks">
        <!-- Share on Twitter and Facebook !-->
    </div>
    
    <%=AdminLinks %>
    
    <asp:Panel ID="DontShowForNow" runat="server" Visible="false">
    <% if (BlogEngine.Core.BlogSettings.Instance.ModerationType == BlogEngine.Core.BlogSettings.Moderation.Disqus)
        { %>
    <a rel="nofollow" href="<%=Post.PermaLink %>#disqus_thread"><%=Resources.labels.comments %></a>
    <%}
        else
        { %>
    <a rel="bookmark" href="<%=Post.PermaLink %>" title="<%=Server.HtmlEncode(Post.Title) %>">Permalink</a>
    <a rel="nofollow" href="<%=Post.RelativeLink %>#comment"><%=Resources.labels.comments %> (<%=Post.ApprovedComments.Count %>)</a>   
    <%} %>
    </asp:Panel>
    </div>
</div>