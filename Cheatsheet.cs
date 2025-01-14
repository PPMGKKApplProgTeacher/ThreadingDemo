using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

class TaskCheatsheet
{
    static async Task Main()
    {
        // Basic Task Creation Methods
        // Task.Run - Creates and starts a task in one step (recommended way)
        var runTask = Task.Run(() => Console.WriteLine("Task.Run"));

        // New Task() - Creates a task that must be manually started
        var newTask = new Task(() => Console.WriteLine("New Task"));
        newTask.Start();

        // Task Factory - Alternative way to create and start tasks
        var factoryTask = Task.Factory.StartNew(() => Console.WriteLine("Task Factory"));

        // Task Completion Source - Create a task that can be manually completed
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);  // Manually complete the task
        var tcsTask = tcs.Task;  // Get the task

        // Task Properties
        await Task.Delay(100);  // Let some tasks complete
        Console.WriteLine($"Status: {runTask.Status}");         // Gets the current state of the task
        Console.WriteLine($"IsCompleted: {runTask.IsCompleted}");   // True if task has completed
        Console.WriteLine($"IsCanceled: {runTask.IsCanceled}");    // True if task was canceled
        Console.WriteLine($"IsFaulted: {runTask.IsFaulted}");     // True if task threw an exception
        Console.WriteLine($"Id: {runTask.Id}");            // Unique identifier for the task

        // Task Creation Methods for Already Completed Tasks
        var completedTask = Task.CompletedTask;            // Returns a completed Task
        var fromResult = Task.FromResult(42);              // Creates a completed Task<T> with result
        var fromException = Task.FromException(new Exception()); // Creates a faulted task
        var cancelledTask = Task.FromCanceled(new CancellationToken(true)); // Creates a canceled task

        // Task Waiting Methods
        await Task.WhenAll(runTask, newTask, factoryTask);  // Waits for all tasks to complete
        var firstTask = await Task.WhenAny(runTask, newTask, factoryTask); // Waits for first task to complete
        await Task.Yield();  // Yields control back to the caller

        // Task Delay Methods
        await Task.Delay(1000);  // Waits for specified milliseconds
        await Task.Delay(TimeSpan.FromSeconds(1));  // Waits for specified TimeSpan

        // Task Continuation
        var continuation = runTask.ContinueWith(t => Console.WriteLine("Continued"));
        
        // Task with Result
        var taskWithResult = Task.Run(() => 42);
        var result = await taskWithResult;  // Gets the result (async)
        var blockingResult = taskWithResult.Result;  // Gets the result (blocking)

        // Task Exception Handling
        try
        {
            await Task.Run(() => throw new Exception("Task Exception"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        // Task Cancellation
        using (var cts = new CancellationTokenSource())
        {
            var cancelTask = Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                }
            }, cts.Token);

            cts.Cancel();  // Request cancellation
            try
            {
                await cancelTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was canceled");
            }
        }

        // Task.WhenAll with results
        var tasks = new List<Task<int>>
        {
            Task.Run(() => 1),
            Task.Run(() => 2),
            Task.Run(() => 3)
        };
        var allResults = await Task.WhenAll(tasks);  // Gets all results as array

        // Task.WhenAny with results
        var firstResult = await (await Task.WhenAny(tasks));  // Gets result of first completed task

        // Task with progress reporting
        var progress = new Progress<int>(percent => 
            Console.WriteLine($"Progress: {percent}%"));
        await Task.Run(async () =>
        {
            for (int i = 0; i <= 100; i += 20)
            {
                ((IProgress<int>)progress).Report(i);
                await Task.Delay(100);
            }
        });

        // Unwrap nested tasks
        Task<Task> nestedTask = Task.Run(async () => 
            await Task.Run(() => Console.WriteLine("Nested")));
        await nestedTask.Unwrap();  // Unwraps the nested Task

        // ConfigureAwait
        await Task.Run(() => Console.WriteLine("ConfigureAwait"))
            .ConfigureAwait(false);  // Configures how the continuation is scheduled
    }
}
