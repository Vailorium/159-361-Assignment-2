using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

class Console
{
    List<ConsoleLine> lines;

    public static KeyMap[] keys = new KeyMap[]{
            new KeyMap(KeyCode.A, "a"),
            new KeyMap(KeyCode.B, "b"),
            new KeyMap(KeyCode.C, "c"),
            new KeyMap(KeyCode.D, "d"),
            new KeyMap(KeyCode.E, "e"),
            new KeyMap(KeyCode.F, "f"),
            new KeyMap(KeyCode.G, "g"),
            new KeyMap(KeyCode.H, "h"),
            new KeyMap(KeyCode.I, "i"),
            new KeyMap(KeyCode.J, "j"),
            new KeyMap(KeyCode.K, "k"),
            new KeyMap(KeyCode.L, "l"),
            new KeyMap(KeyCode.M, "m"),
            new KeyMap(KeyCode.N, "n"),
            new KeyMap(KeyCode.O, "o"),
            new KeyMap(KeyCode.P, "p"),
            new KeyMap(KeyCode.Q, "q"),
            new KeyMap(KeyCode.R, "r"),
            new KeyMap(KeyCode.S, "s"),
            new KeyMap(KeyCode.T, "t"),
            new KeyMap(KeyCode.U, "u"),
            new KeyMap(KeyCode.V, "v"),
            new KeyMap(KeyCode.W, "w"),
            new KeyMap(KeyCode.X, "x"),
            new KeyMap(KeyCode.Y, "y"),
            new KeyMap(KeyCode.Z, "z"),
            new KeyMap(KeyCode.Alpha0, "0"),
            new KeyMap(KeyCode.Alpha1, "1"),
            new KeyMap(KeyCode.Alpha2, "2"),
            new KeyMap(KeyCode.Alpha3, "3"),
            new KeyMap(KeyCode.Alpha4, "4"),
            new KeyMap(KeyCode.Alpha5, "5"),
            new KeyMap(KeyCode.Alpha6, "6"),
            new KeyMap(KeyCode.Alpha7, "7"),
            new KeyMap(KeyCode.Alpha8, "8"),
            new KeyMap(KeyCode.Alpha9, "9"),
            new KeyMap(KeyCode.Space, " "),
            new KeyMap(KeyCode.Period, "."),
            new KeyMap(KeyCode.KeypadPeriod, "."),
            new KeyMap(KeyCode.Slash, "/")
        };

    public FileSystem fileSystem;
    public Console(TextAsset systemJson)
    {
        this.lines = new List<ConsoleLine>();

        // Load file structure from JSON file
        Folder rootFolder = JsonConvert.DeserializeObject<Folder>(systemJson.text);
        this.fileSystem = new FileSystem(rootFolder);
    }

    public void addLine(string uneditableText, string editableText = "")
    {
        this.lines.Add(new ConsoleLine(uneditableText, editableText));
    }

    public void addLine(ConsoleLine line)
    {
        this.lines.Add(line);
    }

    public void addLines(IEnumerable lines)
    {
        foreach (ConsoleLine line in lines)
        {
            this.addLine(line);
        }
    }

    public override string ToString()
    {
        string output = "";
        foreach (ConsoleLine line in this.lines)
        {
            output += line.ToString();
            output += "\n";
        }
        return output;
    }

    /// <summary>
    /// Gets current line in console
    /// </summary>
    /// <returns>Current line</returns>
    private ConsoleLine getLastLine()
    {
        return this.lines[this.lines.Count - 1];
    }

    /// <summary>
    /// Adds character to current line
    /// </summary>
    /// <param name="s">Character to add</param>
    public void addChar(string s)
    {
        this.getLastLine().editableText += s;
    }

    /// <summary>
    /// Removes the editable character in the last position in the current line's editable text
    /// </summary>
    public void removeLastChar()
    {
        ConsoleLine line = this.getLastLine();
        if (line.editableText.Length > 0)
        {
            line.editableText = line.editableText.Remove(line.editableText.Length - 1);
        }
    }

    /// <summary>
    /// Help menu for console
    /// </summary>
    /// <returns>Help menu console lines</returns>
    private List<ConsoleLine> helpCommand()
    {
        List<ConsoleLine> commands = new List<ConsoleLine>();
        commands.Add(new ConsoleLine("help: Displays usable commands"));
        commands.Add(new ConsoleLine("decrypt <key> <filename>: Decrypts file and displays contents"));
        commands.Add(new ConsoleLine("ls: Displays files and sub-folders in current folder"));
        commands.Add(new ConsoleLine("read <filename>: Reads file contents"));
        commands.Add(new ConsoleLine("cd <path>: paths to directory"));
        return commands;
    }

    /// <summary>
    /// Process command that user has entered
    /// </summary>
    /// <returns>List of lines to print</returns>
    public List<ConsoleLine> processCommand()
    {
        // get command from the last line the user typed
        string command = this.getLastLine().editableText;

        // split command into args
        string[] args = command.Split(" ");

        List<ConsoleLine> returnLines = new List<ConsoleLine>();
        if (args.Length > 0)
        {
            switch (args[0].Trim())
            {
                case "help":
                    returnLines.AddRange(helpCommand());
                    break;
                case "decrypt":
                    if (args.Length < 3)
                    {
                        returnLines.Add(new ConsoleLine("Insufficient number of arguments"));
                        break;
                    }
                    else
                    {
                        string key = args[1];
                        string filename = args[2];
                        try
                        {
                            string content = fileSystem.getFromPath().files[filename].content;
                            string decrypted = ConsoleUtilities.decryptText(key, content);
                            returnLines.Add(new ConsoleLine("Decrypted Content:"));
                            returnLines.Add(new ConsoleLine(ConsoleUtilities.parseString(decrypted)));
                            break;
                        }
                        catch (KeyNotFoundException)
                        {
                            returnLines.Add(new ConsoleLine("File not found"));
                            break;
                        }
                    }
                case "ls":
                    List<string> itemsInFolder = fileSystem.getFromPath().getItems();
                    foreach (string item in itemsInFolder)
                    {
                        returnLines.Add(new ConsoleLine(item));
                    }
                    break;
                case "read":
                    if (args.Length < 2)
                    {
                        returnLines.Add(new ConsoleLine("Insufficient number of arguments"));
                        break;
                    }
                    try
                    {
                        string content = fileSystem.getFromPath().files[args[1]].content;
                        returnLines.Add(new ConsoleLine(ConsoleUtilities.parseString(content)));
                        break;
                    }
                    catch (KeyNotFoundException)
                    {
                        returnLines.Add(new ConsoleLine("File not found"));
                        break;
                    }
                case "cd":
                    if (args.Length < 2)
                    {
                        returnLines.Add(new ConsoleLine("Insufficient number of arguments"));
                        break;
                    }
                    bool result = fileSystem.navigateTo(args[1]);
                    if (result == true)
                    {
                        break;
                    }
                    else
                    {
                        returnLines.Add(new ConsoleLine(string.Format("Cannot path to {0}", args[1])));
                        break;
                    }
                default:
                    returnLines.Add(
                        new ConsoleLine(
                            string.Format("{0} is not recognized as an internal or external command, operable program or batch file.\nEnter 'help' to see help.", args[0])
                        )
                    );
                    break;
            }
            return returnLines;
        }
        returnLines.Add(
                        new ConsoleLine(
                            string.Format(" is not recognized as an internal or external command, operable program or batch file.\nEnter 'help' to see help."),
                            ""
                        )
                    );
        return returnLines;
    }

    public void addEditableLine()
    {
        this.addLine(this.fileSystem.getPathString() + ">", "");
    }
}
