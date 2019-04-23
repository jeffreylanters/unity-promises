# Promise

![](https://img.shields.io/badge/dependencies-unity--packages-%233bc6d8.svg) ![](https://img.shields.io/badge/license-MIT-%23ecc531.svg)

A Promise is a proxy for a value not necessarily known when the promise is created. It allows you to associate handlers with an asynchronous action's eventual success value or failure reason. This lets asynchronous methods return values like synchronous methods: instead of immediately returning the final value, the asynchronous method returns a promise to supply the value at some point in the future.

> NOTE When using this Unity Package, make sure to **Star** this repository. When using any of the packages please make sure to give credits to **Jeffrey Lanters** somewhere in your app or game. **These packages are not allowed to be sold anywhere!**

## Install

```
"com.unity-packages.promise": "git+https://github.com/unity-packages/promise"
```

[Click here to read the Unity Packages installation guide](https://github.com/unity-packages/installation)

## Usage

#### Syntax

```cs
new Promise( /* executor */ ((resolve, reject) => { ... }) );
```

A function that is passed with the arguments resolve and reject. The executor function is executed immediately by the Promise implementation, passing resolve and reject functions (the executor is called before the Promise constructor even returns the created object). The resolve and reject functions, when called, resolve or reject the promise, respectively. The executor normally initiates some asynchronous work, and then, once that completes, either calls the resolve function to resolve the promise or else rejects it if an error occurred. If an error is thrown in the executor function, the promise is rejected. The return value of the executor is ignored.

#### Description

A Promise is a proxy for a value not necessarily known when the promise is created. It allows you to associate handlers with an asynchronous action's eventual success value or failure reason. This lets asynchronous methods return values like synchronous methods: instead of immediately returning the final value, the asynchronous method returns a promise to supply the value at some point in the future.

A Promise is in one of these states:

- pending: initial state, neither fulfilled nor rejected.
- fulfilled: meaning that the operation completed successfully.
- rejected: meaning that the operation failed.

A pending promise can either be fulfilled with a value, or rejected with a reason (error). When either of these options happens, the associated handlers queued up by a promise's then method are called. (If the promise has already been fulfilled or rejected when a corresponding handler is attached, the handler will be called, so there is no race condition between an asynchronous operation completing and its handlers being attached.)

#### Creating a Promise

A Promise object is created using the new keyword and its constructor. This constructor takes as its argument a function, called the "executor function". This function should take two functions as parameters. The first of these functions (resolve) is called when the asynchronous task completes successfully and returns the results of the task as a value. The second (reject) is called when the task fails, and returns the reason for failure, which is typically an error object.

```cs
var myNewPromise = new Promise<bool> ((resolve, reject) => {
  // do something asynchronous which eventually calls either:
  //
  //   resolve(someValue); // fulfilled
  // or
  //   reject("failure reason"); // rejected
});
```

#### Using a Promise

Execute your function and return a promise. You can use the Promise.Then() and Promise.Catch() methods to set actions that should be triggered when the promise resolves or failes. These methods return promises so they can be chained.

```cs
public void Awake () {
  this.LoadData()
    .Then(num => { /* ... */ })
    .Catch(reason => { /* ... */ })
    .Finally(() => { /* ... */ });
}

public Promise<int> LoadData () {
  return new Promise<int> ((resolve, reject) => {
    // Do something async...
    resolve(100);
  });
}
```
