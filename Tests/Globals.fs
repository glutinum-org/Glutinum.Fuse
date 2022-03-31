[<AutoOpen>]
module Globals

open Fable.Core
open Fable.Core.JS

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

[<Emit("undefined")>]
let inline jsUndefined<'T> : 'T = jsNative

[<Emit("void 0")>]
let inline returnNothingHack<'T> : 'T = jsNative

type NumberConstructor with
    [<Emit("$0($1...)")>]
    member __.Create(v : obj) = jsNative

[<Emit("typeof $0")>]
let internal jsTypeOf _ : string = jsNative
