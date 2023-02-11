// See https://aka.ms/new-console-template for more information

using PhotoOrganizer.Core;

Console.WriteLine($"[INFO] *** Start from path: {Environment.CurrentDirectory}");

var organizer = new Organizer(Environment.CurrentDirectory);
var counter = organizer.Analys();

Console.WriteLine("*** Step 1: Analysis result ***");
Console.WriteLine();

var totalSize = Helpers.GetSizeForUI(counter.FilesSize);
Console.WriteLine($"The total number of folders and subfolders is {counter.DirectoryCounter}");
Console.WriteLine($"The total number of files viewed is {counter.FileCounter} with size {totalSize.Item1:F} {totalSize.Item2}");
Console.WriteLine();

var otherSize = Helpers.GetSizeForUI(counter.OtherFileSizeCounter);
Console.WriteLine($"- Not media files: {counter.OtherFileCounter}; Size: {otherSize.Item1:F} {otherSize.Item2}");
Console.WriteLine();

var mediaSize = Helpers.GetSizeForUI(counter.MediaSize);
Console.WriteLine($"- Media files: {counter.MediaCount}; Size: {mediaSize.Item1:F} {mediaSize.Item2}");
Console.WriteLine("-");
var imageSize = Helpers.GetSizeForUI(counter.ImagesSize);
Console.WriteLine($"-- Image files: {counter.ImageCount}; Size: {imageSize.Item1:F} {imageSize.Item2}");
var videoSize = Helpers.GetSizeForUI(counter.VideosSize);
Console.WriteLine($"-- Video files: {counter.VideoCount}; Size: {videoSize.Item1:F} {videoSize.Item2}");
Console.WriteLine("- OR -");
imageSize = Helpers.GetSizeForUI(counter.CamImageSizeCounter);
Console.WriteLine($"-- CAMERA Image files: {counter.CamImageCounter}; Size: {imageSize.Item1:F} {imageSize.Item2}");
videoSize = Helpers.GetSizeForUI(counter.CamVideoSizeCounter);
Console.WriteLine($"-- CAMERA Video files: {counter.CamVideoCounter}; Size: {videoSize.Item1:F} {videoSize.Item2}");
Console.WriteLine("- OR -");
imageSize = Helpers.GetSizeForUI(counter.OtherImageSizeCounter);
Console.WriteLine($"-- Other Image files: {counter.OtherImageCounter}; Size: {imageSize.Item1:F} {imageSize.Item2}");
videoSize = Helpers.GetSizeForUI(counter.OtherVideoSizeCounter);
Console.WriteLine($"-- Other Video files: {counter.OtherVideoCounter}; Size: {videoSize.Item1:F} {videoSize.Item2}");
