namespace BlazorTerminal.Tools
{
    public class HtmlConvertString
    {
        public static string HtmlStringToTerminal(string command)
        {
            command = command.Replace('\u00A0', ' ');
            return command;
        }

        public static string TerminalStringToHtml(string command)
        {
            command = command.TrimEnd();
            command = command.Replace(' ', '\u00A0');
            command = command.Replace("" + '\n', "<br>");
            return command;
        }
    }
}