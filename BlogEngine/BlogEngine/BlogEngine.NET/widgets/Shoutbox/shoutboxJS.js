$(document).ready(loadShouts);

function loadShouts() {
    $.ajax({
        type: "GET",
        url: "widgets/Shoutbox/shouts.xml",
        dataType: "xml",
        success: function showShouts(xml) {
            $('#shouts').html("");
            $(xml).find('shout').each(function () {
                var id = $(this).attr('id');
                var name = $(this).find('name').text();
                var msg = $(this).find('message').text();
                $('<div class="shout" id="shout_' + id + '"></div>').html(/* INSERT DELETE BUTTON + HIDE */name + ': ' + msg).appendTo('#shouts');
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
            success: function () {
                alert('Message sent!');
                loadShouts();
            }
        });
    }
    else {
        alert('Enter Name + Message');
    }
}