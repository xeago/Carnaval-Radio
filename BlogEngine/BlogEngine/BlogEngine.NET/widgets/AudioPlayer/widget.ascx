<%@ Control Language="C#" AutoEventWireup="true" CodeFile="widget.ascx.cs" Inherits="Widgets.AudioPlayer.Widget" %>
<script src="widgets/AudioPlayer/JScript.js" type="text/javascript"></script>
<asp:ImageButton ID="popupButton" ImageUrl="images/popup_icon.gif" runat="server" AlternateText="Pop-up" ImageAlign="Right" OnClientClick= "return openAudioPlayer();" />
<div id="player"></div>
<script type="text/javascript">
    var audioplayer = document.getElementById('player');
    if (document.createElement('audio').canPlayType) {
        if (!document.createElement('audio').canPlayType('audio/mpeg')) {
            audioplayer.innerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="130" height="60" bgcolor="#FFFFFF"><param name="movie" value="widgets/AudioPlayer/ffmp3-tiny.swf" /><param name="flashvars" value="url=http://50.7.241.10:8021/;&lang=auto&codec=mp3&volume=100&introurl=&tracking=true&jsevents=false&buffering=5&title=CarnavalRadio" /><param name="wmode" value="window" /><param name="allowscriptaccess" value="always" /><param name="scale" value="noscale" /><embed src="widgets/AudioPlayer/ffmp3-tiny.swf" flashvars="url=http://50.7.241.10:8021/;&lang=auto&codec=mp3&volume=100&introurl=&tracking=true&jsevents=false&buffering=5&title=CarnavalRadio" width="130" scale="noscale" height="60" wmode="window" bgcolor="#FFFFFF" allowscriptaccess="always" type="application/x-shockwave-flash" /></object>';
        }
        else {
            audioplayer.innerHTML = '<audio id="audio_with_controls" controls="controls" src="http://50.7.241.10:8021/;" style="width: 200px"></audio>';
        }
    }
    else {
        audioplayer.innerHTML = '<audio id="audio_with_controls" controls="controls" src="http://50.7.241.10:8021/;" style="width: 200px"></audio>';
    }
</script>