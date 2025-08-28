# 测试通过率检测系统API规范

## 概述

本文档定义了测试通过率检测系统的完整API接口规范，包括内部API、外部接口和数据模型。API基于RESTful设计原则，支持JSON数据格式，并与现有的BannerlordModEditor项目架构保持一致。

## API基础信息

### 服务器信息
```yaml
servers:
  - url: https://api.bannerlordmodeditor.com/v1
    description: 生产环境服务器
  - url: https://staging-api.bannerlordmodeditor.com/v1
    description: 测试环境服务器
  - url: http://localhost:5000/v1
    description: 本地开发服务器
```

### 认证方式
```yaml
security:
  - BearerAuth: []
  - ApiKeyAuth: []

components:
  securitySchemes:
    BearerAuth:
      type: http
      scheme: bearer
      bearerFormat: JWT
    ApiKeyAuth:
      type: apiKey
      in: header
      name: X-API-Key
```

## 数据模型

### 基础数据模型

#### TestExecutionRequest
```yaml
TestExecutionRequest:
  type: object
  required:
    - testProjects
  properties:
    testProjects:
      type: array
      items:
        type: string
      description: 要执行的测试项目列表
    options:
      $ref: '#/components/schemas/TestExecutionOptions'
    thresholds:
      $ref: '#/components/schemas/QualityGateThresholds'
    callbackUrl:
      type: string
      format: uri
      description: 执行完成后的回调URL
    priority:
      type: string
      enum: [low, normal, high]
      default: normal
      description: 执行优先级
```

#### TestExecutionOptions
```yaml
TestExecutionOptions:
  type: object
  properties:
    parallelExecution:
      type: boolean
      default: true
      description: 是否并行执行测试
    timeoutSeconds:
      type: integer
      default: 1800
      description: 测试执行超时时间（秒）
    coverageEnabled:
      type: boolean
      default: true
      description: 是否启用覆盖率分析
    failFast:
      type: boolean
      default: false
      description: 是否在第一个测试失败时停止
    retryFailedTests:
      type: boolean
      default: false
      description: 是否重试失败的测试
    maxRetries:
      type: integer
      default: 3
      description: 最大重试次数
    environment:
      type: object
      additionalProperties:
        type: string
      description: 测试执行环境变量
```

#### QualityGateThresholds
```yaml
QualityGateThresholds:
  type: object
  properties:
    minPassRate:
      type: number
      format: float
      minimum: 0
      maximum: 100
      default: 95.0
      description: 最低通过率阈值
    minLineCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      default: 80.0
      description: 最低行覆盖率阈值
    minBranchCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      default: 75.0
      description: 最低分支覆盖率阈值
    minMethodCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      default: 85.0
      description: 最低方法覆盖率阈值
    maxFailedTests:
      type: integer
      default: 0
      description: 最大失败测试数量
    maxTestExecutionTime:
      type: integer
      default: 3600
      description: 最大测试执行时间（秒）
```

### 执行结果模型

#### TestExecutionResult
```yaml
TestExecutionResult:
  type: object
  required:
    - executionId
    - status
    - summary
    - projectResults
    - executionDuration
  properties:
    executionId:
      type: string
      format: uuid
      description: 执行ID
    status:
      $ref: '#/components/schemas/TestExecutionStatus'
    summary:
      $ref: '#/components/schemas/TestSummary'
    projectResults:
      type: array
      items:
        $ref: '#/components/schemas/TestProjectResult'
    executionDuration:
      type: string
      format: duration
      description: 执行持续时间
    startTime:
      type: string
      format: date-time
      description: 开始时间
    endTime:
      type: string
      format: date-time
      description: 结束时间
    triggeredBy:
      type: string
      description: 触发者
    triggerType:
      type: string
      enum: [manual, scheduled, ci_cd, api]
      description: 触发类型
    qualityGateResult:
      $ref: '#/components/schemas/QualityGateResult'
    analysis:
      $ref: '#/components/schemas/TestAnalysisResult'
```

#### TestExecutionStatus
```yaml
TestExecutionStatus:
  type: string
  enum:
    - pending
    - running
    - completed
    - failed
    - cancelled
    - timeout
  description: 测试执行状态
```

