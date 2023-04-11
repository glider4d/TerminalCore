using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using WebApiCommandLine.Hubs;
using WebApiCommandLine.Tools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
 
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
 builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddSingleton(provider =>
{
    var hubContext = provider.GetService<IHubContext<CommandHub>>();
    var updateRandomNumber = new UpdateRandomNumber(hubContext);
    return updateRandomNumber;
});
 
builder.Services.AddSingleton<ShellRuntime>();
var rs = OperatingSystem.IsLinux();

rs = OperatingSystem.IsWindows();



var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Shows UseCors with CorsPolicyBuilder.
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<CommandHub>("/commandhub");
app.MapFallbackToFile("index.hrml");

 app.Services.GetService<ShellRuntime>()!.Test();
 
app.Run();


public class UpdateRandomNumber
{
    private bool _continue = true;
    private IHubContext<CommandHub> testHub;
    private Task randomNumberTask;
    public UpdateRandomNumber()
    {
        Console.WriteLine("WITHOUT");
    }
    public UpdateRandomNumber(IHubContext<CommandHub> testHub)
    {
        Console.WriteLine("UpdateRandomNumber!!");
        this.testHub = testHub;
        randomNumberTask = new Task(() => RandomNumberLoop(),
            TaskCreationOptions.LongRunning);
        randomNumberTask.Start();
    }
    private async void RandomNumberLoop()
    {
        Random r = new Random();

        while (_continue)
        {
            Thread.Sleep(3000);
            int number = r.Next(0, 100);
            // Send new random number to connected subscribers here
            await testHub.Clients.All.SendAsync($"ReceiveRandomNumber", number);

        }
    }

    public void Stop()
    {
        _continue = false;
    }
}