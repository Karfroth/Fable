// fsharplint:disable MemberNames InterfaceNames
namespace rec Fable.AST.Babel

open Fable.AST

/// The type field is a string representing the AST variant type.
/// Each subtype of Node is documented below with the specific string of its type field.
/// You can use this field to determine which interface a node implements.
/// The loc field represents the source location information of the node.
/// If the node contains no information about the source location, the field is null;
/// otherwise it is an object consisting of a start position (the position of the first character of the parsed source region)
/// and an end position (the position of the first character after the parsed source region):
type Node =
    | Pattern of Pattern
    | Program of Program
    | Statement of Statement
    | Directive of value: DirectiveLiteral // e.g. "use strict";
    | ClassBody of ClassBody
    | Expression of Expression
    | SwitchCase of SwitchCase
    | CatchClause of CatchClause
    | ObjectMember of ObjectMember
    | TypeParameter of TypeParameter
    | TypeAnnotation of TypeAnnotation
    | ExportSpecifier of ExportSpecifier
    | ImportSpecifier of ImportSpecifier
    | InterfaceExtends of InterfaceExtends
    | ObjectTypeIndexer of ObjectTypeIndexer
    | FunctionTypeParam of FunctionTypeParam
    | ModuleDeclaration of ModuleDeclaration
    | VariableDeclarator of VariableDeclarator
    | TypeAnnotationInfo of TypeAnnotationInfo
    | ObjectTypeProperty of ObjectTypeProperty
    | ObjectTypeCallProperty of ObjectTypeCallProperty
    | ObjectTypeInternalSlot of ObjectTypeInternalSlot
    | TypeParameterDeclaration of TypeParameterDeclaration
    | TypeParameterInstantiation of TypeParameterInstantiation

/// Since the left-hand side of an assignment may be any expression in general, an expression can also be a pattern.
type Expression =
    | Literal of Literal
    | Identifier of Identifier
    | ClassExpression of ClassExpression
    | ClassImplements of ClassImplements
    | UnaryExpression of UnaryExpression
    | UpdateExpression of UpdateExpression
    | BinaryExpression of BinaryExpression
    | Super of loc: SourceLocation option
    | Undefined of Loc: SourceLocation option
    | FunctionExpression of FunctionExpression
    | ThisExpression of loc: SourceLocation option
    | SpreadElement of argument: Expression * loc: SourceLocation option
    | ArrayExpression of elements: Expression array * loc: SourceLocation option
    | ObjectExpression of properties: ObjectMember array * loc: SourceLocation option
    | SequenceExpression of expressions: Expression array * loc: SourceLocation option
    | EmitExpression of value: string * args: Expression array * loc: SourceLocation option
    | CallExpression of callee: Expression * arguments: Expression array * loc: SourceLocation option
    | LogicalExpression of left: Expression * operator: string * right: Expression * loc: SourceLocation option
    | AssignmentExpression of left: Expression * right: Expression * operator: string * loc: SourceLocation option
    | ConditionalExpression of test: Expression * consequent: Expression * alternate: Expression * loc: SourceLocation option
    | MemberExpression of name: string * object: Expression * property: Expression * computed: bool * loc: SourceLocation option
    | NewExpression of callee: Expression * arguments: Expression array * typeArguments: TypeParameterInstantiation option * loc: SourceLocation option
    | ArrowFunctionExpression of ``params``: Pattern array * body: BlockStatement * returnType: TypeAnnotation option * typeParameters: TypeParameterDeclaration option * loc: SourceLocation option

type Pattern =
    | RestElement of RestElement
    | IdentifierPattern of Identifier

    member this.Name =
        match this with
        | RestElement(el) -> el.Name
        | IdentifierPattern(Identifier(name=name)) -> name

type Literal =
    | RegExp of pattern: string * flags: string * loc: SourceLocation option
    | NullLiteral of loc: SourceLocation option
    | StringLiteral of StringLiteral
    | BooleanLiteral of value: bool * loc: SourceLocation option
    | NumericLiteral of value: float * loc: SourceLocation option
    | DirectiveLiteral of DirectiveLiteral

type Statement =
    | Declaration of Declaration
    | BlockStatement of BlockStatement
    | ExpressionStatement of expr: Expression /// An expression statement, i.e., a statement consisting of a single expression.
    | DebuggerStatement of loc: SourceLocation option
    | LabeledStatement of body: Statement * label: Identifier
    | ThrowStatement of argument: Expression * loc: SourceLocation option
    | ReturnStatement of argument: Expression * loc: SourceLocation option
    | BreakStatement of label: Identifier option * loc: SourceLocation option
    | ContinueStatement of label: Identifier option * loc: SourceLocation option
    | WhileStatement of test: Expression * body: BlockStatement * loc: SourceLocation option
    | SwitchStatement of discriminant: Expression * cases: SwitchCase array * loc: SourceLocation option
    | IfStatement of test: Expression * consequent: BlockStatement * alternate: Statement option * loc: SourceLocation option
    | TryStatement of block: BlockStatement * handler: CatchClause option * finalizer: BlockStatement option * loc: SourceLocation option
    | ForStatement of body: BlockStatement * init: VariableDeclaration option * test: Expression option * update: Expression option * loc: SourceLocation option