#### TestSummary
```yaml
TestSummary:
  type: object
  required:
    - totalTests
    - passedTests
    - failedTests
    - skippedTests
    - passRate
  properties:
    totalTests:
      type: integer
      description: 总测试数量
    passedTests:
      type: integer
      description: 通过测试数量
    failedTests:
      type: integer
      description: 失败测试数量
    skippedTests:
      type: integer
      description: 跳过测试数量
    passRate:
      type: number
      format: float
      description: 通过率百分比
    executionTime:
      type: integer
      description: 执行时间（毫秒）
    coverage:
      $ref: '#/components/schemas/CoverageMetrics'
```

#### TestProjectResult
```yaml
TestProjectResult:
  type: object
  required:
    - projectName
    - totalTests
    - passedTests
    - failedTests
    - skippedTests
    - passRate
    - executionTime
  properties:
    projectName:
      type: string
      description: 项目名称
    totalTests:
      type: integer
      description: 总测试数量
    passedTests:
      type: integer
      description: 通过测试数量
    failedTests:
      type: integer
      description: 失败测试数量
    skippedTests:
      type: integer
      description: 跳过测试数量
    passRate:
      type: number
      format: float
      description: 通过率百分比
    executionTime:
      type: integer
      description: 执行时间（毫秒）
    coverage:
      $ref: '#/components/schemas/CoverageMetrics'
    failures:
      type: array
      items:
        $ref: '#/components/schemas/TestFailure'
```

#### TestFailure
```yaml
TestFailure:
  type: object
  required:
    - testName
    - failureType
    - errorMessage
    - failureTime
  properties:
    testName:
      type: string
      description: 测试名称
    failureType:
      type: string
      enum: [assertion, timeout, exception, compilation, other]
      description: 失败类型
    errorMessage:
      type: string
      description: 错误消息
    stackTrace:
      type: string
      description: 堆栈跟踪
    failureTime:
      type: string
      format: date-time
      description: 失败时间
    className:
      type: string
      description: 测试类名
    methodName:
      type: string
      description: 测试方法名
```

### 覆盖率和分析模型

#### CoverageMetrics
```yaml
CoverageMetrics:
  type: object
  properties:
    lineCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      description: 行覆盖率百分比
    branchCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      description: 分支覆盖率百分比
    methodCoverage:
      type: number
      format: float
      minimum: 0
      maximum: 100
      description: 方法覆盖率百分比
    coveredLines:
      type: integer
      description: 覆盖的代码行数
    totalLines:
      type: integer
      description: 总代码行数
    coveredBranches:
      type: integer
      description: 覆盖的分支数
    totalBranches:
      type: integer
      description: 总分支数
    coveredMethods:
      type: integer
      description: 覆盖的方法数
    totalMethods:
      type: integer
      description: 总方法数
```

#### TestAnalysisResult
```yaml
TestAnalysisResult:
  type: object
  required:
    - overallPassRate
    - projectPassRates
    - coverageMetrics
    - qualityGateStatus
  properties:
    overallPassRate:
      type: number
      format: float
      description: 整体通过率
    projectPassRates:
      type: object
      additionalProperties:
        type: number
        format: float
      description: 各项目通过率
    coverageMetrics:
      $ref: '#/components/schemas/CoverageMetrics'
    qualityGateStatus:
      $ref: '#/components/schemas/QualityGateStatus'
    insights:
      type: array
      items:
        $ref: '#/components/schemas/TestInsight'
    recommendations:
      type: array
      items:
        type: string
      description: 改进建议
```

#### TestInsight
```yaml
TestInsight:
  type: object
  required:
    - type
    - message
    - severity
  properties:
    type:
      type: string
      enum: [performance, reliability, coverage, flakiness, trend]
      description: 洞察类型
    message:
      type: string
      description: 洞察消息
    severity:
      type: string
      enum: [info, warning, error, critical]
      description: 严重程度
    metrics:
      type: object
      additionalProperties:
        type: number
      description: 相关指标
    timestamp:
      type: string
      format: date-time
      description: 洞察时间
```

### 质量门禁模型

