//获取系统时间
var newDate = '';
getLangDate();
//值小于10时，在前面补0
function dateFilter(date) {
    if (date < 10) { return "0" + date; }
    return date;
}
function getLangDate() {
    var dateObj = new Date(); //表示当前系统时间的Date对象
    var year = dateObj.getFullYear(); //当前系统时间的完整年份值
    var month = dateObj.getMonth() + 1; //当前系统时间的月份值
    var date = dateObj.getDate(); //当前系统时间的月份中的日
    var day = dateObj.getDay(); //当前系统时间中的星期值
    var weeks = ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"];
    var week = weeks[day]; //根据星期值，从数组中获取对应的星期字符串
    var hour = dateObj.getHours(); //当前系统时间的小时值
    var minute = dateObj.getMinutes(); //当前系统时间的分钟值
    var second = dateObj.getSeconds(); //当前系统时间的秒钟值
    var timeValue = "" + ((hour >= 12) ? (hour >= 18) ? "晚上" : "下午" : "上午"); //当前时间属于上午、晚上还是下午
    newDate = dateFilter(year) + "年" + dateFilter(month) + "月" + dateFilter(date) + "日 " + " " + dateFilter(hour) + ":" + dateFilter(minute) + ":" + dateFilter(second);
    document.getElementById("nowTime").innerHTML = "亲爱的系统管理员，" + timeValue + "好！ 欢迎来到芒果计量中心。当前时间为： " + newDate + "　" + week;
    setTimeout("getLangDate()", 1000);
}


layui.config({
    base: "/Scripts/"
}).use(['form', 'element', 'layer', 'jquery'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : parent.layer,
        element = layui.element,
        $ = layui.jquery;
    

    $(".panel a").on("click", function () {
        window.parent.addTab($(this));
    });
    getLangDate();
    //用户数
    $.getJSON("/UserManager/Load?limit=1&page=1",
        function (data) {
            $(".userAll span").text(data.count);
        }
    )

    //机构
    $.getJSON("/UserSession/GetOrgs",
        function (data) {
            $(".orgs span").text(data.length);
        }
    )

    //机构
    $.getJSON("/RoleManager/Load?limit=1&page=1",
        function (data) {
            $(".roles span").text(data.count);
        }
    )

    //我的流程
    $.getJSON("/Flowinstances/Load?limit=1&page=1",
        function (data) {
            $(".flows span").text(data.count);
        }
    )

    //流程模板
    $.getJSON("/flowschemes/Load?limit=1&page=1",
        function (data) {
            $(".flowschemes span").text(data.count);
        }
    )

    //表单
    $.getJSON("/Forms/Load?limit=1&page=1",
        function (data) {
            $(".forms span").text(data.count);
        }
    )

    //数字格式化
    $(".panel span").each(function () {
        $(this).html($(this).text() > 9999 ? ($(this).text() / 10000).toFixed(2) + "<em>万</em>" : $(this).text());
    })

    //系统基本参数
    $(".version").text("4.0");      //当前版本
    $(".author").text("yubaolee");        //开发作者
    $(".homePage").text("/Home/Index");    //网站首页
    $(".server").text("Windows Server 2012");        //服务器环境
    $(".dataBase").text("Sql Server 2012");    //数据库版本
    $(".maxUpload").text("100M");    //最大上传限制
    $(".userRights").text("管理员");//当前用户权限

});