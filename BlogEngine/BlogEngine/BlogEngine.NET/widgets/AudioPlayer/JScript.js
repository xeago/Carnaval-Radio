var FlashDetect = new function () {
    var self = this; self.installed = false; self.raw = ""; self.major = -1; self.minor = -1; self.revision = -1; self.revisionStr = ""; var activeXDetectRules = [{ "name": "ShockwaveFlash.ShockwaveFlash.7", "version": function (obj) { return getActiveXVersion(obj); } }, { "name": "ShockwaveFlash.ShockwaveFlash.6", "version": function (obj) {
        var version = "6,0,21"; try { obj.AllowScriptAccess = "always"; version = getActiveXVersion(obj); } catch (err) { }
        return version;
    }
    }, { "name": "ShockwaveFlash.ShockwaveFlash", "version": function (obj) { return getActiveXVersion(obj); } }]; var getActiveXVersion = function (activeXObj) {
        var version = -1; try { version = activeXObj.GetVariable("$version"); } catch (err) { }
        return version;
    }; var getActiveXObject = function (name) {
        var obj = -1; try { obj = new ActiveXObject(name); } catch (err) { obj = { activeXError: true }; }
        return obj;
    }; var parseActiveXVersion = function (str) { var versionArray = str.split(","); return { "raw": str, "major": parseInt(versionArray[0].split(" ")[1], 10), "minor": parseInt(versionArray[1], 10), "revision": parseInt(versionArray[2], 10), "revisionStr": versionArray[2] }; }; var parseStandardVersion = function (str) { var descParts = str.split(/ +/); var majorMinor = descParts[2].split(/\./); var revisionStr = descParts[3]; return { "raw": str, "major": parseInt(majorMinor[0], 10), "minor": parseInt(majorMinor[1], 10), "revisionStr": revisionStr, "revision": parseRevisionStrToInt(revisionStr) }; }; var parseRevisionStrToInt = function (str) { return parseInt(str.replace(/[a-zA-Z]/g, ""), 10) || self.revision; }; self.majorAtLeast = function (version) { return self.major >= version; }; self.minorAtLeast = function (version) { return self.minor >= version; }; self.revisionAtLeast = function (version) { return self.revision >= version; }; self.versionAtLeast = function (major) { var properties = [self.major, self.minor, self.revision]; var len = Math.min(properties.length, arguments.length); for (i = 0; i < len; i++) { if (properties[i] >= arguments[i]) { if (i + 1 < len && properties[i] == arguments[i]) { continue; } else { return true; } } else { return false; } } }; self.FlashDetect = function () { if (navigator.plugins && navigator.plugins.length > 0) { var type = 'application/x-shockwave-flash'; var mimeTypes = navigator.mimeTypes; if (mimeTypes && mimeTypes[type] && mimeTypes[type].enabledPlugin && mimeTypes[type].enabledPlugin.description) { var version = mimeTypes[type].enabledPlugin.description; var versionObj = parseStandardVersion(version); self.raw = versionObj.raw; self.major = versionObj.major; self.minor = versionObj.minor; self.revisionStr = versionObj.revisionStr; self.revision = versionObj.revision; self.installed = true; } } else if (navigator.appVersion.indexOf("Mac") == -1 && window.execScript) { var version = -1; for (var i = 0; i < activeXDetectRules.length && version == -1; i++) { var obj = getActiveXObject(activeXDetectRules[i].name); if (!obj.activeXError) { self.installed = true; version = activeXDetectRules[i].version(obj); if (version != -1) { var versionObj = parseActiveXVersion(version); self.raw = versionObj.raw; self.major = versionObj.major; self.minor = versionObj.minor; self.revision = versionObj.revision; self.revisionStr = versionObj.revisionStr; } } } } } ();
};

var wmpDetect = new function() {
    result = false;
    document.write('<SCRIPT LANGUAGE=VBScript>\n on error resume next \n result = IsObject(CreateObject("MediaPlayer.MediaPlayer.1"))</SCRIPT>\n');
    if (result) return true; else return false;
};

