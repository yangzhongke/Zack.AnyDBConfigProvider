using System;
using System.Data;

namespace Microsoft.Extensions.Configuration
{
    public class DBConfigOptions
    {
        public Func<IDbConnection> CreateDbConnection { get; set; }
        public string TableName { get; set; } = "T_Configs";
    }
}
