// See https://aka.ms/new-console-template for more information
using BuggyConsole;

Console.WriteLine("Hello from BuggyConsole !");

Console.WriteLine("Press the");
Console.WriteLine("1) Crash - Null reference exceptions.");
Console.WriteLine("2) Crash - GC Heap presssure, OOM Exceptions.");
Console.WriteLine("3) Crash - Stack overflow.");
Console.WriteLine("4) Crash - Dead Lock.");
Console.WriteLine("5) Crash - Threadpool OutOfThreads");


ConsoleKeyInfo keyReaded = Console.ReadKey();
Console.WriteLine();

switch (keyReaded.Key)
{
    case ConsoleKey.D1: 
        NullReferenceException();
        break;

    case ConsoleKey.D2:
        OutOfMemoryException();
        break;

    case ConsoleKey.D3:
        InfiniteRecurse();
        break;

    case ConsoleKey.D4:
        TaskAcquire().GetAwaiter().GetResult();
        break;

    case ConsoleKey.D5:
        ThreadPoolRace().GetAwaiter().GetResult();
        break;

    default: //Not known key pressed
        Console.WriteLine("Wrong key, please try again.");
        break;
}

Console.WriteLine("Hit any key to exit");
Console.ReadKey();

static void NullReferenceException()
{
    var f = new Foo();
    var name = f.Bar.Baz.Name;
}


static void OutOfMemoryException()
{
    List<Product> products = new List<Product>();
    string answer = "";
    do
    {
        for (int i = 0; i < 1_000; i++)
        {
            products.Add(new Product(i, "product" + i));
        }
        Console.WriteLine("Leak some more? Y/N");
        answer = Console.ReadLine()?.ToUpper() ?? "";

    } while (answer == "Y");
}


static void InfiniteRecurse(int start = 0)
{    
    InfiniteRecurse(++start);
}

static async Task TaskAcquire()
{ 
    List<Task> tasks = new List<Task>();

    count = 0;
    for (int i = 0; i < 5; i++)
    {
        tasks.Add(Task.Run(TaskAcquireOne));
        tasks.Add(Task.Run(TaskAcquireTwo));
    }

    await Task.WhenAll(tasks);
}

static async Task TaskAcquireOne()
{
    await Task.Run(() =>
    {
        int i = count++;
        lock (Seller)
        {
            lock (Order)
            {
                Thread.Sleep(1000);
            }
        }
    });
}

static async Task TaskAcquireTwo()
{
    await Task.Run(() =>
    {
        int i = count++;
        lock (Order)
        {
            lock (Seller)
            {
                Thread.Sleep(1000);
            }
        }
    });
}

static async Task ThreadPoolRace()
{
    ThreadPool.SetMaxThreads(10, 10);
    ThreadPool.SetMinThreads(1, 1);

    List<Task> tasks = new List<Task>();

    for (int i = 0; i < 1000; i++)
    {
        tasks.Add(Task.Run(() => TaskDoSomethings(i)));
    }

    await Task.WhenAll(tasks);
}

static void TaskDoSomethings(int idx)
{
    Task.Run(() =>
    {
        double value = 123456789;

        for (int y = 1; y < 1_000; y++)
        {
            for (int i = 1; i < 1_000_000; i++)
            {
                double tmp = value / i;
                tmp = value * i;
                tmp = Math.Sqrt(tmp);
            }
        }

        resetEvent.WaitOne();
    }).Wait();
}


static public partial class Program
{
    static volatile int count = 0;

    static object Seller = new();
    static object Order = new();
    static ManualResetEvent resetEvent = new ManualResetEvent(false);
};


