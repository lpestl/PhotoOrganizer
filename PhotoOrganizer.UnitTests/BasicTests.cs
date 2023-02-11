using PhotoOrganizer.Core;

namespace PhotoOrganizer.UnitTests;

public class BasicTests
{
    public void CreateBasicConfigFile()
    {
        var iniManager = new IniConfig();
        
        iniManager.Write("AffectingExtensions", "" +
                                                // APNG - Animated Portable Network Graphics
                                                ".apng," +
                                                // AVIF - AV1 Image File Format
                                                ".avif," +
                                                // GIF - Graphics Interchange Format
                                                ".gif," +
                                                // JPEG - Joint Photographic Expert Group image
                                                ".jpg,.jpeg,.jtif,.pjpeg,.pjp," +
                                                // PNG - Portable Network Graphics
                                                ".png," +
                                                // SVG - Scalable Vector Graphics
                                                ".svg," +
                                                // WebP - Web Picture format
                                                ".webp," +
                                                // BMP - Bitmap file
                                                ".bmp," +
                                                // ICO - Microsoft Icon
                                                ".ico,.cur," +
                                                // TIFF - Tagged Image File Format
                                                ".tif,.tiff");
    } 
        
    [SetUp]
    public void Setup()
    {
        CreateBasicConfigFile();
    }

    [Test]
    public void Test1()
    {
        if (Path.Exists("config.ini"))
            Assert.Pass();
        else
            Assert.Fail();
    }
}