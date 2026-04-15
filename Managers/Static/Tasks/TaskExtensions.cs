using System;
using System.Threading;
using Core.Tasks;
using UnityEngine;

namespace Core.Managers
{
	public static class TaskExtensions
	{
		/// <summary>
		/// Runs the specified task and handles any exceptions that occur during the process.
		/// </summary>
		/// <param name="asyncTask">The asynchronous task to run from synchronous code.</param>
		public static async void FireAndForget(this AsyncTask asyncTask)
		{
			try
			{
				await asyncTask.Execute(CancellationToken.None);

				asyncTask.InvokeOnCompleted();
			}
			catch (OperationCanceledException)
			{
				Debug.Log($"{asyncTask.GetType().Name} is canceled.");

				asyncTask.InvokeOnCanceled();
			}
			catch (Exception error)
			{
				Debug.LogError(error);

				asyncTask.InvokeOnFaulted();
			}
		}

		/// <summary>
		/// Runs the specified task and handles any exceptions that occur during the process.
		/// </summary>
		/// <param name="asyncTask">The asynchronous task to run from synchronous code.</param>
		/// <typeparam name="T">The type of result of successful task completion</typeparam>
		public static async void FireAndForget<T>(this AsyncTask<T> asyncTask)
		{
			try
			{
				var result = await asyncTask.Execute(CancellationToken.None);

				asyncTask.SaveResult(result);
				asyncTask.InvokeOnCompleted();
			}
			catch (OperationCanceledException)
			{
				Debug.Log($"{asyncTask.GetType().Name} is canceled.");

				asyncTask.InvokeOnCanceled();
			}
			catch (Exception error)
			{
				Debug.LogError(error);

				asyncTask.InvokeOnFaulted();
			}
		}
	}
}