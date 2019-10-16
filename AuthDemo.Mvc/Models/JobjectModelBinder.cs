using Newtonsoft.Json.Linq;
using System.Web.Mvc;
namespace AuthDemo.Mvc.Models
{
    public class JobjectModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //todo:需要判断前端是否是FormData
            var obj = new JObject();
            var request = controllerContext.HttpContext.Request;
            foreach (var key in request.Form.AllKeys)
            {
                obj[key] = request.Form[key];
            }
            return obj;
        }
    }
}