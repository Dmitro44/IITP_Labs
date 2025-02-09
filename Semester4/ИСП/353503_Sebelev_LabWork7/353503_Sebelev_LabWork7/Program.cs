using ClassLibrary;
using System.Threading;

namespace _353503_Sebelev_LabWork7;

public delegate double ThreadStart();

class Program
{
    static void Main(string[] args)
    {
        IntegralCalculation calc = new IntegralCalculation(3);

        calc.OnExecutionTimeMeasured += time =>
        {
            Console.WriteLine($"Program ran for {time.Ticks} ticks with {Thread.CurrentThread.Priority.ToString().ToUpper()} priority");
        };

        calc.ProgressChanged += OnProgressChanged;
        
        

        StartTwoThreads(calc);
        
        StartNThreads(calc, 5);
    }

    public static void StartNThreads(IntegralCalculation calc, int n)
    {
        for (int i = 0; i < n + 1; ++i)
        {
            Thread thread = new Thread(calc.LeftRectangle);
            thread.Start();
        }
    }

    public static void StartTwoThreads(IntegralCalculation calc)
    {
        Thread threadHi = new Thread(calc.LeftRectangle);
        
        Thread threadLo = new Thread(calc.LeftRectangle);

        threadHi.Priority = ThreadPriority.Highest;
        threadLo.Priority = ThreadPriority.Lowest;
        threadHi.Start();
        threadLo.Start();
    }

    static void OnProgressChanged(int progressPercentage)
    {
        string threadId = Thread.CurrentThread.ManagedThreadId.ToString();
        string progressBar = new string('=', progressPercentage) + ">";
        string spaces = new string(' ', 100 - progressPercentage);
        
        Console.WriteLine($"Поток {threadId}: [{progressBar + spaces}] {progressPercentage}%");
    }
}