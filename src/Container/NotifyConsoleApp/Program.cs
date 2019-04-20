using Guanwu.Notify.Views;
using Guanwu.Notify.Widget;
using Guanwu.Toolkit.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.AddIn.Hosting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guanwu.NotifyConsoleApp
{
    class Program
    {
        private ICollection<IPlugin> Plugins;
        private ICollection<IDomainWidget> DomainWidgets;
        private IPluginObject PluginObject = new PluginObject(Logger);
        private static ILogger Logger = NullLogger.Instance;

        public static int Main(string[] args)
        {
            Logger = InitLogger();
            return new Program().RunMain();
        }

        private static ILogger InitLogger()
        {
            ILoggerFactory factory = new LoggerFactory().AddConsole();
            return factory.CreateLogger<Program>();
        }

        private int RunMain()
        {
            try
            {
                Task.WaitAll(
                    Task.Run(() => LoadPlugins()),
                    Task.Run(() => LoadDomainWidgets())
                );

                Task.WaitAll(
                    Task.Run(() => InitializePlugins())
                        .ContinueWith(_ => InitializeMessengers())
                );

                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private void InitializeMessengers()
        {
            Logger.LogTrace(nameof(InitializeMessengers));

            if (DomainWidgets == null) return;

            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
            Parallel.ForEach(DomainWidgets, options, w =>
            {
                try
                {
                    if (typeof(IPipelineMessenger).IsAssignableFrom(w.GetType()))
                    {
                        var messenger = w as IPipelineMessenger;
                        messenger.OnMessageReceived += OnMessageRecived;
                        messenger.Initialize(Logger);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            });
        }

        private void OnMessageRecived(byte[] body, Dictionary<string, string> context)
        {
            try
            {
                var message = new PipelineMessage
                {
                    Id = context[WidgetConst.PMSG_ID],
                    Source = context[WidgetConst.PMSGR_ID],
                    Content = Encoding.UTF8.GetString(body),
                };
                PluginObject.ReceiveMessage(message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        private void InitializePlugins()
        {
            Logger.LogTrace(nameof(InitializePlugins));

            if (Plugins == null) return;

            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
            Parallel.ForEach(Plugins, options, p =>
            {
                try
                {
                    p.Initialize(PluginObject);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            });
        }

        private void LoadDomainWidgets()
        {
            Logger.LogTrace(nameof(LoadDomainWidgets));

            DomainWidgets = WidgetContainer.Instance
                .ResolveWidgets<IDomainWidget>()
                .ToArray();
        }

        private void LoadPlugins()
        {
            Logger.LogTrace(nameof(LoadPlugins));

            string pluginPath = Path.Combine(Environment.CurrentDirectory, ContainerConst.PLUGIN_PATH);
            Logger.LogWarning(string.Join(Environment.NewLine, AddInStore.Rebuild(pluginPath)));

            var pluginBag = new ConcurrentBag<IPlugin>();
            ICollection<AddInToken> pluginTokens = AddInStore.FindAddIns(typeof(IPlugin), pluginPath);

            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
            Parallel.ForEach(pluginTokens, options, t =>
            {
                try
                {
                    AppDomain pluginDomain = CreatePluginDomain(t);
                    pluginDomain.DoCallBack(CrossDomainCallback);

                    IPlugin plugin = t.Activate<IPlugin>(pluginDomain);
                    pluginBag.Add(plugin);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);
                }
            });

            Plugins = pluginBag.ToArray();
        }

        private static void CrossDomainCallback()
        {
            // Here's the plugin app domain
            AppDomain pluginDomain = AppDomain.CurrentDomain;

            IDbContext dbContext = LoadDbContextWidget();
            pluginDomain.SetData(WidgetConst.IDBCONTEXT, dbContext);

            IProfile profile = LoadProfileWidget();
            profile.Refresh();
            pluginDomain.SetData(WidgetConst.IPROFILE, profile);

            ILogger logger = InitLogger();
            pluginDomain.SetData(WidgetConst.ILOGGER, logger);
        }

        private static IProfile LoadProfileWidget()
        {
            return WidgetContainer.Instance
                .ResolveWidgets<IProfile>()
                .FirstOrDefault();
        }

        private static IDbContext LoadDbContextWidget()
        {
            return WidgetContainer.Instance
                .ResolveWidgets<IDbContext>()
                .FirstOrDefault();
        }

        private AppDomain CreatePluginDomain(AddInToken token)
        {
            // Here's the console app domain
            // So we need create new plugin domain
            string pluginLocation = GetPluginLocation(token);
            string pluginConfig = $"{pluginLocation}.config";

            var securityInfo = AppDomain.CurrentDomain.Evidence;
            var domainSetup = new AppDomainSetup()
            {
                ConfigurationFile = pluginConfig,
                PrivateBinPath = ContainerConst.WIDGET_PRIVATE_PATH
            };

            return AppDomain.CreateDomain(token.Name, securityInfo, domainSetup);
        }

        private string GetPluginLocation(AddInToken token)
        {
            object tokenAddIn = token.GetField<object>("_addin");
            return tokenAddIn.GetProperty<string>("Location");
        }

    }
}
