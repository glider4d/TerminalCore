using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters;

namespace consoleCallTerminal
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = "";
            string gwegwe = 2 == 3 ? "" : "";

            try
            {
                var process = new Process()
                {
                    
                    StartInfo = Environment.OSVersion.Platform == PlatformID.Win32NT ? new ProcessStartInfo
                    {
                        /*
                         /*
                         flnEx = "cmd.exe";
                    cmndEx = "/K " + Command;
                         */
                        
                        FileName = "cmd.exe",
                        Arguments = $"/K \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                    } :
                    new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                    }
                };

                process.Start();
                result = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                result = e.Message;
            }
            return result;
        }
    }
}