using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.JSInterop;



using BlazorTerminal.Services;
using Share.Models;
using BlazorTerminal.Tools;
using Figgle;

namespace BlazorTerminal
{
    public class IndexComponent : ComponentBase
    {
        [Inject]
        protected ICommandLineService commandLineService { get; set; }
        public class BoundingClientRect
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public double Top { get; set; }
            public double Right { get; set; }
            public double Bottom { get; set; }
            public double Left { get; set; }
        }

        protected ElementReference myDiv;  // set by the @ref attribute
        protected ElementReference commandLineRef;

        [Inject]
        IJSRuntime? JSRuntime { get; set; }

        public string? mergeString = new string("");
        public List<char> MergeChars = new List<char>();

        public string preCommandLine { get; set; } = "root>";

        private JsBridge? jsBridge = null;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {


            if (firstRender)
            {
                await myDiv.FocusAsync();
                jsBridge = new JsBridge(JSRuntime!);
                if (MergeChars.Count == 0)
                {
                    MergeChars.Add('\u00A0');

                }



                //await JS.InvokeVoidAsync("SetFocusToElement", myDiv);
            }
        }
        public double intX { get; set; } = 0;
        public HubConnection hubConnection;
        public string messages = string.Empty;
        public string username = string.Empty;
        public string message = string.Empty;
        [Inject]
        public NavigationManager? NavigationManager { get; set; }
        public async Task Connection()
        {
            Console.WriteLine("Connection");
            if (hubConnection == null || !IsConnected())
            {
                 hubConnection = new HubConnectionBuilder().WithUrl("https://127.0.0.1:7296/commandhub").Build();
                hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    var result = HtmlConvertString.TerminalStringToHtml(message);
                    outLines.Add(result);
                    StateHasChanged();
                    jsBridge?.Nav_ScrollIntoView("charClass");
                });
                await hubConnection.StartAsync();
            }
        }

        public async Task Send(string from, string sendMess)
        {
            if (hubConnection != null)
            {
                Console.WriteLine("SEEEEEEEEEND");
                await hubConnection.SendAsync("AddMessageToChat", from, sendMess);
                message = string.Empty;
            }
        }

        public bool IsConnected() => hubConnection?.State == HubConnectionState.Connected;
        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("DisposeAsync");
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        public List<string?> outLines { get; set; } = new List<string?>();
        public List<string?> commands { get; set; } = new List<string?>();
        public List<string?> commandsOutResult { get; set; } = new List<string?>();

        public int indexOfPosition = 0;

        public void KeyPress(KeyboardEventArgs e)
        {
            //Console.WriteLine($"press = {e.Code} {e.Key}");
        }
        public void Click(MouseEventArgs e)
        {
            if (MergeChars.Count == 0) MergeChars.Add('\u00A0');
            myDiv.FocusAsync();
        }

        int indexOfCommandList = 0;

        public bool OwnCommands(string commands)
        {
            bool result = false;

            string commandsRes = Tools.HtmlConvertString.HtmlStringToTerminal(commands.Trim().ToUpper());
            if (commandsRes.Equals("CLEAR"))
            {
                outLines.Clear();
                result = true;
            }
            else if (commandsRes.Equals("ABOUTBOX"))
            {
                jsBridge.AboutBox();
                result = true;
            }
            else if (commandsRes.Equals("GET"))
            {
                jsBridge. WinBoxTest();
                result = true;
            }
            return result;
        }

        public void ParsToCode(string outLine)
        {
            var arr = outLine.ToArray();
            foreach (char ch in arr)
            {
                System.Console.WriteLine("ch = " + (int)ch);
            }
        }
        public async void KeyDown(KeyboardEventArgs e)
        {
            double dX = 9.1;


            var t = e.CtrlKey;

            if (MergeChars.Count == 0)
                MergeChars.Add('_');






            if (e.Key.Length > 1)
            {
                if (e.Key.Equals("Enter"))
                {
                    mergeString = new String(MergeChars.ToArray());
                    commands.Add(mergeString);
                    outLines.Add(preCommandLine + mergeString);
                    indexOfPosition = 0;
                    MergeChars = new();
                    MergeChars.Add('\u00A0');
                    indexOfCommandList = commands.Count();
                    mergeString = Tools.HtmlConvertString.HtmlStringToTerminal(mergeString);


                    if (!OwnCommands(mergeString))
                    {

                        await Connection();
                        mergeString = mergeString.Remove(mergeString.Count() - 1);
                        await Send("fromMe", mergeString);
                        jsBridge?.Nav_ScrollIntoView("charClass");


                    }



                }
                else if (e.Key.Equals("Backspace") && indexOfPosition > 0 && MergeChars.Count > 0)
                {
                    if (indexOfPosition > 0)
                    {

                        MergeChars.RemoveAt(indexOfPosition - 1);
                        indexOfPosition--;
                    }
                }
                else if (e.Key == "ArrowLeft" && indexOfPosition > 0)
                {
                    intX -= dX;
                    indexOfPosition--;
                }
                else if (e.Key == "ArrowRight" && indexOfPosition < (MergeChars.Count - 1))
                {
                    intX += dX;
                    indexOfPosition++;
                }
                else if (e.Key == "Home" && indexOfPosition > 0)
                {
                    indexOfPosition = 0;
                }
                else if (e.Key == "End" && indexOfPosition < (MergeChars.Count - 1))
                {
                    indexOfPosition = MergeChars.Count - 1;
                }
                else if (e.Key == "ArrowUp")
                {
                    if (commands.Count() > 0)
                    {
                        MergeChars = commands[indexOfCommandList != 0 ? --indexOfCommandList : (indexOfCommandList = commands.Count() - 1)]?.ToList() ?? new List<char>();
                        indexOfPosition = MergeChars.Count - 1;
                    }
                }


            }






            else if (e.Key.Equals("Backspace"))
            {
                if (indexOfPosition > 0)
                {
                    MergeChars.RemoveAt(indexOfPosition);
                    indexOfPosition--;
                }



            }
            else
            {


                if (indexOfPosition == MergeChars.Count)
                {
                    //MergeChars.Add((char)e.Key.ToArray().GetValue(0));
                }
                else
                {

                    char ch = (char)e.Key.ToArray().GetValue(0);
                    if (ch == ' ')
                        ch = '\u00A0';
                    MergeChars.Insert(indexOfPosition, ch);
                }

                indexOfPosition++;

            }

        }
    }
}
