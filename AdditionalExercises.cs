using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

class AdditionalExercises
{
    static async Task Main()
    {
        Console.WriteLine("Exercise 1: Task Progress Reporting");
        await Exercise1();
        Console.WriteLine("\nExercise 2: Task Continuation");
        await Exercise2();
        Console.WriteLine("\nExercise 3: Task Race Condition");
        await Exercise3();
        Console.WriteLine("\nExercise 4: Task Cancellation");
        await Exercise4();
        Console.WriteLine("\nExercise 5: Batch Processing");
        await Exercise5();
        Console.WriteLine("\nChallenge Exercise: Task Pipeline");
        await ChallengeExercise();
    }

    /// <summary>
    /// Exercise 1: Task Progress Reporting
    /// Create a task that simulates a file processing operation:
    /// - Process 10 "chunks" of a file
    /// - Report progress after each chunk (10%, 20%, etc.)
    /// - Display a completion message
    /// Expected Output:
    /// Processing: 10% complete
    /// Processing: 20% complete
    /// ...
    /// Processing: 100% complete
    /// File processing completed!
    /// </summary>
    static async Task Exercise1()
    {
    }

    /// <summary>
    /// Exercise 2: Task Continuation
    /// Create a chain of tasks where each task depends on the previous one:
    /// - Task 1: Generate a random number (1-100)
    /// - Task 2: Double the number
    /// - Task 3: Add 50 to the result
    /// Print the result after each operation
    /// Expected Output:
    /// Generated number: 42
    /// After doubling: 84
    /// Final result: 134
    /// </summary>
    static async Task Exercise2()
    {
    }

    /// <summary>
    /// Exercise 3: Task Race Condition
    /// Create two tasks that compete to complete first:
    /// - Task 1: Count up from 1 to 5 with random delays
    /// - Task 2: Count down from 5 to 1 with random delays
    /// Print which task finishes first
    /// Expected Output:
    /// Up: 1
    /// Down: 5
    /// Up: 2
    /// Down: 4
    /// ...
    /// Count Up task finished first!
    /// </summary>
    static async Task Exercise3()
    {
    }

    /// <summary>
    /// Exercise 4: Task Cancellation
    /// Create a task that performs a long-running operation:
    /// - Count from 1 to 20 with a delay between each number
    /// - Allow cancellation after 5 numbers
    /// - Handle the cancellation gracefully
    /// Expected Output:
    /// Counting: 1
    /// Counting: 2
    /// ...
    /// Counting: 5
    /// Operation cancelled at count 5
    /// </summary>
    static async Task Exercise4()
    {
    }

    /// <summary>
    /// Exercise 5: Batch Processing
    /// Process a collection of items in batches:
    /// - Create a list of 20 numbers
    /// - Process them in batches of 5
    /// - Print progress after each batch
    /// Expected Output:
    /// Starting batch 1 (items 1-5)
    /// Completed batch 1
    /// Starting batch 2 (items 6-10)
    /// ...
    /// All batches processed!
    /// </summary>
    static async Task Exercise5()
    {
    }

    /// <summary>
    /// Challenge Exercise: Task Pipeline
    /// Create a pipeline of tasks that process data:
    /// - Stage 1: Generate 5 random numbers
    /// - Stage 2: Square each number
    /// - Stage 3: Sum only the even results
    /// Process data through the pipeline and show results after each stage
    /// Expected Output:
    /// Generated numbers: 3, 7, 4, 1, 9
    /// Squared numbers: 9, 49, 16, 1, 81
    /// Sum of even squares: 16
    /// Pipeline complete!
    /// </summary>
    static async Task ChallengeExercise()
    {
        // Stage 1: Generate numbers

        // Stage 2: Square numbers

        // Stage 3: Sum even numbers
    }
}
