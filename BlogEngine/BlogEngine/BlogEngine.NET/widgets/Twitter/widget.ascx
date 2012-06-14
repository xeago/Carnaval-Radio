<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.Twitter.Widget" %>
<div style="max-height:300px; overflow-y: scroll; overflow-x: hidden;">
<asp:Repeater runat="server" ID="repItems" OnItemDataBound="RepItemsItemDataBound">
  <ItemTemplate>
    <asp:image runat="server" ID="twtImg" />
    <asp:Label runat="server" ID="lblDate" style="color:gray" /><br />
    <asp:Label runat="server" ID="lblItem" /><br /><br />
  </ItemTemplate>
</asp:Repeater>
</div>
<asp:HyperLink runat="server" ID="hlTwitterAccount" Text="Follow me on Twitter" />
