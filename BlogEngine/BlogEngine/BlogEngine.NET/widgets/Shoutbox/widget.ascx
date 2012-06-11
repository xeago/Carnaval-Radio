<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.Shoutbox.Widget" %>
<asp:ScriptManager runat="server" />
<asp:UpdatePanel ID="shoutPanel" runat="server">
    <ContentTemplate>
        <asp:Repeater runat="server" ID="shoutList" OnItemDataBound="ShoutsItemDataBound">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblName" Style="color: gray" /><br />
                <asp:Label runat="server" ID="lblMessage" /><br />
                <br />
            </ItemTemplate>
        </asp:Repeater>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<asp:Table runat="server">
    <asp:TableRow>
        <asp:TableCell><asp:label text="Naam" runat="server" /></asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="tbName" runat="server" /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell><asp:label text="Bericht" runat="server" /></asp:TableCell>
        <asp:TableCell>
            <asp:TextBox ID="tbMessage" runat="server" /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell><asp:button ID="btnSubmit" text="Submit" runat="server" OnClick="submitMessage" /></asp:TableCell>
    </asp:TableRow>
</asp:Table>
