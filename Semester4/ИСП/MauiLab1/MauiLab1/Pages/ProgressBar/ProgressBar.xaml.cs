using System.Diagnostics;
using Microsoft.Maui.Handlers;

namespace MauiLab1.Pages.ProgressBar;

public partial class ProgressBar : ContentPage
{
    private const double H = 0.00000001;
    private const int A = 0;
    private const int B = 1;
    
    private CancellationTokenSource _cts;
    
    public ProgressBar()
    {
        InitializeComponent();
    }

    private void UpdateLabel(string info)
    {
        InfoLabel.Text = info;
    }
    
    public double LeftRectangle(IProgress<double> progress, CancellationToken ct)
    {
        double totalIterations = (B - A) / H;
        double iterationCount = 0;
        
        Stopwatch stopwatch = new Stopwatch();
        
        double sum = 0.0;

        stopwatch.Start();
        
        for (double i = A; i < B; i += H)
        {
            if (ct.IsCancellationRequested)
            {
                return 0;
            }
            
            sum += H * Math.Sin(i);
            iterationCount++;
        
            Latency();
        
            if (iterationCount % 1e6 == 0)
            {
                double progressPercentage = (iterationCount / totalIterations) * 100;
                progress.Report(progressPercentage);
            }
        }

        progress.Report(100);
        
        stopwatch.Stop();

        return sum;
    }

    public void Latency()
    {
        for (int i = 0; i < 100; ++i)
        {
            _ = i * i;
        }
    }

    public void UpdatePercentage(double percentage)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ProgBar.Progress = percentage / 100;
            PercentageLabel.Text = $"{percentage:F2}%";
        });
    }

    private async void OnStartButtonClicked(object? sender, EventArgs e)
    {
        StartButton.IsEnabled = false;
        CancelButton.IsEnabled = true;

        ProgBar.Progress = 0;
        PercentageLabel.Text = "0%";
        
        UpdateLabel("Calculating expression...");
        
        _cts = new CancellationTokenSource();
        var progress = new Progress<double>(UpdatePercentage);

        try
        {
            var result = await Task.Run(() => LeftRectangle(progress, _cts.Token), _cts.Token);

            if (!_cts.IsCancellationRequested)
            {
                UpdateLabel($"Calculated result: {result}");
            }
        }
        catch (OperationCanceledException)
        {
            UpdateLabel("Calculating expression cancelled");
        }
        finally
        {
            StartButton.IsEnabled = true;
            CancelButton.IsEnabled = false;
        }
    }

    private void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        _cts.Cancel();
        CancelButton.IsEnabled = false;
        UpdateLabel("Calculating expression cancelled");
    }
}