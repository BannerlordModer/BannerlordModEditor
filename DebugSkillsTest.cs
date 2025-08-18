using System;
using BannerlordModEditor.Common.Models.DO.Game;
using BannerlordModEditor.Common.Tests;

namespace DebugSkillsTest
{
    class Program
    {
        static void Main()
        {
            var skills = new SkillsDO
            {
                SkillDataList = new System.Collections.Generic.List<SkillDataDO>
                {
                    new SkillDataDO
                    {
                        Id = "IronFlesh1",
                        Name = "Iron Flesh 1",
                        HasModifiers = true,
                        Modifiers = new ModifiersDO
                        {
                            AttributeModifiers = new System.Collections.Generic.List<AttributeModifierDO>
                            {
                                new AttributeModifierDO
                                {
                                    AttribCode = "AgentHitPoints",
                                    Modification = "Multiply",
                                    Value = "1.01"
                                }
                            }
                        },
                        Documentation = "Iron flesh increases hit points"
                    }
                }
            };

            var serializedXml = XmlTestUtils.Serialize(skills);
            Console.WriteLine("序列化结果:");
            Console.WriteLine(serializedXml);
        }
    }
}