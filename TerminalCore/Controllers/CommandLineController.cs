using consoleCallTerminal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Share.Models;
using WebApiCommandLine.Hubs;

namespace TerminalCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandLineController : ControllerBase
    {
        private readonly ILogger<CommandLineController> _logger;
        private readonly IHubContext<CommandHub> _notificationHubContext;

        public CommandLineController(ILogger<CommandLineController> logger, IHubContext<CommandHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
            _logger = logger;
        }



        [HttpGet(nameof(CommandExec))]
        public async Task<Out> CommandExec(string Input) =>
            await Task.Run(() => new Out { Message = ShellHelper.Bash(Input) });

        [HttpGet]
        public async Task<string> ReturnString(string test)
        {
            return String.Format($"test =  {test}");
        }


    }
}