using System;
using System.Data;
using Zack.AnyDBConfigProvider;

namespace Microsoft.Extensions.Configuration
{
    public static class DBConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder,
            DBConfigOptions setup)
        {
            return
                builder.Add(new DBConfigurationSource(setup));
        }

        public static IConfigurationBuilder AddDbConfiguration(this IConfigurationBuilder builder, Func<IDbConnection> createDbConnection, string tableName= "T_Configs")
        {
            return AddDbConfiguration(builder, new DBConfigOptions {CreateDbConnection=createDbConnection,TableName=tableName });
        }
    }
}
