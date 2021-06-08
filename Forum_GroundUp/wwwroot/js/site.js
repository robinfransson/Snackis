///*require("jquery");
var token = $('input[name="__RequestVerificationToken"]').val();


let path = window.location.pathname.toLowerCase();


var chatRefreshTimer;








function scrollAuto() {
    var element = document.getElementById('messages-container');
    console.log(element.scrollTop >= (element.scrollHeight - element.offsetHeight))
    if (element.scrollTop >= (element.scrollHeight - element.offsetHeight)) {
        return true;
    }
    return false;
}




function addSubforum(name, parentID) {
    $.post({
        data: { subName: name, parent : parentID},
        url: "/Ajax?Handler=CreateSubforum",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function () {
            location.reload();
        }

    });
}


function loadNewMessages(chatID) {
    $.get({
        url: '/messages?handler=loadnewmessages',
        data: {
            chatID : chatID,
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

function addForum(name) {
    $.post({
        data: { forumName : name },
        url: "/Ajax?Handler=CreateForum",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function () {
            location.reload();
        }

    });
}
function checkUsername() {
    console.log();
    let submit = $('#sendmessage');
    if ($('#usernamefield').val().length > 0) {
    $.get({
        url: "/messages?handler=userexists",
        data: { username : $('#usernamefield').val() },
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
        $.post("/messages?handler=SendMessageFromChat", data, function (result) {
            $('textarea[name="message"]').val("");
            $('input[name="title"]').val("");

            if (scrollAuto()) {

                $('.message-container').append(result);
                $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 1000);
            }
        })
    });

}

$(document).ready(function () {


    $('.open-messages').click(function (e) {

        clearTimeout(chatRefreshTimer);
        currentChat = $(this).data("chat-id");
        $.get("/messages?handler=LoadMessages&id=" + currentChat, function (result) {
            $('.chat-container').html(result);
            chatRefreshTimer = setInterval(function () {
                                                        loadNewMessages(currentChat)
                                                      }, 1000);
            $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 0);
            registerSendMessageForm();
        })
    })

   


    $('#compose-button').click(function () {
        console.log('clicked')

        clearTimeout(chatRefreshTimer);
        $.get({
            url: '/messages?handler=compose',

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





    if (path.split("/").includes("treeview")) {
        loadTreeView();
        //$('.jstree').on("show_node.jstree", function (e, data) {
        //    console.log(data.selected);
        //});
    }

    $("#makeadmin").on('click', function (e) {
        e.preventDefault();
        $.post({
            url: "/Ajax?Handler=MakeAdmin",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
            },
            success: function (result) {
                if (result.success == true) {
                    alert('you are now admin!');
                }
                else {
                    alert('something went wrong!')
                }
            }

        })

    })

    $('.go-to-thread').click(function (e) {
        let postID = $(this).data("post-id");
        let threadID = $(this).data("thread-id");
        console.log(postID + "," + threadID);
        //$.post({
        //    url: "/Thread?handler=GoToLastReply",
        //    beforeSend: function (xhr) {
        //        xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        //    },
        //    data: {
        //        threadID: threadID,
        //        postID:postID
        //    },
        //    success: function (result) {
        //        console.log(result);
        window.location.replace("/Thread/" + threadID + "#reply-" + postID);
        //    }
        //});
    });

    $('.messages').hover(function () {
    console.log("hovered over messages");
    $('.messages-icon').text("drafts");
});




    $('.add-forum').click(function () {
        //let forumid = $(this).data("forum-id");

        var forum = prompt("Skriv in ett namn på forumet:", "");
        if (forum == null || forum == "") {
            alert("Avbröt");
        } else {
            addForum(forum);
        }
    });


    $('.add-subforum').click(function () {
        let forumid = $(this).data("forum-id");

        var subforumname = prompt("Skriv in ett namn på delforumet:", "");
        if (subforumname == null || subforumname == "") {
            alert("Avbröt");
        } else {
            addSubforum(subforumname,forumid);
        }
    });


$('.messages').mouseleave(function () {
    console.log("mouse left messages");
    $('.messages-icon').text("markunread");
});









$('.login-form').on('submit', function (e) {
    e.preventDefault();
    let data = ($(this).serialize());
    console.log(data);
    $.post("/ajax?handler=login", data, function (result) {
        console.log(result);
        if (result.success == false) {
            alert('wrong username or password');
        }
        else {

            window.location.href = window.location.pathname + window.location.hash;
        }
    });
})

$('#registerForm').on('submit', function (e) {
    e.preventDefault();
    let data = ($(this).serialize());
    console.log(data);
    let dataw = JSON.stringify(data);
    console.log(dataw);
    $.post("/ajax?handler=Register", data, function (result) {

        console.log(result);
        if (result.hasOwnProperty('exception')) {
            alert('Exception! \n' + result.exception + '\n' + result.inner);
        }
        else if (result.success == true) {
            window.location.reload();
        }
        else if (result.success == false) {
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
//$('.message-container').scroll(function () {
//    if ($(this).scrollTop() +
//        $(this).innerHeight() >=
//        $(this)[0].scrollHeight - 1) {
//        console.log("end reached");
//        autoScroll = true;
//    }
//    else {
//        let value = $(this).scrollTop() + $(this).innerHeight();
//        console.log("scrollTop = " + $(this).scrollTop() + "\ninner height = " +
//            $(this).innerHeight() + "\n scroll height = " +
//            $(this)[0].scrollHeight)
//        console.log("value to match : " + value)
//        console.log($(this)[0].scrollHeight);
//        console.log("end not reached");
//        autoScroll = false;
//    }
//});
});




function logout() {
    $.ajax({
        url: "/ajax?handler=Logout",
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
        .fail(function () {
            alert('Could not log out!');
        })
        .done(function (result) {

            //debugger;
            //if (result.hasOwnProperty('loggedout')) {
            //    window.location.replace("/");
            //    console.log("yay");
            //}
            //else {
            //    alert('could not log out, try again');
            //}
        });
}





