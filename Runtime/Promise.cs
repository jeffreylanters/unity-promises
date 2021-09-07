using System;
using System.Threading.Tasks;

namespace ElRaccoone.Promises {

  /// <summary>
  /// A promise.
  /// </summary>
  public class Promise : Promise<Promise.Void, Exception> {

    /// <summary>
    /// Void placeholder for generic resolver.
    /// </summary>
    public class Void { }

    /// <summary>
    /// Instantiates a new promise with an executor consisting of a resolver and a rejector.
    /// </summary>
    /// <param name="executor">The executor.</param>
    public Promise (Action<Action, Action<Exception>> executor) : base (executor) { }
  }

  /// <summary>
  /// A promise with a generic resolve type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  public class Promise<ResolveType> : Promise<ResolveType, Exception> {

    /// <summary>
    /// Instantiates a new promise with an executor consisting of a generic resolver and a rejector.
    /// </summary>
    /// <param name="executor">The executor.</param>
    public Promise (Action<Action<ResolveType>, Action<Exception>> executor) : base (executor) { }
  }

  /// <summary>
  /// A promise with a generic resolve and reject type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  /// <typeparam name="RejectType">the type of the rejector's value.</typeparam>
  public class Promise<ResolveType, RejectType> where RejectType : Exception {

    /// <summary>
    /// Optional callback for when the promise resolves with a generic.
    /// </summary>
    private Action<ResolveType> onGenericResolveCallback;

    /// <summary>
    /// Optional callback for when the promise resolves.
    /// </summary>
    private Action onResolveCallback;

    /// <summary>
    /// Optional callback for when the promise rejects with a generic.
    /// </summary>
    private Action<RejectType> onGenericRejectedCallback;

    /// <summary>
    /// Optional callback for when the promise finalizes.
    /// </summary>
    private Action onFinallyCallback;

    /// <summary>
    /// Stores the value of a resolved promise.
    /// </summary>
    private ResolveType resolvedValue;

    /// <summary>
    /// Stores the value of a rejected promise.
    /// </summary>
    private RejectType rejectedValue;

    /// <summary>
    /// The state of the promise.
    /// </summary>
    public State state { get; private set; } = State.Pending;

    /// <summary>
    /// Promise states.
    /// </summary>
    public enum State {

      /// <summary>
      /// Initial state, neither fulfilled nor rejected.
      /// </summary>
      Pending = 0,

      /// <summary>
      /// The operation completed successfully.
      /// </summary>
      Fulfilled = 1,

      /// <summary>
      /// The operation failed.
      /// </summary>
      Rejected = 2,
    }

    /// <summary>
    /// Instantiates a new promise with an executor consisting of a resolver and a generic rejector.
    /// </summary>
    /// <param name="executor">The executor.</param>
    public Promise (Action<Action, Action<RejectType>> executor) {
      this.Execute (executor);
    }

    /// <summary>
    /// Instantiates a new promise with an executor consisting of a generic resolver and a generic rejector.
    /// </summary>
    /// <param name="executor">The executor.</param>
    public Promise (Action<Action<ResolveType>, Action<RejectType>> executor) {
      this.Execute (executor);
    }

    /// <summary>
    /// Executes the promise with a resolver.
    /// </summary>
    /// <param name="executor">The executor.</param>
    private async void Execute (Action<Action, Action<RejectType>> executor) {
      await Task.Yield ();
      executor (this.ExecuteResolve, this.ExecuteReject);
    }

    /// <summary>
    /// Executes the promise with a generic resolver and generic rejector.
    /// </summary>
    /// <param name="executor">The executor.</param>
    private async void Execute (Action<Action<ResolveType>, Action<RejectType>> executor) {
      await Task.Yield ();
      executor (this.ExecuteResolve, this.ExecuteReject);
    }

    /// <summary>
    /// Invokes the executor's resolve method.
    /// </summary>
    private void ExecuteResolve () {
      if (this.state != State.Pending)
        return;
      this.state = State.Fulfilled;
      if (this.onResolveCallback != null)
        this.onResolveCallback ();
      if (this.onFinallyCallback != null)
        this.onFinallyCallback ();
    }

    /// <summary>
    /// Invokes the executor's generic resolve method.
    /// </summary>
    /// <param name="value">The resolver value.</param>
    private void ExecuteResolve (ResolveType value) {
      if (this.state != State.Pending)
        return;
      this.state = State.Fulfilled;
      this.resolvedValue = value;
      if (this.onResolveCallback != null)
        this.onResolveCallback ();
      if (this.onGenericResolveCallback != null)
        this.onGenericResolveCallback (value);
      if (this.onFinallyCallback != null)
        this.onFinallyCallback ();
    }

    /// <summary>
    /// Invokes the executor's generic reject method.
    /// </summary>
    /// <param name="exception">The exception.</param>
    private void ExecuteReject (RejectType exception) {
      if (this.state != State.Pending)
        return;
      this.state = State.Rejected;
      this.rejectedValue = exception;
      if (this.onGenericRejectedCallback != null)
        this.onGenericRejectedCallback (exception);
      if (this.onFinallyCallback != null)
        this.onFinallyCallback ();
    }

    /// <summary>
    /// Sets the resolve callback.
    /// </summary>
    /// <param name="action">The resolver callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Then (Action action) {
      this.onResolveCallback = action;
      return this;
    }

    /// <summary>
    /// Sets the generic resolve callback.
    /// </summary>
    /// <param name="action">The resolver callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Then (Action<ResolveType> action) {
      this.onGenericResolveCallback = action;
      return this;
    }

    /// <summary>
    /// Sets the generic reject callback.
    /// </summary>
    /// <param name="action">The rejection callback</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Catch (Action<RejectType> action) {
      this.onGenericRejectedCallback = action;
      return this;
    }

    /// <summary>
    /// Sets the finalize callback.
    /// </summary>
    /// <param name="action">The final callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Finally (Action action) {
      this.onFinallyCallback = action;
      return this;
    }

    /// <summary>
    /// Awaits the promise.
    /// </summary>
    /// <returns>A task containing the resolved value.</returns>
    public async Task<ResolveType> Async () {
      while (this.state == State.Pending)
        await Task.Yield ();
      if (this.state == State.Rejected)
        throw this.rejectedValue;
      return this.resolvedValue;
    }
  }
}