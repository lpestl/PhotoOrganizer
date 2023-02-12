// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

using System.Data;
using System.Diagnostics;
using System.IO;

namespace PhotoOrganizer.Core;

public class Organizer
{
    public DirectoryInfo ScanDir { get; set; }
        
    public Organizer(string? scanDirPath = null)
    {
        if (string.IsNullOrEmpty(scanDirPath))
        {
            ScanDir = new DirectoryInfo(Environment.CurrentDirectory);
        }
        else
        {
            if (Path.Exists(scanDirPath))
            {
                try
                {
                    ScanDir = new DirectoryInfo(scanDirPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[WARNING] {e}");
                    ScanDir = new DirectoryInfo(Environment.CurrentDirectory);
                    Console.WriteLine($"[WARNING] Used current directory path: {ScanDir.FullName}");
                }
            }
            else
            {
                Console.WriteLine($"[WARNING] Path is not exist: {scanDirPath}");
                ScanDir = new DirectoryInfo(Environment.CurrentDirectory);
                Console.WriteLine($"[WARNING] Used current directory path: {ScanDir.FullName}");
            }
        }
    }

    public DataAnalysis Analys()
    {
        var counter = new DataAnalysis();

        if (!Directory.Exists(ScanDir.FullName))
            throw new DirectoryNotFoundException($"[ERROR] Path for analize is not valid: {ScanDir.FullName}");
        
        var timer = new Stopwatch();
        timer.Start();

        AnalizeFolder(ScanDir, counter);
        
        timer.Stop();
        Console.WriteLine($"[INFO] Operation completed in {timer.Elapsed.TotalHours:F0}h {timer.Elapsed.Minutes:F0}m {timer.Elapsed.Seconds:F0}s {timer.Elapsed.Milliseconds:F0}ms");
        
        return counter;
    }

 #region - Internal analyzer funcs -

    private void AnalizeFolder(DirectoryInfo scanDir, DataAnalysis? counter = null)
    {
        var subDirs = scanDir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            AnalizeFolder(subDir, counter);
        }

        var files = scanDir.GetFiles("*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            AnalizeFile(file, counter);
        }

        if (counter != null) 
            counter.DirectoryCounter++;
    }

    private void AnalizeFile(FileInfo fileInfo, DataAnalysis? counter)
    {
        var data = BasicFileSystemCommands.GetDataAnalysis(fileInfo, ScanDir.FullName);

        if (counter == null) 
            return;

        IncrementCounter(data, counter);
    }