#### QualityGateResult
```yaml
QualityGateResult:
  type: object
  required:
    - passed
    - decision
  properties:
    passed:
      type: boolean
      description: 是否通过质量门禁
    decision:
      $ref: '#/components/schemas/QualityGateDecision'
    violations:
      type: array
      items:
        $ref: '#/components/schemas/QualityGateViolation'
    reason:
      type: string
      description: 决策原因
    evaluatedAt:
      type: string
      format: date-time
      description: 评估时间
    thresholds:
      $ref: '#/components/schemas/QualityGateThresholds'
```

#### QualityGateDecision
```yaml
QualityGateDecision:
  type: string
  enum:
    - pass
    - pass_with_warnings
    - fail
    - manual_review
  description: 质量门禁决策
```

#### QualityGateViolation
```yaml
QualityGateViolation:
  type: object
  required:
    - ruleName
    - thresholdValue
    - actualValue
    - severity
  properties:
    ruleName:
      type: string
      description: 规则名称
    thresholdValue:
      type: number
      description: 阈值
    actualValue:
      type: number
      description: 实际值
    severity:
      type: string
      enum: [low, medium, high, critical]
      description: 严重程度
    message:
      type: string
      description: 违规消息
    category:
      type: string
      enum: [pass_rate, coverage, performance, reliability]
      description: 违规类别
```

### 报告和导出模型

#### ReportRequest
```yaml
ReportRequest:
  type: object
  required:
    - executionId
    - format
  properties:
    executionId:
      type: string
      format: uuid
      description: 执行ID
    format:
      type: string
      enum: [html, json, csv, pdf, xml]
      description: 报告格式
    template:
      type: string
      description: 自定义模板名称
    includeDetails:
      type: boolean
      default: true
      description: 是否包含详细信息
    includeCharts:
      type: boolean
      default: true
      description: 是否包含图表
    language:
      type: string
      default: en
      description: 报告语言
```

#### ExportRequest
```yaml
ExportRequest:
  type: object
  required:
    - data
    - format
  properties:
    data:
      type: string
      enum: [execution_results, trend_analysis, failure_analysis, coverage_data]
      description: 导出数据类型
    format:
      type: string
      enum: [json, csv, xml, excel]
      description: 导出格式
    filters:
      type: object
      additionalProperties:
        type: string
      description: 数据过滤器
    dateRange:
      $ref: '#/components/schemas/DateRange'
    projects:
      type: array
      items:
        type: string
      description: 项目过滤器
```

### 通用模型

#### DateRange
```yaml
DateRange:
  type: object
  required:
    - start
    - end
  properties:
    start:
      type: string
      format: date-time
      description: 开始日期
    end:
      type: string
      format: date-time
      description: 结束日期
```

#### PaginationRequest
```yaml
PaginationRequest:
  type: object
  properties:
    page:
      type: integer
      default: 1
      minimum: 1
      description: 页码
    pageSize:
      type: integer
      default: 20
      minimum: 1
      maximum: 100
      description: 页面大小
    sortBy:
      type: string
      description: 排序字段
    sortOrder:
      type: string
      enum: [asc, desc]
      default: desc
      description: 排序顺序
```

#### PaginationResponse
```yaml
PaginationResponse:
  type: object
  required:
    - items
    - totalCount
    - page
    - pageSize
    - totalPages
  properties:
    items:
      type: array
      description: 数据项
    totalCount:
      type: integer
      description: 总数量
    page:
      type: integer
      description: 当前页码
    pageSize:
      type: integer
      description: 页面大小
    totalPages:
      type: integer
      description: 总页数
    hasNext:
      type: boolean
      description: 是否有下一页
    hasPrevious:
      type: boolean
      description: 是否有上一页
```

## API端点

### 1. 测试执行管理

#### POST /test-executions
启动测试执行
```yaml
post:
  tags:
    - Test Execution
  summary: 启动测试执行
  description: 启动一个或多个测试项目的测试执行
  operationId: startTestExecution
  requestBody:
    required: true
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/TestExecutionRequest'
  responses:
    '202':
      description: 测试执行已启动
      content:
        application/json:
          schema:
            type: object
            properties:
              executionId:
                type: string
                format: uuid
              status:
                $ref: '#/components/schemas/TestExecutionStatus'
              message:
                type: string
    '400':
      description: 请求参数错误
    '401':
      description: 未授权
    '429':
      description: 请求过于频繁
```

