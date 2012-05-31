<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="Sponsoren.aspx.cs" ValidateRequest="false" Inherits="Admin.Sponsoren.SponsorenPage"
    Title="Sponsor toevoegen" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tableToolBox a").click(function () {
                $(".tableToolBox a").removeClass("current");
                $(this).addClass("current");
            });
        });
    </script>
    <div class="content-box-outer">
		<div class="content-box-right">
			<menu:TabMenu ID="TabMenu" runat="server" />
		</div>
		<div class="content-box-left">
            <h1><%=Resources.labels.sponsoren %></h1>
            <div class="tableToolBox">
               
            </div>
            <div id="Container"></div>
        </div>
    </div>
</asp:Content>
