module Tests.Fuse

open Mocha
open Node
open Fable.Core
open Fable.Core.Testing
open Glutinum.Fuse


let fruitList =
    [
        "Apple"
        "Orange"
        "Banana"
    ]

let bookList =
    [
        {|
            title = "Old Man's War fiction"
            author = "John X"
            tags = ["war"]
        |}
        {|
            title = "Right Ho Jeeves"
            author = "P.D. Mans"
            tags = ["fiction"; "war"]
        |}
        {|
            title = "The life of Jane"
            author = "John Smith"
            tags = ["john"; "smith"]
        |}
        {|
            title = "John Smith"
            author = "Steve Pearson"
            tags = ["steve"; "pearson"]
        |}
    ]

describe """Fuse.search""" (fun () ->

    it "works with strings" (fun () ->

        let fuse = fuse.Create(ResizeArray fruitList)

        let result = fuse.search("apple")

        Assert.AreEqual(result.Count, 1)
    )

    it "can access the item property of the result" (fun () ->

        let fuse = fuse.Create(ResizeArray fruitList)

        let result = fuse.search("apple")

        Assert.AreEqual(result[0].item, "Apple")
    )

    it "can access the refIndex property of the result" (fun () ->

        let fuse = fuse.Create(ResizeArray fruitList)

        let result = fuse.search("apple")

        Assert.AreEqual(result[0].refIndex, 0)
    )

    it "result is typed with the same type as in the input list" (fun () ->

        let books =
            [
                {|
                    title = "Old Man's War"
                    author =
                        {|
                            firstName = "John"
                            lastName = "Scalzi"
                        |}
                |}
                {|
                    title = "The Lock Artist"
                    author =
                        {|
                            firstName = "Steve"
                            lastName = "Hamilton"
                        |}
                |}
            ]

        let fuse =
            fuse.Create(
                ResizeArray books,
                Fuse.IFuseOptions<_>(
                    keys =
                        [|
                            U3.Case2 "title"
                            U3.Case2 "author.firstName"
                        |]
                )
            )

        let result = fuse.search("jon")

        Assert.AreEqual(result.[0].item.author, books.[0].author)
    )

    it "can pass FuseSerarchOptions object" (fun () ->

        let fuse = fuse.Create(ResizeArray fruitList)

        let result = fuse.search("nan", Fuse.FuseSearchOptions(1))

        Assert.AreEqual(result.Count, 1)
    )

    it "can pass FuseSerarchOptions directly via the method" (fun () ->

        let fuse = fuse.Create(ResizeArray fruitList)

        let result = fuse.search("nan", 1)

        Assert.AreEqual(result.Count, 1)
    )

    it "can use weigthed keys" (fun () ->

        let fuse =
            fuse.Create(
                ResizeArray bookList,
                Fuse.IFuseOptions<_>(
                    keys =
                        [|
                            U3.Case1 (Fuse.FuseOptionKeyObject("title", 0.3))
                            U3.Case1 (Fuse.FuseOptionKeyObject("author", 0.7))
                        |]
                )
            )

        let result = fuse.search("John Smith")

        Assert.AreEqual(result[0].item.title, "The life of Jane")
        Assert.AreEqual(result[0].item.author, "John Smith")
        Assert.AreEqual(result[0].item.tags, ["john"; "smith"])
        Assert.AreEqual(result[0].refIndex, 2)
    )
)

describe "Fuse.FuseOptionKeyObject" (fun () ->

    it "accept name as a string" (fun () ->

        let options =
            Fuse.FuseOptionKeyObject("title", 0.3)

        Assert.AreEqual(options.name, U2.Case1 "title")
        Assert.AreEqual(options.weight, 0.3)
    )

    it "accept name as an array of string" (fun () ->

        let options =
            Fuse.FuseOptionKeyObject(
                ResizeArray [
                    "title"
                    "author"
                ]
                ,0.3
            )

        Assert.AreEqual(options.name, U2.Case2 (ResizeArray[ "title"; "author" ]))
        Assert.AreEqual(options.weight, 0.3)
    )

)
