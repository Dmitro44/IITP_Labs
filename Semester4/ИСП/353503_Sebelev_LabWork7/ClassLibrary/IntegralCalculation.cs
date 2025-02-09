using System.Diagnostics;

namespace ClassLibrary;

public class IntegralCalculation(int maxThreadsCount)
{
    public event Action<TimeSpan>? OnExecutionTimeMeasured;
    public event Action<int>? ProgressChanged;
    
    private readonly Semaphore _semaphore = new (maxThreadsCount, maxThreadsCount);
    
    private const double H = 0.00000001;
    private const int A = 0;
    private const int B = 1;
    
    private readonly object _locker = new();
    
    
    public void LeftRectangle()
    {
        _semaphore.WaitOne();
        Stopwatch stopwatch = new Stopwatch();
        
        double sum = 0.0;

        stopwatch.Start();
        
        for (double i = A; i < B; i += H)
        {
            sum += H * Math.Sin(i);
        
        
            Latency();
        
            if ((int)(i * 1e8) % 1e6 == 0)
            {
                int progressPercentage = (int)(i * 100);
                ProgressChanged?.Invoke(progressPercentage);
            }
        }

        ProgressChanged?.Invoke(100);

        stopwatch.Stop();
        OnExecutionTimeMeasured?.Invoke(stopwatch.Elapsed);

        Console.WriteLine($"Calculated result: {sum}");
        _semaphore.Release();
    }

    public void Latency()
    {
        int temp = 0;
        for (int i = 0; i < 100; ++i)
        {
            temp = i * i;
        }
    }
}