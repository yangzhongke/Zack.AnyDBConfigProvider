using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory())   
                   .AddJsonFile("appsettings.json"); 
            //read the connectionstring of database first.
            var configFile = configBuilder.Build();

            string connStr = configFile["DbSettings:ConnectionString"];
            //add the DbConfiguration to configBuilder
            configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr));
            var config = configBuilder.Build();

            var appItems = config.GetSection("Api:AppItems").Get<ApplicationItem[]>();
            //read configuration 
            string[] strs = config.GetSection("Cors:Origins").Get<string[]>();
            Console.WriteLine(string.Join("|", strs.Where(s=>s!=null)));
            var jwt = config.GetSection("Api:Jwt").Get<JWT>();
            Console.WriteLine(jwt.Secret);
            Console.WriteLine(jwt.Audience);
            Console.WriteLine(string.Join(",",jwt.Ids));
            Console.WriteLine(config["Age"]);

            Console.ReadKey();
        }
    }
}
