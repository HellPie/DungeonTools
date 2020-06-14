using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DungeonTools.Server {
    public static class Program {
        public static async Task Main(string[] args) {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(i => {
                    i.ClearProviders();
                    i.AddConsole(o => o.IncludeScopes = true);
                    i.AddDebug();
                })
                .ConfigureWebHostDefaults(i => i.UseStartup<Startup>());
        }
    }
}
