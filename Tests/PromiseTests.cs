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
      if (GUILayout.Button ("Test 1: Promise With Generic Resolve Type Using Callbacks"))
        this.RunTest1 ();
      if (GUILayout.Button ("Test 2: Promise With Generic Resolve Type Using Async/Await"))
        this.RunTest2 ();
      if (GUILayout.Button ("Test 3: Promise With Generic Resolve And Reject Type Using Callbacks"))
        this.RunTest3 ();
      if (GUILayout.Button ("Test 4: Promise With Generic Resolve And Reject Type Using Async/Await"))
        this.RunTest4 ();
    }

    /// <summary>
    /// Runs test 1.
    /// </summary>
    private void RunTest1 () {
      Debug.Log ("Test 1: Running...");
      this.GetRandomNumber ()
        .Then (value => Debug.Log ($"Test 1: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"Test 1: Rejected with exception: {exception}"))
        .Finally (() => Debug.Log ("Test 1: Finalized!"));
    }

    /// <summary>
    /// Runs test 2.
    /// </summary>
    private async void RunTest2 () {
      Debug.Log ("Test 2: Running...");
      try {
        var value = await this.GetRandomNumber ().Async ();
        Debug.Log ($"Test 2: Resolved with value: {value}");
      } catch (Exception exception) {
        Debug.LogError ($"Test 2: Rejected with exception: {exception}");
      }
      Debug.Log ("Test 2: Finalized!");
    }

    /// <summary>
    /// Runs test 3.
    /// </summary>
    private void RunTest3 () {
      Debug.Log ("Test 3: Running...");
      this.GetRandomNumberWithCustomException ()
        .Then (value => Debug.Log ($"Test 3: Resolved with value: {value}"))
        .Catch (exception => Debug.LogError ($"Test 3: Rejected with custom exception: {exception.number}"))
        .Finally (() => Debug.Log ("Test 3: Finalized!"));
    }

    /// <summary>
    /// Runs test 4.
    /// </summary>
    private async void RunTest4 () {
      Debug.Log ("Test 4: Running...");
      try {
        var value = await this.GetRandomNumberWithCustomException ().Async ();
        Debug.Log ($"Test 4: Resolved with value: {value}");
      } catch (CustomException exception) {
        Debug.LogError ($"Test 4: Rejected with custom exception: {exception.number}");
      }
      Debug.Log ("Test 4: Finalized!");
    }

    /// <summary>
    /// Returns a random number between 0 and 100.
    /// </summary>
    /// <returns>A promise containing the number.</returns>
    private Promise<int> GetRandomNumber () {
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