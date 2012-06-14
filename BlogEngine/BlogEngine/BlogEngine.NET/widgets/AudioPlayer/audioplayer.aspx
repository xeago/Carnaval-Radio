<%@ Page Language="C#" AutoEventWireup="true" CodeFile="audioplayer.aspx.cs" Inherits="widgets_AudioPlayer_audioplayer" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Carnaval Radio Audio Player</title>
    <script src="JScript.js" type="text/javascript"></script>
</head>
<body>
    <div id="player"></div>
    <script type="text/javascript">    $('player').ready(loadStream('<%=stream %>'));</script>
    <img src="images/download.png" id="dlimg" alt="Download" onclick="toggleDownload();" />
    <div id="dltab" hidden="hidden">
        <img src="images/winamp.png" alt="Winamp" onclick="download('<%=streamFiles[0] %>');" />
        <img src="images/wmp.png" alt="Windows Media Player" onclick="download('<%=streamFiles[1] %>');" />
    </div>
</body>
</html>