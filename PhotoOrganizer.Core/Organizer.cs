// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

using System.Data;
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

    public DataAnalysis? Analys()
    {
        var counter = new DataAnalysis();

        if (!Directory.Exists(ScanDir.FullName))
            throw new DirectoryNotFoundException($"[ERROR] Path for analize is not valid: {ScanDir.FullName}");
        
        AnalizeFolder(ScanDir, counter);
        
        return counter;
    }

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

    public void Organize()
    {
        
    }
}