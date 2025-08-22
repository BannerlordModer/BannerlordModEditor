# XML适配状态检查工具API规范

openapi: 3.0.0
info:
  title: XML适配状态检查工具API
  version: 1.0.0
  description: 骑马与砍杀2（Bannerlord）Mod编辑器XML适配状态检查工具的完整API规范

servers:
  - url: http://localhost:5000
    description: 本地开发服务器
  - url: https://api.bannerlord-mod-editor.com
    description: 生产服务器

paths:
  /api/v1/check:
    post:
      summary: 执行XML适配状态检查
      operationId: checkAdaptationStatus
      tags:
        - AdaptationCheck
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/AdaptationCheckRequest'
      responses:
        '200':
          description: 检查成功完成
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AdaptationCheckResponse'
        '400':
          description: 请求参数错误
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'
        '500':
          description: 服务器内部错误
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/check/{checkId}:
    get:
      summary: 获取检查结果
      operationId: getCheckResult
      tags:
        - AdaptationCheck
      parameters:
        - name: checkId
          in: path
          required: true
          description: 检查任务ID
          schema:
            type: string
            format: uuid
      responses:
        '200':
          description: 获取检查结果成功
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AdaptationCheckResponse'
        '404':
          description: 检查任务不存在
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/check/{checkId}/report:
    get:
      summary: 生成检查报告
      operationId: generateCheckReport
      tags:
        - AdaptationCheck
      parameters:
        - name: checkId
          in: path
          required: true
          description: 检查任务ID
          schema:
            type: string
            format: uuid
        - name: format
          in: query
          required: false
          description: 报告格式
          schema:
            type: string
            enum: [json, csv, markdown, html]
            default: json
      responses:
        '200':
          description: 报告生成成功
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ReportResponse'
            text/csv:
              schema:
                type: string
                description: CSV格式报告
            text/markdown:
              schema:
                type: string
                description: Markdown格式报告
            text/html:
              schema:
                type: string
                description: HTML格式报告
        '404':
          description: 检查任务不存在
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/check/{checkId}/statistics:
    get:
      summary: 获取检查统计信息
      operationId: getCheckStatistics
      tags:
        - AdaptationCheck
      parameters:
        - name: checkId
          in: path
          required: true
          description: 检查任务ID
          schema:
            type: string
            format: uuid
      responses:
        '200':
          description: 获取统计信息成功
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AdaptationStatistics'
        '404':
          description: 检查任务不存在
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/config:
    get:
      summary: 获取工具配置
      operationId: getConfiguration
      tags:
        - Configuration
      responses:
        '200':
          description: 获取配置成功
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AdaptationCheckOptions'
    
    put:
      summary: 更新工具配置
      operationId: updateConfiguration
      tags:
        - Configuration
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/AdaptationCheckOptions'
      responses:
        '200':
          description: 配置更新成功
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/AdaptationCheckOptions'
        '400':
          description: 配置参数错误
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/naming-mappings:
    get:
      summary: 获取命名映射规则
      operationId: getNamingMappings
      tags:
        - NamingConvention
      responses:
        '200':
          description: 获取命名映射成功
          content:
            application/json:
              schema:
                type: object
                additionalProperties:
                  type: string
                description: XML文件名到C#类名的映射规则
    
    post:
      summary: 添加命名映射规则
      operationId: addNamingMapping
      tags:
        - NamingConvention
      requestBody:
        required: true
        content:
          application/json:
            schema:
              type: object
              required:
                - xmlName
                - className
              properties:
                xmlName:
                  type: string
                  description: XML文件名
                className:
                  type: string
                  description: C#类名
      responses:
        '201':
          description: 命名映射添加成功
          content:
            application/json:
              schema:
                type: object
                properties:
                  success:
                    type: boolean
                    description: 操作是否成功
                  message:
                    type: string
                    description: 操作结果消息
        '400':
          description: 映射参数错误
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/ErrorResponse'

  /api/v1/health:
    get:
      summary: 健康检查
      operationId: healthCheck
      tags:
        - System
      responses:
        '200':
          description: 系统健康状态
          content:
            application/json:
              schema:
                type: object
                properties:
                  status:
                    type: string
                    description: 系统状态
                  timestamp:
                    type: string
                    format: date-time
                    description: 检查时间
                  version:
                    type: string
                    description: API版本

