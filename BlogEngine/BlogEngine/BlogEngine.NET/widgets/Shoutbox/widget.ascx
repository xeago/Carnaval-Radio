<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.Shoutbox.Widget" %>
<asp:Repeater runat="server" ID="shouts" OnItemDataBound="ShoutsItemDataBound">
  <ItemTemplate>
    <asp:Label runat="server" ID="lblName" style="color:gray" /><br />
    <asp:Label runat="server" ID="lblMessage" /><br /><br />
  </ItemTemplate>
</asp:Repeater>
<asp:Table runat="server">
    <asp:TableRow>
        <asp:TableCell><asp:label text="Naam" runat="server" /></asp:TableCell>
        <asp:TableCell><asp:textbox ID="tbName" runat="server" /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell><asp:label text="Bericht" runat="server" /></asp:TableCell>
        <asp:TableCell><asp:textbox ID="tbMessage" runat="server" /></asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell><asp:button text="Submit" runat="server" /></asp:TableCell>
    </asp:TableRow>
</asp:Table>