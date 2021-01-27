using Microsoft.Extensions.Configuration;

namespace Zack.AnyDBConfigProvider
{
    class DBConfigurationSource : IConfigurationSource
    {
        private DBConfigOptions options;
        public DBConfigurationSource(DBConfigOptions options)
        {
            this.options = options;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DBConfigurationProvider(options);
        }
    }
}
