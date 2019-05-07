using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Notify.Widget.Persistence;
using Guanwu.Toolkit.Extensions;
using Guanwu.Toolkit.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.AddIn.Hosting;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guanwu.NotifyConsoleApp
{
    class Program
    {
        private static ILogger Logger = LoadLogger().Result;
        private static readonly string HANGFIRE = ConfigurationManager.AppSettings[WidgetConst.HANGFIRE];


        public static int Main(string[] args)
        {
            return RunMain();
        }

        private static int RunMain()
        {
            try {
                var pluginObject = new PluginObject();
                Task.WaitAll(
                    Task.Run(() => InitPlugins(LoadPlugins(), pluginObject))
                        .ContinueWith(_ => InitMessengers(LoadMessengers(), pluginObject))
                );
                Console.ReadKey();
                return 0;
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                Console.WriteLine("\nPress and key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        #region Plugins

        private static async Task InitPlugins(Task<IPlugin[]> plugins, PluginObject pluginObject)
        {
            Guard.AgainstNull(nameof(plugins), plugins);
            Guard.AgainstNull(nameof(pluginObject), pluginObject);

            await Task.Run(() => {
                try {
                    var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
                    Parallel.ForEach(plugins.Result, options, plugin => {
                        plugin.Initialize(pluginObject);
                        plugin.Execute();
                    });
                }
                catch (AggregateException e) {
                    foreach (var ie in e.InnerExceptions)
                        Console.WriteLine(ie.ToString());
                }
            });
        }
        private static async Task<IPlugin[]> LoadPlugins()
        {
            async Task<IPlugin> ActivePlugin(AddInToken token)
            {
                AppDomain pluginDomain = CreatePluginDomain(token);
                pluginDomain.SetData(WidgetConst.HANGFIRE, HANGFIRE);
                pluginDomain.DoCallBack(new CrossAppDomainDelegate(PluginCallBackAsync));
                return await Task.Run(() => token.Activate<IPlugin>(pluginDomain));
            }

            return await Task.Run(() => {
                var tasks = new List<Task<IPlugin>>();
                try {
                    var pipeline = Path.Combine(
                        Environment.CurrentDirectory, ContainerConst.PLUGIN_PATH);
                    var warnings = AddInStore.Rebuild(pipeline);
                    var tokens = AddInStore.FindAddIns(typeof(IPlugin), pipeline);

                    foreach (var token in tokens)
                        tasks.Add(ActivePlugin(token));
                    Task.WaitAll(tasks.ToArray());

                    return tasks
                        .Select(t => t.Result)
                        .ToArray();
                }
                catch (AggregateException e) {
                    foreach (var ie in e.InnerExceptions)
                        Console.WriteLine(ie.ToString());
                    return new IPlugin[0];
                }
            });
        }
        private static AppDomain CreatePluginDomain(AddInToken token)
        {
            string GetPluginLocation(AddInToken t)
            {
                object tokenAddIn = t.GetField<object>("_addin");
                return tokenAddIn.GetProperty<string>("Location");
            }

            var security = AppDomain.CurrentDomain.Evidence;
            var setup = new AppDomainSetup {
                ConfigurationFile = $"{GetPluginLocation(token)}.config",
            };
            return AppDomain.CreateDomain(token.Name, security, setup);
        }
        private static void PluginCallBackAsync()
        {
            var pluginDbContext = LoadDbContextWidget();
            var pluginRepository = LoadRepository();
            var pluginProfile = LoadProfileWidget();
            var pluginLogger = LoadLogger();

            AppDomain pluginDomain = AppDomain.CurrentDomain;
            pluginDomain.SetData(WidgetConst.IDBCONTEXT, pluginDbContext.Result);
            pluginDomain.SetData(WidgetConst.IREPOSITORY, pluginRepository.Result);
            pluginDomain.SetData(WidgetConst.IPROFILE, pluginProfile.Result);
            pluginDomain.SetData(WidgetConst.ILOGGER, pluginLogger.Result);

            Task.WaitAll(pluginDbContext, pluginRepository, pluginProfile, pluginLogger);
        }
        private static async Task<ILogger> LoadLogger()
        {
            try {
                return await Task.Run(() => {
                    ILoggerFactory factory = new LoggerFactory();
                    return factory.AddConsole().CreateLogger<Program>();
                });
            }
            catch {
                return NullLogger.Instance;
            }
        }
        private static async Task<IProfile> LoadProfileWidget()
        {
            try {
                return await Task.Run(() => {
                    return WidgetContainer.Instance
                        .ResolveWidgets<IProfile>()
                        .FirstOrDefault();
                });
            }
            catch {
                return null;
            }
        }
        private static async Task<IDbContext> LoadDbContextWidget()
        {
            try {
                return await Task.Run(() => {
                    return WidgetContainer.Instance
                        .ResolveWidgets<IDbContext>()
                        .FirstOrDefault();
                });
            }
            catch {
                return null;
            }
        }
        private static async Task<IRepository> LoadRepository()
        {
            try {
                return await Task.Run(() => {
                    return WidgetContainer.Instance
                        .ResolveWidgets<IRepository>()
                        .FirstOrDefault();
                });
            }
            catch {
                return null;
            }
        }

        #endregion

        #region Messengers

        private static async Task InitMessengers(Task<IPipelineMessenger[]> messengers, PluginObject pluginObject)
        {
            Guard.AgainstNull(nameof(messengers), messengers);
            Guard.AgainstNull(nameof(pluginObject), pluginObject);

            void OnMessageRecived(byte[] body, Dictionary<string, string> context)
            {
                var message = new PipelineMessage {
                    Id = context[WidgetConst.PMSG_ID],
                    Source = context[WidgetConst.PMSG_SOURCE],
                    Content = Encoding.UTF8.GetString(body),
                    Targets = new List<string> { context[WidgetConst.PMSG_JOBID] },
                };
                pluginObject.ReceiveMessage(message);
            }

            await Task.Run(() => {
                try {
                    var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
                    Parallel.ForEach(messengers.Result, options, messenger => {
                        messenger.OnMessageReceived += OnMessageRecived;
                        messenger.Initialize(Logger);
                    });
                }
                catch (AggregateException e) {
                    foreach (var ie in e.InnerExceptions)
                        Console.WriteLine(ie.ToString());
                }
            });
        }
        private static async Task<IPipelineMessenger[]> LoadMessengers()
        {
            try {
                return await Task.Run(() => {
                    return WidgetContainer.Instance
                        .ResolveWidgets<IPipelineMessenger>()
                        .ToArray();
                });
            }
            catch {
                return new IPipelineMessenger[0];
            }
        }

        #endregion
    }
}
