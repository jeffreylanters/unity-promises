using System;
using System.Threading.Tasks;

// TODO add typeless Promise
// TODO add enumatable Promise

namespace ElRaccoone.Promises {

  /// <summary>
  /// A promise with a generic resolve type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  /// <typeparam name="RejectType">The type of the rejector's value.</typeparam>
  public class Promise<ResolveType> : Promise<ResolveType, Exception> {

    /// <summary>
    /// Instantiates a new promise with an executor to resolve.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve method.</param>
    public Promise (Action<Action<ResolveType>> executor) : base (executor) { }

    /// <summary>
    /// Instantiates a new promise with an executor to either resolve or reject.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve and reject methods.</param>
    public Promise (Action<Action<ResolveType>, Action<Exception>> executor) : base (executor) { }
  }

  /// <summary>
  /// A promise with a generic resolve and reject type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  /// <typeparam name="RejectType">the type of the rejector's value.</typeparam>
  public class Promise<ResolveType, RejectType> where RejectType : Exception {

    /// <summary>
    /// Optional callback for when the promise resolves with a paramter.
    /// </summary>
    private Action<ResolveType> onResolveWithParameter;

    /// <summary>
    /// Optional callback for when the promise resolves without a paramter.
    /// </summary>
    private Action onResolveWithoutParameter;

    /// <summary>
    /// Optional callback for when the promise rejects.
    /// </summary>
    private Action<RejectType> onRejected;

    /// <summary>
    /// Optional callback for when the promise ends.
    /// </summary>
    private Action onFinally;

    /// <summary>
    /// 
    /// </summary>
    private ResolveType resolveValue;

    /// <summary>
    /// 
    /// </summary>
    private RejectType rejectValue;

    /// <summary>
    /// The state of the promise.
    /// </summary>
    public State state { get; private set; } = State.Pending;

    /// <summary>
    /// Defines the state of the promise.
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
      Rejected = 2
    }

    /// <summary>
    /// Instantiates a new promise with an executor to resolve.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve method.</param>
    public Promise (Action<Action<ResolveType>> executor) {
      this.Execute (executor);
    }

    /// <summary>
    /// Instantiates a new promise with an executor to either resolve or reject.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve and reject methods.</param>
    public Promise (Action<Action<ResolveType>, Action<RejectType>> executor) {
      this.Execute (executor);
    }

    /// <summary>
    /// Executes the promise with just a resolver.
    /// </summary>
    /// <param name="executor">The promise executor.</param>
    /// <returns>A couritine sleeping one frame allowing to set callbacks.</returns>
    private async void Execute (Action<Action<ResolveType>> executor) {
      await Task.Yield ();
      executor (this.ExecuteResolver);
    }

    /// <summary>
    /// Executes the promise with a resolver and rejector.
    /// </summary>
    /// <param name="executor">The promise executor.</param>
    /// <returns>A couritine sleeping one frame allowing to set callbacks.</returns>
    private async void Execute (Action<Action<ResolveType>, Action<RejectType>> executor) {
      await Task.Yield ();
      executor (this.ExecuteResolver, this.ExecuteRejector);
    }

    /// <summary>
    /// Invokes the executor's resolver method with a parameter.
    /// </summary>
    /// <param name="value">The resolver value.</param>
    private void ExecuteResolver (ResolveType value) {
      if (this.state != State.Pending)
        return;
      this.state = State.Fulfilled;
      this.resolveValue = value;
      if (this.onResolveWithoutParameter != null)
        this.onResolveWithoutParameter ();
      if (this.onResolveWithParameter != null)
        this.onResolveWithParameter (value);
      if (this.onFinally != null)
        this.onFinally ();
    }

    /// <summary>
    /// Invokes the executor's resolver method without a parameter.
    /// </summary>
    private void ExecuteResolver () {
      if (this.state != State.Pending)
        return;
      this.state = State.Fulfilled;
      if (this.onResolveWithoutParameter != null)
        this.onResolveWithoutParameter ();
      if (this.onFinally != null)
        this.onFinally ();
    }

    /// <summary>
    /// Invokes the executor's rejector method.
    /// </summary>
    /// <param name="exception">The exception.</param>
    private void ExecuteRejector (RejectType exception) {
      if (this.state != State.Pending)
        return;
      this.state = State.Rejected;
      this.rejectValue = exception;
      if (this.onRejected != null)
        this.onRejected (exception);
      if (this.onFinally != null)
        this.onFinally ();
    }

    /// <summary>
    /// Sets the resolve callback to a value without a parameter.
    /// </summary>
    /// <param name="onResolve">The resolver callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Then (Action onResolve) {
      this.onResolveWithoutParameter = onResolve;
      return this;
    }

    /// <summary>
    /// Sets the resolve callback to a value with a parameter.
    /// </summary>
    /// <param name="onResolve">The resolver callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Then (Action<ResolveType> onResolve) {
      this.onResolveWithParameter = onResolve;
      return this;
    }

    /// <summary>
    /// Sets the reject callback.
    /// </summary>
    /// <param name="onRejected">The rejection callback</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Catch (Action<RejectType> onRejected) {
      this.onRejected = onRejected;
      return this;
    }

    /// <summary>
    /// Sets the final callback/
    /// </summary>
    /// <param name="onFinally">The final callback.</param>
    /// <returns>The promise.</returns>
    public Promise<ResolveType, RejectType> Finally (Action onFinally) {
      this.onFinally = onFinally;
      return this;
    }

    /// <summary>
    /// Consumes the promise, the executor will no longer invoke any callback.
    /// </summary>
    public void Consume () {
      this.state = State.Rejected;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<ResolveType> Async () {
      while (this.state == State.Pending)
        await Task.Yield ();
      if (this.state == State.Rejected)
        throw this.rejectValue;
      return this.resolveValue;
    }
  }
}