using System;
using System.Configuration;
using Autofac;
using NLog;
using SiteCopier.Models;

namespace SiteCopier.DI
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ConfigurationManager.RefreshSection("appSettings");

            var options = new DownloadOptions
            {
                StartUri = ConfigurationManager.AppSettings["startUri"],
                DownloadPath = ConfigurationManager.AppSettings["downloadPath"],
                ReferenceDepth = int.Parse(ConfigurationManager.AppSettings["maxReferenceDepth"]),
                ExtensionRestriction = ConfigurationManager.AppSettings["extensions"],
                DomainTransfer = (DomainTransfer)Enum.Parse(typeof(DomainTransfer), ConfigurationManager.AppSettings["domainTransfer"])
            };

            builder.RegisterType<HttpDownloader>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<GeneratedFileSaver>()
                .WithParameter("logger", LogManager.GetLogger(nameof(GeneratedFileSaver)))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UsualFileSaver>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<HtmlLinkParser>()
                .WithParameter("logger", LogManager.GetLogger(nameof(HtmlLinkParser)))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SiteDownloader>()
                .WithParameter("logger", LogManager.GetLogger(nameof(SiteDownloader)))
                .WithParameter("options", options)
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder.Register(c => LogManager.GetLogger("SiteCopier")).As<ILogger>().SingleInstance();
        }
    }
}
