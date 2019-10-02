using UnityEngine;

namespace UnityPackages {
  public class PromiseTicker : MonoBehaviour {
    private static PromiseTicker _Enumerator;
    public static PromiseTicker Enumerator {
      get {
        if (_Enumerator == null) {
          _Enumerator = new GameObject ("~promise")
            .AddComponent<PromiseTicker> ();
          GameObject.DontDestroyOnLoad (_Enumerator);
        }
        return _Enumerator;
      }
    }
  }
}