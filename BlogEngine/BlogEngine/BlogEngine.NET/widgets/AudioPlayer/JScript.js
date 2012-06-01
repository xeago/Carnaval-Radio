function openAudioPlayer() {
    var audio = document.getElementById("audio_with_controls");
    audio.removeAttribute("controls");
    var url = 'widgets/AudioPlayer/audioplayer.aspx';
    window.open(url, 'Audio Player', 'width=316,height=53,left=150,top=200,scrollbars=0,toolbar=no,status=1');
    return false;
}