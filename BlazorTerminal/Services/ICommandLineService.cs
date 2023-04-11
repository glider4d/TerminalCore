using Share.Models;

namespace BlazorTerminal.Services
{
    public interface ICommandLineService
    {
        Task<Out> CommandEnter(Command Input);
    }
}