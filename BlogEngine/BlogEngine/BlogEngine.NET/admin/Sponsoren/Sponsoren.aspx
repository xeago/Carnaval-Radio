﻿<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
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
                <asp:GridView runat="server" OnRowDataBound="GridRowDataBound" ID="grid" RowStyle-BorderWidth="0" RowStyle-BorderStyle="None" GridLines="None"
                    Width="100%" AlternatingRowStyle-BackColor="#f8f8f8" AlternatingRowStyle-BorderColor="#f8f8f8"
                    HeaderStyle-BackColor="#F1F1F1" UseAccessibleHeader="true" AutoGenerateColumns="false"
                    CssClass="beTable rounded">
                    <Columns>
                        <asp:BoundField DataField="Active" HeaderText="<%$ Resources:labels, Active %>" />
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:labels,name %>" />
                        <asp:BoundField DataField="EndDate" HeaderText="<%$ Resources:labels,EndDate %>" />
                        <asp:TemplateField ItemStyle-Width="65">
                            <ItemTemplate>
                                <ul class="rowTools">
                                    <li><a class="toolsAction" href="#"><span class="">Tools</span></a>
                                        <ul class="rowToolsMenu">
                                            <li><a class="editAction" href="EditSponsor.aspx?id=<%# Eval("ID") %>">Wijzigen</a></li>
                                            <li><a href="#" class="deleteAction" onclick="return DeletePage(this);">Verwijderen</a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>
            <div id="Container"></div>
        </div>
    </div>
</asp:Content>
