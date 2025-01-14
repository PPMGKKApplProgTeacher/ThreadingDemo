using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

public class TaskCheatsheet
{
	public static async Task Main()
	{
		Console.WriteLine("=== Task Cheatsheet Demo ===\n");
		var cheatsheet = new TaskCheatsheet();
		Console.WriteLine("1. Basic Task Creation");
		await cheatsheet.BasicTaskCreation();
		Console.WriteLine("Basic task completed\n");
		Console.WriteLine("2. Task With Result");
		int resultValue = await cheatsheet.TaskWithResult();
		Console.WriteLine($"Received result: {resultValue}\n");
		Console.WriteLine("3. Task.Run Example");
		await cheatsheet.TaskRunExample();
		Console.WriteLine("Task.Run example completed\n");
		Console.WriteLine("4. Exception Handling");
		await cheatsheet.ExceptionHandling();
		Console.WriteLine("Exception handling example completed\n");
		Console.WriteLine("5. Task Continuation");
		await cheatsheet.TaskContinuation();
		Console.WriteLine("Continuation example completed\n");
		Console.WriteLine("6. Parallel Tasks");
		Console.WriteLine("Starting parallel tasks...");
		await cheatsheet.ParallelTasks();
		Console.WriteLine("All parallel tasks completed\n");
		Console.WriteLine("7. Wait For First Task");
		await cheatsheet.WaitForFirst();
		Console.WriteLine("WaitForFirst example completed\n");
		Console.WriteLine("8. Cancellable Task");
		await cheatsheet.CancellableTask();
		Console.WriteLine("Cancellation example completed\n");
		Console.WriteLine("9. Completed Task Example");
		await cheatsheet.CompletedTaskExample();
		Console.WriteLine("Completed task example finished\n");
		Console.WriteLine("10. Delay Example");
		await cheatsheet.DelayExample();
		Console.WriteLine("Delay example completed\n");
		Console.WriteLine("11. Propetries Example");
		await cheatsheet.TaskPropertiesExample();
		Console.WriteLine("Delay Propetries completed\n");
		Console.WriteLine("12. Continuation And Creation Comparison ");
		await cheatsheet.ContinuationAndCreationComparison();
		Console.WriteLine("Continuation And Creation Comparison completed\n");
		Console.WriteLine("13. Combinators Example");
		await cheatsheet.TaskCombinators();
		Console.WriteLine("Combinators Example completed\n");
		Console.WriteLine("14. Waiting Mechanisms Example");
		await cheatsheet.WaitingMechanisms();
		Console.WriteLine("Waiting Mechanisms Example completed\n");
		Console.WriteLine("15. Yield Example");
		await cheatsheet.YieldExample();
		Console.WriteLine("Yield Example completed\n");
		Console.WriteLine("=== All examples completed ===");
	}

	/// <summary>
	/// Demonstrates the basic creation of a Task using Task constructor
	/// Expected behavior: Creates and starts a task that runs a simple action
	/// </summary>
	public async Task BasicTaskCreation()
	{
		// Create task with an Action delegate
		var task = new Task(() =>
		{
			Console.WriteLine("Task is running");
			Thread.Sleep(1000);
		});
		// Start the task
		task.Start();
		// Wait for completion
		await task;
	}

	/// <summary>
	/// Shows how to create and run a task that returns a value
	/// Expected behavior: Returns the computed result after task completion
	/// </summary>
	public async Task<int> TaskWithResult()
	{
		var task = new Task<int>(() =>
		{
			Thread.Sleep(1000);
			return 42;
		});
		task.Start();
		int result = await task;
		return result;
	}

	/// <summary>
	/// Demonstrates Task.Run() - the recommended way to create and start a task
	/// Expected behavior: Immediately starts the task and returns the result
	/// </summary>
	public async Task TaskRunExample()
	{
		// Simple Task.Run with Action
		await Task.Run(() => Console.WriteLine("Task.Run executing"));
		// Task.Run with return value
		int result = await Task.Run(() =>
		{
			Thread.Sleep(1000);
			return 100;
		});
	}

	/// <summary>
	/// Shows how to handle exceptions in tasks
	/// Expected behavior: Catches and handles exceptions thrown within the task
	/// </summary>
	public async Task ExceptionHandling()
	{
		try
		{
			await Task.Run(() =>
			{
				throw new InvalidOperationException("Task failed");
			});
		}
		catch (InvalidOperationException ex)
		{
			Console.WriteLine($"Caught exception: {ex.Message}");
		}
	}

