<%@ Page Language="C#" AutoEventWireup="true" CodeFile="audioplayer.aspx.cs" Inherits="widgets_AudioPlayer_audioplayer" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Carnaval Radio Audio Player</title>
    <script src="Scripts/jquery.js" type="text/javascript"></script>
    <script src="Scripts/AudioPlayer.js" type="text/javascript"></script>
</head>
<body>
    <div id="player"></div>
    <script type="text/javascript">    $('player').ready(loadStream('<%=stream %>'));</script>
    <img src="pics/audioplayer/download.png" id="dlimg" alt="Download" onclick="toggleDownload();" />
    <div id="dltab" hidden="hidden">
        <img src="pics/audioplayer/winamp.png" alt="Winamp" onclick="download('<%=streamFiles[0] %>');" />
        <img src="pics/audioplayer/wmp.png" alt="Windows Media Player" onclick="download('<%=streamFiles[1] %>');" />
    </div>
</body>
</html>