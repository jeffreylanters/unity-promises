using System;
using System.Collections;
using System.Threading.Tasks;
using ElRaccoone.Promises.Core;

namespace ElRaccoone.Promises {

  /// <summary>
  /// Promise with a generic resolve parameter type.
  /// </summary>
  /// <typeparam name="ResolveType">The type of the resolver's value.</typeparam>
  public class Promise<ResolveType> : Promise<ResolveType, Exception> {

    /// <summary>
    /// Instanciates a new promise with an executor to either resolve or reject.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve and reject methods.</param>
    public Promise (Action<Action<ResolveType>, Action<Exception>> executor) : base (executor) { }
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
    public PromiseState state = PromiseState.Pending;

    /// <summary>
    /// Instanciates a new promise with an executor to either resolve or reject.
    /// </summary>
    /// <param name="executor">Callback method containing the resolve and reject methods.</param>
    public Promise (Action<Action<ResolveType>, Action<RejectType>> executor) {
      PromiseTicker.enumerator.StartCoroutine (this.Execute (executor));
    }

    private IEnumerator Execute (Action<Action<ResolveType>, Action<RejectType>> executor) {
      yield return null;
      executor (
        value => {
          if (this.state != PromiseState.Pending)
            return;
          this.state = PromiseState.Fulfilled;
          this.resolveValue = value;
          if (this.onResolve != null)
            this.onResolve (value);
          if (this.onFinally != null)
            this.onFinally ();
        }, reason => {
          if (this.state != PromiseState.Pending)
            return;
          this.state = PromiseState.Rejected;
          this.rejectValue = reason;
          if (this.onRejected != null)
            this.onRejected (reason);
          if (this.onFinally != null)
            this.onFinally ();
        });
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
      this.state = PromiseState.Rejected;
    }

    public async Task<ResolveType> Async () {
      while (this.state == PromiseState.Pending)
        await Task.Delay (1);
      if (this.state == PromiseState.Rejected)
        throw new Exception (this.rejectValue.ToString ());
      return this.resolveValue;
    }
  }
}