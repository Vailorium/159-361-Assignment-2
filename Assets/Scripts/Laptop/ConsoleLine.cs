using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ConsoleLine
{
    string uneditableText;
    public string editableText;

    public ConsoleLine(string uneditableText)
    {
        this.uneditableText = uneditableText;
        this.editableText = "";
    }

    public ConsoleLine(string uneditableText, string editableText)
    {
        this.editableText = editableText;
        this.uneditableText = uneditableText;
    }

    public override string ToString()
    {
        return uneditableText + editableText;
    }
}
