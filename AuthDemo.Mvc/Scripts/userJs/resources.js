layui.config({
    base: "/Scripts/"
}).use(['form', 'vue', 'ztree', 'layer', 'jquery', 'table', 'droptree', 'openauth', 'utils'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var openauth = layui.openauth;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER

    $("#menus").loadMenus("Resource");

    layui.droptree("/Applications/GetList", "#AppName", "#AppId", false);

    //主列表加载，可反复调用进行刷新
    var config = {};  //table的参数，如搜索key，点击tree的id
    var mainList = function (options) {
        if (options != undefined) {
            $.extend(config, options);
        }
        $.extend(config, { key: $(".searchVal").val(), Status: $("#selStatus").find("option:selected").val() });
        table.reload('mainList',
            {
                url: '/Resources/Load',
                where: config
            });
    };
    mainList();

    //搜索
    $(".search_btn").on("click", function () {
        mainList();
    });

    //添加（编辑）对话框
    var editDlg = function () {
        var vm = new Vue({
            el: "#formEdit"
        });
        var update = false;  //是否为更新
        var show = function (data) {
            var title = update ? "编辑信息" : "添加";
            var index = layer.open({
                title: title,
                area: ["500px", "400px"],
                type: 1,
                content: $('#divEdit'),
                success: function () {
                    vm.$set('$data', data);
                },
                end: mainList
            });
            var url = "/Resources/Add";
            if (update) {
                url = "/Resources/Update";
            }
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    $.post(url,
                        data.field,
                        function (data) {
                            layer.msg(data.Message);
                            if (data.Code == "200") {
                                layer.close(index);
                            }
                        },
                        "json");
                    return false;
                });
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
        if (obj.event === 'edit') {
            editDlg.update(data);
        } else if (obj.event === 'del') {
            deleteResource(data.Id);
        } else if (obj.event === 'active') {
            activeResource(data.Id);
        }
    });

    //删除资源操作
    var deleteResource = function (resID) {
        toplayer.confirm('确定删除选中的资源？', { icon: 3, title: '提示信息' }, function (index) {
            $.post("/Resources/SetResourcesStatus", { ids: resID, status: 0}, function (data) {
                if (data.Code == 200) {
                    mainList();
                } else {
                    toplayer.msg(data.Message);
                }
            }, "json");
            toplayer.close(index);
        })
    }

    //启用资源操作
    var activeResource = function (resID) {
        toplayer.confirm('确定启用选中的资源？', { icon: 3, title: '提示信息' }, function (index) {
            $.post("/Resources/SetResourcesStatus", { ids: resID, status: 1 }, function (data) {
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
            //openauth.del("/Resources/Delete",
            //    data.map(function (e) { return e.Id; }),
            //    mainList);
            if (data.length < 1) {
                toplayer.msg("请选择要删除的资源");
                return;
            }
            var resID = data.map(function (e) {
                return e.Id;
            });
            deleteResource(resID);
        }
        , btnAdd: function () {  //添加
            editDlg.add();
        }
        , btnEdit: function () {  //编辑
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data;
            if (data.length != 1) {
                layer.msg("请选择编辑的行，且同时只能编辑一行");
                return;
            }
            editDlg.update(data[0]);
        }

        , search: function () {   //搜索
            mainList({ key: $('#key').val() });
        }
        , btnRefresh: function () {
            mainList();
        }
    };

    $('.toolList .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });

    //监听页面主按钮操作 end
})