#### GET /test-executions/{executionId}
获取测试执行状态
```yaml
get:
  tags:
    - Test Execution
  summary: 获取测试执行状态
  description: 获取指定测试执行的详细状态和结果
  operationId: getTestExecution
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
  responses:
    '200':
      description: 测试执行详情
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/TestExecutionResult'
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### GET /test-executions
获取测试执行列表
```yaml
get:
  tags:
    - Test Execution
  summary: 获取测试执行列表
  description: 分页获取测试执行历史记录
  operationId: listTestExecutions
  parameters:
    - name: status
      in: query
      schema:
        $ref: '#/components/schemas/TestExecutionStatus'
    - name: triggerType
      in: query
      schema:
        type: string
        enum: [manual, scheduled, ci_cd, api]
    - name: startDate
      in: query
      schema:
        type: string
        format: date-time
    - name: endDate
      in: query
      schema:
        type: string
        format: date-time
    - $ref: '#/components/parameters/Pagination'
  responses:
    '200':
      description: 测试执行列表
      content:
        application/json:
          schema:
            type: object
            properties:
              items:
                type: array
                items:
                  $ref: '#/components/schemas/TestExecutionResult'
              pagination:
                $ref: '#/components/schemas/PaginationResponse'
    '401':
      description: 未授权
```

#### DELETE /test-executions/{executionId}
取消测试执行
```yaml
delete:
  tags:
    - Test Execution
  summary: 取消测试执行
  description: 取消正在运行的测试执行
  operationId: cancelTestExecution
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
  responses:
    '200':
      description: 测试执行已取消
    '404':
      description: 测试执行不存在
    '409':
      description: 测试执行已完成，无法取消
    '401':
      description: 未授权
```

### 2. 测试结果分析

#### GET /test-executions/{executionId}/analysis
获取测试分析结果
```yaml
get:
  tags:
    - Test Analysis
  summary: 获取测试分析结果
  description: 获取指定测试执行的详细分析结果
  operationId: getTestAnalysis
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
  responses:
    '200':
      description: 测试分析结果
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/TestAnalysisResult'
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### GET /test-executions/{executionId}/failures
获取测试失败详情
```yaml
get:
  tags:
    - Test Analysis
  summary: 获取测试失败详情
  description: 获取指定测试执行的失败测试详情
  operationId: getTestFailures
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
    - name: project
      in: query
      schema:
        type: string
      description: 项目名称过滤器
    - name: failureType
      in: query
      schema:
        type: string
        enum: [assertion, timeout, exception, compilation, other]
    - $ref: '#/components/parameters/Pagination'
  responses:
    '200':
      description: 测试失败列表
      content:
        application/json:
          schema:
            type: object
            properties:
              items:
                type: array
                items:
                  $ref: '#/components/schemas/TestFailure'
              pagination:
                $ref: '#/components/schemas/PaginationResponse'
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### GET /test-executions/{executionId}/coverage
获取覆盖率数据
```yaml
get:
  tags:
    - Test Analysis
  summary: 获取覆盖率数据
  description: 获取指定测试执行的代码覆盖率数据
  operationId: getTestCoverage
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
    - name: project
      in: query
      schema:
        type: string
      description: 项目名称过滤器
  responses:
    '200':
      description: 覆盖率数据
      content:
        application/json:
          schema:
            type: object
            properties:
              overall:
                $ref: '#/components/schemas/CoverageMetrics'
              projects:
                type: object
                additionalProperties:
                  $ref: '#/components/schemas/CoverageMetrics'
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

### 3. 质量门禁管理

#### GET /test-executions/{executionId}/quality-gate
获取质量门禁结果
```yaml
get:
  tags:
    - Quality Gate
  summary: 获取质量门禁结果
  description: 获取指定测试执行的质量门禁评估结果
  operationId: getQualityGateResult
  parameters:
    - name: executionId
      in: path
      required: true
      schema:
        type: string
        format: uuid
  responses:
    '200':
      description: 质量门禁结果
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/QualityGateResult'
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### POST /quality-gate/evaluate
评估质量门禁
```yaml
post:
  tags:
    - Quality Gate
  summary: 评估质量门禁
  description: 基于测试结果评估质量门禁
  operationId: evaluateQualityGate
  requestBody:
    required: true
    content:
      application/json:
        schema:
          type: object
          properties:
            executionId:
              type: string
              format: uuid
            thresholds:
              $ref: '#/components/schemas/QualityGateThresholds'
  responses:
    '200':
      description: 质量门禁评估结果
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/QualityGateResult'
    '400':
      description: 请求参数错误
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### PUT /quality-gate/thresholds
更新质量门禁阈值
```yaml
put:
  tags:
    - Quality Gate
  summary: 更新质量门禁阈值
  description: 更新质量门禁的阈值配置
  operationId: updateQualityGateThresholds
  requestBody:
    required: true
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/QualityGateThresholds'
  responses:
    '200':
      description: 质量门禁阈值已更新
    '400':
      description: 请求参数错误
    '401':
      description: 未授权
    '403':
      description: 权限不足
```

