import { item, singleton, ofArray } from "./fable_modules/fable-library.3.7.8/List.js";
import fuse_7 from "fuse.js";
import { assertEqual } from "./fable_modules/fable-library.3.7.8/Util.js";

export const fruitList = ofArray(["Apple", "Orange", "Banana"]);

export const bookList = ofArray([{
    author: "John X",
    tags: singleton("war"),
    title: "Old Man\u0027s War fiction",
}, {
    author: "P.D. Mans",
    tags: ofArray(["fiction", "war"]),
    title: "Right Ho Jeeves",
}, {
    author: "John Smith",
    tags: ofArray(["john", "smith"]),
    title: "The life of Jane",
}, {
    author: "Steve Pearson",
    tags: ofArray(["steve", "pearson"]),
    title: "John Smith",
}]);

describe("Fuse.search", () => {
    it("works with strings", () => {
        const fuse = new fuse_7(Array.from(fruitList));
        const result = fuse.search("apple");
        assertEqual(result.length, 1);
    });
    it("can access the item property of the result", () => {
        const fuse_1 = new fuse_7(Array.from(fruitList));
        const result_1 = fuse_1.search("apple");
        assertEqual(result_1[0].item, "Apple");
    });
    it("can access the refIndex property of the result", () => {
        const fuse_2 = new fuse_7(Array.from(fruitList));
        const result_2 = fuse_2.search("apple");
        assertEqual(result_2[0].refIndex, 0);
    });
    it("result is typed with the same type as in the input list", () => {
        const books = ofArray([{
            author: {
                firstName: "John",
                lastName: "Scalzi",
            },
            title: "Old Man\u0027s War",
        }, {
            author: {
                firstName: "Steve",
                lastName: "Hamilton",
            },
            title: "The Lock Artist",
        }]);
        const fuse_3 = new fuse_7(Array.from(books), ({
            keys: ["title", "author.firstName"],
        }));
        const result_3 = fuse_3.search("jon");
        assertEqual(result_3[0].item.author, item(0, books).author);
    });
    it("can pass FuseSerarchOptions object", () => {
        const fuse_4 = new fuse_7(Array.from(fruitList));
        const result_4 = fuse_4.search("nan", {
            limit: 1,
        });
        assertEqual(result_4.length, 1);
    });
    it("can pass FuseSerarchOptions directly via the method", () => {
        const fuse_5 = new fuse_7(Array.from(fruitList));
        const result_5 = fuse_5.search("nan", {
            limit: 1,
        });
        assertEqual(result_5.length, 1);
    });
    it("can use weigthed keys", () => {
        const fuse_6 = new fuse_7(Array.from(bookList), ({
            keys: [{
                name: "title",
                weight: 0.3,
            }, {
                name: "author",
                weight: 0.7,
            }],
        }));
        const result_6 = fuse_6.search("John Smith");
        assertEqual(result_6[0].item.title, "The life of Jane");
        assertEqual(result_6[0].item.author, "John Smith");
        assertEqual(result_6[0].item.tags, ofArray(["john", "smith"]));
        assertEqual(result_6[0].refIndex, 2);
    });
});

describe("Fuse.FuseOptionKeyObject", () => {
    it("accept name as a string", () => {
        const options = {
            name: "title",
            weight: 0.3,
        };
        assertEqual(options.name, "title");
        assertEqual(options.weight, 0.3);
    });
    it("accept name as an array of string", () => {
        const options_1 = {
            name: ["title", "author"],
            weight: 0.3,
        };
        assertEqual(options_1.name, ["title", "author"]);
        assertEqual(options_1.weight, 0.3);
    });
});

