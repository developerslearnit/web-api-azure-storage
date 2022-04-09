using Azure.Storage.Blobs;
using BlobStorage.API.Servies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

//var config = new ConfigurationBuilder()
//    .SetBasePath(Directory.GetCurrentDirectory())
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddEnvironmentVariables()
//    .Build();

//var bconfig = config.GetSection("Azure:AzureBlobStorageConnectionString");

builder.Services.AddSingleton(x =>
            new BlobServiceClient(
                builder.Configuration.GetSection("Azure:AzureBlobStorageConnectionString").Value)
            );
builder.Services.AddSingleton<IAzureStorageService, AzureStorageService>();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();



app.Run();

