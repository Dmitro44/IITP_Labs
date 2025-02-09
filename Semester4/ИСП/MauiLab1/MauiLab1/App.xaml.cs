namespace MauiLab1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 400;
        const int newHeight = 540;
        
        window.Width = window.MinimumWidth = newWidth;
        window.Height = window.MinimumHeight = newHeight;

        return window;
    }
}