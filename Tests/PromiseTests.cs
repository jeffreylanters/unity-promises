using System.Collections;
using UnityEngine;

namespace ElRaccoone.Promises.Tests {
  [AddComponentMenu ("El Raccoone/Promises/Tests/Promises Tests")]
  public class PromiseTests : MonoBehaviour {
    private void OnGUI () {
      if (GUILayout.Button ("Run Test: Basic")) {
        Debug.Log ("Running Test: Basic");
        this.Test_Basic ()
          .Then (() => Debug.Log ("Resolved Test: Basic"))
          .Catch (reason => Debug.LogError ("Rejected Test: Basic"))
          .Finally (() => Debug.Log ("Finnaly Test: Basic"));
      }
      if (GUILayout.Button ("Run Test: With Enumerator")) {
        Debug.Log ("Running Test: With Enumerator");
        this.Test_WithEnumerator ()
          .Then (() => Debug.Log ("Resolved Test: With Enumerator"))
          .Catch (reason => Debug.LogError ("Rejected Test: With Enumerator"))
          .Finally (() => Debug.Log ("Finnaly Test: With Enumerator"));
      }
    }

    private Promise Test_Basic () {
      return new Promise ((resolve, reject) => {
        if (Random.Range (0, 100) > 10)
          resolve ();
        else
          reject ("Something went wrong");
      });
    }

    private Promise Test_WithEnumerator () {
      return new Promise (this.WaitCoroutine ());
    }
    private IEnumerator WaitCoroutine () {
      Debug.Log ("...Uno...");
      yield return new WaitForSeconds (1);
      Debug.Log ("...Dos...");
    }
  }
}