using System.Net.Http.Json;
using Share.Models;

namespace BlazorTerminal.Services
{
    public class CommandLineService : ICommandLineService
    {
        private readonly HttpClient _httpClient;
        public CommandLineService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Out> CommandEnter(Command Input)
        {
            var result = await _httpClient.GetFromJsonAsync<Out>($"/CommandLine/commandexec?Input={Input.command}") ?? new Out();
            Console.WriteLine("message = " + result.Message);
            return result;
        }
    }
}