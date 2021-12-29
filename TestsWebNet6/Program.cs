using MySql.Data.MySqlClient;
using TestsWebNet6;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var webBuilder = builder.Host;
webBuilder.ConfigureAppConfiguration((hostCtx, configBuilder) => {
    var configRoot = builder.Configuration;
    string connStr = configRoot.GetConnectionString("conn1");
    configBuilder.AddDbConfiguration(() => new MySqlConnection(connStr), reloadOnChange: true, reloadInterval: TimeSpan.FromSeconds(2));
});
builder.Services.Configure<Ftp>(builder.Configuration.GetSection("Ftp"));
builder.Services.Configure<Cors>(builder.Configuration.GetSection("Cors"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
