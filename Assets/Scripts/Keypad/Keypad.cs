using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    public string password = "9090";
    public bool success = false;
    public TextMeshPro screen;
    public int charLimit = 6;

    private string userInput = "";

    // Start function not really needed but likely best practice
    private void Start()
    {
        userInput = "";
    }

    public void ButtonClicked(string keyPress) 
    {
        if (!success) {
            userInput += keyPress;
            if(userInput.Length >= charLimit) 
            {
                screen.SetText("Invalid");
                userInput = "";
            } else 
            {
                if(keyPress == "*" || keyPress == "#")
                {
                    if(userInput == (password + keyPress))
                    {
                        success = true;
                        screen.SetText("Correct");
                    } else 
                    {
                        screen.SetText("Invalid");
                        userInput = "";
                    }
                } else {
                    screen.SetText(userInput);
                }
            }
        }
    }
}
