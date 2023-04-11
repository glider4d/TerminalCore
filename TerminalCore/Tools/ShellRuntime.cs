using System.Diagnostics;
using System.Text;
using WebApiCommandLine.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;

namespace WebApiCommandLine.Tools
{
    public class ShellRuntime
    {
        readonly public CommandHub commandHub;
        private readonly IHubContext<CommandHub> _notificationHubContext;
        public void Test()
        {
            Console.WriteLine("TTTTTTTTEEEEEEESTST");
        }
        public ShellRuntime(CommandHub commandHub)
        {
            Console.WriteLine("lalala");
            this.commandHub = commandHub;
        }
        public ShellRuntime(IHubContext<CommandHub> notificationHubContext)
        {
            Console.WriteLine("SHELL_RUNTIME");
            _notificationHubContext = notificationHubContext;
        }

        private StringBuilder? sortOutput = null;
        private StreamWriter? sortStreamWriter = null;
        private Process? processShell = null;
        public void CreateInteractiveSession()
        {


            processShell = new Process();
            /*
            StartInfo = Environment.OSVersion.Platform == PlatformID.Win32NT ? new ProcessStartInfo
            {


                FileName = "cmd.exe",
                Arguments = $"/K \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                CreateNoWindow = true,
            } */

processShell.StartInfo.FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ?  "cmd.exe" : "/bin/bash";
            processShell.StartInfo.UseShellExecute = false;
            processShell.StartInfo.RedirectStandardOutput = true;
            sortOutput = new StringBuilder();

            processShell.OutputDataReceived += ShellOutputHandler;
            processShell.ErrorDataReceived += ShellErrorHandler;

            processShell.StartInfo.RedirectStandardInput = true;
            processShell.StartInfo.RedirectStandardError = true;
            bool IsStartSuccess = processShell.Start();
            sortStreamWriter = processShell.StandardInput;

            processShell.BeginOutputReadLine();
            processShell.BeginErrorReadLine();

            System.Console.WriteLine("CreateInteractiveSession");
        }

        public void SendCommand(String command)
        {
            try
            {
                Console.WriteLine("SendCommand = " + command + " " + command.ToUpper().Equals("CLOSE") + " command.len " + command.Length + " len2=" + "CLOSE".Length);
                if (command.ToUpper().Equals("CLOSE"))
                {
                    //processShell.stop
                    Console.WriteLine("CLOSE");
                    Console.WriteLine("processResponding = " + processShell?.Responding + " " + processShell?.HasExited);

                    processShell?.StandardInput.WriteLine("\x3");
                    processShell?.Close();
                    processShell?.Kill();
                    Console.WriteLine("2processResponding = " + processShell?.Responding + " " + processShell?.HasExited);

                    //processShell.Kill();
                    //processShell.StandardInput.Close();
                    //processShell.Close();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("HAS EXITED " + processShell?.HasExited);// + " EXIT CODE: " + processShell.ExitCode);
                    Console.WriteLine();

                    if (sortStreamWriter == null) Console.WriteLine("== null");
                    Console.WriteLine("processResponding = " + processShell.Responding);
                    sortStreamWriter.WriteLine(command);




                    Console.WriteLine("redirectStandardError = " + processShell.StartInfo.RedirectStandardError +
                    " redirectStandardOutput = " + processShell.StartInfo.RedirectStandardOutput +
                    " redirectStandardInput = " + processShell.StartInfo.RedirectStandardInput);
                    //processShell.StandardInput = 
                    //sortStreamWriter = processShell.StandardInput;
                    //Console.WriteLine(" hash" + processShell.StandardError.GetHashCode());
                    /*
                    Console.WriteLine(
                    "\n input = " + processShell.StandardInput.GetHashCode() +
                    "\n output = " + processShell.StandardOutput.GetHashCode() +
                    "\n error = " + processShell.StandardError.GetHashCode() +
                    "\n writer = " +  sortStreamWriter.GetHashCode()
                    );*/

                    Console.WriteLine("out");
                }
            }
            catch (Exception e)
            {
                CreateInteractiveSession();
            }
        }
        public HubConnection hubConnection;
        public bool IsConnected() => hubConnection?.State == HubConnectionState.Connected;

        public async Task Connection()
        {
            Console.WriteLine("Connection");
            if (hubConnection == null || !IsConnected())
            {
                hubConnection = new HubConnectionBuilder().WithUrl("https://127.0.0.1:7296/commandhub").Build();

                await hubConnection.StartAsync();
            }
        }
        public async Task Send(string from, string sendMess)
        {
            Console.WriteLine("SEND !!!!!!!!!!!!!");
            if (hubConnection == null || !IsConnected())
            {
                Console.WriteLine("before");
                await Connection();
                Console.WriteLine("after");
            }
            Console.WriteLine("FROM SERVER");
            await hubConnection.SendAsync("AddMessageToChat", "SERVER:FROM", sendMess);
        }

        public void ShellOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //commandHub.AddMessageToChat(outLine.Data);
            //Task.Run(() => commandHub.AddMessageToChat("",outLine.Data));
            // Send("", outLine.Data);
            // commandHub.SendToClients(outLine.Data);
            _notificationHubContext.Clients.All.SendAsync("ReceiveMessage", "", outLine.Data);

            Console.WriteLine("OUTPUT " + outLine.Data);
        }

        public void ShellErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //commandHub.SendToClients(outLine.Data);
            // Send("", outLine.Data);
            //commandHub.AddMessageToChat("", outLine.Data);
            Console.WriteLine("ERROR = " + outLine.Data);
            _notificationHubContext.Clients.All.SendAsync("ReceiveMessage", "", outLine.Data);
        }
    }
}