### 4. 历史数据和趋势分析

#### GET /analytics/trends
获取趋势分析数据
```yaml
get:
  tags:
    - Analytics
  summary: 获取趋势分析数据
  description: 获取测试通过率和覆盖率的历史趋势数据
  operationId: getTrendAnalysis
  parameters:
    - name: startDate
      in: query
      required: true
      schema:
        type: string
        format: date-time
    - name: endDate
      in: query
      required: true
      schema:
        type: string
        format: date-time
    - name: projects
      in: query
      schema:
        type: array
        items:
          type: string
      description: 项目名称列表
    - name: interval
      in: query
      schema:
        type: string
        enum: [hour, day, week, month]
        default: day
      description: 时间间隔
  responses:
    '200':
      description: 趋势分析数据
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/TestTrendAnalysis'
    '400':
      description: 请求参数错误
    '401':
      description: 未授权
```

#### GET /analytics/insights
获取洞察数据
```yaml
get:
  tags:
    - Analytics
  summary: 获取洞察数据
  description: 获取基于历史数据的测试质量洞察
  operationId: getInsights
  parameters:
    - name: days
      in: query
      schema:
        type: integer
        default: 30
        minimum: 1
        maximum: 365
      description: 分析天数
    - name: project
      in: query
      schema:
        type: string
      description: 项目名称过滤器
    - name: severity
      in: query
      schema:
        type: string
        enum: [info, warning, error, critical]
      description: 洞察严重程度过滤器
  responses:
    '200':
      description: 洞察数据
      content:
        application/json:
          schema:
            type: array
            items:
              $ref: '#/components/schemas/TestInsight'
    '401':
      description: 未授权
```

#### GET /analytics/failure-patterns
获取失败模式分析
```yaml
get:
  tags:
    - Analytics
  summary: 获取失败模式分析
  description: 分析测试失败的模式和趋势
  operationId: getFailurePatterns
  parameters:
    - name: startDate
      in: query
      required: true
      schema:
        type: string
        format: date-time
    - name: endDate
      in: query
      required: true
      schema:
        type: string
        format: date-time
    - name: project
      in: query
      schema:
        type: string
      description: 项目名称过滤器
  responses:
    '200':
      description: 失败模式分析
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/TestFailureAnalysis'
    '400':
      description: 请求参数错误
    '401':
      description: 未授权
```

### 5. 报告生成和导出

#### POST /reports/generate
生成报告
```yaml
post:
  tags:
    - Reports
  summary: 生成报告
  description: 生成指定格式的测试报告
  operationId: generateReport
  requestBody:
    required: true
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/ReportRequest'
  responses:
    '200':
      description: 报告已生成
      content:
        application/json:
          schema:
            type: object
            properties:
              reportId:
                type: string
                format: uuid
              downloadUrl:
                type: string
                format: uri
              expiresAt:
                type: string
                format: date-time
    '400':
      description: 请求参数错误
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

#### GET /reports/{reportId}/download
下载报告
```yaml
get:
  tags:
    - Reports
  summary: 下载报告
  description: 下载生成的测试报告
  operationId: downloadReport
  parameters:
    - name: reportId
      in: path
      required: true
      schema:
        type: string
        format: uuid
  responses:
    '200':
      description: 报告文件
      content:
        application/octet-stream:
          schema:
            type: string
            format: binary
    '404':
      description: 报告不存在或已过期
    '401':
      description: 未授权
