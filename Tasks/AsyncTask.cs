using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Tasks
{
	public abstract class AsyncTask
	{
		private event Action OnCompleted;
		private event Action OnCanceled;
		private event Action OnFaulted;

		public abstract UniTask Execute(CancellationToken token);

		public AsyncTask OnComplete(Action action)
		{
			OnCompleted += action;

			return this;
		}

		public AsyncTask OnCancel(Action action)
		{
			OnCanceled += action;

			return this;
		}

		public AsyncTask OnFault(Action action)
		{
			OnFaulted += action;

			return this;
		}

		internal void InvokeOnCompleted()
		{
			OnCompleted?.Invoke();
		}

		internal void InvokeOnCanceled()
		{
			OnCanceled?.Invoke();
		}

		internal void InvokeOnFaulted()
		{
			OnFaulted?.Invoke();
		}
	}

	public abstract class AsyncTask<T>
	{
		public T Result { get; private set; }

		private event Action<T> OnCompleted;
		private event Action OnCanceled;
		private event Action OnFaulted;

		public abstract UniTask<T> Execute(CancellationToken token);

		public AsyncTask<T> OnComplete(Action<T> action)
		{
			OnCompleted += action;

			return this;
		}

		public AsyncTask<T> OnCancel(Action action)
		{
			OnCanceled += action;

			return this;
		}

		public AsyncTask<T> OnFault(Action action)
		{
			OnFaulted += action;

			return this;
		}

		internal void SaveResult(T result)
		{
			Result = result;
		}

		internal void InvokeOnCompleted()
		{
			OnCompleted?.Invoke(Result);
		}

		internal void InvokeOnCanceled()
		{
			OnCanceled?.Invoke();
		}

		internal void InvokeOnFaulted()
		{
			OnFaulted?.Invoke();
		}
	}
}