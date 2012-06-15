<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
    CodeFile="AudioStream.aspx.cs" Inherits="admin.Settings.Picasa" %>

<%@ Register Src="Menu.ascx" TagName="TabMenu" TagPrefix="menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <div class="content-box-outer">
        <div class="content-box-right">
            <menu:TabMenu ID="TabMenu" runat="server" />
        </div>
        <div class="content-box-left">
            <div class="settingsxx">
                <h1>
                    <%= Resources.labels.AudioStream %></h1>
                <h2>
                    <%=Resources.labels.AudioStreamSettingsDescription %></h2>
                <div id="formContainer" runat="server" class="mgr">
                    <div id="spamExtension" style="width: 800px; padding: 5px; margin-bottom: 5px;">
                        <table>
                            <tr>
                                <td>
                                    <label for="TxbHighStream">
                                        <%=Resources.labels.HighAudioStreamServer%></label>
                                </td>
                                <td>
                                    <asp:TextBox Width="400" ID="TxbHighStream" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label for="TxbLowStream">
                                        <%=Resources.labels.LowAudioStreamServer%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxbLowStream" Width="400" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-left: 0">
                        <asp:Button ID="btnSave" class="btn primary" runat="server" OnClick="BtnSaveClick" />
                        <%=Resources.labels.or %>
                        <a href="~/admin/Extension Manager/default.aspx" runat="server">
                            <%=Resources.labels.cancel %></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
