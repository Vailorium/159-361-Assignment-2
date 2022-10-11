using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMap
{
    public KeyCode code;
    public string letter;

    public KeyMap(KeyCode code, string letter)
    {
        this.code = code;
        this.letter = letter;
    }
}