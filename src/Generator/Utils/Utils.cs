﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cxxi
{
    public static class IntHelpers
    {
        public static bool IsPowerOfTwo(this long x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
    }

    public static class PrimitiveTypeExtensions
    {
        public static System.Type ConvertToType(this PrimitiveType primitive)
        {
            switch (primitive)
            {
                case PrimitiveType.Bool: return typeof(bool);
                case PrimitiveType.Void: return typeof(void);
                case PrimitiveType.WideChar: return typeof(char);
                case PrimitiveType.Int8: return typeof(sbyte);
                case PrimitiveType.UInt8: return typeof(byte);
                case PrimitiveType.Int16: return typeof(short);
                case PrimitiveType.UInt16: return typeof(ushort);
                case PrimitiveType.Int32: return typeof(int);
                case PrimitiveType.UInt32: return typeof(uint);
                case PrimitiveType.Int64: return typeof(long);
                case PrimitiveType.UInt64: return typeof(ulong);
                case PrimitiveType.Float: return typeof(float);
                case PrimitiveType.Double: return typeof(double);
            }

            return typeof(int);
        }
    }

    public static class StringHelpers
    {
        public static string CommonPrefix(this string[] ss)
        {
            if (ss.Length == 0)
            {
                return "";
            }

            if (ss.Length == 1)
            {
                return ss[0];
            }

            int prefixLength = 0;

            foreach (char c in ss[0])
            {
                foreach (string s in ss)
                {
                    if (s.Length <= prefixLength || s[prefixLength] != c)
                    {
                        return ss[0].Substring(0, prefixLength);
                    }
                }
                prefixLength++;
            }

            return ss[0]; // all strings identical
        }

        /// <summary>
        /// Word wraps the given text to fit within the specified width.
        /// </summary>
        /// <param name="text">Text to be word wrapped</param>
        /// <param name="width">Width, in characters, to which the text
        /// should be word wrapped</param>
        /// <returns>The modified text</returns>
        public static List<string> WordWrapLines(string text, int width)
        {
            int pos, next;
            var lines = new List<string>();

            // Lucidity check
            if (width < 1)
            {
                lines.Add(text);
                return lines;
            }

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos,
                    System.StringComparison.Ordinal);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        lines.Add(text.Substring(pos, len));

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else lines.Add(string.Empty); // Empty line
            }
            return lines;
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }

        public static void CleanupText(ref string debugText)
        {
            // Strip off newlines from the debug text.
            if (String.IsNullOrWhiteSpace(debugText))
                debugText = String.Empty;

            // TODO: Make this transformation in the output.
            debugText = Regex.Replace(debugText, " ( )+", " ");
            debugText = Regex.Replace(debugText, "\n", "");
        }
    }

    static class AssemblyHelpers
    {
        public static IEnumerable<System.Type> FindDerivedTypes(this Assembly assembly,
                                                                System.Type baseType)
        {
            return assembly.GetTypes().Where(baseType.IsAssignableFrom);
        }        
    }

    static class Log
    {
        
    }
}