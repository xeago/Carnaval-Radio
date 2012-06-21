<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="BlogEngine.Core.Web.Controls.PostViewBase" %>
<%@ Register Src="~/themes/CarnavalRadio/CommentView.ascx" TagPrefix="CR" TagName="comments" %>
<%@ Import Namespace="BlogEngine.Core" %>
<div class="post xfolkentry" id="post<%=Index %>">
    <h1><a href="<%=Post.RelativeLink %>" class="taggedlink"><%=Server.HtmlEncode(Post.Title) %></a></h1>
    <div class="text">
        <asp:PlaceHolder ID="BodyContent" runat="server" />
    </div>
</div>

