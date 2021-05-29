
//let url = "/?Handler=LoadMessages";
let timesRan = 0;
//let myTimer = setInterval(chatLoad, 5000);
let autoScroll = true;

var token = $('input[name="__RequestVerificationToken"]').val();
$(document).ready(function () {

    let path = window.location.pathname.toLowerCase();
    if (path.split("/").includes("thread")) {
        let threadID = path.split('/').reverse()[0];
        console.log(threadID);

        debugger;
        $('.jstree').jstree({
            'core': {
                'data': {
                    'url': path + "?handler=replies?id=" + threadID,
                    'data': function (node) {

                        debugger;
                        console.log(node);
                        return {
                            'id': node.id,
                            'parent': node.parent,
                            'text': node.text
                        };
                    }
                }
            }
        });
    }





    $('.messages').hover(function () {
        console.log("hovered over messages");
        $('.messages-icon').text("drafts");
    });










    $('.messages').mouseleave(function () {
        console.log("mouse left messages");
        $('.messages-icon').text("markunread");
    });









    $('.login-form').on('submit', function (e) {
        e.preventDefault();
        let data = ($(this).serialize());
        console.log(data);
        let dataw = JSON.stringify(data);
        console.log(dataw);
        $.post("/Account?handler=login", data, function (result) {
            console.log(result);
            if (result.success == false) {
                alert('wrong username or password');
            }
            else {

                window.location.replace("/");
            }
        });
    })

    $('#registerForm').on('submit', function (e) {
        e.preventDefault();
        let data = ($(this).serialize());
        console.log(data);
        let dataw = JSON.stringify(data);
        console.log(dataw);
        $.post("/Account?handler=Register", data, function (result) {

            console.log(result);
            if (result.hasOwnProperty('exception')) {
                alert('Exception! \n' + result.exception + '\n' + result.inner);
            }
            else if (result.success == true) {
                window.location.replace("/");
            }
            else if (result.success == false){
                alert('could not log in, try again');
            }
        });
    })


    $('#logout').on('click', function (e) {
        e.preventDefault();
        
    })

    $(".add-message").click(function () {
        chatLoad();
    })
    $('.message-container').scroll(function () {
        if ($(this).scrollTop() +
            $(this).innerHeight() >=
            $(this)[0].scrollHeight - 1) {
            console.log("end reached");
            autoScroll = true;
        }
        else {
            let value = $(this).scrollTop() + $(this).innerHeight();
            console.log("scrollTop = " + $(this).scrollTop() + "\ninner height = " +
                $(this).innerHeight() + "\n scroll height = " +
                $(this)[0].scrollHeight)
            console.log("value to match : " + value)
            console.log($(this)[0].scrollHeight);
            console.log("end not reached");
            autoScroll = false;
        }
    });
});




function logout() {
    $.ajax({
        url: "/Account?handler=Logout",
        dataType: 'json',

        type: 'post', //get form GET/POST method
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function (result) {
            if (result.hasOwnProperty('loggedout')) {
                window.location.replace("/");
                console.log("yay");
            }
            else {
                alert('could not log out, try again');
            }
        },
    })
        .done(function (result) {

            debugger;
            if (result.hasOwnProperty('loggedout')) {
                window.location.replace("/");
                console.log("yay");
            }
            else {
                alert('could not log out, try again');
            }
        });
}
function getMessage(data) {
    console.log(data);
    if (data.reciever == false) {
        return '<div class="justify-content-start row">' +
            '<div class="box sb2 shadow text-break" style="max-width: 65%">' + data.message + '</div>' +
            '</div >';
    }
    else {

        return '<div class="justify-content-end row">' +
            '<div class="box sb1 shadow text-break" style="max-width: 65%">' + data.message + '</div>' +
            '</div>';
    }
}


function scrollAuto() {
    var element = $('.messages-container');
    if (element.scrollTop == (element.scrollHeight - element.offsetHeight)) { 
        return true;
    }
    return false;
}
async function chatLoad() {
    if (timesRan < 100) {
        timesRan++;
    $.get(url, function (data) {
        console.log(scrollAuto());
        $(".messages").append(getMessage(data));
        console.log(autoScroll);
        if (autoScroll) {
            setTimeout(2000);
            $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 1000);
        }

        
    })
    }
}



