$(document).ready(function () {
    
    //Setup ajax options
    $.ajaxSetup({
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8 "    
    });

    //Display tree
    $("#tree").jstree({
        "json_data": {
            "ajax" : {
                "url": "/Categories/GetChildren",
                "data": function(n) {
                    return {
                        "id": n.attr ? n.attr("id").replace("node_", "") : 1
                    };
                }
            }
        },
        "types": {
            "valid_children": [ "none" ],
            "types": {
                "file": {
                    "valid_children": "none",
                    "icon": {
                        "image": "Content/images/file.png"
                    }
                },
                "folder": {
                    "valid_children": ["file", "folder"],
                    "icon": {
                        "image": "Content/images/folder.png"
                    }
                },
                "drive": {
                    "valid_children": ["file", "folder"],
                    "icon": {
                        "image": "Content/images/root.png"
                    },
                    "start_drag": false,
                    "move_node": false,
                    "delete_node": false,
                    "remove": false
                }
            }
        },
        "ui": {
            "initially_select": ["node_1"],
            "select_limit" : 1
        },
        "themes": {
            "theme": "apple"
        },
        plugins: ["themes", "html_data", "dnd", "ui", "crrm", "types"]
    }).bind("rename.jstree", function(e, data) {
        //RENAME
        var node = {
            CategoryID: data.rslt.obj.attr("id").replace("node_", ""),
            Title: data.rslt.new_name
        };

        $.ajax({
            url: "/Categories/Rename",
            data: JSON.stringify(node)
        });

    }).bind("remove.jstree", function(e, data) {
        //REMOVE
        var node = { CategoryID: data.rslt.obj.attr("id").replace("node_", "") };
        $.ajax({
            url: "/Categories/Remove",
            data: JSON.stringify(node)
        });
        
    }).bind("create.jstree", function (e, data) {
        //CREATE
        var node = {
            ParentCategoryID: data.rslt.parent.attr("id").replace("node_", ""),
            Title: data.rslt.name,
            type: data.rslt.obj.attr("rel")
        };
        
        $.ajax({
            url: "/Categories/AddNode",
            data: JSON.stringify(node),
            success: function(msg) {
                data.rslt.obj.attr("id", "node_" + msg);
            }
        });
        
    }).bind("move_node.jstree", function(e, data) {
        data.rslt.o.each(function (i) {
            //MOVE
            var node = {
                CategoryID: $(this).attr("id").replace("node_", ""),
                ParentCategoryID: data.rslt.np.attr("id").replace("node_", "")
            };

            $.ajax({
                url: "/Categories/MoveNode",
                data: JSON.stringify(node)
            });
            
        });
    }).bind("open_node.jstree", function(e) {
            
    });

    //Add action on menu
    $("#menu input").click(function () {
        switch (this.id) {
            case "add_file":
            case "add_folder": 
                $("#tree").jstree("create", null, "last", { "attr": { "rel": this.id.toString().replace("add_", "") } });
                break;
            case "text": break;
            default:
                $("#tree").jstree(this.id);
                break;
        }
    });
    
});