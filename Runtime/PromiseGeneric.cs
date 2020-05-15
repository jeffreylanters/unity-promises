using System;
using System.Collections;
using ElRaccoone.Promises.Core;

namespace ElRaccoone.Promises {

  public class Promise<T> {
    private Action<Action<T>, Action<string>> executor;
    private Action<T> onFulfilled;
    private Action<string> onRejected;
    private Action onFinally;

    public PromiseState state = PromiseState.pending;

    public Promise (Action<Action<T>, Action<string>> executor) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (executor));
    }

    public IEnumerator Execute (Action<Action<T>, Action<string>> executor) {
      yield return null;
      executor (
        value => {
          if (this.state != PromiseState.pending)
            return;
          this.state = PromiseState.fulfilled;
          if (this.onFulfilled != null)
            this.onFulfilled (value);
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

    public Promise<T> Then (Action<T> onFulfilled) {
      this.onFulfilled = onFulfilled;
      return this;
    }

    public Promise<T> Catch (Action<string> onRejected) {
      this.onRejected = onRejected;
      return this;
    }

    public Promise<T> Finally (Action onFinally) {
      this.onFinally = onFinally;
      return this;
    }

    public void Consume () {
      this.state = PromiseState.rejected;
    }
  }
}