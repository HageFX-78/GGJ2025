using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnumHelper
{
    // private static HashSet<string> existingEnumNames = new HashSet<string>(); // Store used names
    private static readonly HashSet<string> csharpKeywords = new HashSet<string>
    {
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class",
        "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event",
        "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
        "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new",
        "null", "object", "operator", "out", "override", "params", "private", "protected", "public",
        "readonly", "ref", "remove", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
        "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
        "ulong", "unchecked", "unsafe", "ushort", "using", "var", "virtual", "void", "volatile", "while",
        // Add any other C# keywords as needed
    };

    public static bool TryMakeStringEnumCompatible(string name, out string result)
    {
        result = string.Empty;

        if (string.IsNullOrEmpty(name))
        {
            return false; // Invalid input
        }

        StringBuilder returnString = new StringBuilder();

        // Ensure first character is a letter or underscore
        if (!char.IsLetter(name[0]) && name[0] != '_')
        {
            returnString.Append('_'); // Start with underscore if not valid
        }

        // Strip out invalid characters
        foreach (char c in name)
        {
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                returnString.Append(c);
            }
            else if (c == '-')
            {
                returnString.Append('_'); // Replace spaces with underscores
            }
        }

        // Convert to string and check for emptiness
        result = returnString.ToString();
        if (string.IsNullOrEmpty(result))
        {
            return false; // Generated name is invalid
        }

        // // Avoid duplicates
        // if (existingEnumNames.Contains(result))
        // {
        //     int counter = 1;
        //     string newName;

        //     // Generate a new name to avoid duplicates
        //     do
        //     {
        //         newName = result + "_" + counter++;
        //     } while (existingEnumNames.Contains(newName));

        //     result = newName;
        // }

        // Handle C# keywords
        if (csharpKeywords.Contains(result))
        {
            result = "_" + result; // Prefix with underscore
        }

        // Add the final name to the set of existing names
        //existingEnumNames.Add(result);
        return true; // Success
    }
}