$(document).ready(loadShouts);

function loadShouts() {
    $.ajax({
        type: "GET",
        cache: false,
        url: "../widgets/Shoutbox/shouts.xml",
        dataType: "xml",
        success: function showShouts(xml) {
            $('#shouts').html("");
            $(xml).find('shout').slice(-20).each(function () {
                var id = $(this).attr('id');
                var name = $(this).find('name').text();
                var msg = $(this).find('message').text();
                $('<div class="shout" id="shout_' + id + '"></div>').html('<input type="button" value="Delete" onclick="deleteShout(' + id + ');" /> ' + name + ': ' + msg).appendTo('#shouts');
            })
        }
    });
}

function deleteShout(id) {
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        url: '../widgets/Shoutbox/postShout.asmx/DeleteMessage',
        data: "{ id: '" + id + "' }",
        success: function (msg) {
            if (msg.d.Success) {
                loadShouts();
            }
            else {
                alert('Something went wrong :(');
            }
        }
    });
}