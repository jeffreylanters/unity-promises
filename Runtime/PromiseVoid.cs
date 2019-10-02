using System;
using System.Collections;
using UnityEngine;

namespace UnityPackages {

  public class Promise {
    private Action<Action, Action<string>> executor;
    private Action onFulfilled;
    private Action<string> onRejected;
    private Action onFinally;

    public PromiseState state = PromiseState.pending;

    public Promise (Action<Action, Action<string>> executor) {
      PromiseTicker.Enumerator.StartCoroutine (this.Execute (executor));
    }

    public IEnumerator Execute (Action<Action, Action<string>> executor) {
      yield return null;
      executor (
        () => {
          if (this.state != PromiseState.pending)
            return;
          this.state = PromiseState.fulfilled;
          if (this.onFulfilled != null)
            this.onFulfilled ();
          if (this.onFinally != null)
            this.onFinally ();
        }, reason => {
          if (this.state != PromiseState.pending)
            return;
          this.state = PromiseState.rejected;
          if (this.onRejected != null)
            this.onRejected (reason);
          if (this.onFinally != null)
            this.onFinally ();
        });
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
      this.state = PromiseState.rejected;
    }
  }
}