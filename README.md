# Zack.AnyDBConfigProvider
ConfigurationProvider for loading configuration from any database, including but not limited to SQLServer, MySQL,PostgreSQL, Oracle, etc.

Step One:

Create a table for retrieving configuration data from database.
The table name is 't_configs', which can be changed to other name with further configuration.
The table must have there columns: Id(int, autoincrement), Name(text/varchar/nvarchar), Value(text/varchar/nvarchar).

Multi rows of the same 'Name' value is allowed, and the row with the maximum id value takes effect , which makes version controlling possible.
The value of column 'Name' conform with (the flattening of hierarchical data)[https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#optpat], for example:

```
Api:Jwt:Audience
Age
Api:Names:0
Api:Names:1
```

The 'Value' column is for storing value of the corresponding value associated with that 'Name'.
The 'Value' can be plain value, string value of json array, and string value of json object. for example:

```
["a","d"]
{"Secret": "afd3","Issuer": "youzack","Ids":[3,5,8]} 
ffff
3
```

![example data in MySQL](https://raw.githubusercontent.com/yangzhongke/Zack.AnyDBConfigProvider/main/images/datainmysql.png)

Step Two:
Install the package:

```
Install-Package Zack.AnyDBConfigProvider
```

Step Three:

```csharp
ConfigurationBuilder configBuilder = new ConfigurationBuilder();
configBuilder.SetBasePath(Directory.GetCurrentDirectory())   
	   .AddJsonFile("appsettings.json"); 
//read the connectionstring of database first.
var configFile = configBuilder.Build();

string connStr = configFile["DbSettings:ConnectionString"];
//add the DbConfiguration to configBuilder
configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr));
var config = configBuilder.Build();

//read configuration 
string[] strs = config.GetSection("Cors:Origins").Get<string[]>();
Console.WriteLine(string.Join("|", strs));
var jwt = config.GetSection("Api:Jwt").Get<JWT>();
Console.WriteLine(jwt.Secret);
Console.WriteLine(jwt.Audience);
Console.WriteLine(string.Join(",",jwt.Ids));
Console.WriteLine(config["Age"]);
```

![output](https://raw.githubusercontent.com/yangzhongke/Zack.AnyDBConfigProvider/main/images/result.png)
