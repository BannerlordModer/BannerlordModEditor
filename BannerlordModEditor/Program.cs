using Microsoft.VisualBasic.Logging;
using Velopack;
using Velopack.Sources;

namespace BannerlordModEditor {
    internal static class Program {
        private static async Task UpdateMyApp() {
            var mgr = new UpdateManager(new GithubSource("https://github.com/ModerRAS/WeMeetRecorder", null, false));

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
            VelopackApp.Build().Run();
            Task.Run(async () => {
                try {
                    await UpdateMyApp();
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            });
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainFrom());
        }
    }
}
