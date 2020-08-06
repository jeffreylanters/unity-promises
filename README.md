<div align="center">

<img src="https://raw.githubusercontent.com/elraccoone/unity-promises/master/.github/WIKI/logo.jpg" height="100px"></br>

# Promises

[![npm](https://img.shields.io/badge/upm-1.1.0-232c37.svg?style=for-the-badge)]()
[![license](https://img.shields.io/badge/license-Custom-%23ecc531.svg?style=for-the-badge)](./LICENSE.md)
[![npm](https://img.shields.io/badge/sponsor-donate-E12C9A.svg?style=for-the-badge)](https://paypal.me/jeffreylanters)
[![npm](https://img.shields.io/github/stars/elraccoone/unity-promises.svg?style=for-the-badge)]()

Promises provide a simpler alternative for executing, composing, and managing asynchronous operations when compared to traditional callback-based approaches. They also allow you to handle asynchronous errors using approaches that are similar to synchronous try/catch.

When using any of the packages, please make sure to **Star** this repository and give credit to **Jeffrey Lanters / El Raccoone** somewhere in your app or game. **It it prohibited to sublicense and/or sell copies of the Software in stores such as the Unity Asset Store!**

**&Lt;**
[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md) &middot;
[**Sponsor**](https://paypal.me/jeffreylanters)
**&Gt;**

**Made with &hearts; by Jeffrey Lanters**

</div>

# Installation

Install using the Unity Package Manager. add the following line to your `manifest.json` file located within your project's packages directory.

```json
"nl.elraccoone.promises": "git+https://github.com/elraccoone/unity-promises"
```

# Documentation

## Syntax

```cs
new Promise(((resolve, reject) => { /* ... */ }) );
new Promise<T>(((resolve, reject) => { /* ... */ }) );
new Promise(enumerator);
```

A function that is passed with the arguments resolve and reject. The executor function is executed immediately by the Promise implementation, passing resolve and reject functions (the executor is called before the Promise constructor even returns the created object). The resolve and reject functions, when called, resolve or reject the promise, respectively. The executor normally initiates some asynchronous work, and then, once that completes, either calls the resolve function to resolve the promise or else rejects it if an error occurred. If an error is thrown in the executor function, the promise is rejected. The return value of the executor is ignored.

## Description

A Promise is a proxy for a value not necessarily known when the promise is created. It allows you to associate handlers with an asynchronous action's eventual success value or failure reason. This lets asynchronous methods return values like synchronous methods: instead of immediately returning the final value, the asynchronous method returns a promise to supply the value at some point in the future.

A Promise is in one of these states:

- pending: initial state, neither fulfilled nor rejected.
- fulfilled: meaning that the operation completed successfully.
- rejected: meaning that the operation failed.

A pending promise can either be fulfilled with a value, or rejected with a reason (error). When either of these options happens, the associated handlers queued up by a promise's then method are called. (If the promise has already been fulfilled or rejected when a corresponding handler is attached, the handler will be called, so there is no race condition between an asynchronous operation completing and its handlers being attached.)

## Creating a Promise

A Promise object is created using the new keyword and its constructor. This constructor takes as its argument a function, called the "executor function". This function should take two functions as parameters. The first of these functions (resolve) is called when the asynchronous task completes successfully and returns the results of the task as a value. The second (reject) is called when the task fails, and returns the reason for failure, which is typically an error object.

```cs
public Promise<int> LoadSomeData () {
  return new Promise<int> ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    //   resolve(someValue); // fulfilled
    // or
    //   reject("failure reason"); // rejected
  });
}

public Promise LoadSomeData () {
  return new Promise ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    //   resolve(); // fulfilled
    // or
    //   reject("failure reason"); // rejected
  });
}


public Promise LoadSomeData () {
  return new Promise (LoadDataCoroutine());
}
```

## Using a Promise

Execute your function and return a promise. You can use the Promise.Then() and Promise.Catch() methods to set actions that should be triggered when the promise resolves or failes. These methods return promises so they can be chained.

```cs
public void Awake () {
  this.LoadSomeData()
    .Then(num => { /* ... */ })
    .Catch(reason => { /* ... */ })
    .Finally(() => { /* ... */ });
}
```
