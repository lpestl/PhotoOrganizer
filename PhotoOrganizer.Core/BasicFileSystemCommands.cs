// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

namespace PhotoOrganizer.Core;

public static class BasicFileSystemCommands
{
    public static FileDataAnalysis GetDataAnalysis(FileInfo fileInfo, string relativeTo)
    {
        FileDataAnalysis data = new FileDataAnalysis {FileInfo = fileInfo};
        data.FileType = GetFileType(fileInfo);
        data.MediaType = GetMediaType(fileInfo, data.FileType);
        data.Date = GetDateFile(fileInfo, data.FileType, data.MediaType);
        data.RelativePath = Path.GetRelativePath(relativeTo, fileInfo.FullName);
        return data;
    }

    private static EFileClassifier GetFileType(FileInfo fileInfo)
    {
        var lowerExtension = fileInfo.Extension.ToLower();
        
        var imageExtension = Settings.AffectingImageExtensions.FirstOrDefault(x => x.ToLower().Equals(lowerExtension));
        if (imageExtension != null)
            return EFileClassifier.Picture;

        var videoExtension = Settings.AffectingVideoExtensions.FirstOrDefault(x => x.ToLower().Equals(lowerExtension));
        if (videoExtension != null)
            return EFileClassifier.Video;
        
        return EFileClassifier.OtherFile;
    }
    
    private static EMediaSubClassifier GetMediaType(FileInfo fileInfo, EFileClassifier fileClassifier)
    {
        switch (fileClassifier)
        {
            case EFileClassifier.Unknown:
                var fType = GetFileType(fileInfo);
                return GetMediaType(fileInfo, fType);
            case EFileClassifier.Picture:
                var lowerPicName = fileInfo.Name.ToLower();
                var camPrefix = Settings.ImagePrefixes.FirstOrDefault(x => lowerPicName.StartsWith(x.ToLower()));
                return camPrefix == null ? EMediaSubClassifier.Other : EMediaSubClassifier.Camera;
            case EFileClassifier.Video:
                var lowerVideoName = fileInfo.Name.ToLower();
                var vidCamPrefix = Settings.VideoPrefixes.FirstOrDefault(x => lowerVideoName.StartsWith(x.ToLower()));
                return vidCamPrefix == null ? EMediaSubClassifier.Other : EMediaSubClassifier.Camera;
            case EFileClassifier.OtherFile:
                return EMediaSubClassifier.NotMedia;
            default:
                throw new ArgumentOutOfRangeException(nameof(fileClassifier), fileClassifier, null);
        }
    }
    
    
    private static DateTime GetDateFile(FileInfo fileInfo, EFileClassifier dataFileType, EMediaSubClassifier dataMediaType)
    {
        var creationDateTime = fileInfo.CreationTime;
        var updateDateTime = fileInfo.LastWriteTime;
        var minDate = creationDateTime <= updateDateTime ? creationDateTime : updateDateTime;

        switch (dataMediaType)
        {
            case EMediaSubClassifier.NotMedia:
            case EMediaSubClassifier.Other:
                break;
            case EMediaSubClassifier.Camera:
                if (TryGetDateFromName(fileInfo.Name, dataFileType, out DateTime nameDate))
                {
                    return nameDate;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dataMediaType), dataMediaType, null);
        }

        return minDate;
    }

    private static bool TryGetDateFromName(string fileName, EFileClassifier dataFileType, out DateTime dateTime)
    {
        switch (dataFileType)
        {
            case EFileClassifier.Unknown:
                break;
            case EFileClassifier.Picture:
            case EFileClassifier.Video:
                var parts = fileName.Split(new char[] {'_', '-'});
                foreach (var part in parts)
                {
                    if ((part.Length == 8) && (long.TryParse(part, out _)))
                    {
                        var yearStr = $"{part[0]}{part[1]}{part[2]}{part[3]}";
                        var monStr = $"{part[4]}{part[5]}";
                        var dateStr = $"{part[6]}{part[7]}";

                        if ((int.TryParse(yearStr, out int year)) &&
                            (int.TryParse(monStr, out int mon)) &&
                            (int.TryParse(dateStr, out int date)))
                        {
                            dateTime = new DateTime(year, mon, date);
                            return true;
                        }
                    }
                }
                break;
            case EFileClassifier.OtherFile:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(dataFileType), dataFileType, null);
        }
        
        dateTime = default;
        return false;
    }
}