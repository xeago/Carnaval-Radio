<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="SocialMedia.aspx.cs" Inherits="SocialMedia" %>

<%@ Register Src="Menu.ascx" TagName="TabMenu" TagPrefix="menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <div class="content-box-outer">
        <div class="content-box-right">
            <menu:TabMenu ID="TabMenu" runat="server" />
        </div>
        <div class="content-box-left">
           <iframe src="http://www.twitterfeed.com" width="100%" height="100%"></iframe>
        </div>
    </div>
</asp:Content>
