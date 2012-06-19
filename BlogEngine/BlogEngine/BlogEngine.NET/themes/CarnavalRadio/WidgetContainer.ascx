<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Code.Controls.WidgetContainer" %>
<div class="widget <%= Widget.Name.Replace(" ", String.Empty).ToLowerInvariant() %>"
    id="widget<%= Widget.WidgetId %>">
    <%= AdminLinks %>
    <% if (this.Widget.ShowTitle)
       { %>
    <div class="title"><h4>
        <%= Widget.Title%></h4>
    <% } %>
    </div>
    <div class="content">
        <asp:PlaceHolder ID="phWidgetBody" runat="server"></asp:PlaceHolder>
    </div>
    <div class="footer">
    </div>
    <div class="bol"></div>
</div>
