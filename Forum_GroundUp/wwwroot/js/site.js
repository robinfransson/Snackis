////require("jquery");

//let url = "/?Handler=LoadMessages";
let timesRan = 0;
//let myTimer = setInterval(chatLoad, 5000);
let autoScroll = true;

var token = $('input[name="__RequestVerificationToken"]').val();


let path = window.location.pathname.toLowerCase();









async function chatLoad() {
    if (timesRan < 100) {
        timesRan++;
        $.get(url, function (data) {
            console.log(scrollAuto());
            $(".messages").append(getMessage(data));
            console.log(autoScroll);
            if (autoScroll) {
                setTimeout(1000);
                $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 1000);
            }


        })
    }
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
let currentChat = null;






function loadTreeView() {

    $.ajax({
        type: 'GET',
        url: '/treeview?handler=loadforum',
        dataType: "json",
        success: function (result) {
            console.log(result);
            createTree(result);
        }
    });
}
function createTree(data) {
    $('.jstree')
        .on("select_node.jstree", function (e, data) {
            e.preventDefault();
            e.stopPropagation();
            let nodeType = data.node.a_attr.class;
            let id = parseInt(data.node.id.split("-")[1]);
            console.log(nodeType);
            switch (nodeType) {
                case "forum-tree":
                    console.log("forum clicked");
                    
                    break;
                case "sub-tree":
                    loadSubforum(id)
                    console.log("subforum clicked");
                    break;
                case "thread-tree":
                    console.log("thread clicked");
                    loadThread(id)
                    break;
                case "reply-tree":
                    console.log("reply clicked");
                    loadReply(id);
                    break;
                default:
                    break;
            }
        }).jstree({
            'themes': {
                "icons": false
            },
            'plugins': ["material", "types" ],
            'core': {

                'data': data
            }
        });
}

function showModal(result) {
    $('#exampleModal > #exampleModalMain').html(result)
    $("#exampleModal").modal('show');
}

function loadReply(id) {
    $.get("/treeview?handler=LoadComment&id="+id, function (result) {
        // comment = comment.Body, poster = comment.Author.UserName, title = comment.Title, date = comment.DaysAgo() 
        showModal(result)
        //console.log(result.title, result.poster, result.comment, result.date, result.picture)
        
    })
}

function loadThread(id) {
    $.get("/treeview?handler=LoadThread&id=" + id, function (result) {
        // comment = comment.Body, poster = comment.Author.UserName, title = comment.Title, date = comment.DaysAgo() 
        showModal(result)
        //console.log(result.title, result.poster, result.comment, result.date, result.picture)

    })
}
function loadSubforum(id) {
    $.get("/treeview?handler=LoadSubforum&id=" + id, function (result) {
        // comment = comment.Body, poster = comment.Author.UserName, title = comment.Title, date = comment.DaysAgo() 
        showModal(result)
        //console.log(result.title, result.poster, result.comment, result.date, result.picture)

    })
}

$(document).ready(function () {


    $('.open-messages').click(function (e) {
        currentChat = $(this).data("chat-id");
        $.get("/messages?handler=LoadMessages&id=" + currentChat, function (result) {
            $('.chat-container').html(result);
            console.log(currentChat);
            $(".message-container").animate({ scrollTop: $('.message-container').prop("scrollHeight") }, 0);
        })
    })



    $('#compose-button').click(function () {
        console.log('clicked')
        $.get({
            url: '/messages?handler=compose',

            success: function (result) {
                $('chat-container').html(result);
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





