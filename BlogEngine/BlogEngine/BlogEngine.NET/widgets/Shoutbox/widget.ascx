<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.Shoutbox.Widget" %>
<%@ Import Namespace="BlogEngine.Core" %>
<script src="<%=Utils.AbsoluteWebRoot %>widgets/Shoutbox/shoutboxJS.js" type="text/javascript"></script>
<div id="shouts" style="max-height:300px; overflow-y: scroll; overflow-x: hidden;">
</div>
<table>
    <tr>
        <td>
            Naam:
        </td>
        <td>
            <input type="text" id="tbName" />
        </td>
    </tr>
    <tr>
        <td>
            Bericht:
        </td>
        <td>
            <input type="text" id="tbMessage" />
        </td>
    </tr>
</table>
<input type="button" value="Verstuur Bericht" onclick="submitMsg();" />