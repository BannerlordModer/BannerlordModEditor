# BannerlordModEditor XML校验系统架构图

## 整体架构

```
┌─────────────────────────────────────────────────────────────────┐
│                    BannerlordModEditor                         │
│                    XML Validation System                        │
└─────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                     IXmlValidationSystem                        │
│                    (Core Interface)                             │
└─────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                 BannerlordXmlValidationSystem                   │
│                    (Main Implementation)                         │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Dependency    │    │   Implicit      │    │    Schema       │
│   Analyzer     │    │   Validation    │    │    Validator    │
│                │    │   Detector      │    │                │
│ - Dependency   │    │ - Rule Extractor│    │ - XSD Validator │
│   Analysis     │    │ - Business      │    │ - Schema        │
│ - Loading Order│    │   Logic         │    │   Generator    │
│ - Circular Dep │    │ - Auto-Suggest   │    │ - Type Resolver │
└─────────────────┘    └─────────────────┘    └─────────────────┘
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ Reference       │    │ Data Type       │    │ Value Range     │
│ Integrity       │    │ Validator       │    │ Validator       │
│ Checker         │    │                 │    │                 │
│ - Object Index  │    │ - Type Safety   │    │ - Range Check   │
│ - Reference     │    │ - Convert Check │    │ - Bounds        │
│   Validation    │    │ - Format Check  │    │   Validation    │
│ - Cross-File    │    │                 │    │                 │
│   References    │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 详细组件架构

### 1. 依赖关系分析器

```
┌─────────────────────────────────────────────────────────────────┐
│                 IXmlDependencyAnalyzer                        │
└─────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                MbObjectDependencyAnalyzer                       │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Dependency    │    │   Reference     │    │   Topological   │
│   Extractor    │    │   Extractor     │    │    Sorter       │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### 2. 隐式校验逻辑检测器

```
┌─────────────────────────────────────────────────────────────────┐
│               IImplicitValidationDetector                      │
└─────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│              MbImplicitValidationDetector                     │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Rule          │    │  Validation     │    │  Suggestion     │
│  Extractor     │    │  Engine         │    │  Generator      │
│                 │    │                 │    │                 │
│ - MBObjectBase │    │ - Predicate      │    │ - Auto-Fix      │
│   Rules        │    │   Evaluation    │    │   Suggestions   │
│ - Type-Specific│    │ - Error          │    │ - Confidence    │
│   Rules        │    │   Detection      │    │   Scoring       │
│ - Property     │    │ - Context        │    │                 │
│   Rules        │    │   Analysis       │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### 3. Schema验证器

```
┌─────────────────────────────────────────────────────────────────┐
│                   ISchemaValidator                             │
└─────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────┐
│                 XsdSchemaValidator                           │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Schema        │    │  Schema         │    │  Validation     │
│  Loader        │    │  Generator      │    │  Event Handler  │
│                 │    │                 │    │                 │
│ - XSD File     │    │ - Type Analysis │    │ - Error         │
│   Loading      │    │ - XML Structure │    │   Detection      │
│ - Schema Cache │    │   Analysis      │    │ - Warning        │
│ - Dependency   │    │ - XSD           │    │   Collection    │
│   Resolution   │    │   Generation    │    │ - Severity       │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 数据流架构

```
XML Files → Dependency Analysis → Schema Validation → Implicit Validation → Reference Check → Report
    │               │                   │                  │              │           │
    │               │                   │                  │              │           │
    ▼               ▼                   ▼                  ▼              ▼           ▼
Loading Order → Object Index → Type Safety → Business Logic → Cross-Ref → Final Report
```

## 错误处理流程

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Error         │    │  Error          │    │  Error          │
│  Detection     │    │  Classification │    │  Reporting      │
│                 │    │                 │    │                 │
│ - Schema       │    │ - Severity       │    │ - Structured     │
│   Errors       │    │ - Category       │    │   Format        │
│ - Dependency   │    │ - Impact         │    │ - Location      │
│   Errors       │    │ - Context        │    │   Info           │
│ - Logic        │    │ - Source         │    │ - Suggestions   │
│   Errors       │    │   Analysis       │    │ - Fix Actions   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 扩展点架构

```
┌─────────────────────────────────────────────────────────────────┐
│                     Extension Points                            │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  IXmlValidator │    │  IRuleExtractor │    │  ISchemaGenerator│
│                 │    │                 │    │                 │
│ - Custom        │    │ - Source Code    │    │ - Dynamic       │
│   Validators   │    │   Analysis      │    │   Schema        │
│ - Plugin        │    │ - Pattern        │    │   Generation    │
│   Architecture │    │   Recognition    │    │ - Type Inference │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 性能优化架构

```
┌─────────────────────────────────────────────────────────────────┐
│                  Performance Optimization                     │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Parallel      │    │  Caching        │    │  Incremental    │
│  Processing    │    │  Strategy       │    │  Validation     │
│                 │    │                 │    │                 │
│ - Multi-threaded│    │ - Schema Cache  │    │ - Change         │
│   Validation   │    │ - Object Index  │    │   Detection      │
│ - Task Queue   │    │ - Rule Cache    │    │ - Delta          │
│   Management   │    │ - Result Cache  │    │   Processing    │
│ - Load         │    │ - Metadata      │    │ - Smart         │
│   Balancing    │    │   Cache         │    │   Invalidation  │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 配置管理架构

```
┌─────────────────────────────────────────────────────────────────┐
│                  Configuration Management                      │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Validation    │    │  Schema         │    │  Performance    │
│  Settings      │    │  Configuration │    │  Configuration │
│                 │    │                 │    │                 │
│ - Validation    │    │ - Custom Schema │    │ - Threading     │
│   Types        │    │   Paths         │    │   Settings      │
│ - Severity      │    │ - Validation    │    │ - Caching       │
│   Levels        │    │   Options       │    │   Options       │
│ - Custom        │    │ - Type Mappings │    │ - Memory        │
│   Options      │    │                 │    │   Limits        │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 报告生成架构

```
┌─────────────────────────────────────────────────────────────────┐
│                   Report Generation                           │
└─────────────────────────────────────────────────────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           │                        │                        │
           ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  Data          │    │  Analysis       │    │  Output         │
│  Collection    │    │  Engine         │    │  Generation     │
│                 │    │                 │    │                 │
│ - Validation   │    │ - Statistics    │    │ - HTML Reports  │
│   Results      │    │ - Trend          │    │ - JSON Reports  │
│ - Error        │    │   Analysis       │    │ - XML Reports   │
│   Aggregation  │    │ - Recommendation│    │ - Text Reports  │
│ - Metadata     │    │   Engine         │    │ - Dashboard     │
│   Extraction   │    │ - Severity       │    │   Integration   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

这个架构图展示了BannerlordModEditor XML校验系统的完整架构，包括核心组件、数据流、错误处理、扩展点、性能优化、配置管理和报告生成等各个方面。该架构基于Mount & Blade的MBObjectManager机制，提供了一个全面、可扩展的XML校验解决方案。