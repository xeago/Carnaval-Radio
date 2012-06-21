<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gastenboek.aspx.cs" Inherits="guestbook" ValidateRequest="false" %>
<%@ Register Src="~/User controls/GuestbookView.ascx" TagPrefix="CR" TagName="comments" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
<asp:Panel runat="server" id="crGuestbook" />
<div id="GuestbookView">
<CR:comments runat="server" ID="commentView" />
</div>
</asp:Content>