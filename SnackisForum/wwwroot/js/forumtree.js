
$(document).ready(function () {
    loadTreeView();
});


function loadTreeView() {
    const urlParams = new URLSearchParams(window.location.search.toLowerCase());
    var url;

    //if (window.location.pathname.toLowerCase().split("/").includes("treeview")) {
    //    loadTreeView();
    //    //$('.jstree').on("show_node.jstree", function (e, data) {
    //    //    console.log(data.selected);
    //    //});
    //}
    if (!location.href.toLowerCase().includes("nodeid")) {
        url = '/treeview?handler=loadforum'
    }
    else {
        url = '/treeview?handler=newentry&nodeid=' + urlParams.get('nodeid');
    }
    $.ajax({
        type: 'GET',
        url: url,
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
            'plugins': ["material", "types"],
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
    $.get("/treeview?handler=LoadComment&id=" + id, function (result) {
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