	/// <summary>
	/// Demonstrates task continuation using ContinueWith
	/// Expected behavior: Executes second task after first one completes
	/// </summary>
	public async Task TaskContinuation()
	{
		var task = Task.Run(() =>
		{
			Console.WriteLine("First task");
			return 10;
		});
		var continuation = task.ContinueWith(previousTask =>
		{
			Console.WriteLine($"Continuation with result: {previousTask.Result}");
			return previousTask.Result * 2;
		});
		int result = await continuation;
	}

	/// <summary>
	/// Shows how to use Task.WhenAll to run multiple tasks concurrently
	/// Expected behavior: Runs all tasks in parallel and waits for all to complete
	/// </summary>
	public async Task ParallelTasks()
	{
		var task1 = Task.Run(() => Thread.Sleep(1000));
		var task2 = Task.Run(() => Thread.Sleep(2000));
		var task3 = Task.Run(() => Thread.Sleep(1500));
		await Task.WhenAll(task1, task2, task3);
	}

	/// <summary>
	/// Demonstrates Task.WhenAny to handle the first completed task
	/// Expected behavior: Returns as soon as any task completes
	/// </summary>
	public async Task WaitForFirst()
	{
		var task1 = Task.Run(async () =>
		{
			await Task.Delay(1000);
			return "Task 1";
		});
		var task2 = Task.Run(async () =>
		{
			await Task.Delay(2000);
			return "Task 2";
		});
		Task<string> completedTask = await Task.WhenAny(task1, task2);
		string result = await completedTask;
	}

	/// <summary>
	/// Shows how to use CancellationToken with tasks
	/// Expected behavior: Cancels the task when requested
	/// </summary>
	public async Task CancellableTask()
	{
		using (var cts = new CancellationTokenSource())
		{
			try
			{
				var task = Task.Run(async () =>
				{
					while (true)
					{
						cts.Token.ThrowIfCancellationRequested();
						await Task.Delay(100, cts.Token);
						Console.WriteLine("Working...");
					}
				}, cts.Token);
				// Cancel after 2 seconds
				await Task.Delay(2000);
				cts.Cancel();
				await task;
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Task was cancelled");
			}
		}
	}

	/// <summary>
	/// Demonstrates how to use Task.FromResult for already completed tasks
	/// Expected behavior: Immediately returns a completed task with the specified result
	/// </summary>
	public async Task CompletedTaskExample()
	{
		// Create a completed task with a result
		Task<int> completedTask = Task.FromResult(42);
		int result = await completedTask;
		// Create a completed task without result
		Task completed = Task.CompletedTask;
		await completed;
	}

	/// <summary>
	/// Shows how to use Task.Delay for time-based operations
	/// Expected behavior: Waits for the specified time before continuing
	/// </summary>
	public async Task DelayExample()
	{
		Console.WriteLine("Starting delay");
		await Task.Delay(TimeSpan.FromSeconds(2));
		Console.WriteLine("Delay completed");
	}

	/// <summary>
	/// Demonstrates various Task properties and their uses
	/// Expected behavior: Shows the state changes of a task through its lifecycle
	/// </summary>
	public async Task TaskPropertiesExample()
	{
		var task = new Task<int>(() =>
		{
			Thread.Sleep(1000);
			return 42;
		});
		// Check state before starting
		Console.WriteLine($"Created: Status = {task.Status}");
		Console.WriteLine($"IsCompleted = {task.IsCompleted}");
		Console.WriteLine($"IsCanceled = {task.IsCanceled}");
		Console.WriteLine($"IsFaulted = {task.IsFaulted}");
		task.Start();
		// Check state while running
		Console.WriteLine($"After Start: Status = {task.Status}");
		await task;
		// Check state after completion
		Console.WriteLine($"Completed: Status = {task.Status}");
		Console.WriteLine($"Result = {task.Result}");
		Console.WriteLine($"Id = {task.Id}");
		Console.WriteLine($"IsCompletedSuccessfully = {task.IsCompletedSuccessfully}");
	}

	/// <summary>
	/// Compares different ways of continuing and creating tasks
	/// Expected behavior: Shows different continuation scenarios and task creation methods
	/// </summary>
	public async Task ContinuationAndCreationComparison()
	{
		// ContinueWith with different continuation options
		var initialTask = Task.Run(() => "Initial Task");
		var normalContinuation = initialTask.ContinueWith(t => Console.WriteLine($"Normal continuation with result: {t.Result}"));
		var onlyOnSuccess = initialTask.ContinueWith(t => Console.WriteLine("Only on success"), TaskContinuationOptions.OnlyOnRanToCompletion);
		var onlyOnFault = initialTask.ContinueWith(t => Console.WriteLine("Only on fault"), TaskContinuationOptions.OnlyOnFaulted);
		// FromResult example
		var completedTask = Task.FromResult(42);
		// FromException example
		var failedTask = Task.FromException<int>(new InvalidOperationException("Task failed"));
		// FromCanceled example
		var cts = new CancellationTokenSource();
		cts.Cancel();
		var canceledTask = Task.FromCanceled<int>(cts.Token);
		try
		{
			await failedTask;
		}
		catch (InvalidOperationException ex)
		{
			Console.WriteLine($"Caught expected exception: {ex.Message}");
		}

		try
		{
			await canceledTask;
		}
		catch (TaskCanceledException)
		{
			Console.WriteLine("Caught expected cancellation");
		}
	}

