using System;
using Core.Tasks;
using UnityEngine;

namespace Core.Managers
{
	public static class TaskExtensions
	{
		/// <summary>
		/// Runs the specified task and handles any exceptions that occur during the process
		/// </summary>
		/// <param name="asyncTask">A task that needs to be started from synchronous code</param>
		public static async void RunAndForget(this AsyncTask asyncTask)
		{
			try
			{
				var task = asyncTask.Execute();

				await task;

				if (task.IsCompletedSuccessfully)
				{
					asyncTask.InvokeOnCompleted();
				}

				if (task.IsCanceled)
				{
					asyncTask.InvokeOnCanceled();

					Debug.Log($"{asyncTask.GetType().Name} is canceled");
				}
			}
			catch (Exception error)
			{
				asyncTask.InvokeOnFaulted();

				Debug.LogError(error);
			}
		}

		/// <summary>
		/// Runs the specified task and handles any exceptions that occur during the process
		/// </summary>
		/// <param name="asyncTask">A task that needs to be started from synchronous code</param>
		/// <typeparam name="T">The type of result of successful task completion</typeparam>
		public static async void RunAndForget<T>(this AsyncTask<T> asyncTask)
		{
			try
			{
				var task = asyncTask.Execute();

				await task;

				if (task.IsCompletedSuccessfully)
				{
					asyncTask.SaveResult(task.Result);
					asyncTask.InvokeOnCompleted();
				}

				if (task.IsCanceled)
				{
					asyncTask.InvokeOnCanceled();

					Debug.Log($"{asyncTask.GetType().Name} is canceled");
				}
			}
			catch (Exception error)
			{
				asyncTask.InvokeOnFaulted();

				Debug.LogError(error);
			}
		}
	}
}