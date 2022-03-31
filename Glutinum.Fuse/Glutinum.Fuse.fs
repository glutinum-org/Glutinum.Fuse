// ts2fable 0.8.0
module rec Glutinum.Fuse

#nowarn "3390" // disable warnings for invalid XML comments

open System
open Fable.Core
open Fable.Core.JS

type Array<'T> = System.Collections.Generic.IList<'T>
type ReadonlyArray<'T> = System.Collections.Generic.IReadOnlyList<'T>

[<Import("default", "fuse.js")>]
let fuse: FuseStatic = jsNative

[<AllowNullLiteral>]
type IExports =
    abstract Fuse: FuseStatic

[<AllowNullLiteral>]
type Fuse<'T> =
    /// <summary>
    /// Search function for the Fuse instance.
    ///
    /// <code lang="typescript">
    /// const list: MyType[] = [myType1, myType2, etc...]
    ///
    /// const options: Fuse.IFuseOptions&lt;MyType&gt; = {
    ///   keys: ['key1', 'key2']
    /// }
    ///
    /// const myFuse = new Fuse(list, options)
    /// let result = myFuse.search('pattern')
    /// </code>
    /// </summary>
    /// <param name="pattern">The pattern to search</param>
    /// <param name="options"><c>Fuse.FuseSearchOptions</c></param>
    /// <returns>An array of search results</returns>
    abstract search: pattern: Fuse.Expression -> ResizeArray<Fuse.FuseResult<'T>>

    abstract search: pattern: Fuse.Expression * ?options: Fuse.FuseSearchOptions -> ResizeArray<Fuse.FuseResult<'T>>

    [<ParamObject(1)>]
    abstract search: pattern: Fuse.Expression * ?limit: float -> ResizeArray<Fuse.FuseResult<'T>>

    abstract search: pattern: string -> ResizeArray<Fuse.FuseResult<'T>>
    abstract search: pattern: string * ?options: Fuse.FuseSearchOptions -> ResizeArray<Fuse.FuseResult<'T>>

    [<ParamObject(1)>]
    abstract search: pattern: string * ?limit: float -> ResizeArray<Fuse.FuseResult<'T>>

    abstract setCollection: docs: ResizeArray<'T> * ?index: Fuse.FuseIndex<'T> -> unit
    /// Adds a doc to the end the list.
    abstract add: doc: 'T -> unit

    /// Removes all documents from the list which the predicate returns truthy for,
    /// and returns an array of the removed docs.
    /// The predicate is invoked with two arguments: (doc, index).
    abstract remove: predicate: ('T -> float -> bool) -> ResizeArray<'T>

    /// Removes the doc at the specified index.
    abstract removeAt: idx: float -> unit
    /// Returns the generated Fuse index
    abstract getIndex: unit -> Fuse.FuseIndex<'T>

[<AllowNullLiteral>]
type FuseStatic =
    [<EmitConstructor>]
    abstract Create: list: ResizeArray<'T> * ?options: Fuse.IFuseOptions<'T> * ?index: Fuse.FuseIndex<'T> -> Fuse<'T>

    /// Return the current version.
    abstract version: string with get, set

    /// <summary>
    /// Use this method to pre-generate the index from the list, and pass it
    /// directly into the Fuse instance.
    ///
    /// _Note that Fuse will automatically index the table if one isn't provided
    /// during instantiation._
    ///
    /// <code lang="typescript">
    /// const list: MyType[] = [myType1, myType2, etc...]
    ///
    /// const index = Fuse.createIndex&lt;MyType&gt;(
    ///   keys: ['key1', 'key2']
    ///   list: list
    /// )
    ///
    /// const options: Fuse.IFuseOptions&lt;MyType&gt; = {
    ///   keys: ['key1', 'key2']
    /// }
    ///
    /// const myFuse = new Fuse(list, options, index)
    /// </code>
    /// </summary>
    /// <param name="keys">The keys to index</param>
    /// <param name="list">The list from which to create an index</param>
    /// <param name="options">?</param>
    /// <returns>An indexed list</returns>
    abstract createIndex:
        keys: Array<Fuse.FuseOptionKey> * list: ResizeArray<'U> * ?options: Fuse.FuseIndexOptions<'U> ->
            Fuse.FuseIndex<'U>

    abstract parseIndex: index: obj option * ?options: Fuse.FuseIndexOptions<'U> -> Fuse.FuseIndex<'U>

module Fuse =

    [<AllowNullLiteral>]
    type IExports =
        abstract FuseIndex: FuseIndexStatic
        // abstract config: Required<IFuseOptions<obj option>>
        abstract config: IFuseOptions<obj option>

    [<AllowNullLiteral>]
    type FuseIndex<'T> =
        abstract setSources: docs: ResizeArray<'T> -> unit
        abstract setKeys: keys: ResizeArray<string> -> unit
        abstract setIndexRecords: records: FuseIndexRecords -> unit
        abstract create: unit -> unit
        abstract add: doc: 'T -> unit

        abstract toJSON:
            unit ->
                {| keys: ResizeArray<string>
                   collection: FuseIndexRecords |}

    [<AllowNullLiteral>]
    type FuseIndexStatic =
        [<EmitConstructor>]
        abstract Create: ?options: FuseIndexOptions<'T> -> FuseIndex<'T>

    [<AllowNullLiteral>]
    type FuseGetFunction<'T> =
        [<Emit "$0($1...)">]
        abstract Invoke: obj: 'T * path: U2<string, ResizeArray<string>> -> U2<ResizeArray<string>, string>

    [<AllowNullLiteral>]
    type FuseIndexOptions<'T> =
        abstract getFn: FuseGetFunction<'T> with get, set

    [<AllowNullLiteral>]
    type FuseSortFunctionItem =
        [<EmitIndexer>]
        abstract Item: key: string -> U2<{| ``$``: string |}, ResizeArray<{| ``$``: string; idx: float |}>> with get, set


    [<AllowNullLiteral>]
    type FuseSortFunctionMatch =
        abstract score: float with get, set
        abstract key: string with get, set
        abstract value: string with get, set
        abstract indices: ResizeArray<ReadonlyArray<float>> with get, set

    [<AllowNullLiteral>]
    type FuseSortFunctionMatchList =
        inherit FuseSortFunctionMatch
        abstract idx: float with get, set

    [<AllowNullLiteral>]
    type FuseSortFunctionArg =
        abstract idx: float with get, set
        abstract item: FuseSortFunctionItem with get, set
        abstract score: float with get, set
        abstract matches: ResizeArray<U2<FuseSortFunctionMatch, FuseSortFunctionMatchList>> option with get, set

    [<AllowNullLiteral>]
    type FuseSortFunction =
        [<Emit "$0($1...)">]
        abstract Invoke: a: FuseSortFunctionArg * b: FuseSortFunctionArg -> float

    [<AllowNullLiteral>]
    type RecordEntryObject =
        abstract v: string with get, set
        abstract n: float with get, set

    [<AllowNullLiteral>]
    type RecordEntryArrayItemType =
        inherit RecordEntryObject
        abstract i : int with get, set

    type RecordEntryArrayItem = ReadonlyArray<RecordEntryArrayItemType>

    [<AllowNullLiteral>]
    type RecordEntry =
        [<EmitIndexer>]
        abstract Item: key: string -> U2<RecordEntryObject, RecordEntryArrayItem> with get, set

    [<AllowNullLiteral>]
    type FuseIndexObjectRecord =
        abstract i: float with get, set
        abstract ``$``: RecordEntry with get, set

    [<AllowNullLiteral>]
    type FuseIndexStringRecord =
        abstract i: float with get, set
        abstract v: string with get, set
        abstract n: float with get, set

    type FuseIndexRecords = U2<ReadonlyArray<FuseIndexObjectRecord>, ReadonlyArray<FuseIndexStringRecord>>


    [<AllowNullLiteral>]
    [<Global>]
    type FuseOptionKeyObject [<ParamObject; Emit("$0")>]
        private (name: U2<string, ResizeArray<string>>, weight: float) =

        [<ParamObject; Emit("$0")>]
        new (name: string, weight: float) =
            FuseOptionKeyObject(U2.Case1 name, weight)

        [<ParamObject; Emit("$0")>]
        new (name: ResizeArray<string>, weight: float) =
            FuseOptionKeyObject(U2.Case2 name, weight)

        member val name: U2<string, ResizeArray<string>> = jsNative with get, set
        member val weight: float = jsNative with get, set

    type FuseOptionKey = U3<FuseOptionKeyObject, string, ResizeArray<string>>

    [<AllowNullLiteral>]
    [<Global>]
    type IFuseOptions<'T> [<ParamObject; Emit("$0")>]
        (
            ?isCaseSensitive: bool,
            ?distance: float,
            ?findAllMatches: bool,
            ?getFn: FuseGetFunction<'T>,
            ?ignoreLocation: bool,
            ?ignoreFieldNorm: bool,
            ?includeMatches: bool,
            ?includeScore: bool,
            ?keys: Array<FuseOptionKey>,
            ?location: float,
            ?minMatchCharLength: float,
            ?shouldSort: bool,
            ?sortFn: FuseSortFunction,
            ?threshold: float,
            ?useExtendedSearch: bool
        ) =

        member val isCaseSensitive: bool option = jsNative with get, set
        member val distance: float option = jsNative with get, set
        member val findAllMatches: bool option = jsNative with get, set
        member val getFn: FuseGetFunction<'T> option = jsNative with get, set
        member val ignoreLocation: bool option = jsNative with get, set
        member val ignoreFieldNorm: bool option = jsNative with get, set
        member val includeMatches: bool option = jsNative with get, set
        member val includeScore: bool option = jsNative with get, set
        member val keys: Array<FuseOptionKey> option = jsNative with get, set
        member val location: float option = jsNative with get, set
        member val minMatchCharLength: float option = jsNative with get, set
        member val shouldSort: bool option = jsNative with get, set
        member val sortFn: FuseSortFunction option = jsNative with get, set
        member val threshold: float option = jsNative with get, set
        member val useExtendedSearch: bool option = jsNative with get, set

    type RangeTuple = float * float

    [<AllowNullLiteral>]
    type FuseResultMatch =
        abstract indices: ReadonlyArray<RangeTuple> with get, set
        abstract key: string option with get, set
        abstract refIndex: float option with get, set
        abstract value: string option with get, set


    [<AllowNullLiteral>]
    [<Global>]
    type FuseSearchOptions [<ParamObject; Emit("$0")>] (limit: float) =
        member val limit: float = jsNative with get, set

    [<AllowNullLiteral>]
    type FuseResult<'T> =
        abstract item: 'T with get, set
        abstract refIndex: float with get, set
        abstract score: float option with get, set
        abstract matches: ReadonlyArray<FuseResultMatch> option with get, set

    type Expression = U4<ExpressionCase1, ExpressionCase2, ExpressionCase3, ExpressionCase4>

    [<AllowNullLiteral>]
    type ExpressionCase1 =
        [<EmitIndexer>]
        abstract Item: key: string -> string with get, set

    [<AllowNullLiteral>]
    type ExpressionCase2 =
        abstract ``$and``: ResizeArray<Expression> with get, set
        abstract ``$val``: ResizeArray<Expression> with get, set

    [<AllowNullLiteral>]
    type ExpressionCase3 =
        abstract ``$and``: ResizeArray<Expression> option with get, set

    [<AllowNullLiteral>]
    type ExpressionCase4 =
        abstract ``$or``: ResizeArray<Expression> option with get, set