    private void IncrementCounter(FileDataAnalysis data, DataAnalysis counter)
    {
        switch (data.FileType)
        {
            case EFileClassifier.Unknown:
                throw new DataException($"[ERROR] Error while analyze file. The file cannot be Unknown: {data.FileInfo.FullName}");
            case EFileClassifier.Picture:
                switch (data.MediaType)
                {
                    case EMediaSubClassifier.NotMedia:
                        throw new DataException($"[ERROR] Image cannot be classified as Non-Media: {data.FileInfo.FullName}");
                    case EMediaSubClassifier.Other:
                        counter.OtherImageCounter++;
                        counter.OtherImageSizeCounter += (ulong) data.FileInfo.Length;
                        break;
                    case EMediaSubClassifier.Camera:
                        counter.CamImageCounter++;
                        counter.CamImageSizeCounter += (ulong) data.FileInfo.Length;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case EFileClassifier.Video:
                switch (data.MediaType)
                {
                    case EMediaSubClassifier.NotMedia:
                        throw new DataException($"[ERROR] Video cannot be classified as Non-Media: {data.FileInfo.FullName}");
                    case EMediaSubClassifier.Other:
                        counter.OtherVideoCounter++;
                        counter.OtherVideoSizeCounter += (ulong) data.FileInfo.Length;
                        break;
                    case EMediaSubClassifier.Camera:
                        counter.CamVideoCounter++;
                        counter.CamVideoSizeCounter += (ulong) data.FileInfo.Length;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case EFileClassifier.OtherFile:
                counter.OtherFileCounter++;
                counter.OtherFileSizeCounter += (ulong)data.FileInfo.Length;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        counter.FileCounter++;
    }

#endregion
    
    public void Organize()
    {
        if (!Directory.Exists(ScanDir.FullName))
            throw new DirectoryNotFoundException($"[ERROR] Path for analize is not valid: {ScanDir.FullName}");
        
        var timer = new Stopwatch();
        timer.Start();

        OrganizeFolder(ScanDir);
        
        timer.Stop();
        Console.WriteLine($"[INFO] Operation completed in {timer.Elapsed.TotalHours:F0}h {timer.Elapsed.Minutes:F0}m {timer.Elapsed.Seconds:F0}s {timer.Elapsed.Milliseconds:F0}ms");
    }

#region - Internal Organize funcs -

    private void OrganizeFolder(DirectoryInfo scanDir)
    {
        var subDirs = scanDir.GetDirectories();
        foreach (var subDir in subDirs)
        {
            OrganizeFolder(subDir);
        }

        var files = scanDir.GetFiles("*", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            OrganizeFile(file);
        }

        if (!scanDir.GetFiles("*", SearchOption.AllDirectories).Any())
            scanDir.Delete(true);
    }

    private void OrganizeFile(FileInfo file)
    {
        var data = BasicFileSystemCommands.GetDataAnalysis(file, ScanDir.FullName);

        string newPath;
        switch (data.FileType)
        {
            case EFileClassifier.Unknown:
                throw new DataException($"[ERROR] Error while analyze file. The file cannot be Unknown: {data.FileInfo.FullName}");
            case EFileClassifier.Picture:
                switch (data.MediaType)
                {
                    case EMediaSubClassifier.NotMedia:
                        throw new DataException($"[ERROR] Image cannot be classified as Non-Media: {data.FileInfo.FullName}");
                    case EMediaSubClassifier.Other:
                        newPath = Path.Combine(ScanDir.FullName, "Pictures", data.RelativePath);
                        var otherPic = new FileInfo(newPath);
                        if (otherPic.Directory != null && !Directory.Exists(otherPic.Directory.FullName))
                            Directory.CreateDirectory(otherPic.Directory.FullName);
                        if (!File.Exists(newPath))
                            file.MoveTo(newPath);
                        break;
                    case EMediaSubClassifier.Camera:
                        newPath = Path.Combine(ScanDir.FullName, "Photo", $"{data.Date.Year}",
                            $"{data.Date.Year}-{data.Date.Month:D2}");
                        if (!Directory.Exists(newPath))
                            Directory.CreateDirectory(newPath);
                        newPath = Path.Combine(newPath, $"{file.Name}{file.Extension}");
                        if (!File.Exists(newPath))
                            file.MoveTo(newPath);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case EFileClassifier.Video:
                switch (data.MediaType)
                {
                    case EMediaSubClassifier.NotMedia:
                        throw new DataException($"[ERROR] Video cannot be classified as Non-Media: {data.FileInfo.FullName}");
                    case EMediaSubClassifier.Other:
                        newPath = Path.Combine(ScanDir.FullName, "Videos", data.RelativePath);
                        var otherVid = new FileInfo(newPath);
                        if (otherVid.Directory != null && !Directory.Exists(otherVid.Directory.FullName))
                            Directory.CreateDirectory(otherVid.Directory.FullName);
                        if (!File.Exists(newPath))
                            file.MoveTo(newPath);
                        break;
                    case EMediaSubClassifier.Camera:
                        newPath = Path.Combine(ScanDir.FullName, "CamVideos", $"{data.Date.Year}",
                            $"{data.Date.Year}-{data.Date.Month:D2}");
                        if (!Directory.Exists(newPath))
                            Directory.CreateDirectory(newPath);
                        newPath = Path.Combine(newPath, $"{file.Name}{file.Extension}");
                        if (!File.Exists(newPath))
                            file.MoveTo(newPath);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case EFileClassifier.OtherFile:
                newPath = Path.Combine(ScanDir.FullName, "Other", data.RelativePath);
                var otherFile = new FileInfo(newPath);
                if (otherFile.Directory != null && !Directory.Exists(otherFile.Directory.FullName))
                    Directory.CreateDirectory(otherFile.Directory.FullName);
                if (!File.Exists(newPath))
                    file.MoveTo(newPath);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion
}