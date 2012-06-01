<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" 
ValidateRequest="false" CodeFile="EditSponsor.aspx.cs" Inherits="Admin.Sponsoren.EditSponsor" %>
<%@ Register Src="~/User controls/simpleImageUploadUserControl/SimpleImageUpload.ascx" TagName="SimpleImageUpload" TagPrefix="ccPiczardUC" %>
<%@ Register Src="~/User controls/NControls/NDateBox.ascx" TagName="DateBox" TagPrefix="cr" %>

<%@ Register Src="~/admin/htmlEditor.ascx" TagPrefix="Blog" TagName="TextEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
<script type="text/javascript">
    function ShowHideCheckbox() {
        var isChecked = $("#<%= cbShowInMobileSwitch.ClientID %>").is(':checked');
        if (isChecked == true) {
            $("#uidMobileFrequency").show();
        }
        else {
            $("#uidMobileFrequency").hide();
        }
    }

    $(document).ready(function () {
        ShowHideCheckbox();

        $("#<%= cbShowInMobileSwitch.ClientID %>").click(function () {
            ShowHideCheckbox();
        });
    });
</script>
    <div class="content-box-outer">
        <div class="content-box-full">
            <h1><%=Resources.labels.editSponsor %></h1>
            <table class="tblForm largeForm" style="width:100%; margin:0;">
                <tr>
                    <td style="vertical-align:top; padding:0 40px 0 0;">
                        <ul class="fl">
                            <li>
                                <label class="lbl" for="<%=txtName.ClientID %>">
                                    <%=Resources.labels.name %></label>
                                <asp:TextBox runat="server" ID="txtName" Width="600" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" Display="Dynamic"
                                    ErrorMessage="<%$Resources:labels,enterName %>" />
                            </li>
                            <li>
                                <label class="lbl" for="<%=txtUrl.ClientID %>"><%=Resources.labels.website %></label>
                                <asp:TextBox runat="server" ID="txtUrl" Width="600" />
                            </li>
                            <li>
                                <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <div class="pageContainer">
                                            <label class="lbl"><%=Resources.labels.sponsorlogo %></label>
                                            <ccpiczarduc:simpleimageupload id="imgLogo" runat="server" width="380px" autoopenimageeditpopupafterupload="true" culture="nl" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </li>
                            <li>
                                <label class="lbl" for="<%=txtDescription.ClientID %>"><%=Resources.labels.description %></label>
                                <asp:TextBox runat="server" ID="txtDescription" Width="600" TextMode="multiLine" Columns="50" Rows="4" />
                            </li>
                            <li>
                                <label class="lbl" for="<%=txtDescription.ClientID %>"><%=Resources.labels.endDate %></label>
                                <cr:DateBox runat="server" ID="dtEndDate"  />
                            </li>
                            <li>
                                <asp:CheckBox runat="Server" ID="cbActive" Checked="true" Text="<%$ Resources:labels, Active %>" />
                            </li>
                        </ul>
                    </td>
                    <td class="secondaryForm" style="padding:0; vertical-align:top;">
                        <ul class="fl">
                            <li>
                                <label class="lbl" for="<%=ddlSponsorType.ClientID %>"><%=Resources.labels.sponsorType %></label>
                                <asp:DropDownList runat="server" ID="ddlSponsorType" Width="250" />
                            </li>
                            <li>
                                <label class="lbl"><%=Resources.labels.VisibilityOptions %></label>
                            </li>
                            <li>
                                <label class="lbl" style="font-style: italic; font-size: Small;">
                                    <%=Resources.labels.Player %></label>
                                <asp:CheckBox runat="Server" ID="cbShowInPlayerSwitch" Text="<%$ Resources:labels, Switch %>" />
                                <asp:CheckBox runat="Server" ID="cbShowInPlayerSolid" Text="<%$ Resources:labels, Solid %>" />
                            </li>
                            <li>
                                <label class="lbl" style="font-style: italic; font-size: Small;"><%=Resources.labels.MobileApp %></label>
                                <asp:CheckBox runat="Server" ID="cbShowInMobileSwitch" Text="<%$ Resources:labels, Switch %>" />
                                <asp:CheckBox runat="Server" ID="cbShowInMobileSolid" Text="<%$ Resources:labels, Solid %>" />
                                <div id="uidMobileFrequency" class="clMobileFrequency" style="margin-top: 10px; display: none;">
                                    <label style="margin-right:13px;" for="<%=ddlMobileFrequency.ClientID %>"><%=Resources.labels.Frequency %></label>
                                    <asp:DropDownList runat="server" ID="ddlMobileFrequency" Width="170" />
                                </div>
                            </li>
                            <li>
                                <label class="lbl" style="font-style: italic; font-size: Small;"><%=Resources.labels.widget %></label>
                                <asp:CheckBox runat="Server" ID="cbShowInWidget" Text="<%$ Resources:labels, Switch %>" />
                            </li>

                        </ul>
                    </td>
                </tr>
            </table>

            <div class="action_buttons">
                <asp:Button runat="server" ID="btnSave" CssClass="btn primary" OnClick="BtnSaveClick" Text="<%$ Resources:labels, saveSponsor %>" />
                <%=Resources.labels.or %> 
                <a href="Sponsoren.aspx" title="Cancel"><%=Resources.labels.cancel %></a>
            </div>
        </div>
    </div>
</asp:Content>