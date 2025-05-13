using System.Collections.ObjectModel;
using MauiLab1.Entities;
using MauiLab1.Services;

namespace MauiLab1.Pages.CurrencyConverter;

public partial class CurrencyConverterPage : ContentPage
{
    private readonly IRateService _rateService;
    private ObservableCollection<Rate> _allRates = new();
    private ObservableCollection<Rate> _filteredRates = new();
    private Rate? _selectedRate;
    private bool _isFromBynToCurrency = true;
    
    private readonly string[] _currencyCodes = new[] { "RUB", "EUR", "USD", "CHF", "CNY", "GBP" };
    
    public DateTime CurrentDate { get; set; } = DateTime.Now;

    public CurrencyConverterPage(IRateService rateService)
    {
        InitializeComponent();
        _rateService = rateService;
        
        BindingContext = this;
        
        DateSelector.Date = CurrentDate;
        RatesCollection.ItemsSource = _filteredRates;
        
        LoadRatesAsync(CurrentDate);
    }

    private async void LoadRatesAsync(DateTime date)
    {
        try
        {
            LoadingIndicator.IsRunning = true;
            
            var rates = await _rateService.GetRates(date);
            
            _allRates.Clear();
            _filteredRates.Clear();

            foreach (var rate in rates)
            {
                _allRates.Add(rate);
            }
                
            foreach (var code in _currencyCodes)
            {
                var currency = _allRates.FirstOrDefault(r => r.Cur_Abbreviation == code);
                if (currency != null)
                {
                    _filteredRates.Add(currency);
                }
            }

            RatesCollection.ItemsSource = null;
            RatesCollection.ItemsSource = _filteredRates;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось загрузить курсы валют: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
        }
    }

    private void DateSelector_DateSelected(object sender, DateChangedEventArgs e)
    {
        LoadRatesAsync(e.NewDate);
    }

    private void RefreshButton_Clicked(object sender, EventArgs e)
    {
        LoadRatesAsync(DateSelector.Date);
    }

    private void RatesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            _selectedRate = e.CurrentSelection[0] as Rate;
            
            if (_selectedRate != null)
            {
                SelectedCurrencyLabel.Text = $"{_selectedRate.Cur_Name} ({_selectedRate.Cur_Abbreviation})";
                
                UpdateConversionLabels();
                CalculateConversion();
            }
        }
    }

    private void ConversionDirection_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _isFromBynToCurrency = sender == FromBynRadio;
            UpdateConversionLabels();
            CalculateConversion();
        }
    }

    private void UpdateConversionLabels()
    {
        if (_selectedRate == null) return;
        
        if (_isFromBynToCurrency)
        {
            FromCurrencyLabel.Text = "BYN";
            ToCurrencyLabel.Text = _selectedRate.Cur_Abbreviation;
        }
        else
        {
            FromCurrencyLabel.Text = _selectedRate.Cur_Abbreviation;
            ToCurrencyLabel.Text = "BYN";
        }
    }

    private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        CalculateConversion();
    }

    private void CalculateConversion()
    {
        if (_selectedRate == null || string.IsNullOrWhiteSpace(AmountEntry.Text))
        {
            ResultLabel.Text = "0.00";
            return;
        }

        if (!double.TryParse(AmountEntry.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Any, 
                System.Globalization.CultureInfo.InvariantCulture, out double amount))
        {
            ResultLabel.Text = "Ошибка";
            return;
        }

        double result;
        
        if (_isFromBynToCurrency)
        {
            // Конвертация из BYN в валюту
            result = amount / (double)_selectedRate.Cur_OfficialRate * _selectedRate.Cur_Scale;
            ResultLabel.Text = result.ToString("F4");
        }
        else
        {
            // Конвертация из валюты в BYN
            result = amount * (double)_selectedRate.Cur_OfficialRate / _selectedRate.Cur_Scale;
            ResultLabel.Text = result.ToString("F4");
        }
    }
}