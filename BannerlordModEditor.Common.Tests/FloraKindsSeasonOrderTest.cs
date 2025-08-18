using System;
using System.IO;
using System.Linq;
using BannerlordModEditor.Common.Models.DO.Layouts;
using BannerlordModEditor.Common.Tests;
using Xunit;

namespace BannerlordModEditor.Common.Tests
{
    public class FloraKindsSeasonOrderTest
    {
        [Fact]
        public void Test_Season_Attribute_Order()
        {
            Console.WriteLine("=== 测试季节属性顺序 ===");
            
            var xmlPath = Path.Combine("TestData", "Layouts", "flora_kinds_layout.xml");
            var xml = File.ReadAllText(xmlPath);
            var obj = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(xml);
            
            // 获取第一个layout的items
            var firstLayout = obj.Layouts.LayoutList[0];
            var items = firstLayout.Items.ItemList;
            
            Console.WriteLine("Items in the layout:");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"  {i}: {items[i].Name} ({items[i].Type})");
            }
            
            // 查找季节属性
            var summerKind = items.FirstOrDefault(i => i.Name == "summer_kind");
            var winterKind = items.FirstOrDefault(i => i.Name == "winter_kind");
            var fallKind = items.FirstOrDefault(i => i.Name == "fall_kind");
            var springKind = items.FirstOrDefault(i => i.Name == "spring_kind");
            
            Console.WriteLine($"Summer Kind index: {items.IndexOf(summerKind)}");
            Console.WriteLine($"Winter Kind index: {items.IndexOf(winterKind)}");
            Console.WriteLine($"Fall Kind index: {items.IndexOf(fallKind)}");
            Console.WriteLine($"Spring Kind index: {items.IndexOf(springKind)}");
            
            // 验证顺序是否正确
            int summerIndex = items.IndexOf(summerKind);
            int winterIndex = items.IndexOf(winterKind);
            int fallIndex = items.IndexOf(fallKind);
            int springIndex = items.IndexOf(springKind);
            
            Console.WriteLine($"Expected order: summer < winter < fall < spring");
            Console.WriteLine($"Actual order: {summerIndex} < {winterIndex} < {fallIndex} < {springIndex}");
            
            // 检查是否是正确的顺序
            bool isCorrectOrder = summerIndex < winterIndex && winterIndex < fallIndex && fallIndex < springIndex;
            Console.WriteLine($"Order is correct: {isCorrectOrder}");
            
            // 如果顺序不对，输出实际的顺序
            if (!isCorrectOrder)
            {
                Console.WriteLine("WARNING: Season attributes are not in the expected order!");
                
                // 获取所有季节属性并按索引排序
                var seasonItems = new[]
                {
                    new { Name = "summer_kind", Item = summerKind, Index = items.IndexOf(summerKind) },
                    new { Name = "winter_kind", Item = winterKind, Index = items.IndexOf(winterKind) },
                    new { Name = "fall_kind", Item = fallKind, Index = items.IndexOf(fallKind) },
                    new { Name = "spring_kind", Item = springKind, Index = items.IndexOf(springKind) }
                }.OrderBy(x => x.Index).ToList();
                
                Console.WriteLine("Actual season order:");
                foreach (var season in seasonItems)
                {
                    Console.WriteLine($"  {season.Name} at index {season.Index}");
                }
            }
            
            // 序列化并再次检查
            var serializedXml = XmlTestUtils.Serialize(obj);
            var serializedObj = XmlTestUtils.Deserialize<FloraKindsLayoutDO>(serializedXml);
            var serializedItems = serializedObj.Layouts.LayoutList[0].Items.ItemList;
            
            Console.WriteLine("\n=== After serialization ===");
            for (int i = 0; i < serializedItems.Count; i++)
            {
                Console.WriteLine($"  {i}: {serializedItems[i].Name} ({serializedItems[i].Type})");
            }
            
            // 检查序列化后的顺序
            int serSummerIndex = serializedItems.IndexOf(serializedItems.FirstOrDefault(i => i.Name == "summer_kind"));
            int serWinterIndex = serializedItems.IndexOf(serializedItems.FirstOrDefault(i => i.Name == "winter_kind"));
            int serFallIndex = serializedItems.IndexOf(serializedItems.FirstOrDefault(i => i.Name == "fall_kind"));
            int serSpringIndex = serializedItems.IndexOf(serializedItems.FirstOrDefault(i => i.Name == "spring_kind"));
            
            Console.WriteLine($"Serialized order: {serSummerIndex} < {serWinterIndex} < {serFallIndex} < {serSpringIndex}");
            
            bool isSerializedOrderCorrect = serSummerIndex < serWinterIndex && serWinterIndex < serFallIndex && serFallIndex < serSpringIndex;
            Console.WriteLine($"Serialized order is correct: {isSerializedOrderCorrect}");
            
            // 比较原始和序列化的顺序
            bool orderMatches = summerIndex == serSummerIndex && 
                               winterIndex == serWinterIndex && 
                               fallIndex == serFallIndex && 
                               springIndex == serSpringIndex;
            
            Console.WriteLine($"Order matches between original and serialized: {orderMatches}");
            
            Assert.True(isCorrectOrder, "Original season attributes should be in correct order");
            Assert.True(isSerializedOrderCorrect, "Serialized season attributes should be in correct order");
            Assert.True(orderMatches, "Season attribute order should be preserved after serialization");
        }
    }
}