﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace Pancake.Debugging
{
    internal static class ChannelClassBuilder
    {
        private static readonly HashSet<char> number = new HashSet<char>()
        {
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '0'
        };

        private static readonly HashSet<char> letter = new HashSet<char>()
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z'
        };

        private static readonly HashSet<char> letterOrNumber = new HashSet<char>()
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '0',
        };

        private static string GetChannelClassPath()
        {
            //string settingsPath = AssetUtility.FindByNameAndExtension("Debug.ReleaseBuild", ".dll");
            string settingsPath = "Assets/_Root/Packages/Debug/Debug.ReleaseBuild.dll";
            if (settingsPath.Length == 0) return "";
            string directory = Path.GetDirectoryName(settingsPath);
            if (!directory.DirectoryExists()) directory.CreateDirectory();
            return Path.Combine(directory, "Channel.cs");
        }

        public static bool ChannelClassContentsMatch(DebugChannelInfo[] channels)
        {
            string path = GetChannelClassPath();
            if (path.Length == 0)
            {
                return true;
            }

            string newContents = GenerateClassCode(channels);
            string currentContents = "";
            if (File.Exists(path)) currentContents = File.ReadAllText(path);

            return File.Exists(path) && string.Equals(currentContents, newContents);
        }

        public static void BuildClass(DebugChannelInfo[] channels)
        {
            try
            {
                string path = GetChannelClassPath();
                if (path.Length == 0)
                {
                    return;
                }

                string newContents = GenerateClassCode(channels);
                string currentContents = "";
                if (File.Exists(path)) currentContents = File.ReadAllText(path);
                if (File.Exists(path) && string.Equals(currentContents, newContents)) return;

#if DEV_MODE
				Debug.Log("Rebuilding Channel.cs...\nCurrent:\n"+currentContents+"\n\nNew:\n" + newContents);
#endif
                File.WriteAllText(path, newContents);
                AssetDatabase.ImportAsset(path);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private static string GenerateClassCode(DebugChannelInfo[] channels)
        {
            var sb = new StringBuilder(1024);
            int channelCount = channels.Length;
            var memberNamesToChannelNames = new Dictionary<string, string>(channelCount);

            for (int c = 0; c < channelCount; c++)
            {
                string channelName = channels[c].id;

                if (channelName.Length == 0)
                {
                    continue;
                }

                char test = channelName[0];

                if (letter.Contains(test))
                {
                    sb.Append(test);
                }
                else if (number.Contains(test))
                {
                    sb.Append('_');
                    sb.Append(test);
                }
                else
                {
                    sb.Append('_');
                }

                for (int l = 1, lcount = channelName.Length; l < lcount; l++)
                {
                    test = channelName[l];
                    sb.Append(letterOrNumber.Contains(test) ? test : '_');
                }

                string memberName = sb.ToString();
                while (memberNamesToChannelNames.ContainsKey(memberName))
                {
                    sb.Append('_');
                    memberName = sb.ToString();
                }

                memberNamesToChannelNames.Add(memberName, channelName);
                sb.Length = 0;
            }

            string inNamespace = typeof(Debug).Namespace;

            string indent;
            if (!string.IsNullOrEmpty(inNamespace))
            {
                sb.Append("#region AutoGenerated\r\nusing UnityEngine;\r\n\r\nnamespace ");
                sb.Append(inNamespace);
                sb.Append("\r\n{\r\n");
                const string summary = "	/// <summary>\r\n" + "	/// Defines a channel for a message logged to the Console.\r\n" + "	/// <para>\r\n" +
                                       "	/// The contents of this class are auto-generated based on the channels added in Project Settings > Console.\r\n" +
                                       "	/// </para>\r\n" + "	/// <seealso cref=\"Debug.Log(int, string, Object)\"/>\r\n" + "	/// </summary>";
                sb.Append(summary);
                sb.Append("\r\n	public static class Channel\r\n	{\r\n		public const int None = 0;");
                indent = "		";
            }
            else
            {
                sb.Append("#region AutoGenerated\r\nusing UnityEngine;\r\n\r\n");
                const string summary = "/// <summary>\r\n" + "/// Defines a channel for a message logged to the Console.\r\n" + "/// <para>\r\n" +
                                       "/// The contents of this class are auto-generated based on the channels added in Project Settings > Console.\r\n" +
                                       "/// </para>\r\n" + "/// <seealso cref=\"Debug.Log(int, string, Object)\"/>\r\n" + "/// </summary>";
                sb.Append(summary);
                sb.Append("\r\npublic static class Channel\r\n{\r\n	public const int None = 0;");
                indent = "	";
            }

            int index = 1;
            foreach (var build in memberNamesToChannelNames)
            {
                string memberName = build.Key;
                string inspectorName = build.Value;
                if (!string.Equals(memberName, inspectorName))
                {
                    // InspectorName attribute only exists in Unity 2019.2 or newer
                    sb.Append("\r\n#if UNITY_2019_2_OR_NEWER\r\n");
                    sb.Append(indent);
                    sb.Append("[UnityEngine.InspectorName(\"");
                    sb.Append(inspectorName);
                    sb.Append("\")]\r\n#endif\r\n");
                }
                else
                {
                    sb.Append("\r\n");
                }

                sb.Append(indent);
                sb.Append("public const int ");
                sb.Append(memberName);
                sb.Append(" = ");
                sb.Append(index);
                sb.Append(";");

                index++;
            }

            if (!string.IsNullOrEmpty(inNamespace))
            {
                sb.Append("\r\n	}\r\n}\r\n#endregion");
            }
            else
            {
                sb.Append("\r\n}\r\n#endregion");
            }

            return sb.ToString();
        }
    }
}