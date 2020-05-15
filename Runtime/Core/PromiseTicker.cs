using UnityEngine;

namespace ElRaccoone.Promises.Core {
  public static class PromiseTicker {
    public class DummyBehaviour : MonoBehaviour { }
    private static DummyBehaviour _enumerator;
    public static DummyBehaviour enumerator {
      get {
        if (_enumerator == null) {
          _enumerator = new GameObject ("~promises")
            .AddComponent<DummyBehaviour> ();
          Object.DontDestroyOnLoad (_enumerator);
        }
        return _enumerator;
      }
    }
  }
}