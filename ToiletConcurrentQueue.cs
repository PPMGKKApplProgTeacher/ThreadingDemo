using System;
using System.Collections.Concurrent;
using System.Threading;


// решение, използващо ConcurrentQueue и Monitor класове. 

class ToiletConcurrentQueue
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Интелигентна заключваща система за тоалетни ===");

        // Create SmartLock objects
        SmartLock lock1 = new SmartLock();
        SmartLock lock2 = new SmartLock();

        // Create Bathroom objects
        Bathroom bathroom1 = new Bathroom(lock1, "Тоалетна 1");
        Bathroom bathroom2 = new Bathroom(lock2, "Тоалетна 2");

        // Create a shared queue of people
        ConcurrentQueue<Person> peopleQueue = new ConcurrentQueue<Person>();
        peopleQueue.Enqueue(new Person("Иван", 25));
        peopleQueue.Enqueue(new Person("Мария", 30));
        peopleQueue.Enqueue(new Person("Георги", 35));
        peopleQueue.Enqueue(new Person("Елена", 40));
        peopleQueue.Enqueue(new Person("Петър", 20));
        peopleQueue.Enqueue(new Person("Николай", 28));

        // Create threads for each bathroom
        Thread bathroomThread1 = new Thread(() => SimulateBathroom(bathroom1, peopleQueue));
        Thread bathroomThread2 = new Thread(() => SimulateBathroom(bathroom2, peopleQueue));

        // Start bathroom threads
        bathroomThread1.Start();
        bathroomThread2.Start();

        // Wait for the bathroom threads to finish (after serving all people)
        bathroomThread1.Join();
        bathroomThread2.Join();

        Console.WriteLine("Симулацията завърши успешно!");
    }

    static void SimulateBathroom(Bathroom bathroom, ConcurrentQueue<Person> queue)
    {
        while (!queue.IsEmpty)
        {
            if (queue.TryDequeue(out Person person))
            {
                Console.WriteLine($"{person.Name} чака да използва {bathroom.Id}.");
                bathroom.Enter(person); // Attempt to enter the bathroom
                Thread.Sleep(new Random().Next(1000, 3000)); // Simulate time in the bathroom
                bathroom.Leave(person); // Leave the bathroom
            }
        }
    }
}

class Person
{
    public string Name { get; private set; }
    public int Age { get; private set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void GoToToilet(Bathroom bathroom)
    {
        Console.WriteLine($"{Name} влезе в {bathroom.Id}.");
    }

    public void LeaveToilet(Bathroom bathroom)
    {
        Console.WriteLine($"{Name} излезе от {bathroom.Id}.");
    }
}

class SmartLock
{
    private bool isLocked;
    private readonly object lockObject = new object();

    public bool IsLocked
    {
        get
        {
            lock (lockObject)
            {
                return isLocked;
            }
        }
    }

    public bool Open()
    {
        lock (lockObject)
        {
            if (!isLocked)
            {
                isLocked = true;
                return true;
            }
            return false;
        }
    }

    public void Unlock()
    {
        lock (lockObject)
        {
            isLocked = false;
            Monitor.Pulse(lockObject); // Notify waiting threads
        }
    }

    public void WaitUntilAvailable()
    {
        lock (lockObject)
        {
            while (isLocked)
            {
                Monitor.Wait(lockObject); // Wait until the lock is released
            }
        }
    }
}

class Bathroom
{
    public SmartLock SmartLock { get; private set; }
    public string Id { get; private set; }

    public Bathroom(SmartLock smartLock, string id)
    {
        SmartLock = smartLock;
        Id = id;
    }

    public void Enter(Person person)
    {
        lock (SmartLock)
        {
            while (SmartLock.IsLocked)
            {
                Console.WriteLine($"{person.Name} чака за {Id}.");
                SmartLock.WaitUntilAvailable();
            }

            if (SmartLock.Open())
            {
                person.GoToToilet(this);
                Console.WriteLine($"{Id} е заета от {person.Name}.");
            }
        }
    }

    public void Leave(Person person)
    {
        lock (SmartLock)
        {
            SmartLock.Unlock();
            person.LeaveToilet(this);
            Console.WriteLine($"{Id} вече е свободна.");
        }
    }
}
