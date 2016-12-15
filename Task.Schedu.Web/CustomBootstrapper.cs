using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using Task.Schedu.Utility;
using Task.Schedu.Web.Modules;

namespace Task.Schedu.Web
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
            var formsAuthConfiguration = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/Login",
                UserMapper = container.Resolve<IUserMapper>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
            pipelines.OnError += Error;
        }
        protected override IRootPathProvider RootPathProvider
        {
            get { return new CustomRootPathProvider(); }
        }

        /// <summary>
        /// 配置静态文件访问权限
        /// </summary>
        /// <param name="conventions"></param>
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);
            ///静态文件夹访问 设置 css,js,image
            conventions.StaticContentsConventions.AddDirectory("Content");
            conventions.StaticContentsConventions.AddDirectory("TempFile");
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<ISerializer, CustomJsonNetSerializer>();
            container.Register<IUserMapper, UserMapper>();
        }

        private dynamic Error(NancyContext context, Exception ex)
        {
            LogHelper.WriteLog("Web站点请求异常", ex);
            return ex.Message;
        }
    }

    internal class CustomRootPathProvider : IRootPathProvider
    {
        private static readonly string ROOT_PATH = AppDomain.CurrentDomain.BaseDirectory;
        public string GetRootPath()
        {
            return ROOT_PATH;
        }
    }
    /// <summary>
    /// 使用Newtonsoft.Json 替换Nancy默认的序列化方式
    /// </summary>
    public class CustomJsonNetSerializer : JsonSerializer, ISerializer
    {
        public CustomJsonNetSerializer()
        {
            ContractResolver = new DefaultContractResolver();
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            Formatting = Formatting.None;
            NullValueHandling = NullValueHandling.Ignore;
        }

        public bool CanSerialize(string contentType)
        {
            return contentType.Equals("application/json", StringComparison.OrdinalIgnoreCase);
        }

        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            using (var streamWriter = new StreamWriter(outputStream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                Serialize(jsonWriter, model);
            }
        }
        public IEnumerable<string> Extensions { get { yield return "json"; } }
    }
}