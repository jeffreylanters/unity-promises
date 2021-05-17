using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace ElRaccoone.Promises.Tests {

  /// <summary>
  /// Promise Tests.
  /// </summary>
  [AddComponentMenu ("El Raccoone/Promises/Tests/Promises Tests")]
  public class PromiseTests : MonoBehaviour {

    /// <summary>
    /// Draws the tester gui.
    /// </summary>
    private void OnGUI () {
      if (GUILayout.Button ("[TEST 1a]: Promise With Reject (Then/Catch)"))
        this.RunTest1a ();
      if (GUILayout.Button ("[TEST 1b]: Promise With Reject (Async/Await)"))
        this.RunTest1b ();
      if (GUILayout.Button ("[TEST 2a]: Promise With Generic Resolve And Reject (Then/Catch)"))
        this.RunTest2a ();
      if (GUILayout.Button ("[TEST 2b]: Promise With Generic Resolve And Reject (Async/Await)"))
        this.RunTest2b ();
      if (GUILayout.Button ("[TEST 3a]: Promise With Generic Resolve And Generic Reject (Then/Catch)"))
        this.RunTest3a ();
      if (GUILayout.Button ("[TEST 3b]: Promise With Generic Resolve And Generic Reject (Async/Await)"))
        this.RunTest3b ();
    }

    /// <summary>
    /// Runs test 1a.
    /// </summary>
    private void RunTest1a () {
      Debug.Log ("[TEST 1a]: Running...");
      this.EmptyPromise ()
        .Then (() => Debug.Log ("[TEST 1a]: Resolved"))
        .Catch (exception => Debug.LogError ($"[TEST 1a]: Rejected with exception: {exception}"))
        .Finally (() => Debug.Log ("[TEST 1a]: Finalized!"));
    }

    /// <summary>
    /// Runs test 1b.
    /// </summary>
    private async void RunTest1b () {
      Debug.Log ("[TEST 1b]: Running...");
      try {
        await this.EmptyPromise ().Async ();
        Debug.Log ("[TEST 1b]: Resolved");
      } catch (Exception exception) {
        Debug.LogError ($"[TEST 1b]: Rejected with exception: {exception}");
      }
      Debug.Log ("[TEST 1b]: Finalized!");
    }

    /// <summary>
    /// Runs test 2a.
    /// </summary>
    private void RunTest2a () {
      Debug.Log ("[TEST 2a]: Running...");
      this.GetRandomNumberPromise ()
        .Then (value => Debug.Log ($"[TEST 2a]: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"[TEST 2a]: Rejected with exception: {exception}"))
        .Finally (() => Debug.Log ("[TEST 2a]: Finalized!"));
    }

    /// <summary>
    /// Runs test 2b.
    /// </summary>
    private async void RunTest2b () {
      Debug.Log ("[TEST 2b]: Running...");
      try {
        var value = await this.GetRandomNumberPromise ().Async ();
        Debug.Log ($"[TEST 2b]: Resolved with value: {value}");
      } catch (Exception exception) {
        Debug.LogError ($"[TEST 2b]: Rejected with exception: {exception}");
      }
      Debug.Log ("[TEST 2b]: Finalized!");
    }

    /// <summary>
    /// Runs test 3a.
    /// </summary>
    private void RunTest3a () {
      Debug.Log ("[TEST 3a]: Running...");
      this.GetRandomNumberWithCustomException ()
        .Then (value => Debug.Log ($"[TEST 3a]: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"[TEST 3a]: Rejected with custom exception: {exception.number}"))
        .Finally (() => Debug.Log ("[TEST 3a]: Finalized!"));
    }

    /// <summary>
    /// Runs test 3b.
    /// </summary>
    private async void RunTest3b () {
      Debug.Log ("[TEST 3b]: Running...");
      try {
        var value = await this.GetRandomNumberWithCustomException ().Async ();
        Debug.Log ($"[TEST 3b]: Resolved with value: {value}");
      } catch (CustomException exception) {
        Debug.LogError ($"[TEST 3b]: Rejected with custom exception: {exception.number}");
      }
      Debug.Log ("[TEST 3b]: Finalized!");
    }

    /// <summary>
    /// An empty promise randomly resolving or rejecting after a while.
    /// </summary>
    /// <returns>A promise containing the number.</returns>
    private Promise EmptyPromise () {
      return new Promise ((resolve, reject) => {
        this.StartCoroutine (this.GetRandomNumberRoutine (randomNumber => {
          if (randomNumber > 25) {
            resolve ();
          } else {
            reject (new Exception ($"Something went wrong {randomNumber}"));
          }
        }));
      });
    }

    /// <summary>
    /// An promise randomly resolving or rejecting with a random number after a while.
    /// </summary>
    /// <returns>A promise containing the number.</returns>
    private Promise<int> GetRandomNumberPromise () {
      return new Promise<int> ((resolve, reject) => {
        this.StartCoroutine (this.GetRandomNumberRoutine (randomNumber => {
          if (randomNumber > 25) {
            resolve (randomNumber);
          } else {
            reject (new Exception ($"Something went wrong {randomNumber}"));
          }
        }));
      });
    }

    /// <summary>
    /// Returns a random number between 0 and 100 with a custom exception.
    /// </summary>
    /// <returns>A promise containing the number.</returns>
    private Promise<int, CustomException> GetRandomNumberWithCustomException () {
      return new Promise<int, CustomException> ((resolve, reject) => {
        this.StartCoroutine (this.GetRandomNumberRoutine (randomNumber => {
          if (randomNumber > 25) {
            resolve (randomNumber);
          } else {
            reject (new CustomException (randomNumber));
          }
        }));
      });
    }

    /// <summary>
    /// Invokes a callback with a number between 0 and 100 after some seconds...
    /// </summary>
    /// <param name="action">Action to be invoked.</param>
    /// <returns>A coroutine.</returns>
    private IEnumerator GetRandomNumberRoutine (Action<int> action) {
      yield return new WaitForSeconds (UnityEngine.Random.Range (0f, 1f));
      action (UnityEngine.Random.Range (0, 100));
    }

    /// <summary>
    /// A custom exception type holding a single number.
    /// </summary>
    private class CustomException : Exception {
      public int number { get; private set; } = -1;
      public CustomException (int number) {
        this.number = number;
      }
    }
  }
}