function openAudioPlayer() {
    var audio = document.getElementById("audio_with_controls");
    audio.removeAttribute("controls");
    var url = 'widgets/AudioPlayer/audioplayer.aspx';
    window.open(url, 'Audio Player', 'width=316,height=53,left=150,top=200,scrollbars=0,toolbar=no,status=1');
    return false;
}

function loadStream(stream) {
    var audioplayer = document.getElementById('player');
    if (document.createElement('audio').canPlayType) {
        if (!document.createElement('audio').canPlayType('audio/mpeg')) {
            mediaCheck(audioplayer, stream);
        }
        else {
            audioplayer.innerHTML = '<audio controls="controls" src="' + stream + '" style="width: 200px"></audio>';
        }
    }
    else {
        mediaCheck(audioplayer, stream);
    }
}

function mediaCheck(audioplayer, stream) {
    if (FlashDetect.versionAtLeast(10)) {
        audioplayer.innerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="130" height="60" bgcolor="#FFFFFF"><param name="movie" value="widgets/AudioPlayer/ffmp3-tiny.swf" /><param name="flashvars" value="url=' + stream + '&lang=auto&codec=mp3&volume=100&introurl=&tracking=true&jsevents=false&buffering=5&title=CarnavalRadio" /><param name="wmode" value="window" /><param name="allowscriptaccess" value="always" /><param name="scale" value="noscale" /><embed src="widgets/AudioPlayer/ffmp3-tiny.swf" flashvars="url=' + stream + '&lang=auto&codec=mp3&volume=100&introurl=&tracking=true&jsevents=false&buffering=5&title=CarnavalRadio" width="130" scale="noscale" height="60" wmode="window" bgcolor="#FFFFFF" allowscriptaccess="always" type="application/x-shockwave-flash" /></object>';
    }
    else if (wmpDetect) {
        audioplayer.innerHTML = '<object id="mediaplayer" classid="6BF52A52-394A-11d3-B153-00C04F79FAA6" codebase="http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#version=5,1,52,701" standby="loading microsoft windows media player components..." type="application/x-oleobject" width="200" height="40"><param name="filename" value="App_Data/hi.asx"><param name="animationatstart" value="true"><param name="transparentatstart" value="true"><param name="autostart" value="false"><param name="showcontrols" value="true"><param name="ShowStatusBar" value="true"><param name="windowlessvideo" value="true"><embed src="App_Data/hi.asx" autostart="false" showcontrols="true" showstatusbar="1" bgcolor="white" width="200" height="40"></object>';
    }
    else {
        audioplayer.innerHTML = '<a href="http://get.adobe.com/flashplayer"><img src="http://www.adobe.com/images/shared/download_buttons/get_adobe_flash_player.png" alt="Get Flash Player" /></a>';
    }
}

function toggleDownload() {
    $('#dltab').toggle();
    if ($('#dltab').is(':visible')) {
        $('#dlimg').css({ '-moz-transform': 'rotate(180deg)' });
        $('#dlimg').css({ WebkitTransform: 'rotate(180deg)' });
        $('#dlimg').css({ '-o-transform': 'rotate(180deg)' });
        $('#dlimg').css({ '-ms-transform': 'rotate(180deg)' });
        $('#dlimg').css({ 'filter:progid': 'DXImageTransform.Microsoft.BasicImage(rotation=1)' });
    }
    else {
        $('#dlimg').css({ '-moz-transform': 'rotate(0deg)' });
        $('#dlimg').css({ WebkitTransform: 'rotate(0deg)' });
        $('#dlimg').css({ '-o-transform': 'rotate(0deg)' });
        $('#dlimg').css({ '-ms-transform': 'rotate(0deg)' });
        $('#dlimg').css({ 'filter:progid': 'DXImageTransform.Microsoft.BasicImage(rotation=0)' });
    }
}

function download(file) {
    if (file != null) {
        window.open(file, 'Download');
    }
}