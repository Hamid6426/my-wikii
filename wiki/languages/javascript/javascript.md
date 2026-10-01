# Rewind: Learning Javascript DSA from freeCodeCamp Again

## A Variable
Everything start with defining a variable
- var (Not recommended except it use case in global type file)
- let (Can not be re-initialized but can be re-defined)
- const (Can not be re-initialized and re-defined)

## Types:
There are 7 primitive(prime/base/from which other come) types

### Primitive Types
They are Immutable - can not be edited after defining

- String: text
- Number: number
- Boolean: true, false
- Undefined: No even defined
- Null: Defined as a no value variable
- Symbol: (ES6) – Unique and immutable identifier = Symbol("id")
- BigInt: (ES2020) – For very large integers =  1234567890123456789012345678901234567890n

### Reference Types (Objects)
These hold collections or more complex data and are mutable.
Simply a collection of primitive type - can also be a collection of its own type e.g object within an object

#### Object – Key-value pairs
```
const user = { name: "Hamid", age: 25 };
```

#### Array – Ordered list of items
```
const numbers = [1, 2, 3, 4];
```

#### Function – Callable object
```
function greet() { 
console.log("Hello!"); 
}
```

#### Built-in object types
Date, RegExp, Map, Set, WeakMap, WeakSet, Error etc

### Type Checking
```
typeof 123           // "number"
typeof "abc"         // "string"
typeof null          // "object" (quirk in JS)
typeof undefined     // "undefined"
typeof []            // "object"
typeof function(){}  // "function"
```

## Arrays
Create:
const array = ["hamid", "full-stack", "MERN"]
Update:
array[1] = "Full Stack Web Developer"
arrayLength = array.length;
addAtTheEnd = array.push(2025);
