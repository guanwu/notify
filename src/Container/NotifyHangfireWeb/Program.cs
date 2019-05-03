using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace NotifyHangfireWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //AppDomain.CurrentDomain.AppendPrivatePath(@"D:\DevOps\Guanwu\Notify\src\Container\NotifyHangfireConsole\bin\Debug");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
