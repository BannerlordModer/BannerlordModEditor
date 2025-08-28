using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using BannerlordModEditor.Common.Validation.Core;

namespace BannerlordModEditor.Common.Validation.Schema
{
    /// <summary>
    /// XML Schema验证器
    /// 基于Mount & Blade的XSD Schema实现验证
    /// </summary>
    public interface ISchemaValidator
    {
        Task<SchemaValidationResult> ValidateSchemaAsync(string xmlPath);
        Task<SchemaValidationResult> ValidateSchemaAsync(string xmlPath, string schemaPath);
        Task<SchemaValidationResult> ValidateSchemaAsync(XmlDocument xmlDocument, string schemaPath);
        void RegisterSchema(XmlDataType dataType, string schemaPath);
        void UnregisterSchema(XmlDataType dataType);
        Task<List<string>> GenerateSchemaAsync(XmlDocument xmlDocument);
    }

    /// <summary>
    /// 基于XSD的Schema验证器实现
    /// </summary>
    public class XsdSchemaValidator : ISchemaValidator
    {
        private readonly Dictionary<XmlDataType, string> _registeredSchemas = new();
        private readonly Dictionary<string, XmlSchemaSet> _schemaCache = new();
        private readonly ISchemaGenerator _schemaGenerator;
        private readonly IXmlTypeResolver _typeResolver;

        public XsdSchemaValidator(
            ISchemaGenerator schemaGenerator,
            IXmlTypeResolver typeResolver)
        {
            _schemaGenerator = schemaGenerator;
            _typeResolver = typeResolver;
            RegisterBuiltInSchemas();
        }

        public async Task<SchemaValidationResult> ValidateSchemaAsync(string xmlPath)
        {
            var dataType = _typeResolver.ResolveDataTypeFromPath(xmlPath);
            
            if (_registeredSchemas.TryGetValue(dataType, out var schemaPath))
            {
                return await ValidateSchemaAsync(xmlPath, schemaPath);
            }

            // 如果没有注册的Schema，尝试生成一个
            return await ValidateWithGeneratedSchemaAsync(xmlPath, dataType);
        }

        public async Task<SchemaValidationResult> ValidateSchemaAsync(string xmlPath, string schemaPath)
        {
            var result = new SchemaValidationResult { SchemaPath = schemaPath };
            
            try
            {
                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = GetSchemaSet(schemaPath),
                    ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings |
                                     XmlSchemaValidationFlags.AllowXmlAttributes |
                                     XmlSchemaValidationFlags.ProcessIdentityConstraints,
                    IgnoreComments = true,
                    IgnoreWhitespace = true
                };

                settings.ValidationEventHandler += (sender, e) =>
                {
                    HandleValidationEvent(e, result, xmlPath);
                };

                using (var reader = XmlReader.Create(xmlPath, settings))
                {
                    while (reader.Read()) { }
                }

                result.IsValid = !result.Errors.Any();
                result.ValidatedFiles.Add(xmlPath);
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new SchemaValidationError
                {
                    XmlPath = xmlPath,
                    ErrorMessage = $"Schema validation failed: {ex.Message}",
                    ErrorType = SchemaErrorType.SchemaValidationError
                });
            }

