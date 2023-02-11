// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

using System.Runtime.InteropServices;
using System.Text;

namespace PhotoOrganizer.Core;

// The class is written by the user Danny Beckett (https://stackoverflow.com/users/1563422/danny-beckett) 
// and taken from the resource https://stackoverflow.com/questions/217902/reading-writing-an-ini-file
// Minor edits made

public class IniConfig
{
    public string Path { get; set; } = "config.ini";
    private static readonly string _commonSectionString = "Common";

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string filePath);

    public IniConfig(string? iniPath = null)
    {
        if (!string.IsNullOrEmpty(iniPath))
        {
            if (System.IO.Path.Exists(iniPath))
                Path = iniPath;
        }

        if (System.IO.Path.Exists(Path))
            Path = System.IO.Path.GetFullPath(Path);
    }

    public string Read(string key, string? section = null)
    {
        var retVal = new StringBuilder(255);
        GetPrivateProfileString(section ?? _commonSectionString, key, "", retVal, 255, Path);
        return retVal.ToString();
    }

    public void Write(string key, string value, string? section = null)
    {
        WritePrivateProfileString(section ?? _commonSectionString, key, value, Path);
    }

    public void DeleteKey(string key, string? section = null)
    {
        Write(key, null, section ?? _commonSectionString);
    }

    public void DeleteSection(string? section = null)
    {
        Write(null, null, section ?? _commonSectionString);
    }

    public bool KeyExists(string key, string? section = null)
    {
        return Read(key, section).Length > 0;
    }
}