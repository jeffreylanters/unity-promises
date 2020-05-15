using System;
using System.Collections;
using ElRaccoone.Promises.Core;

namespace ElRaccoone.Promises {

  public class PromiseEnumerator {
    private Action<Action, Action<string>> executor;
    private Action onFulfilled;
    private Action<string> onRejected;
    private Action onFinally;

    public PromiseState state = PromiseState.Pending;

    public PromiseEnumerator (IEnumerator enumerator) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (enumerator));
    }

    private IEnumerator Execute (IEnumerator enumerator) {
      try {
        yield return enumerator;
        this.state = PromiseState.Fulfilled;
        if (this.onFulfilled != null)
          this.onFulfilled ();
      } finally {
        if (this.state != PromiseState.Fulfilled) {
          this.state = PromiseState.Rejected;
          if (this.onRejected != null)
            this.onRejected ("Enumator did not fulfill");
        }
        if (this.onFinally != null)
          this.onFinally ();
      }
    }

    public PromiseEnumerator Then (Action onFulfilled) {
      this.onFulfilled = onFulfilled;
      return this;
    }

    public PromiseEnumerator Catch (Action<string> onRejected) {
      this.onRejected = onRejected;
      return this;
    }

    public PromiseEnumerator Finally (Action onFinally) {
      this.onFinally = onFinally;
      return this;
    }

    public void Consume () {
      this.state = PromiseState.Rejected;
    }
  }
}