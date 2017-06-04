using System;
using Autofac;
using SiteCopier.DI;
using SiteCopier.Interfaces;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("To start downloading modify ConsoleApp.exe.config and enter 'c'. Otherwise press enter to exit.");

            while (true)
            {
                string key = Console.ReadLine();
                if (key != "c") break;

                var container = GetContainer();
                var siteDownloader = container.Resolve<ISiteDownloader>();
                siteDownloader.Load();
            }
        }

        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacModule>();
            return builder.Build();
        }
    }
}
