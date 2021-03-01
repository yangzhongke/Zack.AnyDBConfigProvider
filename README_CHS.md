[English Version](https://github.com/yangzhongke/Zack.EFCore.Batch/blob/main/README.md)

# Zack.AnyDBConfigProvider
在.NET（.NET Core及.NET Framework）下，从任意关系数据库中加载配置的ConfigurationProvider，支持的数据库包括但不限于SQLServer, MySQL,PostgreSQL, Oracle等。

## 第一步:

在数据库中建一张表，默认名字是T_Configs，这个表名允许自定义为其他名字，具体见后续步骤。表必须有Id、Name、Value三个列，Id定义为整数、自动增长列，Name和Value都定义为字符串类型列，列的最大长度根据系统配置数据的长度来自行确定，Name列为配置项的名字，Value列为配置项的值。
允许具有相同Name的多行数据，其中Id值最大的一条的值生效，这样就实现了简单的配置版本管理。因此，如果不确认一个新的配置项一定成功的话，可以先新增一条同名的配置，如果出现问题，只要把这条数据删除就可以回滚到旧的配置项。
Name列的值遵循.NET中配置的[“多层级数据的扁平化”]( https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0)，如下都是合法的Name列的值：

```
Api:Jwt:Audience
Age
Api:Names:0
Api:Names:1
```

Value列的值用来保存Name类对应的配置的值。Value的值可以是普通的值，也可以使用json数组，也可以是json对象。比如下面都是合法的Value值：:
```
["a","d"]
{"Secret": "afd3","Issuer": "youzack","Ids":[3,5,8]} 
ffff
3
```

下面这个数据就是后续演示使用的数据：

![example data in MySQL](https://raw.githubusercontent.com/yangzhongke/Zack.AnyDBConfigProvider/main/images/datainmysql.png)

## 第二步:
创建一个ASP.NET 项目，演示案例是使用Visual Studio 2019创建.NET Core 3.1的ASP.NET Core MVC项目，但是Zack.AnyDBConfigProvider的应用范围并不局限于这个版本。
通过NuGet安装开发包：

```
Install-Package Zack.AnyDBConfigProvider
```

## 第三步：配置数据库的连接字符串

虽然说项目中其他配置都可以放到数据库中了，但是数据库本身的连接字符串仍然需要单独配置。它既可以配置到本地配置文件中，也可以通过环境变量等方式配置，下面用配置到本地json文件来举例。

打开项目的appsettings.json，增加如下节点：

```
  "ConnectionStrings": {
    "conn1": "Server=127.0.0.1;database=youzack;uid=root;pwd=123456"
  },
```

接下来在Program.cs里的CreateHostBuilder方法的webBuilder.UseStartup<Startup>();之前增加如下代码：

```csharp
webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder)=>{
	var configRoot = configBuilder.Build();
	string connStr = configRoot.GetConnectionString("conn1");
	configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr),reloadOnChange:true,reloadInterval:TimeSpan.FromSeconds(2));
});
```
上面代码的第3行用来从本地配置中读取到数据库的连接字符串，然后第4行代码使用AddDbConfiguration来添加Zack.AnyDBConfigProvider的支持。我这里是使用MySql数据库，所以使用new MySqlConnection(connStr)创建到MySQL数据库的连接，你可以换任何你想使用的其他数据库管理系统。reloadOnChange参数表示是否在数据库中的配置修改后自动加载，默认值是false。如果把reloadOnChange设置为true，则每隔reloadInterval这个指定的时间段，程序就会扫描一遍数据库中配置表的数据，如果数据库中的配置数据有变化，就会重新加载配置数据。AddDbConfiguration方法还支持一个tableName参数，用来自定义配置表的名字，默认名称为T_Configs。
不同版本的开发工具生成的项目模板不一样，所以初始代码也不一样，所以上面的代码也许并不能原封不动的放到你的项目中，请根据自己项目的情况来定制化配置的代码。

## 第四步：
剩下的就是标准的.NET 中读取配置的方法了，比如我们要读取上面例子中的数据，那么就如下配置。
首先创建Ftp类（有IP、UserName、Password三个属性）、Cors类（有string[]类型的Origins、Headers两个属性）。
然后在Startup.cs的ConfigureServices方法中增加如下代码：


```csharp
services.Configure<Ftp>(Configuration.GetSection("Ftp"));
services.Configure<Cors>(Configuration.GetSection("Cors"));
```
然后在Controller中读取配置：

```csharp
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IConfiguration config;
	private readonly IOptionsSnapshot<Ftp> ftpOpt;
	private readonly IOptionsSnapshot<Cors> corsOpt;

	public HomeController(ILogger<HomeController> logger, IConfiguration config, IOptionsSnapshot<Ftp> ftpOpt, IOptionsSnapshot<Cors> corsOpt)
	{
		_logger = logger;
		this.config = config;
		this.ftpOpt = ftpOpt;
		this.corsOpt = corsOpt;
	}

	public IActionResult Index()
	{
		string redisCS = config.GetSection("RedisConnStr").Get<string>();
		ViewBag.s = redisCS;
		ViewBag.ftp = ftpOpt.Value;
		ViewBag.cors = corsOpt.Value;
		return View();
	}
}
```