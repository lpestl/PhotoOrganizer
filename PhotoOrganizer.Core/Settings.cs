// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

namespace PhotoOrganizer.Core;

public class Settings
{
    public List<string> ImagePrefixes { get; set; } = new List<string>
                                                {
                                                    // Photo
                                                    "IMG_","IMG-","JPEG_","JPEG-","P_","P-","IMAG",".P_","____",
                                                };
    public List<string> VideoPrefixes { get; set; } = new List<string>
                                                {
                                                    // Video
                                                    "VID_","VID-","V_","V-","VIDEO",
                                                };
    public List<string> AffectingImageExtensions { get; set; } = new List<string> 
                                                            {
                                                                // *** Images extensions ***
                                                                // APNG - Animated Portable Network Graphics
                                                                ".apng",
                                                                // AVIF - AV1 Image File Format
                                                                ".avif",
                                                                // GIF - Graphics Interchange Format
                                                                ".gif",
                                                                // JPEG - Joint Photographic Expert Group image
                                                                ".jpg",".jpeg",".jtif",".pjpeg",".pjp",
                                                                // PNG - Portable Network Graphics
                                                                ".png",
                                                                // SVG - Scalable Vector Graphics
                                                                ".svg",
                                                                // WebP - Web Picture format
                                                                ".webp",
                                                                // BMP - Bitmap file
                                                                ".bmp",
                                                                // ICO - Microsoft Icon
                                                                ".ico",".cur",
                                                                // TIFF - Tagged Image File Format
                                                                ".tif",".tiff",
                                                            };
    public List<string> AffectingVideoExtensions { get; set; } = new List<string> 
                                                            {
                                                                // *** Video extensions ***
                                                                ".webm", 
                                                                ".mpg",".mp2",".mpeg",".mpe",".mpv",
                                                                ".ogg",
                                                                ".mp4",".m4p",".m4v",
                                                                ".avi",
                                                                ".wmv",
                                                                ".mov",".qt",
                                                                ".flv",".swf",
                                                                ".avchd",
                                                            };
}