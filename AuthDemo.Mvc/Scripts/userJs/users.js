layui.config({
    base: "/Scripts/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'openauth', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var openauth = layui.openauth;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER
    layui.droptree("/UserSession/GetOrgs", "#Organizations", "#OrganizationIds");
    var currentUrl;

    $("#menus").loadMenus("User");

    //主列表加载，可反复调用进行刷新
    var config = {};  //table的参数，如搜索key，点击tree的id
    var mainList = function (options) {
        if (options != undefined) {
            $.extend(config, options);
        }
        $.extend(config, { key: $(".searchVal").val() ,UserStatus: $("#selUserStatus").find("option:selected").val()});
        table.reload('mainList', {
            url: '/UserManager/Load',
            where: config
        });
    }
    //左边树状机构列表
    var ztree = function () {
        var url = '/UserSession/GetOrgs';
        var zTreeObj;
        var setting = {
            view: { selectedMulti: false },
            data: {
                key: {
                    name: 'Name',
                    title: 'Name'
                },
                simpleData: {
                    enable: true,
                    idKey: 'Id',
                    pIdKey: 'ParentId',
                    rootPId: ""
                }
            },
            callback: {
                onClick: function (event, treeId, treeNode) {
                    mainList({ orgId: treeNode.Id });
                }
            }
        };
        var load = function () {
            $.getJSON(url, function (json) {
                zTreeObj = $.fn.zTree.init($("#tree"), setting);
                var newNode = { Name: "根节点", Id: null, ParentId: "" };
                json.push(newNode);
                zTreeObj.addNodes(null, json);
                mainList({ orgId: "" });
                zTreeObj.expandAll(true);
            });
        };
        load();
        return {
            reload: load
        }
    }();

    $("#tree").height($("div.layui-table-view").height());

    //搜索
    $(".search_btn").on("click", function () {
        //if ($(".searchVal").val() == '') {
        //    layer.msg("请输入搜索的内容");
        //} else {
            
        //}
        mainList();
    });

    //添加（编辑）对话框
    var editDlg = function () {
        //var vm = new Vue({
        //    el: "#formEdit"
        //});
        var update = false;  //是否为更新
        var show = function (data) {
            var title = update ? "编辑用户信息" : "添加用户信息";
            var url = update ? "/UserManager/EditUser?userID=" + data.Id : "/UserManager/AddUser";
            var index = toplayer.open({
                title: title,
                area: ["500px", "455px"],
                type: 2,
                //content: $('#divEdit'),
                content: url,
                //skin: 'layui-layer-rim', 
                //maxmin: true, //开启最大化最小化按钮
                success: function (layero, indexEx) {
                    var body = $($(".layui-layer-iframe", parent.document).find("iframe")[0].contentWindow.document.body);
                    var fromEdit = body.find("#formEdit");
                    alert(fromEdit);
                    //var vm = new Vue({
                    //    el: fromEdit
                    //});
                    //vm.$set('$data', data);
                    $(":radio[name='Sex'][value='" + data.Sex + "']").prop("checked", "checked");
                    $("input:checkbox[name='Status']").prop("checked", data.Status == 1);
                    //下面这种方式适合单独开页面，不然上次选中的结果会对本次有影响
                    // $('input:checkbox[name="Status"][value="' + data.Status + '"]').prop('checked', true);
                    body.find("#Account").val("bbb");
                    currentIndex = index;
                    form.render();
                },
                end: mainList
            });
            var url = "/UserManager/AddUser";
            if (update) {
                url = "/UserManager/UpdateUser"; //暂时和添加一个地址
            }
            //提交数据
            //form.on('submit(formSubmit)',
            //    function (data) {
            //        $.post(url,
            //            data.field,
            //            function (data) {
            //                layer.msg(data.Message);
            //                if (data.Code == "200") {
            //                    layer.close(index);
            //                }
            //            },
            //            "json");
            //        return false;
            //    });
        }
        return {
            add: function () { //弹出添加
                update = false;
                show({
                    Id: ''
                });
            },
            update: function (data) { //弹出编辑框
                update = true;
                show(data);
            }
        };
    }();

    //监听表格内部按钮
    table.on('tool(list)', function (obj) {
        var data = obj.data;
        //修改
        if (obj.event === 'edit') {    
            editDlg.update(data);
        } else if (obj.event === 'del') {
            deleteUser(data.Id);
        } else if (obj.event === 'active') {
            activeUser(data.Id);
        }
    });

    //删除用户操作
    var deleteUser = function (userID) {
        toplayer.confirm('确定删除选中的用户？', { icon: 3, title: '提示信息' }, function (index) {
            $.post("/UserManager/SetUserStatus", { ids: userID },function (data) {
                    if (data.Code == 200) {
                        mainList();
                    } else {
                        toplayer.msg(data.Message);
                    }
                }, "json");
            toplayer.close(index);
        })
    }

    //启用用户操作
    var activeUser = function (userID) {
        toplayer.confirm('确定启用选中的用户？', { icon: 3, title: '提示信息' }, function (index) {
            $.post("/UserManager/SetUserStatus", { ids: userID, status: 1 }, function (data) {
                if (data.Code == 200) {
                    mainList();
                } else {
                    toplayer.msg(data.Message);
                }
            }, "json");
            toplayer.close(index);
        })
    }


    //监听页面主按钮操作
    var active = {
        btnDel: function () {      //批量删除
            var checkStatus = table.checkStatus('mainList'), data = checkStatus.data;
            //openauth.del("/UserManager/Delete",
            //    data.map(function (e) { return e.Id; }),
            //    mainList);
            //deleteUser()
            if (data.length < 1) {
                toplayer.msg("请选择要删除的用户");
                return;
            }
            var userID = data.map(function (e) {
                return e.Id;
            });
            deleteUser(userID);
        }
        , btnAdd: function () {  //添加
            editDlg.add();
        }
        , btnEdit: function () {  //编辑
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择编辑的用户，且同时只能编辑一个");
                return;
            }
            editDlg.update(data[0]);
        }

        , search: function () {   //搜索
            mainList({ key: $(".searchVal").val() });
        }
        , btnRefresh: function () {
            mainList();
        }
        , btnAccessModule: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择要分配的用户");
                return;
            }

            var index = toplayer.open({
                title: "为用户【" + data[0].Name + "】分配模块",
                type: 2,
                area: ['750px', '600px'],
                content: "/ModuleManager/Assign?type=UserModule&menuType=UserElement&id=" + data[0].Id,
                success: function (layero, index) {

                }
            });
        }
        , btnAccessRole: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择要分配的用户");
                return;
            }

            var index = toplayer.open({
                title: "为用户【" + data[0].Name + "】分配角色",
                type: 2,
                area: ['750px', '600px'],
                content: "/RoleManager/Assign?type=UserRole&id=" + data[0].Id,
                success: function (layero, index) {

                }
            });
        }
        , btnAssignReource: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                toplayer.msg("请选择要分配的用户");
                return;
            }

            var index = toplayer.open({
                title: "为用户【" + data[0].Name + "】分配资源",
                type: 2,
                area: ['750px', '600px'],
                content: "/Resources/Assign?type=UserResource&id=" + data[0].Id,
                success: function (layero, index) {

                }
            });
        }
    };

    $('.toolList .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    //监听页面主按钮操作 end


    form.on("submit(formSubmit)", function (data) {
        //弹出loading
        var titleIndex = top.layer.msg('数据提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
        $.post("/UserManager/AddOrUpdate", data.field, function (data) {
            if (data.Code == "200") {
                    top.layer.close(titleIndex);
                    //layer.close(index);
                    top.layer.msg("用户添加成功！");
                    top.layer.closeAll("iframe");
            } else {
                top.layer.msg(data.Message);
            }
            },"json");
        //return false;
        //setTimeout(function () {
        //    top.layer.close(titleIndex);
            
        //    //刷新父页面
        //    //$(".layui-tab-item.layui-show", parent.document).find("iframe")[0].contentWindow.location.reload();
        //}, 500);
        return false;
    });
})