components:
  schemas:
    AdaptationCheckRequest:
      type: object
      required:
        - xmlDirectory
        - modelsDirectory
      properties:
        xmlDirectory:
          type: string
          description: XML文件目录路径
          example: "/path/to/ModuleData"
        modelsDirectory:
          type: string
          description: 模型文件目录路径
          example: "/path/to/BannerlordModEditor.Common/Models"
        outputFormat:
          $ref: '#/components/schemas/ReportFormat'
          description: 输出格式
        minimumComplexity:
          $ref: '#/components/schemas/AdaptationComplexity'
          description: 最小复杂度过滤
        verbose:
          type: boolean
          description: 是否显示详细信息
          default: false
        showProgress:
          type: boolean
          description: 是否显示进度
          default: true
        maxParallelFiles:
          type: integer
          description: 最大并行文件数
          default: 4
          minimum: 1
          maximum: 32

    AdaptationCheckResponse:
      type: object
      properties:
        checkId:
          type: string
          format: uuid
          description: 检查任务ID
        status:
          $ref: '#/components/schemas/CheckStatus'
          description: 检查状态
        result:
          $ref: '#/components/schemas/AdaptationCheckResult'
          description: 检查结果
        progress:
          $ref: '#/components/schemas/CheckProgress'
          description: 检查进度
        startedAt:
          type: string
          format: date-time
          description: 开始时间
        completedAt:
          type: string
          format: date-time
          description: 完成时间
        duration:
          type: integer
          description: 执行时长（毫秒）
        errors:
          type: array
          items:
            $ref: '#/components/schemas/CheckError'
          description: 错误信息

    AdaptationCheckResult:
      type: object
      properties:
        checkTimestamp:
          type: string
          format: date-time
          description: 检查时间
        xmlDirectory:
          type: string
          description: XML目录路径
        modelsDirectory:
          type: string
          description: 模型目录路径
        adaptedFiles:
          type: array
          items:
            $ref: '#/components/schemas/AdaptedFileInfo'
          description: 已适配文件列表
        unadaptedFiles:
          type: array
          items:
            $ref: '#/components/schemas/UnadaptedFile'
          description: 未适配文件列表
        errorFiles:
          type: array
          items:
            type: string
          description: 错误文件列表
        statistics:
          $ref: '#/components/schemas/AdaptationStatistics'
          description: 统计信息

    AdaptedFileInfo:
      type: object
      properties:
        fileName:
          type: string
          description: 文件名
        fullPath:
          type: string
          description: 完整路径
        modelName:
          type: string
          description: 模型类名
        modelPath:
          type: string
          description: 模型文件路径
        fileSize:
          type: integer
          format: int64
          description: 文件大小（字节）
        lastModified:
          type: string
          format: date-time
          description: 最后修改时间
        status:
          $ref: '#/components/schemas/AdaptationStatus'
          description: 适配状态

    UnadaptedFile:
      type: object
      properties:
        fileName:
          type: string
          description: 文件名
        fullPath:
          type: string
          description: 完整路径
        fileSize:
          type: integer
          format: int64
          description: 文件大小（字节）
        expectedModelName:
          type: string
          description: 预期模型类名
        complexity:
          $ref: '#/components/schemas/AdaptationComplexity'
          description: 适配复杂度
        requiresChunking:
          type: boolean
          description: 是否需要分块处理
        suggestedNamespace:
          type: string
          description: 建议命名空间
        estimatedAdaptationTime:
          type: integer
          description: 预估适配时间（分钟）

    AdaptationStatistics:
      type: object
      properties:
        totalXmlFiles:
          type: integer
          description: XML文件总数
        adaptedFilesCount:
          type: integer
          description: 已适配文件数
        unadaptedFilesCount:
          type: integer
          description: 未适配文件数
        errorFilesCount:
          type: integer
          description: 错误文件数
        adaptationRate:
          type: number
          format: double
          description: 适配率（0-1）
        complexityDistribution:
          type: object
          additionalProperties:
            type: integer
          description: 复杂度分布
        namespaceDistribution:
          type: object
          additionalProperties:
            type: integer
          description: 命名空间分布
        totalFileSize:
          type: integer
          format: int64
          description: 总文件大小
        averageFileSize:
          type: number
          format: double
          description: 平均文件大小

    ReportResponse:
      type: object
      properties:
        reportId:
          type: string
          format: uuid
          description: 报告ID
        format:
          $ref: '#/components/schemas/ReportFormat'
          description: 报告格式
        content:
          type: string
          description: 报告内容
        downloadUrl:
          type: string
          description: 下载链接
        generatedAt:
          type: string
          format: date-time
          description: 生成时间
        expiresAt:
          type: string
          format: date-time
          description: 过期时间

    CheckProgress:
      type: object
      properties:
        totalFiles:
          type: integer
          description: 总文件数
        processedFiles:
          type: integer
          description: 已处理文件数
        currentFile:
          type: string
          description: 当前处理文件
        percentage:
          type: number
          format: double
          description: 完成百分比
        estimatedTimeRemaining:
          type: integer
          description: 预估剩余时间（秒）
        speed:
          type: number
          format: double
          description: 处理速度（文件/秒）

    CheckError:
      type: object
      properties:
        file:
          type: string
          description: 错误文件
        error:
          type: string
          description: 错误信息
        stackTrace:
          type: string
          description: 堆栈跟踪
        timestamp:
          type: string
          format: date-time
          description: 错误时间

    ErrorResponse:
      type: object
      properties:
        error:
          type: string
          description: 错误代码
        message:
          type: string
          description: 错误消息
        details:
          type: object
          description: 错误详情
        timestamp:
          type: string
          format: date-time
          description: 错误时间

    AdaptationCheckOptions:
      type: object
      properties:
        xmlDirectory:
          type: string
          description: 默认XML目录
        modelsDirectory:
          type: string
          description: 默认模型目录
        outputFormat:
          $ref: '#/components/schemas/ReportFormat'
          description: 默认输出格式
        minimumComplexity:
          $ref: '#/components/schemas/AdaptationComplexity'
          description: 默认最小复杂度
        verbose:
          type: boolean
          description: 默认详细模式
        showProgress:
          type: boolean
          description: 默认显示进度
        maxParallelFiles:
          type: integer
          description: 默认最大并行文件数
        timeoutMinutes:
          type: integer
          description: 超时时间（分钟）
        enableCaching:
          type: boolean
          description: 是否启用缓存
        logLevel:
          type: string
          enum: [Debug, Info, Warning, Error]
          description: 日志级别

    # 枚举类型
    ReportFormat:
      type: string
      enum: [json, csv, markdown, html, console]
      description: 报告格式

    AdaptationComplexity:
      type: string
      enum: [Simple, Medium, Complex, Large]
      description: 适配复杂度

    AdaptationStatus:
      type: string
      enum: [Adapted, Unadapted, Error, PartiallyAdapted]
      description: 适配状态

    CheckStatus:
      type: string
      enum: [Pending, Running, Completed, Failed, Cancelled]
      description: 检查状态

  securitySchemes:
    ApiKeyAuth:
      type: apiKey
      in: header
      name: X-API-Key
      description: API密钥认证

security:
  - ApiKeyAuth: []

tags:
  - name: AdaptationCheck
    description: XML适配状态检查相关接口
  - name: Configuration
    description: 配置管理相关接口
  - name: NamingConvention
    description: 命名约定相关接口
  - name: System
    description: 系统相关接口