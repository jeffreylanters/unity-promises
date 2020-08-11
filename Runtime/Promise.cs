using System;
using System.Collections;
using ElRaccoone.Promises.Core;

namespace ElRaccoone.Promises {

  public class Promise {
    private Action<Action, Action<string>> executor;
    private Action onFulfilled;
    private Action<string> onRejected;
    private Action onFinally;

    public PromiseState state = PromiseState.Pending;

    public Promise (Action<Action, Action<string>> executor) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (executor));
    }

    public Promise (Action<Action> executor) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (executor));
    }

    public Promise (IEnumerator enumerator) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (enumerator));
    }

    private IEnumerator Execute (Action<Action, Action<string>> executor) {
      yield return null;
      executor (
        () => {
          if (this.state != PromiseState.Pending)
            return;
          this.state = PromiseState.Fulfilled;
          if (this.onFulfilled != null)
            this.onFulfilled ();
          if (this.onFinally != null)
            this.onFinally ();
        },
        reason => {
          if (this.state != PromiseState.Pending)
            return;
          this.state = PromiseState.Rejected;
          if (this.onRejected != null)
            this.onRejected (reason);
          if (this.onFinally != null)
            this.onFinally ();
        });
    }

    private IEnumerator Execute (Action<Action> executor) {
      yield return null;
      executor (
        () => {
          if (this.state != PromiseState.Pending)
            return;
          this.state = PromiseState.Fulfilled;
          if (this.onFulfilled != null)
            this.onFulfilled ();
          if (this.onFinally != null)
            this.onFinally ();
        });
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

    public Promise Then (Action onFulfilled) {
      this.onFulfilled = onFulfilled;
      return this;
    }

    public Promise Catch (Action<string> onRejected) {
      this.onRejected = onRejected;
      return this;
    }

    public Promise Finally (Action onFinally) {
      this.onFinally = onFinally;
      return this;
    }

    public void Consume () {
      this.state = PromiseState.Rejected;
    }
  }
}