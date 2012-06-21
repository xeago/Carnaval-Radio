<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<%@ Import Namespace="BlogEngine.Core" %>
<title></title>
<div class="post xfolkentry" id="post<%=Index %>">
  <h1><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Server.HtmlEncode(Post.Title) %></a></h1>
  <span class="pubDate"><%=Post.DateCreated.ToString("d. MMMM yyyy HH:mm") %></span>
  
  <div class="text"><asp:PlaceHolder ID="BodyContent" runat="server" /></div>
  <div class="bottom">
    <%=Rating %>
    <p class="tags">Tags: <%=TagLinks(", ") %></p>
    <!--<p class="categories"><%=CategoryLinks(" | ") %></p>!-->
  </div>

  <div class="footer">    
    
    <%=AdminLinks %>
    
  </div>
</div>
