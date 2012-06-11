<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.Shoutbox.Widget" %>
<script src="widgets/Shoutbox/shoutboxJS.js" type="text/javascript"></script>
<asp:ScriptManager ID="ScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="postShout.asmx" InlineScript="true"/>
        </Services>
 </asp:ScriptManager>
<div id="shouts">
</div>
<table>
    <tr>
        <td>
            Naam:
        </td>
        <td>
            <input type="text" id="tbName" />
            <%--<asp:TextBox ID="tbName" runat="server" />--%>
        </td>
    </tr>
    <tr>
        <td>
            Bericht:
        </td>
        <td>
            <input type="text" id="tbMessage" />
            <%--<asp:TextBox ID="tbMessage" runat="server" />--%>
        </td>
    </tr>
</table>
<input type="button" value="Verstuur Bericht" onclick="submitMsg();" />