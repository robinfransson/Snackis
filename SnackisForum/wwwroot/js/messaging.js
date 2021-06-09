$(document).ready(function () {





    let timer = setInterval(function () {
        getNewMessageCount();
        updateMessageTable();
    }, 1000);





    $('#message-table').on('click', '.open-messages', (function (e) {

        clearTimeout(chatRefreshTimer);
        currentChat = $(this).data("chat-id");
        $.get("/ajax?handler=LoadMessages&id=" + currentChat, function (result) {
            $('.chat-container').html(result);
            chatRefreshTimer = setInterval(function () {
                loadNewMessages(currentChat)
            }, 1000);
            $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 0);
            registerSendMessageForm();
        })
    })
    );




    $('#compose-button').click(function () {
        console.log('clicked')

        clearTimeout(chatRefreshTimer);
        $.get({
            url: '/ajax?handler=compose',

            success: function (result) {
                console.log(result);
                $('.chat-container').html(result);
                $('#usernamefield').on("input", checkUsername)
            },
            fail: function f() {
                console.log('clicked')
                console.log('fail');
            }
        })
    })


});



function updateMessageTable() {
    $.get({
        url: "/ajax?handler=UpdateMessageTable",
        data: { unreadMessages: getSum() },
        success: function (result) {
            if (result.length > 2) {

                $('#message-table').html(result);
            }
        }
    });
}
function getNewMessageCount() {


    $.get("/ajax?handler=NewMessageCount", function (result) {
        console.log(result.length)
            $('.message-count').text(result.messages);
    });
}


function checkUsername() {
    console.log();
    let submit = $('#sendmessage');
    if ($('#usernamefield').val().length > 0) {
        $.get({
            url: "/ajax?handler=userexists",
            data: { username: $('#usernamefield').val() },
            success: function (result) {
                console.log(result);
                if (result.exists == true) {
                    $('.userexists').removeClass('text-danger');
                    $('.userexists').addClass("text-success");
                    $('.userexists').text("check_circle");
                }
                else {

                    $('.userexists').removeClass('text-success');
                    $('.userexists').addClass("text-danger");
                    $('.userexists').text("error");
                }
                submit.prop("disabled", !result.exists)
            }
        });
    }
}

function registerSendMessageForm() {
    $('#reply-message').on('submit', function (e) {

        e.preventDefault();
        let data = ($(this).serialize());
        $.post("/ajax?handler=SendMessageFromChat", data, function (result) {
            $('textarea[name="message"]').val("");
            $('input[name="title"]').val("");

            if (scrollAuto()) {

                $('.message-container').append(result);
                $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 1000);
            }
        })
    });

}

function getSum() {
    var output = 0;
    $('#message-table').find('tr > td').each(function () {
        output += parseInt($(this).data('new-messages')); // "this" is the current element in the loop
    });
    console.log(output);
    return output;
}






function loadNewMessages(chatID) {
    $.get({
        url: '/ajax?handler=loadnewmessages',
        data: {
            chatID: chatID,
            currentMessagesShown: $(".message-container > div").length
        },
        success: function (result) {
            console.log("fired!");
            console.log(result)
            if (scrollAuto() && result.length > 2) {

                $('.message-container').append(result);
                $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 1000);


            }
        }
    })
}

function scrollAuto() {
    var element = document.getElementById('messages-container');
    console.log(element.scrollTop >= (element.scrollHeight - element.offsetHeight))
    if (element.scrollTop >= (element.scrollHeight - element.offsetHeight)) {
        return true;
    }
    return false;
}