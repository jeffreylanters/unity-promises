using System.Collections;
using UnityEngine;

namespace ElRaccoone.Promises.Tests {
  [AddComponentMenu ("El Raccoone/Promises/Tests/Promises Tests")]
  public class PromiseTests : MonoBehaviour {
    private void OnGUI () {
      this.Draw_Test_Basic ();
      this.Draw_Test_WithGenericResolve ();
      this.Draw_Test_WithGenericResolveReject ();
      this.Draw_Test_WithEnumerator ();
    }

    /// TEST: BASIC
    private void Draw_Test_Basic () {
      if (GUILayout.Button ("Run Test: Basic")) {
        Debug.Log ("Running Test: Basic");
        this.Test_Basic ()
          .Then (() => Debug.Log ("Resolved Test: Basic"))
          .Catch (reason => Debug.LogError ("Rejected Test: Basic: " + reason))
          .Finally (() => Debug.Log ("Finnaly Test: Basic"));
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

    /// TEST: WITHGENERICRESOLVE
    private void Draw_Test_WithGenericResolve () {
      if (GUILayout.Button ("Run Test: WithGenericResolve")) {
        Debug.Log ("Running Test: WithGenericResolve");
        this.Test_WithGenericResolve ()
          .Then (value => Debug.Log ("Resolved Test: WithGenericResolve: " + value))
          .Catch (reason => Debug.LogError ("Rejected Test: WithGenericResolve: " + reason))
          .Finally (() => Debug.Log ("Finnaly Test: WithGenericResolve"));
      }
    }
    private Promise<int> Test_WithGenericResolve () {
      return new Promise<int> ((resolve, reject) => {
        if (Random.Range (0, 100) > 10)
          resolve (Random.Range (0, 100));
        else
          reject ("Something went wrong");
      });
    }

    /// TEST: WITHGENERICRESOLVEREJECT
    private void Draw_Test_WithGenericResolveReject () {
      if (GUILayout.Button ("Run Test: WithGenericResolveReject")) {
        Debug.Log ("Running Test: WithGenericResolveReject");
        this.Test_WithGenericResolveReject ()
          .Then (value => Debug.Log ("Resolved Test: WithGenericResolveReject: " + value))
          .Catch (reason => Debug.LogError ("Rejected Test: WithGenericResolveReject: " + reason))
          .Finally (() => Debug.Log ("Finnaly Test: WithGenericResolveReject"));
      }
    }
    private Promise<int, int> Test_WithGenericResolveReject () {
      return new Promise<int, int> ((resolve, reject) => {
        if (Random.Range (0, 100) > 10)
          resolve (Random.Range (0, 100));
        else
          reject (404);
      });
    }

    /// TEST: WITHENUMERATOR
    private void Draw_Test_WithEnumerator () {
      if (GUILayout.Button ("Run Test: With Enumerator")) {
        Debug.Log ("Running Test: With Enumerator");
        this.Test_WithEnumerator ()
          .Then (() => Debug.Log ("Resolved Test: With Enumerator"))
          .Catch (reason => Debug.LogError ("Rejected Test: With Enumerator: " + reason))
          .Finally (() => Debug.Log ("Finnaly Test: With Enumerator"));
      }
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