using AutoMapper;
using BasicDelivery.HubConfig;
using BasicDelivery.Infrastucture.Configuration;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register database
builder.Services.RegisterContextDb(builder.Configuration);
//register Dependency Injection
builder.Services.RegisterDI();
//add token bear    
builder.Services.RegisterTokenBear(builder.Configuration);
//add controller config 
builder.Services.RegisterController();
// add connectAngular
builder.Services.RegisterConnectAngular();
// add signalR
builder.Services.RegisterSignalR();
builder.Services.AddControllers();
// get claim 
builder.Services.AddHttpContextAccessor();
//register serilog
builder.Host.UseSerilog((ctx, config) =>
{
    config.WriteTo.Console().MinimumLevel.Information();
    config.WriteTo.File(
    path: AppDomain.CurrentDomain.BaseDirectory + "/logs/log-.txt",
    rollingInterval: RollingInterval.Day,
    rollOnFileSizeLimit: true,
    formatter: new JsonFormatter()).MinimumLevel.Information();
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// su dung serilog 
app.UseSerilogRequestLogging();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "UploadFile")),
    RequestPath = "/UploadFile"
});
app.UseHttpsRedirection();

app.UseCors("WebBasicDelivery");

// dat authen truoc author
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MyHub>("/toastr");


app.Run();
