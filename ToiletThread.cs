using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

//решение използващо клас Thread

class ToiletThread
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== Интелигентна заключваща система за тоалетни ===");

        // Create bathrooms
        SmartLock lock1 = new SmartLock();
        SmartLock lock2 = new SmartLock();

        Bathroom bathroom1 = new Bathroom(lock1, "Тоалетна 1");
        Bathroom bathroom2 = new Bathroom(lock2, "Тоалетна 2");

        // Create shared queue of people
        var peopleQueue = new List<Person>
        {
            new Person("Иван", 25),
            new Person("Мария", 30),
            new Person("Георги", 35),
            new Person("Елена", 40),
            new Person("Петър", 20),
            new Person("Николай", 28)
        };

        // Threads for each bathroom
        Thread bathroom1Thread = new Thread(() => BathroomWorker(bathroom1, peopleQueue));
        Thread bathroom2Thread = new Thread(() => BathroomWorker(bathroom2, peopleQueue));

        // Start threads
        bathroom1Thread.Start();
        bathroom2Thread.Start();

        // Wait for threads to finish
        bathroom1Thread.Join();
        bathroom2Thread.Join();

        Console.WriteLine("Симулацията завърши успешно!");
    }

    static void BathroomWorker(Bathroom bathroom, List<Person> peopleQueue)
    {
        while (true)
        {
            Person person = null;

            lock (peopleQueue)
            {
                if (peopleQueue.Count > 0)
                {
                    person = peopleQueue[0];
                    peopleQueue.RemoveAt(0);
                }
                else
                {
                    break; // Exit loop if no one is in the queue
                }
            }

            Console.WriteLine($"{person.Name} чака да използва {bathroom.Id}.");

            // Try to enter the bathroom
            while (!bathroom.Enter(person))
            {
                Console.WriteLine($"{person.Name} чака {bathroom.Id} да се освободи...");
                Thread.Sleep(500); // Retry after a short delay
            }

            // Simulate time spent in the bathroom
            Thread.Sleep(new Random().Next(1000, 3000));

            // Leave the bathroom
            bathroom.Leave(person);
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
    private bool isLocked = false;

    public bool IsLocked => isLocked;

    public bool Open()
    {
        lock (this)
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
        lock (this)
        {
            isLocked = false;
        }
    }
}

class Bathroom
{
    private SmartLock smartLock;
    public string Id { get; private set; }

    public Bathroom(SmartLock smartLock, string id)
    {
        this.smartLock = smartLock;
        Id = id;
    }

    public bool Enter(Person person)
    {
        if (smartLock.Open())
        {
            person.GoToToilet(this);
            Console.WriteLine($"{Id} е заета от {person.Name}.");
            return true;
        }
        return false;
    }

    public void Leave(Person person)
    {
        smartLock.Unlock();
        person.LeaveToilet(this);
        Console.WriteLine($"{Id} вече е свободна.");
    }
}