```

#### POST /export/export-data
导出数据
```yaml
post:
  tags:
    - Export
  summary: 导出数据
  description: 导出测试数据到指定格式
  operationId: exportData
  requestBody:
    required: true
    content:
      application/json:
        schema:
          $ref: '#/components/schemas/ExportRequest'
  responses:
    '200':
      description: 数据导出结果
      content:
        application/json:
          schema:
            type: object
            properties:
              exportId:
                type: string
                format: uuid
              downloadUrl:
                type: string
                format: uri
              fileSize:
                type: integer
              recordCount:
                type: integer
              expiresAt:
                type: string
                format: date-time
    '400':
      description: 请求参数错误
    '401':
      description: 未授权
```

### 6. 配置管理

#### GET /config/thresholds
获取质量门禁阈值配置
```yaml
get:
  tags:
    - Configuration
  summary: 获取质量门禁阈值配置
  description: 获取当前的质量门禁阈值配置
  operationId: getQualityGateThresholds
  responses:
    '200':
      description: 质量门禁阈值配置
      content:
        application/json:
          schema:
            $ref: '#/components/schemas/QualityGateThresholds'
    '401':
      description: 未授权
```

#### GET /config/projects
获取项目配置
```yaml
get:
  tags:
    - Configuration
  summary: 获取项目配置
  description: 获取所有可测试项目的配置信息
  operationId: getProjectConfigurations
  responses:
    '200':
      description: 项目配置列表
      content:
        application/json:
          schema:
            type: array
            items:
              type: object
              properties:
                name:
                  type: string
                path:
                  type: string
                enabled:
                  type: boolean
                priority:
                  type: integer
                lastExecution:
                  type: string
                  format: date-time
    '401':
      description: 未授权
```

#### PUT /config/projects/{projectName}
更新项目配置
```yaml
put:
  tags:
    - Configuration
  summary: 更新项目配置
  description: 更新指定项目的配置信息
  operationId: updateProjectConfiguration
  parameters:
    - name: projectName
      in: path
      required: true
      schema:
        type: string
  requestBody:
    required: true
    content:
      application/json:
        schema:
          type: object
          properties:
            enabled:
              type: boolean
            priority:
              type: integer
            customSettings:
              type: object
              additionalProperties:
                type: string
  responses:
    '200':
      description: 项目配置已更新
    '400':
      description: 请求参数错误
    '404':
      description: 项目不存在
    '401':
      description: 未授权
    '403':
      description: 权限不足
```

### 7. 系统状态和监控

#### GET /health
健康检查
```yaml
get:
  tags:
    - System
  summary: 健康检查
  description: 系统健康状态检查
  operationId: healthCheck
  responses:
    '200':
      description: 系统健康
      content:
        application/json:
          schema:
            type: object
            properties:
              status:
                type: string
                enum: [healthy, degraded, unhealthy]
              timestamp:
                type: string
                format: date-time
              version:
                type: string
              services:
                type: object
                additionalProperties:
                  type: string
                  enum: [healthy, degraded, unhealthy]
    '503':
      description: 系统不健康
```

#### GET /metrics
系统指标
```yaml
get:
  tags:
    - System
  summary: 系统指标
  description: 获取系统运行指标
  operationId: getMetrics
  responses:
    '200':
      description: 系统指标
      content:
        application/json:
          schema:
            type: object
            properties:
              uptime:
                type: integer
                description: 运行时间（秒）
              executions_total:
                type: integer
                description: 总执行次数
              executions_success:
                type: integer
                description: 成功执行次数
              executions_failed:
                type: integer
                description: 失败执行次数
              avg_execution_time:
                type: number
                description: 平均执行时间（秒）
              database_connections:
                type: integer
                description: 数据库连接数
              memory_usage:
                type: number
                description: 内存使用量（MB）
              cpu_usage:
                type: number
                description: CPU使用率
    '401':
      description: 未授权
```

#### GET /system/info
系统信息
```yaml
get:
  tags:
    - System
  summary: 系统信息
  description: 获取系统版本和配置信息
  operationId: getSystemInfo
  responses:
    '200':
      description: 系统信息
      content:
        application/json:
          schema:
            type: object
            properties:
              version:
                type: string
              buildDate:
                type: string
                format: date-time
              environment:
                type: string
                enum: [development, staging, production]
              features:
                type: array
                items:
                  type: string
              limits:
                type: object
                properties:
                  maxConcurrentExecutions:
                    type: integer
                  maxExecutionTime:
                    type: integer
                  maxHistoryDays:
                    type: integer
    '401':
      description: 未授权
