using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

class ConsoleUtilities
{
    public static Font consoleFont;

    /// <summary>
    /// Converts potentially unsafe string to safe string.
    /// Useful with encrypted text which may include control characters (e.g. 0x00)
    /// </summary>
    /// <param name="s">String to parse</param>
    /// <returns>Safe string, all characters not in console font are changed to ?</returns>
    public static string parseString(string s)
    {
        char[] st = s.ToCharArray();
        string output = "";
        foreach (char c in st)
        {
            if (consoleFont.HasCharacter(c))
            {
                output += c;
            }
            else
            {
                output += "?";
            }
        }
        return output;
    }

    /// <summary>
    /// Decrypt text using XOR cipher
    /// </summary>
    /// <param name="key">Decryption key</param>
    /// <param name="content">Encoded content</param>
    /// <returns>Decrypted content</returns>
    public static string decryptText(string key, string content)
    {
        byte[] byteKey = Encoding.ASCII.GetBytes(key);
        byte[] byteContent = Encoding.ASCII.GetBytes(content);

        for (int i = 0; i < byteContent.Length; i++)
        {
            byteContent[i] ^= byteKey[i % byteKey.Length];
        }
        return Encoding.ASCII.GetString(byteContent);
    }
}
