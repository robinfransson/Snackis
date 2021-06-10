///*require("jquery");
var token = $('input[name="__RequestVerificationToken"]').val();



var chatRefreshTimer;

function reportSuccess(result) {
    if (result.succeeded == true) {
        alert('Rapport mottagen')
    }
    else {
        alert('Något gick fel')
    }
}

function report(type, id) {
        $.post({
            url: 'ajax?handler=report',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
            },
            data: {
                type: type,
                id: id
            },
            success: function (result) {
                reportSuccess(result)
            },
            error: function () {

                alert('Något gick fel')
            }
        })
    
}


function getNewMessageCount() {


    $.get("/ajax?handler=NewMessageCount", function (result) {
        console.log(result.length)
        $('.message-count').text(result.messages);
    });
}




function addSubforum(name, parentID) {
    $.post({
        data: { subName: name, parent: parentID },
        url: "/Ajax?Handler=CreateSubforum",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function () {
            location.reload();
        }

    });
}


function addForum(name) {
    $.post({
        data: { forumName: name },
        url: "/Ajax?Handler=CreateForum",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function () {
            location.reload();
        }

    });
}


$(document).ready(function () {
    $('.reported-click').click(function () {
        console.log('action taken!')
        $.post({
            url: '?handler=takeaction',
            data: {
                remove: $(this).data('take-action'),
                type: $(this).data('report-type'),
                id: parseInt($(this).data('id')),
                reportID: parseInt($(this).data('report-id'))
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
            },
            success: function () {
                location.replace(location.href)

            }
        })
    })

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
    if (!location.href.toLowerCase().includes("/messages")) {
        setInterval(getNewMessageCount, 30000)
    }
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
            addSubforum(subforumname, forumid);
        }
    });

    $('.messages').mouseleave(function () {
        console.log("mouse left messages");
        $('.messages-icon').text("markunread");
    });
    $('.messages').hover(function () {
        console.log("hovered over messages");
        $('.messages-icon').text("drafts");
    });


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





