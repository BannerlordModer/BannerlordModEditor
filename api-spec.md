```yaml
openapi: 3.0.0
info:
  title: Bannerlord XML Model API
  version: 1.0.0
  description: API for XML model serialization and deserialization in Bannerlord Mod Editor

servers:
  - url: https://localhost:5001/api/v1
    description: Local development server

paths:
  /models/physics:
    get:
      summary: List all physics materials
      operationId: listPhysicsMaterials
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                type: object
                properties:
                  materials:
                    type: array
                    items:
                      $ref: '#/components/schemas/PhysicsMaterial'
    post:
      summary: Create or update physics materials
      operationId: updatePhysicsMaterials
      requestBody:
        required: true
        content:
          application/xml:
            schema:
              type: string
              format: xml
              description: XML representation of physics materials
      responses:
        '200':
          description: Successful update
        '400':
          description: Invalid XML format

  /models/monsters:
    get:
      summary: List all monster definitions
      operationId: listMonsters
      responses:
        '200':
          description: Successful response
          content:
            application/json:
              schema:
                type: object
                properties:
                  monsters:
                    type: array
                    items:
                      $ref: '#/components/schemas/Monster'

components:
  schemas:
    PhysicsMaterial:
      type: object
      properties:
        id:
          type: string
          description: Unique identifier for the physics material
        overrideMaterialNameForImpactSounds:
          type: string
          nullable: true
          description: Override material name for impact sounds
        dontStickMissiles:
          type: string
          nullable: true
          enum: [true, false]
          description: Whether missiles should not stick to this material
        attacksCanPassThrough:
          type: string
          nullable: true
          enum: [true, false]
          description: Whether attacks can pass through this material
        rainSplashesEnabled:
          type: string
          nullable: true
          enum: [true, false]
          description: Whether rain splashes are enabled
        flammable:
          type: string
          nullable: true
          enum: [true, false]
          description: Whether the material is flammable
        staticFriction:
          type: string
          nullable: true
          description: Static friction coefficient
        dynamicFriction:
          type: string
          nullable: true
          description: Dynamic friction coefficient
        restitution:
          type: string
          nullable: true
          description: Restitution coefficient
        softness:
          type: string
          nullable: true
          description: Material softness
        linearDamping:
          type: string
          nullable: true
          description: Linear damping coefficient
        angularDamping:
          type: string
          nullable: true
          description: Angular damping coefficient
        displayColor:
          type: string
          nullable: true
          description: Display color as RGBA values
      required:
        - id

    Monster:
      type: object
      properties:
        id:
          type: string
          description: Unique identifier for the monster
        baseMonster:
          type: string
          nullable: true
          description: Base monster this monster inherits from
        actionSet:
          type: string
          nullable: true
          description: Action set used by this monster
        femaleActionSet:
          type: string
          nullable: true
          description: Female-specific action set
        monsterUsage:
          type: string
          nullable: true
          description: Monster usage definition
        weight:
          type: string
          nullable: true
          description: Monster weight
        hitPoints:
          type: string
          nullable: true
          description: Monster hit points
        absorbedDamageRatio:
          type: string
          nullable: true
          description: Ratio of damage absorbed
        flags:
          $ref: '#/components/schemas/MonsterFlags'
        capsules:
          $ref: '#/components/schemas/MonsterCapsules'

    MonsterFlags:
      type: object
      properties:
        canAttack:
          type: string
          nullable: true
          enum: [true, false]
        canDefend:
          type: string
          nullable: true
          enum: [true, false]
        canKick:
          type: string
          nullable: true
          enum: [true, false]
        canBeCharged:
          type: string
          nullable: true
          enum: [true, false]
        canCharge:
          type: string
          nullable: true
          enum: [true, false]
        canClimbLadders:
          type: string
          nullable: true
          enum: [true, false]
        canSprint:
          type: string
          nullable: true
          enum: [true, false]
        canCrouch:
          type: string
          nullable: true
          enum: [true, false]
        canRetreat:
          type: string
          nullable: true
          enum: [true, false]
        canRear:
          type: string
          nullable: true
          enum: [true, false]
        canWander:
          type: string
          nullable: true
          enum: [true, false]
        canBeInGroup:
          type: string
          nullable: true
          enum: [true, false]
        moveAsHerd:
          type: string
          nullable: true
          enum: [true, false]
        moveForwardOnly:
          type: string
          nullable: true
          enum: [true, false]
        isHumanoid:
          type: string
          nullable: true
          enum: [true, false]
        mountable:
          type: string
          nullable: true
          enum: [true, false]
        canRide:
          type: string
          nullable: true
          enum: [true, false]
        canWieldWeapon:
          type: string
          nullable: true
          enum: [true, false]
        runsAwayWhenHit:
          type: string
          nullable: true
          enum: [true, false]
        canGetScared:
          type: string
          nullable: true
          enum: [true, false]

    MonsterCapsules:
      type: object
      properties:
        bodyCapsule:
          $ref: '#/components/schemas/MonsterCapsule'
        crouchedBodyCapsule:
          $ref: '#/components/schemas/MonsterCapsule'

    MonsterCapsule:
      type: object
      properties:
        radius:
          type: string
          nullable: true
        pos1:
          type: string
          nullable: true
        pos2:
          type: string
          nullable: true
```