/// Note that declarations are considered statements; this is because declarations can appear in any statement context.
type Declaration =
    | ClassDeclaration of ClassDeclaration
    | VariableDeclaration of VariableDeclaration
    | FunctionDeclaration of FunctionDeclaration
    | InterfaceDeclaration of InterfaceDeclaration

/// A module import or export declaration.
type ModuleDeclaration =
    | ImportDeclaration of ImportDeclaration
    | ExportAllDeclaration of source: Literal * loc: SourceLocation option
    | ExportNamedReferences of ExportNamedReferences
    | ExportNamedDeclaration of declaration: Declaration
    | PrivateModuleDeclaration of statement: Statement
    /// An export default declaration, e.g., export default function () {}; or export default 1;.
    | ExportDefaultDeclaration of declaration: Choice<Declaration, Expression>


    /// An export batch declaration, e.g., export * from "mod";.
// Template Literals
//type TemplateElement(value: string, tail, ?loc) =
//    inherit Node("TemplateElement", ?loc = loc)
//    member _.Tail: bool = tail
//    member _.Value = dict [ ("raw", value); ("cooked", value) ]
//
//type TemplateLiteral(quasis, expressions, ?loc) =
//    inherit Literal("TemplateLiteral", ?loc = loc)
//    member _.Quasis: TemplateElement array = quasis
//    member _.Expressions: Expression array = expressions
//
//type TaggedTemplateExpression(tag, quasi, ?loc) =
//    interface Expression with
//    member _.Tag: Expression = tag
//    member _.Quasi: TemplateLiteral = quasi

// Identifier
/// Note that an identifier may be an expression or a destructuring pattern.
type Identifier =
    | Identifier of name: string * optional: bool option * typeAnnotation: TypeAnnotation option * loc: SourceLocation option


// Literals

type StringLiteral =
    | StringLiteral of value: string * loc: SourceLocation option

// Misc
//type Decorator(value, ?loc) =
//    inherit Node("Decorator", ?loc = loc)
//    member _.Value = value
//
type DirectiveLiteral =
    | DirectiveLiteral of value: string

// Program

/// A complete program source tree.
/// Parsers must specify sourceType as "module" if the source has been parsed as an ES6 module.
/// Otherwise, sourceType must be "script".
type Program =
    | Program of body: ModuleDeclaration array

//    let sourceType = "module" // Don't use "script"
//    member _.Directives: Directive array = directives
//    member _.SourceType: string = sourceType

// Statements

/// A block statement, i.e., a sequence of statements surrounded by braces.
type BlockStatement =
    | BlockStatement of body: Statement array


//    let directives = [||] // defaultArg directives_ [||]
//    member _.Directives: Directive array = directives

/// An empty statement, i.e., a solitary semicolon.
//type EmptyStatement(?loc) =
//    inherit Statement("EmptyStatement", ?loc = loc)
//    member _.Print(_) = ()

// type WithStatement

// Control Flow

/// A case (if test is an Expression) or default (if test === null) clause in the body of a switch statement.
type SwitchCase =
    | SwitchCase of test: Expression option * consequent: Statement array * loc: SourceLocation option


// Exceptions

/// A catch clause following a try block.
type CatchClause =
    | CatchClause of param: Pattern * body: BlockStatement * loc: SourceLocation option


// Declarations
type VariableDeclarator =
    | VariableDeclarator of id: Pattern * init: Expression option

type VariableDeclarationKind =
    | Var
    | Let
    | Const

type VariableDeclaration =
    | VariableDeclaration of declarations: VariableDeclarator array * kind: string * loc: SourceLocation option

// Loops

//type DoWhileStatement(body, test, ?loc) =
//    inherit Statement("DoWhileStatement", ?loc = loc)
//    member _.Body: BlockStatement = body
//    member _.Test: Expression = test

/// When passing a VariableDeclaration, the bound value must go through
/// the `right` parameter instead of `init` property in VariableDeclarator
//type ForInStatement(left, right, body, ?loc) =
//    inherit Statement("ForInStatement", ?loc = loc)
//    member _.Body: BlockStatement = body
//    member _.Left: Choice<VariableDeclaration, Expression> = left
//    member _.Right: Expression = right

/// When passing a VariableDeclaration, the bound value must go through
/// the `right` parameter instead of `init` property in VariableDeclarator
//type ForOfStatement(left, right, body, ?loc) =
//    inherit Statement("ForOfStatement", ?loc = loc)
//    member _.Body: BlockStatement = body
//    member _.Left: Choice<VariableDeclaration, Expression> = left
//    member _.Right: Expression = right

