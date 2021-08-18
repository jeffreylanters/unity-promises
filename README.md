<div align="center">

![readme splash](https://raw.githubusercontent.com/jeffreylanters/unity-promises/master/.github/WIKI/repository-readme-splash.png)

[![license](https://img.shields.io/badge/mit-license-red.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-promises/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/nl.elraccoone.promises?label=UPM&registry_uri=https://package.openupm.com&style=for-the-badge&color=232c37)](https://openupm.com/packages/nl.elraccoone.promises/)
[![build](https://img.shields.io/badge/build-passing-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-promises/actions)
[![deployment](https://img.shields.io/badge/state-success-brightgreen.svg?style=for-the-badge)](https://github.com/jeffreylanters/unity-promises/deployments)
[![stars](https://img.shields.io/github/stars/jeffreylanters/unity-promises.svg?style=for-the-badge&color=fe8523&label=stargazers)](https://github.com/jeffreylanters/unity-promises/stargazers)
[![size](https://img.shields.io/github/languages/code-size/jeffreylanters/unity-promises?style=for-the-badge)](https://github.com/jeffreylanters/unity-promises/blob/master/Runtime)
[![sponsors](https://img.shields.io/github/sponsors/jeffreylanters?color=E12C9A&style=for-the-badge)](https://github.com/sponsors/jeffreylanters)
[![donate](https://img.shields.io/badge/donate-paypal-F23150?style=for-the-badge)](https://paypal.me/jeffreylanters)

Promises provide a simpler alternative for executing, composing, and managing asynchronous operations when compared to traditional callback-based approaches. They also allow you to handle asynchronous errors using approaches that are similar to synchronous try/catch.

[**Installation**](#installation) &middot;
[**Documentation**](#documentation) &middot;
[**License**](./LICENSE.md)

**Made with &hearts; by Jeffrey Lanters**

</div>

# Installation

### Using the Unity Package Manager

Install the latest stable release using the Unity Package Manager by adding the following line to your `manifest.json` file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.elraccoone.promises": "git+https://github.com/jeffreylanters/unity-promises"
```

### Using OpenUPM

The module is availble on the OpenUPM package registry, you can install the latest stable release using the OpenUPM Package manager's Command Line Tool using the following command.

```sh
openupm add nl.elraccoone.promises
```

# Documentation

A Promise is a proxy for a value not necessarily known when the promise is created. It allows you to associate handlers with an asynchronous action's eventual success value or failure reason. This lets asynchronous methods return values like synchronous methods: instead of immediately returning the final value, the asynchronous method returns a promise to supply the value at some point in the future.

A Promise is in one of these states:

- pending: initial state, neither fulfilled nor rejected.
- fulfilled: meaning that the operation completed successfully.
- rejected: meaning that the operation failed.

A pending promise can either be fulfilled with a value, or rejected with a reason (error). When either of these options happens, the associated handlers queued up by a promise's then method are called. (If the promise has already been fulfilled or rejected when a corresponding handler is attached, the handler will be called, so there is no race condition between an asynchronous operation completing and its handlers being attached.)

## Syntax

A function that is passed with the arguments resolve and reject. The executor function is executed immediately by the Promise implementation, passing resolve and reject functions (the executor is called before the Promise constructor even returns the created object). The resolve and reject functions, when called, resolve or reject the promise, respectively. The executor normally initiates some asynchronous work, and then, once that completes, either calls the resolve function to resolve the promise or else rejects it if an error occurred. If an error is thrown in the executor function, the promise is rejected. The return value of the executor is ignored.

```cs
new Promise ((resolve, reject) => { /* ... */ });
new Promise<ResolveType> ((resolve, reject) => { /* ... */ });
new Promise<ResolveType, RejectType> ((resolve, reject) => { /* ... */ });
```

## Creating a Promise

A Promise object is created using the new keyword and its constructor. This constructor takes as its argument a function, called the "executor function". This function should take two functions as parameters. The first of these functions (resolve) is called when the asynchronous task completes successfully and returns the results of the task as a value. The second (reject) is called when the task fails, and returns the reason for failure, which is typically an error object.

```cs
public Promise LoadSomeData () {
  // Return a new void promise and assign the executor.
  return new Promise ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    resolve (); // to fulfill
    reject (new Exception ("Failed")); // or reject
  });
}

public Promise<int> LoadSomeData () {
  // Return a new promise with a generic resolve type and assign the executor.
  return new Promise<int> ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    resolve (100); // to fulfill
    reject (new Exception ("Not found!")); // or reject
  });
}

public Promise<string, CustomException> LoadSomeData () {
  // Return a new promise with a generic resolve and reject type and assign the executor.
  return new Promise<string, CustomException> ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    resolve ("Hello World!"); // to fulfill
    reject (new CustomException ()); // or reject
  });
}
```

## Using a Promise

### Using classic Then/Catch callbacks

Execute your function and return a promise. You can use the Promise.Then() and Promise.Catch() methods to set actions that should be triggered when the promise resolves or failes. These methods return promises so they can be chained.

```cs
public void Start () {
  this.LoadSomeData()
    .Then (value => { /* ... */ })
    .Catch (reason => { /* ... */ })
    .Finally (() => { /* ... */ });
}
```

### Using Async/Await methods

When chaining multiple promises after one another or when simply wanting to prevent a lot of nested callbacks, use the async await logic to keep things simple. There are two ways of doing so, the simplest way does not require any changes on the promise's creation side. A basic implementation could be as following.

```cs
public async void Start () {
  try {
    var value = await this.LoadSomeData ().Async ();
    // "Then"
  } catch (Exception exception) {
    // "Catch"
  }
  // "Finally"
}
```

Another way would be to wrap the Async before returning it as a promise, this way the promise can be awaited right away when invoking it. But this also means it can no longer be used using callbacks. A basic implementation could be as following.

```csharp
public async Task<string> LoadSomeData () {
  // Return and await a new Task with a generic type.
  return await new Promise<string> ((resolve, reject) => {
    // do something asynchronous which eventually calls either:
    resolve ("Hello World!"); // to fulfill
    reject (new Exception ("Whoops")); // or reject
  }).Async();
}

public async void Start () {
  var value = await this.LoadSomeData ();
  // ...
}
```
