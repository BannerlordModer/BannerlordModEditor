openapi: 3.0.0
info:
  title: Bannerlord Mod Editor API
  version: 1.0.0
  description: API specification for the Bannerlord Mod Editor DO/DTO layered architecture system

servers:
  - url: https://api.bannerlordmodeditor.local
    description: Local development server

paths:
  /xml/files:
    get:
      summary: List available XML files
      operationId: listXmlFiles
      parameters:
        - name: directory
          in: query
          schema:
            type: string
          description: Directory to search for XML files
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                type: object
                properties:
                  files:
                    type: array
                    items:
                      $ref: '#/components/schemas/XmlFile'

  /xml/load:
    post:
      summary: Load XML file into DO representation
      operationId: loadXmlFile
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/LoadXmlRequest'
      responses:
        '200':
          description: XML loaded successfully
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/LoadXmlResponse'
        '400':
          description: Invalid request
        '404':
          description: File not found

  /xml/save:
    post:
      summary: Save DO representation to XML file
      operationId: saveXmlFile
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/SaveXmlRequest'
      responses:
        '200':
          description: XML saved successfully
        '400':
          description: Invalid request

  /mapping/dto-to-do:
    post:
      summary: Convert DTO to DO representation
      operationId: convertDtoToDo
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/DtoToDoRequest'
      responses:
        '200':
          description: Conversion successful
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/DtoToDoResponse'
        '400':
          description: Invalid request

  /mapping/do-to-dto:
    post:
      summary: Convert DO to DTO representation
      operationId: convertDoToDto
      requestBody:
        required: true
        content:
          application/json:
            schema:
              $ref: '#/components/schemas/DoToDtoRequest'
      responses:
        '200':
          description: Conversion successful
          content:
            application/json:
              schema:
                $ref: '#/components/schemas/DoToDtoResponse'
        '400':
          description: Invalid request

components:
  schemas:
    XmlFile:
      type: object
      properties:
        name:
          type: string
          description: File name
        path:
          type: string
          description: Full file path
        size:
          type: integer
          description: File size in bytes
        lastModified:
          type: string
          format: date-time
          description: Last modification timestamp

    LoadXmlRequest:
      type: object
      properties:
        filePath:
          type: string
          description: Path to the XML file to load
        modelType:
          type: string
          description: Type of DO model to deserialize into
      required:
        - filePath
        - modelType

    LoadXmlResponse:
      type: object
      properties:
        success:
          type: boolean
        data:
          type: object
          description: DO representation of the XML file
        errorMessage:
          type: string
          description: Error message if operation failed

    SaveXmlRequest:
      type: object
      properties:
        filePath:
          type: string
          description: Path to save the XML file
        data:
          type: object
          description: DO representation to serialize
        originalXml:
          type: string
          description: Original XML content for namespace preservation
      required:
        - filePath
        - data

    DtoToDoRequest:
      type: object
      properties:
        dto:
          type: object
          description: DTO object to convert
        targetType:
          type: string
          description: Target DO type
      required:
        - dto
        - targetType

    DtoToDoResponse:
      type: object
      properties:
        success:
          type: boolean
        data:
          type: object
          description: DO representation
        errorMessage:
          type: string
          description: Error message if conversion failed

    DoToDtoRequest:
      type: object
      properties:
        do:
          type: object
          description: DO object to convert
        targetType:
          type: string
          description: Target DTO type
      required:
        - do
        - targetType

    DoToDtoResponse:
      type: object
      properties:
        success:
          type: boolean
        data:
          type: object
          description: DTO representation
        errorMessage:
          type: string
          description: Error message if conversion failed

    # Example DO model
    ItemDo:
      type: object
      properties:
        id:
          type: string
        multiplayerItem:
          type: string
        weight:
          type: string
        itemComponent:
          $ref: '#/components/schemas/ItemComponentDo'

    ItemComponentDo:
      type: object
      properties:
        armor:
          $ref: '#/components/schemas/ArmorDo'

    ArmorDo:
      type: object
      properties:
        headArmor:
          type: string
        hasGenderVariations:
          type: string

    # Example DTO model
    ItemDto:
      type: object
      properties:
        id:
          type: string
        multiplayerItem:
          $ref: '#/components/schemas/BooleanProperty'
        weight:
          type: number
          format: decimal
        itemComponent:
          $ref: '#/components/schemas/ItemComponentDto'

    ItemComponentDto:
      type: object
      properties:
        armor:
          $ref: '#/components/schemas/ArmorDto'

    ArmorDto:
      type: object
      properties:
        headArmor:
          type: integer
        hasGenderVariations:
          type: boolean

    BooleanProperty:
      type: object
      properties:
        value:
          type: boolean
        originalValue:
          type: string
        xmlValue:
          type: string