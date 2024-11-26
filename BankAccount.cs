using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Banking System with Thread Synchronization ===");

        // Create a BankAccount object
        var account = new BankAccount("BG12UNCR70001520202020", 1000);

        Console.WriteLine($"Initial Balance: {account.Balance}");

        // Create threads to perform deposit and withdrawal operations
        Thread depositThread = new Thread(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                account.Deposit(200);
                Thread.Sleep(100); // Simulate time between operations
            }
        });

        Thread withdrawThread = new Thread(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                account.Withdraw(150);
                Thread.Sleep(150); // Simulate time between operations
            }
        }); 

        // Start the threads
        depositThread.Start();
        withdrawThread.Start();

        // Wait for both threads to complete
        depositThread.Join();
        withdrawThread.Join();

        Console.WriteLine($"Final Balance: {account.Balance}");
    }
}

class BankAccount
{
    private readonly object lockObject = new object();

    public string IBAN { get; private set; }
    public decimal Balance { get; private set; }

    public BankAccount(string iban, decimal initialBalance)
    {
        IBAN = iban;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Deposit amount must be positive.");
            return;
        }

        lock (lockObject) // Synchronize access to the shared resource
        {
            Balance += amount;
            Console.WriteLine($"Deposited {amount:C}. New Balance: {Balance:C}");
        }
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Withdrawal amount must be positive.");
            return;
        }

        lock (lockObject) // Synchronize access to the shared resource
        {
            if (amount > Balance)
            {
                Console.WriteLine("Insufficient funds for withdrawal.");
            }
            else
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount:C}. New Balance: {Balance:C}");
            }
        }
    }
}