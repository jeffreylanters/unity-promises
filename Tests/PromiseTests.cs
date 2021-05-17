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
      if (GUILayout.Button ("[TEST 1]: Promise With Generic Resolve Type Using Callbacks"))
        this.RunTest1 ();
      if (GUILayout.Button ("[TEST 2]: Promise With Generic Resolve Type Using Async/Await"))
        this.RunTest2 ();
      if (GUILayout.Button ("[TEST 3]: Promise With Generic Resolve And Reject Type Using Callbacks"))
        this.RunTest3 ();
      if (GUILayout.Button ("[TEST 4]: Promise With Generic Resolve And Reject Type Using Async/Await"))
        this.RunTest4 ();
    }

    /// <summary>
    /// Runs test 1.
    /// </summary>
    private void RunTest1 () {
      Debug.Log ("[TEST 1]: Running...");
      this.GetRandomNumberPromise ()
        .Then (value => Debug.Log ($"[TEST 1]: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"[TEST 1]: Rejected with exception: {exception}"))
        .Finally (() => Debug.Log ("[TEST 1]: Finalized!"));
    }

    /// <summary>
    /// Runs test 2.
    /// </summary>
    private async void RunTest2 () {
      Debug.Log ("[TEST 2]: Running...");
      try {
        var value = await this.GetRandomNumberPromise ().Async ();
        Debug.Log ($"[TEST 2]: Resolved with value: {value}");
      } catch (Exception exception) {
        Debug.LogError ($"[TEST 2]: Rejected with exception: {exception}");
      }
      Debug.Log ("[TEST 2]: Finalized!");
    }

    /// <summary>
    /// Runs test 3.
    /// </summary>
    private void RunTest3 () {
      Debug.Log ("[TEST 3]: Running...");
      this.GetRandomNumberWithCustomException ()
        .Then (value => Debug.Log ($"[TEST 3]: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"[TEST 3]: Rejected with custom exception: {exception.number}"))
        .Finally (() => Debug.Log ("[TEST 3]: Finalized!"));
    }

    /// <summary>
    /// Runs test 4.
    /// </summary>
    private async void RunTest4 () {
      Debug.Log ("[TEST 4]: Running...");
      try {
        var value = await this.GetRandomNumberWithCustomException ().Async ();
        Debug.Log ($"[TEST 4]: Resolved with value: {value}");
      } catch (CustomException exception) {
        Debug.LogError ($"[TEST 4]: Rejected with custom exception: {exception.number}");
      }
      Debug.Log ("[TEST 4]: Finalized!");
    }

    // /// <summary>
    // /// Returns a random number between 0 and 100.
    // /// </summary>
    // /// <returns>A promise containing the number.</returns>
    // private Promise EmptyPromise () {
    //   return new Promise ((resolve, reject) => {
    //     this.StartCoroutine (this.GetRandomNumberRoutine (randomNumber => {
    //       if (randomNumber > 25) {
    //         resolve (randomNumber);
    //       } else {
    //         reject (new Exception ($"Something went wrong {randomNumber}"));
    //       }
    //     }));
    //   });
    // }

    /// <summary>
    /// Returns a random number between 0 and 100.
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