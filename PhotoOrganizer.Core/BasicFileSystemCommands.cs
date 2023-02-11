// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

namespace PhotoOrganizer.Core;

public static class BasicFileSystemCommands
{
    public static FileDataAnalysis GetDataAnalysis(FileInfo fileInfo)
    {
        FileDataAnalysis data = new FileDataAnalysis {FileInfo = fileInfo};
        data.FileType = GetFileType(fileInfo);
        if ((data.FileType == EFileClassifier.Picture) || (data.FileType == EFileClassifier.Video))
            data.MediaType = GetMediaType(fileInfo);
        data.Date = GetDateFile(fileInfo);
        return data;
    }

    private static DateTime GetDateFile(FileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

    private static EMediaSubClassifier GetMediaType(FileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

    private static EFileClassifier GetFileType(FileInfo fileInfo)
    {
        throw new NotImplementedException();
    }
}