using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using BannerlordModEditor.Common.Models.DO;

namespace Test
{
    class Program
    {
        static void Main()
        {
            var bannerIcons = new BannerIconsDO
            {
                Type = "test",
                HasBannerIconData = true,
                BannerIconData = new BannerIconDataDO
                {
                    HasEmptyBannerIconGroups = true,
                    BannerIconGroups = new List<BannerIconGroupDO>(),
                    HasBannerColors = true,
                    BannerColors = new BannerColorsDO
                    {
                        HasEmptyColors = true,
                        Colors = new List<ColorEntryDO>()
                    }
                }
            };

            var serializer = new XmlSerializer(typeof(BannerIconsDO));
            using var writer = new StringWriter();
            serializer.Serialize(writer, bannerIcons);
            var xml = writer.ToString();

            Console.WriteLine("=== Serialized XML ===");
            Console.WriteLine(xml);
            Console.WriteLine("=== End XML ===");
            
            Console.WriteLine($"Contains BannerIconData: {xml.Contains("BannerIconData")}");
            Console.WriteLine($"Contains BannerIconGroup: {xml.Contains("BannerIconGroup")}");
            Console.WriteLine($"Contains BannerColors: {xml.Contains("BannerColors")}");
            Console.WriteLine($"Contains Color: {xml.Contains("Color")}");
        }
    }
}