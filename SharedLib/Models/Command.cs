namespace Share.Models;
public class Command
{
    public Commands cmd { get; set; } = Commands.NONE;
    public string command { get; set; } = string.Empty;
}

public enum Commands
{
    NONE,
    CLS,
    SHELL,
    AUTORIZATION,
    ECHO,
    SQL
}
