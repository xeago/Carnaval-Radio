$(document).ready(loadShouts);

(function poll() {
   setInterval(loadShouts, 60000);
})();

function loadShouts() {
    $.ajax({
        type: "GET",
        cache: false,
        url: "widgets/Shoutbox/shouts.xml",
        dataType: "xml",
        success: function showShouts(xml) {
            $('#shouts').html("");
            $(xml).find('shout').slice(-20).each(function () {
                var id = $(this).attr('id');
                var name = $(this).find('name').text();
                var msg = $(this).find('message').text();
                $('<div class="shout" id="shout_' + id + '"></div>').html(name + ': ' + msg).appendTo('#shouts');
            })
        }
    });
}

function submitMsg() {
    if ($('#tbName').val() && $('#tbMessage').val()) {
        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            url: 'widgets/Shoutbox/postShout.asmx/SubmitMessage',
            data: "{ name: '" + $('#tbName').val() + "', message: '" + $('#tbMessage').val() + "' }",
            success: function (msg) {
                if (msg.d.Success) {
                    $('#tbName').val('');
                    $('#tbMessage').val('');
                    loadShouts();
                }
                else {
                    alert('Something went wrong :(');
                }
            }
        });
    }
    else {
        alert('Enter Name + Message');
    }
}