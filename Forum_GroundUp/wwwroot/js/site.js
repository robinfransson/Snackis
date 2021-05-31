
//let url = "/?Handler=LoadMessages";
let timesRan = 0;
//let myTimer = setInterval(chatLoad, 5000);
let autoScroll = true;

var token = $('input[name="__RequestVerificationToken"]').val();


let path = window.location.pathname.toLowerCase();

function getThreadReplies() {

    let threadID = path.split('/').reverse()[0];
    $.ajax({
        type: 'GET',
        url: '/ajax?handler=replies',
        data: { "id": threadID },
        dataType: "json",
        success: function (result) {
            console.log(result);
            createTree(result);
        }
    });
}


function loadComment(id) {
    $.get('/ajax?handler=LoadComment', { id : id }, function (result) {
        console.log(result);
    })
}


function createTree(data) {

    $('.jstree')
        .on('click', '.jstree-anchor', function (e) {
            $('.jstree').jstree(true).toggle_node(e.target);
        })
        .on("select_node.jstree", function (e, data) {
            e.preventDefault();
            e.stopPropagation();
            loadComment(data.node.id);
        }).jstree({
            'themes': {
               "icons":false
            },
            'plugins' : [ "material" ],
            'core': {
            
                'data': data,
                dblclick_toggle: false,
                expand_selected_onload: false       
        }
    });
}

$(document).ready(function () {


    if (path.split("/").includes("thread")) {
        getThreadReplies();
        //$('.jstree').on("show_node.jstree", function (e, data) {
        //    console.log(data.selected);
        //});
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
    $.post("/ajax?handler=login", data, function (result) {
        console.log(result);
        if (result.success == false) {
            alert('wrong username or password');
        }
        else {

            window.location.reload();
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
        url: "/ajax?handler=Logout",
        dataType: 'json',

        type: 'post', //get form GET/POST method
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input[name="__RequestVerificationToken"]').val())
        },
        success: function (result) {
            if (result.hasOwnProperty('loggedout')) {
                window.location.reload();
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



