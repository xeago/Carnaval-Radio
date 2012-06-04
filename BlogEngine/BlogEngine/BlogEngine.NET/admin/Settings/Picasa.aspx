<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Picasa.aspx.cs" Inherits="admin.Settings.Picasa" %>
<%@ Register src="Menu.ascx" tagname="TabMenu" tagprefix="menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server"> 
    
<div class="content-box-outer">
    <div class="content-box-right">
        <menu:TabMenu ID="TabMenu" runat="server" />
    </div>
    <div class="content-box-left">
<div class="settingsxx">
    <h1><%= Resources.labels.PhotoAlbums %></h1>
    <div id="formContainer" runat="server" class="mgr">
        <div class="info" style="float: right; width: 350px;">
            <%= Resources.labels.CopyTagMessage %>
        </div>
        
        <div id="spamExtension" style="width:800px; padding:5px; margin-bottom:5px;">
            <table style="padding:0;margin:0">
                <tr>
                    <td width="200">Google Account</td>
                    <td><asp:TextBox ID="txtGMail" runat="server" MaxLength="50"></asp:TextBox></td>
                    <td>@Gmail.com</td>
                </tr>
                <tr>
                    <td><span><%= Resources.labels.password %></span> &nbsp;&nbsp;</td>
                    <td colspan="2"><asp:TextBox ID="txtPwd" TextMode="Password" runat="server" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Slide show <%= Resources.labels.Width %></td>
                    <td colspan="2"><asp:TextBox ID="txtShowWidth" runat="server" MaxLength="20"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Thumbnail <%= Resources.labels.Width %></td>
                    <td colspan="2"><asp:DropDownList ID="ddWidth" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="padding:0 5px 0 0">Auto <%= Resources.labels.Play %></td>
                    <td colspan="2"><asp:CheckBox ID="chkAuto" runat="server" BorderStyle="none" /></td>
                </tr>
            </table>        
        </div>
            
        <div style="margin-left:0">
            <asp:Button ID="btnSave" class="btn primary" runat="server" onclick="BtnSaveClick" />
        </div>
    </div>
</div>

<div runat="server" id="list_comments" class="settings">
  <div class="mgr">  
    <h2><%= Resources.labels.Your %> <%= Resources.labels.PhotoAlbums %></h2>

    <asp:Literal ID="litContent" runat="server"></asp:Literal>
  </div>
</div>

    </div>
</div>

</asp:Content>