<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Admin.Sponsoren.Menu" %>
<ul>
    <li <%=Current("EditSponsor.aspx")%>><a href="EditSponsor.aspx" class="new"><%=Resources.labels.addNewSponsor %></a></li>
    <li <%=Current("Sponsoren.aspx")%>><a href="Sponsoren.aspx"><%=Resources.labels.sponsoren %></a></li>
</ul>