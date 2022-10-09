using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaptopConsoleController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI consoleText;
    [SerializeField] TextAsset jsonFileSystem;
    [SerializeField] Font consoleFont;
    private Console console;

    // Start is called before the first frame update
    void Start()
    {
        ConsoleUtilities.consoleFont = consoleFont;
        console = new Console(jsonFileSystem);
        console.addEditableLine();

        updateConsoleText();
    }

    // Update TMP text with current value
    void updateConsoleText()
    {
        consoleText.text = console.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle backspace
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            console.removeLastChar();
            updateConsoleText();
        }

        // Handle command press
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            List<ConsoleLine> result = console.processCommand();
            console.addLines(result);
            console.addEditableLine();
            updateConsoleText();
        }

        // special exception
        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.Semicolon))
        {
            console.addChar(":");
            updateConsoleText();
            return;
        }

        // Handle rest of keys
        foreach(KeyMap m in Console.keys)
        {
            if(Input.GetKeyDown(m.code))
            {
                // Capital letters, else lowercase letters
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    console.addChar(m.letter.ToUpper());
                } else
                {
                    console.addChar(m.letter);
                }
                updateConsoleText();
                return;
            }
        }
    }
}
