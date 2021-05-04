using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Streaming.Core;
using Streaming.Core.Interfaces;
using Streaming.WpfApp.Interfaces;
using Streaming.WpfApp.ViewModels;

namespace Streaming.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ConfigureLogger();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt")
                .CreateLogger();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logger => logger.AddSerilog());
            services.AddSingleton<Views.MainWindow>();
            services.AddSingleton<IMainWindowViewModel,MainWindowViewModel>();
            services.AddSingleton<IHtmlContentLoader, HtmlContentLoader>();
            services.AddSingleton<ILinkParser, CameraInfoParser>();
            services.AddSingleton<ILinkContainer, LinkContainer>();           
            services.AddSingleton<IBackgroundProcess, BackgroundProcess>();
            services.AddSingleton<ISeparateProcessFactory, SeparateProcessFactory>();
           
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<Views.MainWindow>();
            mainWindow.Show();
        }
    }
}
