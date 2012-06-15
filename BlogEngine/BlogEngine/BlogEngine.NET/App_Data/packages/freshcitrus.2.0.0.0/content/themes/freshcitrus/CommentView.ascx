<%@ Control Language="C#" EnableViewState="False" Inherits="BlogEngine.Core.Web.Controls.CommentViewBase" %>

<div id="id_<%=Comment.Id %>" class="vcard comment<%= Post.Author.Equals(Comment.Author, StringComparison.OrdinalIgnoreCase) ? " self" : "" %>">
<p><%= Comment.Website != null ? "<a href=\"" + Comment.Website + "\" class=\"url fn\">" + Comment.Author + "</a>" : "<span class=\"fn\">" +Comment.Author + "</span>" %>
    <%= Flag %>, on <a href="#id_<%=Comment.Id %>"><%= Comment.DateCreated %></a> Said: </p> 
  <table><tr><td><p class="gravatar"><%= Gravatar(40)%></p></td><td valign="top">
  <p class="content"><%= Text %></p>
  </td></tr></table>
  <p>
    <%= AdminLinks %>
  </p>
</div>