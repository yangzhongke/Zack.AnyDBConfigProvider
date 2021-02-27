using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System;

namespace TestsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //add begin
                    webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder)=>{
                        var configRoot = configBuilder.Build();
                        string connStr = configRoot.GetConnectionString("conn1");
                        configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr),reloadOnChange:true,reloadInterval:TimeSpan.FromSeconds(2));
                    });
                    //end
                    webBuilder.UseStartup<Startup>();
                });
    }
}
