using UnityEngine;

namespace ElRaccoone.Promises.Core {
  public class PromiseTicker : MonoBehaviour {
    private static PromiseTicker _enumerator;
    public static PromiseTicker enumerator {
      get {
        if (_enumerator == null) {
          _enumerator = new GameObject ("~promise")
            .AddComponent<PromiseTicker> ();
          Object.DontDestroyOnLoad (_enumerator);
        }
        return _enumerator;
      }
    }
  }
}