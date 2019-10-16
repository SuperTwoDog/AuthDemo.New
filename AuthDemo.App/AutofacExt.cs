﻿using AuthDemo.Repository;
using AuthDemo.Repository.Interface;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Mvc;
using IContainer = Autofac.IContainer;
using System.Web.Http;

namespace AuthDemo.App
{
    public static class AutofacExt
    {
        private static IContainer _container;

        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();

            //注册数据库基础操作和工作单元
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).PropertiesAutowired();
            builder.RegisterType(typeof(UnitWork)).As(typeof(IUnitWork)).PropertiesAutowired();

            //注册app层
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // 注册controller，使用属性注入
            builder.RegisterControllers(Assembly.GetCallingAssembly()).PropertiesAutowired();

            //注册所有的ApiControllers
            builder.RegisterApiControllers(Assembly.GetCallingAssembly()).PropertiesAutowired();

            builder.RegisterModelBinders(Assembly.GetCallingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // 注册所有的Attribute
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            _container = builder.Build();

            //Set the MVC DependencyResolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

            //Set the WebApi DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)_container);
        }

        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
            //   return (T)DependencyResolver.Current.GetService(typeof(T));
        }
    }
}
