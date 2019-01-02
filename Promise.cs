using System;

public class Promise<T> {

  private Action<T> onFulfilled;
  private Action<string> onRejected;
  private Action onFinally;

  public State state = State.rejected;

  public enum State {
    pending,
    fulfilled,
    rejected
  }

  public Promise (Action<Action<T>, Action<string>> executor) {
    executor (
      value => {
        if (this.state != State.pending)
          return;
        this.state = State.fulfilled;
        if (this.onFulfilled != null)
          this.onFulfilled (value);
        if (this.onFinally != null)
          this.onFinally ();
      }, reason => {
        if (this.state != State.pending)
          return;
        this.state = State.rejected;
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
    this.state = State.rejected;
  }
}