/// A function declaration. Note that id cannot be null.
type FunctionDeclaration =
    { Params: Pattern array
      Body: BlockStatement
      Id: Identifier
      ReturnType: TypeAnnotation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member Create(``params``, body, id, ?returnType, ?typeParameters, ?loc) = // ?async_, ?generator_, ?declare,
        { Params = ``params``
          Body = body
          Id = id
          ReturnType = returnType
          TypeParameters = typeParameters
          Loc = loc }

    //    let async = defaultArg async_ false
//    let generator = defaultArg generator_ false
//    member _.Async: bool = async
//    member _.Generator: bool = generator
//    member _.Declare: bool option = declare

// Expressions

//    let async = defaultArg async_ false
//    let generator = defaultArg generator_ false
//    member _.Async: bool = async
//    member _.Generator: bool = generator
type FunctionExpression =
    { Id: Identifier option
      Params: Pattern array
      Body: BlockStatement
      ReturnType: TypeAnnotation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member Create(``params``, body, ?id, ?returnType, ?typeParameters, ?loc) = //?generator_, ?async_
        { Id = id
          Params = ``params``
          Body = body
          ReturnType = returnType
          TypeParameters = typeParameters
          Loc = loc }


//    let async = defaultArg async_ false
//    let generator = defaultArg generator_ false
//    member _.Async: bool = async
//    member _.Generator: bool = generator

///// e.g., x = do { var t = f(); t * t + 1 };
///// http://wiki.ecmascript.org/doku.php?id=strawman:do_expressions
///// Doesn't seem to work well with block-scoped variables (let, const)
//type DoExpression(body, ?loc) =
//    interface Expression with
//    member _.Body: BlockStatement = body

//type YieldExpression(argument, ``delegate``, ?loc) =
//    interface Expression with
//    member _.Argument: Expression option = argument
//    /// Delegates to another generator? (yield*)
//    member _.Delegate: bool = ``delegate``
//
//type AwaitExpression(argument, ?loc) =
//    interface Expression with
//    member _.Argument: Expression option = argument

//type RestProperty(argument, ?loc) =
//    inherit Node("RestProperty", ?loc = loc)
//    member _.Argument: Expression = argument

///// e.g., var z = { x: 1, ...y } // Copy all properties from y
//type SpreadProperty(argument, ?loc) =
//    inherit Node("SpreadProperty", ?loc = loc)
//    member _.Argument: Expression = argument


type ObjectMember =
    | ObjectProperty of key: Expression * value: Expression * computed: bool
    | ObjectMethod of ObjectMethod

//    let shorthand = defaultArg shorthand_ false
//    member _.Shorthand: bool = shorthand

type ObjectMethodKind = ObjectGetter | ObjectSetter | ObjectMeth

type ObjectMethod =
    { Kind: string
      Key: Expression
      Params: Pattern array
      Body: BlockStatement
      Computed: bool
      ReturnType: TypeAnnotation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member Create(kind_, key, ``params``, body, ?computed_, ?returnType, ?typeParameters, ?loc) = // ?async_, ?generator_,
        let kind =
            match kind_ with
            | ObjectGetter -> "get"
            | ObjectSetter -> "set"
            | ObjectMeth -> "method"
        let computed = defaultArg computed_ false
        //    let async = defaultArg async_ false
        //    let generator = defaultArg generator_ false
        //    member _.Async: bool = async
        //    member _.Generator: bool = generator
        { Kind = kind
          Key = key
          Params = ``params``
          Body = body
          Computed = computed
          ReturnType = returnType
          TypeParameters = typeParameters
          Loc = loc }



// Unary Operations
type UnaryExpression =
    { Prefix: bool
      Argument: Expression
      Operator: string
      Loc: SourceLocation option }

    static member Create(operator_, argument, ?loc) =
        let prefix = true
        let operator =
            match operator_ with
            | UnaryMinus -> "-"
            | UnaryPlus -> "+"
            | UnaryNot -> "!"
            | UnaryNotBitwise -> "~"
            | UnaryTypeof -> "typeof"
            | UnaryVoid -> "void"
            | UnaryDelete -> "delete"

        { Prefix = prefix
          Argument = argument
          Operator = operator
          Loc = loc }

type UpdateExpression =
    { Prefix: bool
      Argument: Expression
      Operator: string
      Loc: SourceLocation option }

    static member AsExpr(operator_, prefix, argument, ?loc) : Expression =
        let operator =
            match operator_ with
            | UpdateMinus -> "--"
            | UpdatePlus -> "++"

        { Prefix = prefix
          Argument = argument
          Operator = operator
          Loc = loc }
        |> UpdateExpression

// Binary Operations
type BinaryExpression =
    { Left: Expression
      Right: Expression
      Operator: string
      Loc: SourceLocation option }

    static member Create(operator_, left, right, ?loc) =
        let operator =
            match operator_ with
            | BinaryEqual -> "=="
            | BinaryUnequal -> "!="
            | BinaryEqualStrict -> "==="
            | BinaryUnequalStrict -> "!=="
            | BinaryLess -> "<"
            | BinaryLessOrEqual -> "<="
            | BinaryGreater -> ">"
            | BinaryGreaterOrEqual -> ">="
            | BinaryShiftLeft -> "<<"
            | BinaryShiftRightSignPropagating -> ">>"
            | BinaryShiftRightZeroFill -> ">>>"
            | BinaryMinus -> "-"
            | BinaryPlus -> "+"
            | BinaryMultiply -> "*"
            | BinaryDivide -> "/"
            | BinaryModulus -> "%"
            | BinaryExponent -> "**"
            | BinaryOrBitwise -> "|"
            | BinaryXorBitwise -> "^"
            | BinaryAndBitwise -> "&"
            | BinaryIn -> "in"
            | BinaryInstanceOf -> "instanceof"

        { Left = left
          Right = right
          Operator = operator
          Loc = loc }
// Patterns
// type AssignmentProperty(key, value, ?loc) =
//     inherit ObjectProperty("AssignmentProperty", ?loc = loc)
//     member _.Value: Pattern = value

// type ObjectPattern(properties, ?loc) =
//     inherit Node("ObjectPattern", ?loc = loc)
//     member _.Properties: Choice<AssignmentProperty, RestProperty> array = properties
//     interface Pattern

//type ArrayPattern(elements, ?typeAnnotation, ?loc) =
//    inherit Pattern("ArrayPattern", ?loc = loc)
//    member _.Elements: Pattern option array = elements
//    member _.TypeAnnotation: TypeAnnotation option = typeAnnotation

//type AssignmentPattern(left, right, ?typeAnnotation, ?loc) =
//    inherit Pattern("AssignmentPattern", ?loc = loc)
//    member _.Left: Pattern = left
//    member _.Right: Expression = right
//    member _.TypeAnnotation: TypeAnnotation option = typeAnnotation

type RestElement =
    { Name: string
      Argument: Pattern
      TypeAnnotation: TypeAnnotation option
      Loc: SourceLocation option }

    static member Create(argument: Pattern, ?typeAnnotation, ?loc) =
        { Name = argument.Name
          Argument = argument
          TypeAnnotation = typeAnnotation
          Loc = loc }

// Classes
type ClassMember =
    | ClassMethod of ClassMethod
    | ClassProperty of ClassProperty

type ClassMethodKind =
    | ClassImplicitConstructor | ClassFunction | ClassGetter | ClassSetter

type ClassMethod =
    { Kind: string
      Key: Expression
      Params: Pattern array
      Body: BlockStatement
      Computed: bool
      Static: bool option
      Abstract: bool option
      ReturnType: TypeAnnotation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member AsClassMember(kind_, key, ``params``, body, ?computed_, ?``static``, ?``abstract``, ?returnType, ?typeParameters, ?loc) : ClassMember =
        let kind =
            match kind_ with
            | ClassImplicitConstructor -> "constructor"
            | ClassGetter -> "get"
            | ClassSetter -> "set"
            | ClassFunction -> "method"
        let computed = defaultArg computed_ false

        { Kind = kind
          Key = key
          Params = ``params``
          Body = body
          Computed = computed
          Static = ``static``
          Abstract = ``abstract``
          ReturnType = returnType
          TypeParameters = typeParameters
          Loc = loc }
        |> ClassMethod
    // This appears in astexplorer.net but it's not documented
    // member _.Expression: bool = false

/// ES Class Fields & Static Properties
/// https://github.com/jeffmo/es-class-fields-and-static-properties
/// e.g, class MyClass { static myStaticProp = 5; myProp /* = 10 */; }
type ClassProperty =
    { Key: Expression
      Value: Expression option
      Computed: bool
      Static: bool
      Optional: bool
      TypeAnnotation: TypeAnnotation option
      Loc: SourceLocation option }

    static member AsClassMember(key, ?value, ?computed_, ?``static``, ?optional, ?typeAnnotation, ?loc): ClassMember =
        let computed = defaultArg computed_ false

        { Key = key
          Value = value
          Computed = computed
          Static = defaultArg ``static`` false
          Optional = defaultArg optional false
          TypeAnnotation = typeAnnotation
          Loc = loc }
        |> ClassProperty

type ClassImplements =
    | ClassImplements of id: Identifier * typeParameters: TypeParameterInstantiation option

type ClassBody =
    { Body: ClassMember array
      Loc: SourceLocation option }

    static member Create(body, ?loc) =
        { Body = body
          Loc = loc }

type ClassDeclaration =
    { Body: ClassBody
      Id: Identifier option
      SuperClass: Expression option
      Implements: ClassImplements array option
      SuperTypeParameters: TypeParameterInstantiation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member Create(body, ?id, ?superClass, ?superTypeParameters, ?typeParameters, ?implements, ?loc) =
        { Body = body
          Id = id
          SuperClass = superClass
          Implements = implements
          SuperTypeParameters = superTypeParameters
          TypeParameters = typeParameters
          Loc = loc }

/// Anonymous class: e.g., var myClass = class { }
type ClassExpression =
    { Body: ClassBody
      Id: Identifier option
      SuperClass: Expression option
      Implements: ClassImplements array option
      SuperTypeParameters: TypeParameterInstantiation option
      TypeParameters: TypeParameterDeclaration option
      Loc: SourceLocation option }

    static member Create(body, ?id, ?superClass, ?superTypeParameters, ?typeParameters, ?implements, ?loc) =
        { Body = body
          Id = id
          SuperClass = superClass
          Implements = implements
          SuperTypeParameters = superTypeParameters
          TypeParameters = typeParameters
          Loc = loc }

// type MetaProperty(meta, property, ?loc) =
//     interface Expression with
//     member _.Meta: Identifier = meta
//     member _.Property: Expression = property

// Modules

/// An imported variable binding, e.g., {foo} in import {foo} from "mod" or {foo as bar} in import {foo as bar} from "mod".
/// The imported field refers to the name of the export imported from the module.
/// The local field refers to the binding imported into the local module scope.
/// If it is a basic named import, such as in import {foo} from "mod", both imported and local are equivalent Identifier nodes; in this case an Identifier node representing foo.
/// If it is an aliased import, such as in import {foo as bar} from "mod", the imported field is an Identifier node representing foo, and the local field is an Identifier node representing bar.
type ImportSpecifier =
    /// e.g., import foo from "mod";.
    | ImportMemberSpecifier of local: Identifier * imported: Identifier
    /// A default import specifier, e.g., foo in import foo from "mod".
    | ImportDefaultSpecifier of local: Identifier
    /// A namespace import specifier, e.g., * as foo in import * as foo from "mod".
    | ImportNamespaceSpecifier of local: Identifier

type ImportDeclaration =
    { Specifiers: ImportSpecifier array
      Source: StringLiteral }

    static member AsModuleDeclaration(specifiers, source): ModuleDeclaration =
        { Specifiers = specifiers
          Source = source }
        |> ImportDeclaration

/// An exported variable binding, e.g., {foo} in export {foo} or {bar as foo} in export {bar as foo}.
/// The exported field refers to the name exported in the module.
/// The local field refers to the binding into the local module scope.
/// If it is a basic named export, such as in export {foo}, both exported and local are equivalent Identifier nodes;
/// in this case an Identifier node representing foo. If it is an aliased export, such as in export {bar as foo},
/// the exported field is an Identifier node representing foo, and the local field is an Identifier node representing bar.
type ExportSpecifier =
    { Local: Identifier
      Exported: Identifier }

    static member Create(local, exported) = { Local = local; Exported = exported }

/// An export named declaration, e.g., export {foo, bar};, export {foo} from "mod"; or export var foo = 1;.
/// Note: Having declaration populated with non-empty specifiers or non-null source results in an invalid state.
type ExportNamedReferences =
    { Specifiers: ExportSpecifier array
      Source: StringLiteral option }

    static member AsModuleDeclaration(specifiers, ?source): ModuleDeclaration =
        { Specifiers = specifiers
          Source = source }
        |> ExportNamedReferences

// Type Annotations
type TypeAnnotationInfo =
    | AnyTypeAnnotation
    | VoidTypeAnnotation
    | StringTypeAnnotation
    | NumberTypeAnnotation
    | BooleanTypeAnnotation
    | TypeAnnotationInfo of TypeAnnotationInfo
    | UnionTypeAnnotation of types: TypeAnnotationInfo array
    | ObjectTypeAnnotation of ObjectTypeAnnotation
    | GenericTypeAnnotation of id: Identifier * typeParameters: TypeParameterInstantiation option
    | FunctionTypeAnnotation of FunctionTypeAnnotation
    | NullableTypeAnnotation of typeAnnotation: TypeAnnotationInfo
    | TupleTypeAnnotation of types: TypeAnnotationInfo array

type TypeAnnotation =
    | TypeAnnotation of TypeAnnotationInfo

type TypeParameter =
    | TypeParameter of name: string * bound: TypeAnnotation option * ``default``: TypeAnnotationInfo option


type TypeParameterDeclaration =
    | TypeParameterDeclaration of ``params``: TypeParameter array


type TypeParameterInstantiation =
    | TypeParameterInstantiation of ``params``: TypeAnnotationInfo array


type FunctionTypeParam =
    { Name: Identifier
      TypeAnnotation: TypeAnnotationInfo
      Optional: bool option }

    static member Create(name, typeInfo, ?optional) =
        { Name = name
          TypeAnnotation = typeInfo
          Optional = optional }

type FunctionTypeAnnotation =
    { Params: FunctionTypeParam array
      ReturnType: TypeAnnotationInfo
      TypeParameters: TypeParameterDeclaration option
      Rest: FunctionTypeParam option }

    static member AsTypeAnnotationInfo(``params``, returnType, ?typeParameters, ?rest): TypeAnnotationInfo =
        { Params = ``params``
          ReturnType = returnType
          TypeParameters = typeParameters
          Rest = rest }
        |> FunctionTypeAnnotation


type ObjectTypeProperty =
    { Key: Expression
      Value: TypeAnnotationInfo
      Kind: string option
      Computed: bool
      Static: bool
      Optional: bool
      Proto: bool
      Method: bool }

    static member Create(key, value, ?computed_, ?kind, ?``static``, ?optional, ?proto, ?method) =
        let computed = defaultArg computed_ false

        { Key = key
          Value = value
          Kind = kind
          Computed = computed
          Static = defaultArg ``static`` false
          Optional = defaultArg optional false
          Proto = defaultArg proto false
          Method = defaultArg method false }

type ObjectTypeIndexer =
    { Id: Identifier option
      Key: Identifier
      Value: TypeAnnotationInfo
      Static: bool option }

    static member Create(key, value, ?id, ?``static``) =
        { Id = id
          Key = key
          Value = value
          Static = ``static`` }

type ObjectTypeCallProperty =
    { Value: TypeAnnotationInfo
      Static: bool option }

    static member Create(value, ?``static``) = { Value = value; Static = ``static`` }

type ObjectTypeInternalSlot =
    { Id: Identifier
      Value: TypeAnnotationInfo
      Optional: bool
      Static: bool
      Method: bool }

    static member Create(id, value, optional, ``static``, method) =
        { Id = id
          Value = value
          Optional = optional
          Static = ``static``
          Method = method }

type ObjectTypeAnnotation =
    { Properties: ObjectTypeProperty array
      Indexers: ObjectTypeIndexer array
      CallProperties: ObjectTypeCallProperty array
      InternalSlots: ObjectTypeInternalSlot array
      Exact: bool }

    static member Create(properties, ?indexers_, ?callProperties_, ?internalSlots_, ?exact_) =
        let exact = defaultArg exact_ false
        let indexers = defaultArg indexers_ [||]
        let callProperties = defaultArg callProperties_ [||]
        let internalSlots = defaultArg internalSlots_ [||]

        { Properties = properties
          Indexers = indexers
          CallProperties = callProperties
          InternalSlots = internalSlots
          Exact = exact }

type InterfaceExtends =
    | InterfaceExtends of id: Identifier * typeParameters: TypeParameterInstantiation option


type InterfaceDeclaration =
    { Id: Identifier
      Body: ObjectTypeAnnotation
      Extends: InterfaceExtends array
      Implements: ClassImplements array
      TypeParameters: TypeParameterDeclaration option }

    static member Create(id, body, ?extends_, ?typeParameters, ?implements_): Declaration = // ?mixins_,
        let extends = defaultArg extends_ [||]
        let implements = defaultArg implements_ [||]

        { Id = id
          Body = body
          Extends = extends
          Implements = implements
          TypeParameters = typeParameters }
        |> InterfaceDeclaration

//    let mixins = defaultArg mixins_ [||]
//    member _.Mixins: InterfaceExtends array = mixins

[<AutoOpen>]
module Helpers =
    type Expression with
        static member super(?loc) = Super loc
        static member emitExpression(value, args, ?loc) = EmitExpression(value, args, loc)
        static member nullLiteral(?loc) = NullLiteral loc |> Literal
        static member numericLiteral(value, ?loc) = NumericLiteral (value, loc) |> Literal
        static member booleanLiteral(value, ?loc) = BooleanLiteral (value, loc) |> Literal
        static member stringLiteral(value, ?loc) = Literal.stringLiteral (value, ?loc=loc) |> Literal
        static member binaryExpression(operator_, left, right, ?loc) =
            BinaryExpression.Create(operator_, left, right, ?loc=loc)
            |> BinaryExpression
        static member arrayExpression(elements, ?loc) = ArrayExpression(elements, ?loc=loc)
        static member unaryExpression(operator_, argument, ?loc) =
            UnaryExpression.Create(operator_, argument, ?loc=loc)
            |> UnaryExpression
        static member identifier(name, ?optional, ?typeAnnotation, ?loc) =
            Identifier.identifier(name, ?optional = optional, ?typeAnnotation = typeAnnotation, ?loc = loc)
            |> Expression.Identifier
        static member regExpLiteral(pattern, flags_, ?loc) =
            Literal.regExpLiteral(pattern, flags_, ?loc=loc) |> Literal
        /// A function or method call expression.
        static member callExpression(callee, arguments, ?loc) = CallExpression(callee, arguments, loc)
        static member assignmentExpression(operator_, left, right, ?loc) =
            let operator =
                match operator_ with
                | AssignEqual -> "="
                | AssignMinus -> "-="
                | AssignPlus -> "+="
                | AssignMultiply -> "*="
                | AssignDivide -> "/="
                | AssignModulus -> "%="
                | AssignShiftLeft -> "<<="
                | AssignShiftRightSignPropagating -> ">>="
                | AssignShiftRightZeroFill -> ">>>="
                | AssignOrBitwise -> "|="
                | AssignXorBitwise -> "^="
                | AssignAndBitwise -> "&="
            AssignmentExpression(left, right, operator, loc)
        /// A super pseudo-expression.
        static member thisExpression(?loc) = ThisExpression loc
        /// A comma-separated sequence of expressions.
        static member sequenceExpression(expressions, ?loc) = SequenceExpression(expressions, loc)
        static member logicalExpression(left, operator, right, ?loc) =
            let operator =
                match operator with
                | LogicalOr -> "||"
                | LogicalAnd -> "&&"

            LogicalExpression(left, operator, right, loc)
        static member objectExpression(properties, ?loc) = ObjectExpression(properties, loc)
        static member newExpression(callee, arguments, ?typeArguments, ?loc) = NewExpression(callee, arguments, typeArguments, loc)
        /// A fat arrow function expression, e.g., let foo = (bar) => { /* body */ }.
        static member arrowFunctionExpression(``params``, body: BlockStatement, ?returnType, ?typeParameters, ?loc) = //?async_, ?generator_,
            ArrowFunctionExpression(``params``, body, returnType, typeParameters, loc)
        static member arrowFunctionExpression(``params``, body: Expression, ?returnType, ?typeParameters, ?loc): Expression =
            let body = BlockStatement [| Statement.returnStatement(body) |]
            Expression.arrowFunctionExpression(``params``, body, ?returnType = returnType, ?typeParameters = typeParameters, ?loc = loc)
        /// If computed is true, the node corresponds to a computed (a[b]) member expression and property is an Expression.
        /// If computed is false, the node corresponds to a static (a.b) member expression and property is an Identifier.
        static member memberExpression(object, property, ?computed_, ?loc) =
            let computed = defaultArg computed_ false
            let name =
                match property with
                | Expression.Identifier(Identifier(name=name)) -> name
                | _ -> ""
            MemberExpression(name, object, property, computed, loc)
        static member functionExpression(``params``, body, ?id, ?returnType, ?typeParameters, ?loc) = //?generator_, ?async_
            FunctionExpression.Create(``params``, body, ?id=id, ?returnType=returnType, ?typeParameters=typeParameters, ?loc=loc)
            |> FunctionExpression
        static member classExpression(body, ?id, ?superClass, ?superTypeParameters, ?typeParameters, ?implements, ?loc) =
            ClassExpression.Create(body, ?id=id, ?superClass=superClass, ?superTypeParameters=superTypeParameters, ?typeParameters=typeParameters, ?implements=implements, ?loc=loc)
            |> ClassExpression
        static member spreadElement(argument, ?loc) =
            SpreadElement(argument, ?loc=loc)
        static member conditionalExpression(test, consequent, alternate, ?loc): Expression =
            ConditionalExpression(test, consequent, alternate, loc)


    type Identifier with
        member this.Name =
            let (Identifier(name=name)) = this
            name
        static member identifier(name, ?optional, ?typeAnnotation, ?loc) : Identifier =
            Identifier(name, optional, typeAnnotation, loc)

    type Statement with
        static member blockStatement(body) = BlockStatement body |> Statement.BlockStatement
        static member returnStatement(argument, ?loc) : Statement = ReturnStatement(argument, loc)
        static member continueStatement(label, ?loc) = ContinueStatement(Some label, loc)
        static member tryStatement(block, ?handler, ?finalizer, ?loc) = TryStatement(block, handler, finalizer, loc)
        static member ifStatement(test, consequent, ?alternate, ?loc): Statement = IfStatement(test, consequent, alternate, loc)
        ///static member blockStatement(body) = BlockStatement(body)
        /// Break can optionally take a label of a loop to break
        static member breakStatement(?label, ?loc) = BreakStatement(label, loc)
        /// Statement (typically loop) prefixed with a label (for continue and break)
        static member labeledStatement(label, body): Statement = LabeledStatement (body, label)
        static member whileStatement(test, body, ?loc) = WhileStatement(test, body, loc)
        static member debuggerStatement(?loc) = DebuggerStatement loc
        static member switchStatement(discriminant, cases, ?loc) = SwitchStatement(discriminant, cases, loc)
        static member variableDeclaration(kind, declarations, ?loc): Statement =
            Declaration.variableDeclaration(kind, declarations, ?loc = loc)
            |> Declaration
        static member variableDeclaration(var, ?init, ?kind, ?loc): Statement =
            Declaration.variableDeclaration(var, ?init = init, ?kind = kind, ?loc = loc)
            |> Declaration
        static member forStatement(body, ?init, ?test, ?update, ?loc) = ForStatement(body, init, test, update, loc)
        static member throwStatement(argument, ?loc) = ThrowStatement(argument, loc)

    type BlockStatement with
        member this.Body =
            let (BlockStatement body) = this
            body

    type Program with
        member this.Body =
            let (Program body) = this
            body

    type CatchClause with
        static member catchClause(param, body, ?loc) =
            CatchClause(param, body, loc)

    type SwitchCase with
        static member switchCase(?consequent, ?test, ?loc) =
            SwitchCase(test, defaultArg consequent Array.empty, loc)

    type Pattern with
        static member identifier(name, ?optional, ?typeAnnotation, ?loc) =
            Identifier(name, ?optional = optional, ?typeAnnotation = typeAnnotation, ?loc = loc)
            |> IdentifierPattern
        static member restElement(argument: Pattern, ?typeAnnotation, ?loc) =
            RestElement.Create(argument, ?typeAnnotation=typeAnnotation, ?loc=loc) |> RestElement

    type ClassImplements with
        static member classImplements(id, ?typeParameters) =
            ClassImplements(id, typeParameters)

    type Declaration with
        static member variableDeclaration(kind, declarations, ?loc) : Declaration =
            VariableDeclaration.variableDeclaration(kind, declarations, ?loc = loc) |> Declaration.VariableDeclaration
        static member variableDeclaration(var, ?init, ?kind, ?loc) =
            Declaration.variableDeclaration(
                defaultArg kind Let,
                [| VariableDeclarator(var, init) |],
                ?loc = loc
            )
        static member functionDeclaration(``params``, body, id, ?returnType, ?typeParameters, ?loc) =
            FunctionDeclaration.Create(``params``, body, id, ?returnType=returnType, ?typeParameters=typeParameters, ?loc=loc)
            |> FunctionDeclaration
        static member classDeclaration(body, ?id, ?superClass, ?superTypeParameters, ?typeParameters, ?implements, ?loc) =
            ClassDeclaration.Create(body, ?id=id, ?superClass=superClass, ?superTypeParameters=superTypeParameters, ?typeParameters=typeParameters, ?implements=implements, ?loc=loc)
            |> ClassDeclaration

    type VariableDeclaration with
        static member variableDeclaration(kind, declarations, ?loc) : VariableDeclaration =
            let kind =
                match kind with
                | Var -> "var"
                | Let -> "let"
                | Const -> "const"
            VariableDeclaration(declarations, kind, loc)

        static member variableDeclaration(var, ?init, ?kind, ?loc) =
            VariableDeclaration.variableDeclaration(defaultArg kind Let, [| VariableDeclarator(var, init) |], ?loc = loc)
    type VariableDeclarator with
        static member variableDeclarator(id, ?init) = VariableDeclarator(id, init)

    type InterfaceExtends with
        static member interfaceExtends(id, ?typeParameters) =
            InterfaceExtends(id, typeParameters)

    type Literal with
        static member nullLiteral(?loc) = NullLiteral loc
        static member numericLiteral(value, ?loc) = NumericLiteral (value, loc)
        static member booleanLiteral(value, ?loc) = BooleanLiteral (value, loc)
        static member stringLiteral(value, ?loc) = StringLiteral (value, loc) |> Literal.StringLiteral
        static member regExpLiteral(pattern, flags, ?loc) =
            let flags =
                flags |> Seq.map (function
                    | RegexGlobal -> "g"
                    | RegexIgnoreCase -> "i"
                    | RegexMultiline -> "m"
                    | RegexSticky -> "y") |> Seq.fold (+) ""
            RegExp(pattern, flags, loc)

    type StringLiteral with
        static member stringLiteral(value, ?loc) = StringLiteral(value, loc)

    type ObjectMember with
        static member objectProperty(key, value, ?computed_) = // ?shorthand_,
            let computed = defaultArg computed_ false
            ObjectProperty(key, value, computed)
        static member objectMethod(kind_, key, ``params``, body, ?computed_, ?returnType, ?typeParameters, ?loc) =
            ObjectMethod.Create(kind_, key, ``params``, body, ?computed_=computed_, ?returnType=returnType, ?typeParameters=typeParameters, ?loc=loc)
            |> ObjectMethod

    type ModuleDeclaration with
        static member exportAllDeclaration(source, ?loc) = ExportAllDeclaration(source, loc)

    type TypeAnnotationInfo with
        static member genericTypeAnnotation(id, ?typeParameters) =
            GenericTypeAnnotation (id, typeParameters)

    type TypeParameter with
        static member typeParameter(name, ?bound, ?``default``) =
            TypeParameter(name, bound, ``default``)
