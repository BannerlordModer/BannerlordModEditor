using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Velopack;
using Velopack.Sources;

namespace BannerlordModEditor {
    internal static class Program {
        public static IHost host { get; private set; }
        private static async Task UpdateMyApp() {
            var mgr = new UpdateManager(new GithubSource("https://github.com/BannerlordModer/BannerlordModEditor", null, false));

            // check for new version
            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
                return; // no update available


            // download new version
            await mgr.DownloadUpdatesAsync(newVersion);

            // install new version and restart app
            mgr.ApplyUpdatesAndRestart(newVersion);
        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            VelopackApp.Build().Run();

            host = Host.CreateDefaultBuilder()
                .ConfigureServices(service => {
                    service.AddSingleton(service);
                    service.AddSingleton<MainForm>();
#if DEBUG
                    service.AddBlazorWebViewDeveloperTools();
#endif
                })
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.AddSimpleConsole(options => {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "[yyyy/MM/dd HH:mm:ss] ";
                    });
#if DEBUG
                    logging.AddDebug();
#endif
                }).Build();

            Task.Run(async () => {
                try {
                    await UpdateMyApp();
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            });
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(host.Services.GetService<MainForm>());
        }
    }
}
