
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using WebApiCommandLine.Tools;

namespace WebApiCommandLine.Hubs;

public class CommandHub : Hub
{
/*
    public CommandHub(HttpContext httpContext){
        Console.WriteLine("CommandHub");
        shellRuntime = httpContext.RequestServices.GetService<ShellRuntime>()!;
    }

    public CommandHub(){

    }*/

    public  ShellRuntime? shellRuntime;
    public override async Task OnConnectedAsync(){

        if (shellRuntime == null) shellRuntime = Context.GetHttpContext()?.RequestServices.GetService<ShellRuntime>();
        //await AddMessageToChat("", "User connected!");
        await base.OnConnectedAsync();
        try{
             shellRuntime?.CreateInteractiveSession();
        } catch(Exception exp){
            System.Console.WriteLine($"shellRuntime exception exp = {exp.Message}");
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("ON_DISCONNECTED_ASYNC");
        return base.OnDisconnectedAsync(exception);
    }
    public async Task AddMessageToChat(string user, string message)
    {
        //Console.WriteLine($"addMessageToChat {user} 11 {message}");
        try{
            if (shellRuntime == null) shellRuntime = this.Context.GetHttpContext()?.RequestServices.GetService<ShellRuntime>();
            shellRuntime?.SendCommand(message);
        } catch(Exception exp){
            System.Console.WriteLine($"AddMessageToChat exception {message} exp = {exp.Message}");
        }
    }

    


}