            return result;
        }

        public async Task<SchemaValidationResult> ValidateSchemaAsync(XmlDocument xmlDocument, string schemaPath)
        {
            var result = new SchemaValidationResult { SchemaPath = schemaPath };
            
            try
            {
                var schemaSet = GetSchemaSet(schemaPath);
                xmlDocument.Schemas = schemaSet;

                var eventHandler = new ValidationEventHandler((sender, e) =>
                {
                    HandleValidationEvent(e, result, "Inline XML");
                });

                xmlDocument.Validate(eventHandler);

                result.IsValid = !result.Errors.Any();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Errors.Add(new SchemaValidationError
                {
                    XmlPath = "Inline XML",
                    ErrorMessage = $"Schema validation failed: {ex.Message}",
                    ErrorType = SchemaErrorType.SchemaValidationError
                });
            }

            return result;
        }

        public void RegisterSchema(XmlDataType dataType, string schemaPath)
        {
            if (File.Exists(schemaPath))
            {
                _registeredSchemas[dataType] = schemaPath;
                // 清除缓存以便重新加载
                _schemaCache.Remove(schemaPath);
            }
        }

        public void UnregisterSchema(XmlDataType dataType)
        {
            if (_registeredSchemas.TryGetValue(dataType, out var schemaPath))
            {
                _registeredSchemas.Remove(dataType);
                _schemaCache.Remove(schemaPath);
            }
        }

        public async Task<List<string>> GenerateSchemaAsync(XmlDocument xmlDocument)
        {
            var dataType = _typeResolver.ResolveDataTypeFromDocument(xmlDocument);
            var schemaPath = await _schemaGenerator.GenerateSchemaAsync(xmlDocument, dataType);
            
            if (!string.IsNullOrEmpty(schemaPath))
            {
                RegisterSchema(dataType, schemaPath);
                return new List<string> { schemaPath };
            }

            return new List<string>();
        }

        private void RegisterBuiltInSchemas()
        {
            // 注册Mount & Blade内置的Schema
            var baseSchemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas");
            
            if (Directory.Exists(baseSchemaPath))
            {
                var schemaFiles = Directory.GetFiles(baseSchemaPath, "*.xsd");
                
                foreach (var schemaFile in schemaFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(schemaFile);
                    if (Enum.TryParse<XmlDataType>(fileName, true, out var dataType))
                    {
                        RegisterSchema(dataType, schemaFile);
                    }
                }
            }
        }

        private XmlSchemaSet GetSchemaSet(string schemaPath)
        {
            if (_schemaCache.TryGetValue(schemaPath, out var cachedSet))
            {
                return cachedSet;
            }

            var schemaSet = new XmlSchemaSet();
            
            try
            {
                // 添加主Schema
                var schema = XmlSchema.Read(XmlReader.Create(schemaPath), (sender, e) =>
                {
                    // 处理Schema加载警告
                });
                
                schemaSet.Add(schema);
                
                // 添加依赖的Schema
                AddDependentSchemas(schema, schemaSet, Path.GetDirectoryName(schemaPath));
                
                schemaSet.Compile();
                
                _schemaCache[schemaPath] = schemaSet;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load schema from {schemaPath}: {ex.Message}", ex);
            }

            return schemaSet;
        }

        private void AddDependentSchemas(XmlSchema schema, XmlSchemaSet schemaSet, string baseSchemaPath)
        {
            // 处理Schema中的导入和包含
            foreach (var include in schema.Includes)
            {
                if (include is XmlSchemaImport schemaImport)
                {
                    var dependentSchemaPath = Path.Combine(baseSchemaPath, schemaImport.SchemaLocation);
                    if (File.Exists(dependentSchemaPath))
                    {
                        var dependentSchema = XmlSchema.Read(XmlReader.Create(dependentSchemaPath), null);
                        schemaSet.Add(dependentSchema);
                    }
                }
                else if (include is XmlSchemaInclude schemaInclude)
                {
                    var dependentSchemaPath = Path.Combine(baseSchemaPath, schemaInclude.SchemaLocation);
                    if (File.Exists(dependentSchemaPath))
                    {
                        var dependentSchema = XmlSchema.Read(XmlReader.Create(dependentSchemaPath), null);
                        schemaSet.Add(dependentSchema);
                    }
                }
            }
        }

        private void HandleValidationEvent(ValidationEventArgs e, SchemaValidationResult result, string xmlPath)
        {
            var exception = e.Exception as XmlSchemaValidationException;
            
            if (e.Severity == XmlSeverityType.Error)
            {
                result.Errors.Add(new SchemaValidationError
                {
                    XmlPath = xmlPath,
                    ErrorMessage = e.Message,
                    LineNumber = exception?.LineNumber ?? 0,
                    LinePosition = exception?.LinePosition ?? 0,
                    XPath = exception?.SourceUri,
                    ErrorType = DetermineErrorType(e)
                });
            }
            else if (e.Severity == XmlSeverityType.Warning)
            {
                result.Warnings.Add(new SchemaValidationWarning
                {
                    XmlPath = xmlPath,
                    WarningMessage = e.Message,
                    LineNumber = exception?.LineNumber ?? 0,
                    XPath = exception?.SourceUri
                });
            }
        }

        private SchemaErrorType DetermineErrorType(ValidationEventArgs e)
        {
            var exception = e.Exception as XmlSchemaValidationException;
            
            return exception?.SchemaType?.Name switch
            {
                "xs:string" when exception.Message.Contains("required") => SchemaErrorType.MissingRequiredAttribute,
                "xs:integer" when exception.Message.Contains("not a valid") => SchemaErrorType.TypeMismatch,
                "xs:decimal" when exception.Message.Contains("not a valid") => SchemaErrorType.TypeMismatch,
                _ => SchemaErrorType.SchemaValidationError
            };
        }

        private async Task<SchemaValidationResult> ValidateWithGeneratedSchemaAsync(string xmlPath, XmlDataType dataType)
        {
            var result = new SchemaValidationResult();
            
            try
            {
                // 生成临时Schema
                var doc = new XmlDocument();
                doc.Load(xmlPath);
                var generatedSchemas = await GenerateSchemaAsync(doc);
                
                if (generatedSchemas.Any())
                {
                    var schemaPath = generatedSchemas.First();
                    return await ValidateSchemaAsync(xmlPath, schemaPath);
                }
                else
                {
                    result.Warnings.Add(new SchemaValidationWarning
                    {
                        XmlPath = xmlPath,
                        WarningMessage = "No schema available for validation"
                    });
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new SchemaValidationError
                {
                    XmlPath = xmlPath,
                    ErrorMessage = $"Failed to generate schema: {ex.Message}",
                    ErrorType = SchemaErrorType.SchemaValidationError
                });
            }

            return result;
        }
    }

    /// <summary>
    /// Schema生成器接口
    /// </summary>
    public interface ISchemaGenerator
    {
        Task<string> GenerateSchemaAsync(XmlDocument xmlDocument, XmlDataType dataType);
        Task<string> GenerateSchemaFromTypeAsync(Type objectType);
        Task<string> GenerateSchemaFromXmlAsync(string xmlPath);
    }

    /// <summary>
    /// 基于Mount & Blade类型的Schema生成器
    /// </summary>
    public class MbSchemaGenerator : ISchemaGenerator
    {
        private readonly ITypeAnalyzer _typeAnalyzer;
        private readonly IXmlReflectionHelper _reflectionHelper;

        public MbSchemaGenerator(
            ITypeAnalyzer typeAnalyzer,
            IXmlReflectionHelper reflectionHelper)
        {
            _typeAnalyzer = typeAnalyzer;
            _reflectionHelper = reflectionHelper;
        }

        public async Task<string> GenerateSchemaAsync(XmlDocument xmlDocument, XmlDataType dataType)
        {
            var objectType = GetObjectType(dataType);
            if (objectType != null)
            {
                return await GenerateSchemaFromTypeAsync(objectType);
            }

            // 如果没有对应的类型，从XML结构生成
            return await GenerateSchemaFromXmlDocumentAsync(xmlDocument, dataType);
        }

        public async Task<string> GenerateSchemaFromTypeAsync(Type objectType)
        {
            var typeAnalysis = await _typeAnalyzer.AnalyzeTypeAsync(objectType);
            return GenerateXsdFromTypeAnalysis(typeAnalysis);
        }

        public async Task<string> GenerateSchemaFromXmlAsync(string xmlPath)
        {
            var doc = new XmlDocument();
            doc.Load(xmlPath);
            var dataType = _typeResolver.ResolveDataTypeFromPath(xmlPath);
            return await GenerateSchemaAsync(doc, dataType);
        }

        private Type GetObjectType(XmlDataType dataType)
        {
            return dataType switch
            {
                XmlDataType.CraftingPieces => typeof(CraftingPiece),
                XmlDataType.Items => typeof(ItemObject),
                XmlDataType.ActionTypes => typeof(ActionSet),
                XmlDataType.ActionSets => typeof(ActionSet),
                XmlDataType.CombatParameters => typeof(CombatParameters),
                XmlDataType.Skeletons => typeof(Skeleton),
                XmlDataType.PhysicsMaterials => typeof(PhysicsMaterial),
                XmlDataType.Monsters => typeof(Monster),
                XmlDataType.Characters => typeof(CharacterObject),
                XmlDataType.Parties => typeof(Party),
                XmlDataType.Scenes => typeof(Scene),
                _ => null
            };
        }

        private async Task<string> GenerateSchemaFromXmlDocumentAsync(XmlDocument xmlDocument, XmlDataType dataType)
        {
            var rootElement = xmlDocument.DocumentElement;
            var schemaBuilder = new System.Text.StringBuilder();

            // 生成Schema头
            schemaBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            schemaBuilder.AppendLine(@"<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema""");

            // 分析XML结构并生成Schema
            var elements = rootElement.ChildNodes;
            var firstElement = elements.OfType<XmlElement>().FirstOrDefault();
            
            if (firstElement != null)
            {
                var elementName = firstElement.LocalName;
                schemaBuilder.AppendLine($@"  targetNamespace=""http://www.taleworlds.com/{dataType.ToString().ToLower()}""");
                schemaBuilder.AppendLine($@"  xmlns:tns=""http://www.taleworlds.com/{dataType.ToString().ToLower()}""");
                schemaBuilder.AppendLine($@"  elementFormDefault=""qualified"">");
                schemaBuilder.AppendLine();

                // 生成主元素定义
                schemaBuilder.AppendLine($"  <xs:element name=""{rootElement.LocalName}"">");
                schemaBuilder.AppendLine("    <xs:complexType>");
                schemaBuilder.AppendLine("      <xs:sequence>");
                schemaBuilder.AppendLine($"        <xs:element name=""{elementName}"" minOccurs=""0"" maxOccurs=""unbounded"">");
                schemaBuilder.AppendLine("          <xs:complexType>");
                
                // 分析元素属性
                var attributes = firstElement.Attributes;
                foreach (XmlAttribute attr in attributes)
                {
                    var attrType = InferAttributeType(attr.Value);
                    schemaBuilder.AppendLine($"          <xs:attribute name=""{attr.LocalName}"" type=""{attrType}"" use=""required""/>");
                }
                
                // 分析子元素
                var childElements = firstElement.ChildNodes.OfType<XmlElement>();
                foreach (var childElement in childElements)
                {
                    var childType = InferElementType(childElement);
                    schemaBuilder.AppendLine($"          <xs:element name=""{childElement.LocalName}"" type=""{childType}"" minOccurs=""0""/>");
                }
                
                schemaBuilder.AppendLine("          </xs:complexType>");
                schemaBuilder.AppendLine("        </xs:element>");
                schemaBuilder.AppendLine("      </xs:sequence>");
                schemaBuilder.AppendLine("    </xs:complexType>");
                schemaBuilder.AppendLine("  </xs:element>");
                schemaBuilder.AppendLine("</xs:schema>");
            }

            return schemaBuilder.ToString();
        }

        private string GenerateXsdFromTypeAnalysis(TypeAnalysis typeAnalysis)
        {
            var schemaBuilder = new System.Text.StringBuilder();
            
            schemaBuilder.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
            schemaBuilder.AppendLine(@"<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">");
            schemaBuilder.AppendLine();

            // 生成类型定义
            foreach (var property in typeAnalysis.Properties)
            {
                schemaBuilder.AppendLine($"  <xs:element name=""{property.XmlName}"" type=""{property.XsdType}"">");
                
                if (property.HasRestrictions)
                {
                    schemaBuilder.AppendLine("    <xs:simpleType>");
                    schemaBuilder.AppendLine($"      <xs:restriction base=""{property.XsdType}"">");
                    
                    if (property.MinLength.HasValue)
                    {
                        schemaBuilder.AppendLine($"        <xs:minLength value=""{property.MinLength}""/>");
                    }
                    
                    if (property.MaxLength.HasValue)
                    {
                        schemaBuilder.AppendLine($"        <xs:maxLength value=""{property.MaxLength}""/>");
                    }
                    
                    if (property.MinValue.HasValue)
                    {
                        schemaBuilder.AppendLine($"        <xs:minInclusive value=""{property.MinValue}""/>");
                    }
                    
                    if (property.MaxValue.HasValue)
                    {
                        schemaBuilder.AppendLine($"        <xs:maxInclusive value=""{property.MaxValue}""/>");
                    }
                    
                    if (property.Pattern != null)
                    {
                        schemaBuilder.AppendLine($"        <xs:pattern value=""{property.Pattern}""/>");
                    }
                    
                    schemaBuilder.AppendLine("      </xs:restriction>");
                    schemaBuilder.AppendLine("    </xs:simpleType>");
                }
                
                schemaBuilder.AppendLine("  </xs:element>");
            }

            schemaBuilder.AppendLine("</xs:schema>");
            return schemaBuilder.ToString();
        }

        private string InferAttributeType(string value)
        {
            if (int.TryParse(value, out _))
                return "xs:integer";
            
            if (double.TryParse(value, out _))
                return "xs:decimal";
            
            if (bool.TryParse(value, out _))
                return "xs:boolean";
            
            if (DateTime.TryParse(value, out _))
                return "xs:dateTime";
            
            return "xs:string";
        }

        private string InferElementType(XmlElement element)
        {
            if (element.ChildNodes.Count == 0)
            {
                return InferAttributeType(element.InnerText);
            }
            
            if (element.ChildNodes.OfType<XmlElement>().Any())
            {
                return "tns:ComplexType";
            }
            
            return "xs:string";
        }
    }

    /// <summary>
    /// 类型分析结果
    /// </summary>
    public class TypeAnalysis
    {
        public Type Type { get; set; }
        public List<PropertyAnalysis> Properties { get; set; } = new();
        public string XmlRootName { get; set; }
        public string Namespace { get; set; }
    }

    /// <summary>
    /// 属性分析结果
    /// </summary>
    public class PropertyAnalysis
    {
        public string PropertyName { get; set; }
        public string XmlName { get; set; }
        public Type PropertyType { get; set; }
        public string XsdType { get; set; }
        public bool IsRequired { get; set; }
        public bool HasRestrictions { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string Pattern { get; set; }
    }

    /// <summary>
    /// 类型分析器接口
    /// </summary>
    public interface ITypeAnalyzer
    {
        Task<TypeAnalysis> AnalyzeTypeAsync(Type type);
    }

    /// <summary>
    /// XML反射助手接口
    /// </summary>
    public interface IXmlReflectionHelper
    {
        string GetXmlName(Type type);
        string GetXmlName(PropertyInfo property);
        string GetXsdType(Type type);
        bool IsRequired(PropertyInfo property);
    }

    /// <summary>
    /// XML类型解析器接口
    /// </summary>
    public interface IXmlTypeResolver
    {
        XmlDataType ResolveDataTypeFromPath(string xmlPath);
        XmlDataType ResolveDataTypeFromDocument(XmlDocument xmlDocument);
        Type GetDomainType(XmlDataType dataType);
    }
}