// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

namespace PhotoOrganizer.Core;

public enum ESizeUnit : ushort
{
    Byte = 0,
    KB = 1,
    MB = 2,
    GB = 3,
    TB = 4
}

public enum EFileClassifier : ushort
{
    Unknown = 0,
    Picture = 1,
    Video = 2,
    OtherFile = 3
}

public enum EMediaSubClassifier : ushort
{
    NotMedia = 0,
    Other = 1,
    Camera = 2
}

public class FileDataAnalysis
{
    public FileInfo FileInfo { get; set; }
    public EFileClassifier FileType { get; set; } = EFileClassifier.Unknown;
    public EMediaSubClassifier MediaType { get; set; } = EMediaSubClassifier.NotMedia;
    public DateTime Date { get; set; }
    public string RelativePath { get; set; }
}

public class DataAnalysis
{
    public ulong FileCounter { get; set; } = 0;
    public ulong DirectoryCounter { get; set; } = 0;

    public ulong OtherFileCounter { get; set; } = 0;
    public ulong CamImageCounter { get; set; } = 0;
    public ulong CamVideoCounter { get; set; } = 0;
    public ulong OtherImageCounter { get; set; } = 0;
    public ulong OtherVideoCounter { get; set; } = 0;
    
    public ulong ImageCount => CamImageCounter + OtherImageCounter;
    public ulong VideoCount => CamVideoCounter + OtherVideoCounter;
    public ulong CamMediaCount => CamImageCounter + CamVideoCounter;
    public ulong OtherMediaCount => OtherImageCounter + OtherVideoCounter;
    public ulong MediaCount => ImageCount + VideoCount;

    public ulong OtherFileSizeCounter { get; set; } = 0;
    public ulong CamImageSizeCounter { get; set; } = 0;
    public ulong CamVideoSizeCounter { get; set; } = 0;
    public ulong OtherImageSizeCounter { get; set; } = 0;
    public ulong OtherVideoSizeCounter { get; set; } = 0;

    public ulong ImagesSize => CamImageSizeCounter + OtherImageSizeCounter;
    public ulong VideosSize => CamVideoSizeCounter + OtherVideoSizeCounter;
    public ulong CamMediaSize => CamImageSizeCounter + CamVideoSizeCounter;
    public ulong OtherMediaSize => OtherImageSizeCounter + OtherVideoSizeCounter;
    public ulong MediaSize => ImagesSize + VideosSize;

    public ulong FilesSize => MediaSize + OtherFileSizeCounter;
}