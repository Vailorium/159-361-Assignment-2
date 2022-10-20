using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Keypad : MonoBehaviour
{
    public UnityEvent OnEntryAllowed;
    public string password = "9090";
    public bool success = false;
    public TextMeshPro screen;
    public int charLimit = 6;

    public AudioClip clickSound;
    public AudioClip invalidSound;
    public AudioClip validSound;
    AudioSource audioSource;

    private string userInput = "";

    // Start function not really needed but likely best practice
    private void Start()
    {
        userInput = "";
        audioSource = GetComponent<AudioSource>();
    }

    public void ButtonClicked(string keyPress) 
    {
        // if keypad hasn't already been completed
        if (!success) {
            userInput += keyPress;

            // If keypad has gone past char limit, reset and display invalid
            if(userInput.Length > charLimit && keyPress != "*" && keyPress != "#") 
            {
                screen.SetText("Invalid");
                userInput = "";
                audioSource.PlayOneShot(invalidSound);
            } else 
            {
                // if user has entered code, verify if its correct or not
                if(keyPress == "*" || keyPress == "#")
                {
                    if(userInput == (password + keyPress))
                    {
                        success = true;
                        screen.SetText("Correct");
                        audioSource.PlayOneShot(validSound);
                        OnEntryAllowed.Invoke();
                    } else 
                    {
                        screen.SetText("Invalid");
                        userInput = "";
                        audioSource.PlayOneShot(invalidSound);
                    }
                } else {
                    // otherwise add character to code
                    screen.SetText(userInput);
                    audioSource.PlayOneShot(clickSound);
                }
            }
        }
    }
}