```

## WebSocket接口

### /ws/test-executions/{executionId}
测试执行实时状态推送
```yaml
/ws/test-executions/{executionId}:
  get:
    tags:
      - WebSocket
    summary: 测试执行实时状态
    description: 通过WebSocket接收测试执行实时状态更新
    operationId: testExecutionWebSocket
    parameters:
      - name: executionId
        in: path
        required: true
        schema:
          type: string
          format: uuid
  responses:
    '101':
      description: WebSocket连接已建立
    '404':
      description: 测试执行不存在
    '401':
      description: 未授权
```

### WebSocket消息格式
```yaml
WebSocketMessage:
  type: object
  required:
    - type
    - timestamp
  properties:
    type:
      type: string
      enum: [status_update, progress_update, test_result, failure_notification, completion]
    timestamp:
      type: string
      format: date-time
    executionId:
      type: string
      format: uuid
    data:
      type: object
      description: 消息数据，根据类型不同而变化
```

## 错误处理

### 错误响应格式
```yaml
ErrorResponse:
  type: object
  required:
    - error
    - timestamp
  properties:
    error:
      type: object
      required:
        - code
        - message
      properties:
        code:
          type: string
          description: 错误代码
        message:
          type: string
          description: 错误消息
        details:
          type: object
          description: 错误详细信息
        stackTrace:
          type: string
          description: 堆栈跟踪（仅开发环境）
    timestamp:
      type: string
      format: date-time
      description: 错误时间
    requestId:
      type: string
      description: 请求ID
```

### 常见错误代码
```yaml
ErrorCodes:
  VALIDATION_ERROR:
    code: "VALIDATION_ERROR"
    message: "请求参数验证失败"
    httpStatus: 400
  
  AUTHENTICATION_ERROR:
    code: "AUTHENTICATION_ERROR"
    message: "认证失败"
    httpStatus: 401
  
  AUTHORIZATION_ERROR:
    code: "AUTHORIZATION_ERROR"
    message: "权限不足"
    httpStatus: 403
  
  NOT_FOUND:
    code: "NOT_FOUND"
    message: "资源不存在"
    httpStatus: 404
  
  CONFLICT:
    code: "CONFLICT"
    message: "资源冲突"
    httpStatus: 409
  
  RATE_LIMIT_EXCEEDED:
    code: "RATE_LIMIT_EXCEEDED"
    message: "请求频率超限"
    httpStatus: 429
  
  INTERNAL_ERROR:
    code: "INTERNAL_ERROR"
    message: "内部服务器错误"
    httpStatus: 500
  
  SERVICE_UNAVAILABLE:
    code: "SERVICE_UNAVAILABLE"
    message: "服务不可用"
    httpStatus: 503
```

## 限流和配额

### 限流规则
```yaml
RateLimits:
  default:
    requests: 100
    per: minute
  
  authenticated:
    requests: 1000
    per: minute
  
  api_key:
    requests: 5000
    per: minute
```

### 配额限制
```yaml
Quotas:
  max_concurrent_executions: 10
  max_execution_history_days: 365
  max_report_retention_days: 30
  max_export_records: 100000
```

## 附录

### 数据类型参考
- **UUID**: 通用唯一标识符，格式为 `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
- **DateTime**: ISO 8601格式，如 `2024-01-01T12:00:00Z`
- **Duration**: ISO 8601持续时间格式，如 `PT30M` 表示30分钟

### HTTP状态码使用
- **200 OK**: 请求成功
- **201 Created**: 资源创建成功
- **202 Accepted**: 请求已接受，正在处理
- **400 Bad Request**: 请求参数错误
- **401 Unauthorized**: 未授权
- **403 Forbidden**: 权限不足
- **404 Not Found**: 资源不存在
- **409 Conflict**: 资源冲突
- **429 Too Many Requests**: 请求过于频繁
- **500 Internal Server Error**: 服务器内部错误
- **503 Service Unavailable**: 服务不可用

### 扩展性考虑
- API版本控制通过URL路径实现（/v1/）
- 支持字段级别的扩展和向后兼容
- 预留了未来功能扩展的接口空间
- 支持自定义字段和元数据