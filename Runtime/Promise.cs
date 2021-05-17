using System;
using System.Threading.Tasks;

// TODO add typeless Promise
// TODO add enumatable Promise

namespace ElRaccoone.Promises {

  /// <summary>
  /// Promise with a generic resolve parameter type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  public class Promise<ResolveType> : Promise<ResolveType, Exception> {

    /// <summary>
    /// Instantiates a new promise with an executor to either resolve or reject.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve and reject methods.</param>
    public Promise (Action<Action<ResolveType>, Action<Exception>> executor) : base (executor) { }

    /// <summary>
    /// Instantiates a new promise with an executor to resolve.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve method.</param>
    public Promise (Action<Action<ResolveType>> executor) : base (executor) { }
  }

  /// <summary>
  /// Promise with a generic resolve and reject parameter type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  /// <typeparam name="RejectType">the type of the rejector's value.</typeparam>
  public class Promise<ResolveType, RejectType> where RejectType : Exception {

    /// <summary>
    /// Optional clalback for when the promise resolves.
    /// </summary>
    private Action<ResolveType> onResolve;

    /// <summary>
    /// Optional clalback for when the promise rejects.
    /// </summary>
    private Action<RejectType> onRejected;

    /// <summary>
    /// Optional clalback for when the promise ends.
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

    private void ExecuteResolver (ResolveType value) {
      if (this.state != State.Pending)
        return;
      this.state = State.Fulfilled;
      this.resolveValue = value;
      if (this.onResolve != null)
        this.onResolve (value);
      if (this.onFinally != null)
        this.onFinally ();
    }

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

    public Promise<ResolveType, RejectType> Then (Action<ResolveType> onResolve) {
      this.onResolve = onResolve;
      return this;
    }

    public Promise<ResolveType, RejectType> Catch (Action<RejectType> onRejected) {
      this.onRejected = onRejected;
      return this;
    }

    public Promise<ResolveType, RejectType> Finally (Action onFinally) {
      this.onFinally = onFinally;
      return this;
    }

    public void Consume () {
      this.state = State.Rejected;
    }

    public async Task<ResolveType> Async () {
      while (this.state == State.Pending)
        await Task.Delay (1);
      if (this.state == State.Rejected)
        throw new Exception (this.rejectValue.ToString ());
      return this.resolveValue;
    }
  }
}