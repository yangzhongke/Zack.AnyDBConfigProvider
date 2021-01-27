using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory())   
                //指定配置文件所在的目录
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);  //指定加载的配置文件
            var configFile = configBuilder.Build();
            string connStr = configFile["DbSettings:ConnectionString"];
            configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr));
            var config = configBuilder.Build();

            string[] strs = config.GetSection("Cors:Origins").Get<string[]>();
            Console.WriteLine(string.Join("|", strs));
            var jwt = config.GetSection("Api:Jwt").Get<JWT>();
            Console.WriteLine(jwt.Secret);
            Console.WriteLine(jwt.Audience);
            Console.WriteLine(string.Join(",",jwt.Ids));
            Console.WriteLine(config["Id"]);
            Console.ReadKey();
        }
    }
}