	/// <summary>
	/// Compares WhenAll, WhenAny, and WhenEach functionality
	/// Expected behavior: Demonstrates different ways of handling multiple tasks
	/// </summary>
	public async Task TaskCombinators()
	{
		// Create a list of tasks
		var tasks = Enumerable.Range(1, 5).Select(i => Task.Run(async () =>
		{
			await Task.Delay(i * 1000);
			return i;
		})).ToList();
		// WhenAll - wait for all tasks
		Console.WriteLine("WhenAll example:");
		var allResults = await Task.WhenAll(tasks);
		Console.WriteLine($"All tasks completed with results: {string.Join(", ", allResults)}");
		// WhenAny - wait for first task
		Console.WriteLine("\nWhenAny example:");
		var firstTask = await Task.WhenAny(tasks);
		Console.WriteLine($"First task completed with result: {await firstTask}");
		// ForEach equivalent using Task.WhenAll
		Console.WriteLine("\nParallel ForEach example:");
		var numbers = Enumerable.Range(1, 5);
		await Task.WhenAll(numbers.Select(async n =>
		{
			await Task.Delay(100);
			Console.WriteLine($"Processed {n}");
		}));
	}

	/// <summary>
	/// Compares different waiting mechanisms
	/// Expected behavior: Shows blocking vs non-blocking wait operations
	/// </summary>
	public async Task WaitingMechanisms()
	{
		var tasks = new List<Task<int>>();
		for (int i = 1; i <= 3; i++)
		{
			int localI = i;
			tasks.Add(Task.Run(async () =>
			{
				await Task.Delay(localI * 1000);
				return localI;
			}));
		}

		// Wait (blocking)
		var task1 = tasks[0];
		task1.Wait(2000); // Wait up to 2 seconds
		Console.WriteLine($"Task1 status after Wait: {task1.Status}");
		// WaitAll (blocking)
		var timeout = Task.WaitAll(tasks.ToArray(), 5000); // Wait up to 5 seconds
		Console.WriteLine($"WaitAll completed within timeout: {timeout}");
		// WaitAny (blocking)
		var index = Task.WaitAny(tasks.ToArray(), 1000); // Wait up to 1 second
		Console.WriteLine($"First completed task index: {index}");
		// WaitAsync (non-blocking)
		using var cts = new CancellationTokenSource(2000); // 2 second timeout
		try
		{
			await task1.WaitAsync(cts.Token);
			Console.WriteLine("Task completed within timeout");
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine("Task timed out");
		}
	}

	/// <summary>
	/// Demonstrates Task.Yield usage for cooperative multitasking
	/// Expected behavior: Shows how Yield can prevent long-running operations from blocking
	/// </summary>
	public async Task YieldExample()
	{
		Console.WriteLine("Starting long operation...");
		await ProcessLargeDataSet();
		Console.WriteLine("Long operation completed");
	}

	private async Task ProcessLargeDataSet()
	{
		for (int i = 0; i < 1000; i++)
		{
			// Simulate some work
			DoSomeWork();
			if (i % 100 == 0)
			{
				// Yield control every 100 iterations to prevent blocking
				await Task.Yield();
				Console.WriteLine($"Yielded at iteration {i}");
			}
		}
	}

	private void DoSomeWork()
	{
		// Simulate CPU-bound work
		Thread.SpinWait(10000);
	}

	/// <summary>
	/// Demonstrates the usage of Task.WhenEach introduced in .NET 9
	/// Expected behavior: Processes tasks in order of completion
	/// </summary>
	public static async Task DemonstrateWhenEach()
	{
		var tasks = new List<Task<string>>(5);
		for (var i = 0; i < 5; i++)
		{
			tasks.Add(GetRandomStringAsync());
		}

		await foreach (var text in Task.WhenEach(tasks))
		{
			Console.WriteLine(await text);
		}
	}

	public static async Task<string> GetRandomStringAsync()
	{
		var delay = Random.Shared.Next(1000, 10000);
		await Task.Delay(delay);
		return Guid.NewGuid().ToString();
	}
	
}
