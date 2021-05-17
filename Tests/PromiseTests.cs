using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace ElRaccoone.Promises.Tests {
  [AddComponentMenu ("El Raccoone/Promises/Tests/Promises Tests")]
  public class PromiseTests : MonoBehaviour {
    private void OnGUI () {
      this.Draw_Test_ThenableGenericResolve ();
      this.Draw_Test_AwaitableGenericResolve ();
      this.Draw_Test_ThenableGenericResolveAndReject ();
      this.Draw_Test_AwaitableGenericResolveAndReject ();
    }

    private IEnumerator SlowRandomNumberGenerator (Action<int> action) {
      yield return new WaitForSeconds (1);
      action (UnityEngine.Random.Range (0, 100));
    }

    private class CustomException : Exception {
      public int number { get; private set; } = -1;
      public CustomException (int number) {
        this.number = number;
      }
    }

    /// ==================================================================== ///
    /// TEST: Thenable Generic Resolve 
    private void Draw_Test_ThenableGenericResolve () {
      if (GUILayout.Button ("Run Test: Thenable Generic Resolve")) {
        Debug.Log ("Running Test: Thenable Generic Resolve");
        this.Run_Test_ThenableGenericResolve ()
          .Then (value => Debug.Log ("Resolved Test: Thenable Generic Resolve: " + value))
          .Catch (reason => Debug.LogError ("Rejected Test: Thenable Generic Resolve: " + reason))
          .Finally (() => Debug.Log ("Finnaly Test: Thenable Generic Resolve"));
      }
    }
    private Promise<int> Run_Test_ThenableGenericResolve () {
      return new Promise<int> ((resolve, reject) => {
        this.StartCoroutine (this.SlowRandomNumberGenerator (randomNumber => {
          if (randomNumber > 25)
            resolve (randomNumber);
          else
            reject (new Exception ($"Something went wrong {randomNumber}"));
        }));
      });
    }
    /// ==================================================================== ///

    /// ==================================================================== ///
    /// TEST: Awaitable Generic Resolve 
    private void Draw_Test_AwaitableGenericResolve () {
      if (GUILayout.Button ("Run Test: Awaitable Generic Resolve"))
        this.Run_Test_AwaitableGenericResolve ();
    }
    private async void Run_Test_AwaitableGenericResolve () {
      Debug.Log ("Running Test: Awaitable Generic Resolve");
      try {
        var value = await this.Task_Test_AwaitableGenericResolve ();
        Debug.Log ("Resolved Test: Awaitable Generic Resolve: " + value);
      } catch (Exception exception) {
        Debug.LogError ("Rejected Test: Awaitable Generic Resolve: " + exception);
      }
      Debug.Log ("Finnaly Test: Awaitable Generic Resolve");
    }
    private async Task<int> Task_Test_AwaitableGenericResolve () {
      return await new Promise<int> ((resolve, reject) => {
        this.StartCoroutine (this.SlowRandomNumberGenerator (randomNumber => {
          if (randomNumber > 25)
            resolve (randomNumber);
          else
            reject (new Exception ($"Something went wrong {randomNumber}"));
        }));
      }).Async ();
    }
    /// ==================================================================== ///

    /// ==================================================================== ///
    /// TEST: Thenable Generic Resolve And Reject
    private void Draw_Test_ThenableGenericResolveAndReject () {
      if (GUILayout.Button ("Run Test: Thenable Generic Resolve And Reject")) {
        Debug.Log ("Running Test: Thenable Generic Resolve And Reject");
        this.Run_Test_ThenableGenericResolveAndReject ()
          .Then (value => Debug.Log ("Resolved Test: Thenable Generic Resolve And Reject: " + value))
          .Catch (reason => Debug.LogError ("Rejected Test: Thenable Generic Resolve And Reject: " + reason.number))
          .Finally (() => Debug.Log ("Finnaly Test: Thenable Generic Resolve And Reject"));
      }
    }
    private Promise<int, CustomException> Run_Test_ThenableGenericResolveAndReject () {
      return new Promise<int, CustomException> ((resolve, reject) => {
        this.StartCoroutine (this.SlowRandomNumberGenerator (randomNumber => {
          if (randomNumber > 25)
            resolve (randomNumber);
          else
            reject (new CustomException (randomNumber));
        }));
      });
    }
    /// ==================================================================== ///

    /// ==================================================================== ///
    /// TEST: Awaitable Generic Resolve And Reject
    private void Draw_Test_AwaitableGenericResolveAndReject () {
      if (GUILayout.Button ("Run Test: Awaitable Generic Resolve And Reject"))
        this.Run_Test_AwaitableGenericResolveAndReject ();
    }
    private async void Run_Test_AwaitableGenericResolveAndReject () {
      Debug.Log ("Running Test: Awaitable Generic Resolve And Reject");
      try {
        var value = await this.Task_Test_AwaitableGenericResolve ();
        Debug.Log ("Resolved Test: Awaitable Generic Resolve And Reject: " + value);
      } catch (CustomException exception) {
        Debug.LogError ("Rejected Test: Awaitable Generic Resolve And Reject: " + exception.number);
      }
      Debug.Log ("Finnaly Test: Awaitable Generic Resolve And Reject");
    }
    private async Task<int> Task_Test_AwaitableGenericResolveAndReject () {
      return await new Promise<int, CustomException> ((resolve, reject) => {
        this.StartCoroutine (this.SlowRandomNumberGenerator (randomNumber => {
          if (randomNumber > 25)
            resolve (randomNumber);
          else
            reject (new CustomException (randomNumber));
        }));
      }).Async ();
    }
    /// ==================================================================== ///
  }
}