using System.Globalization;

namespace MauiLab1.Pages.Calculator;

public partial class Calculator : ContentPage
{
    private double? _firstOperand;
    private double? _lastSecondOperand;
    private string _currentInput = "0";
    private string _expressionInput = "";
    private string? _currentOperator;
    private bool _isNewInput;
    private double _memory;
    
    
    public Calculator()
    {
        InitializeComponent();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        ResultLabel.Text = _currentInput;
    }

    private void UpdateExpressionDisplay()
    {
        ExpressionLabel.Text = _expressionInput;
    }

    private void OnNumberButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string number = button.Text;

        if (_isNewInput)
        {
            _currentInput = number;
            _isNewInput = false;
        }
        else
        {
            _currentInput = (_currentInput == "0") ? number : _currentInput + number;
        }
        UpdateDisplay();
    }

    private void OnMemoryClearButtonClicked(object sender, EventArgs e)
    {
        _memory = 0;
        _expressionInput = "";
        UpdateExpressionDisplay();
    }

    private void OnMemoryReadButtonClicked(object sender, EventArgs e)
    {
        _currentInput = _memory.ToString(CultureInfo.InvariantCulture);
        _expressionInput = "";
        _firstOperand = _memory;
        UpdateDisplay();
        UpdateExpressionDisplay();
    }

    private void OnMemoryAddButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _memory += result;
        }
    }

    private void OnMemorySubButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _memory -= result;
        }
    }

    private void OnMemorySaveButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _memory = result;
        }
    }

    private async void OnMemoryButtonClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Memory", $"Stored value: {_memory.ToString(CultureInfo.InvariantCulture)}", "OK");
    }

    private void OnPercentButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            if (_currentOperator == "+" || _currentOperator == "-")
            {
                if (_firstOperand != null) result = _firstOperand.Value * (result / 100);
                _currentInput = result.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                result /= 100;
                _currentInput = result.ToString(CultureInfo.CurrentCulture);
            }
            UpdateDisplay();
        }
    }

    private void OnClearButtonClicked(object sender, EventArgs e)
    {
        Button clearButton = (Button)sender;
        string operation = clearButton.Text;
        
        switch (operation)
        {
            case "C":
                _currentInput = "0";
                _expressionInput = "";
                _firstOperand = null;
                _currentOperator = null;
                UpdateExpressionDisplay();
                break;
            case "CE":
                _currentInput = "0";
                break;
        }
        
        UpdateDisplay();
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (!_isNewInput && _currentInput.Length > 0)
        {
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            if (_currentInput == "")
            {
                _currentInput = "0";
            }
            UpdateDisplay();
        }
    }

    private void OnDivByOneButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _expressionInput = $"1/({_currentInput}) =";
            UpdateExpressionDisplay();
            
            result = Math.Round(1 / result, 10);
            _currentInput = result.ToString(CultureInfo.CurrentCulture);
            UpdateDisplay();
            
            _isNewInput = true;
        }
    }

    private void OnSquaringButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _expressionInput = $"sqr({_currentInput}) =";
            UpdateExpressionDisplay();
            
            result = Math.Round(Math.Pow(result, 2), 6);
            _currentInput = result.ToString(CultureInfo.CurrentCulture);
            UpdateDisplay();
            
            _isNewInput = true;
        }
    }

    private void OnSquareRootButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            if (result >= 0)
            {
                _expressionInput = $"√({_currentInput}) =";
                UpdateExpressionDisplay();
                
                result = Math.Round(Math.Sqrt(result), 10);
                _currentInput = result.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                _currentInput = "Err";
            }
            UpdateDisplay();
            
            _isNewInput = true;
        }
    }

    private void OnOperationButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string operation = button.Text;

        if (ResultLabel.Text != _firstOperand.ToString())
        {
            OnEqualButtonClicked(sender, e);
        }
        
        _firstOperand = double.Parse(_currentInput);

        _currentOperator = operation;
        _expressionInput = _firstOperand + _currentOperator;
        _lastSecondOperand = null;
        UpdateExpressionDisplay();
        _isNewInput = true;
    }

    private void OnPlusMinusButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double number))
        {
            number = -number;
            _currentInput = number.ToString(CultureInfo.CurrentCulture);
            
            if (_isNewInput)
            {
                _firstOperand = number;
            }
        }
        
        UpdateExpressionDisplay();
        UpdateDisplay();
    }

    private void OnCommaButtonClicked(object sender, EventArgs e)
    {
        if (_currentInput.Contains(',')) return;
        _currentInput += ',';
        UpdateDisplay();
    }

    private void OnEqualButtonClicked(object sender, EventArgs e)
    {
        if (_firstOperand == null || _currentOperator == null)
            return;

        double secondOperand;

        if (!_isNewInput)
        {
            if (_lastSecondOperand.HasValue)
            {
                _firstOperand = double.Parse(_currentInput);
                secondOperand = _lastSecondOperand.Value;
            }
            else
            {
                secondOperand = double.Parse(_currentInput);
                _lastSecondOperand = secondOperand;
            }
        }
        else
        {
            secondOperand = _lastSecondOperand.HasValue ? _lastSecondOperand.Value : double.Parse(_currentInput);
        }
        
        double result = _currentOperator switch
        {
            "+" => _firstOperand.Value + secondOperand,
            "-" => _firstOperand.Value - secondOperand,
            "\u00d7" => _firstOperand.Value * secondOperand,
            "\u00f7" => secondOperand != 0 ? _firstOperand.Value / secondOperand : double.NaN,
            _ => secondOperand
        };
        
        _currentInput = result.ToString(CultureInfo.CurrentCulture);
        
        if (_expressionInput == "")
        {
            _expressionInput = _firstOperand + _currentOperator + secondOperand + "=";
        }
        else
        {
            _expressionInput += secondOperand + "=";
        }
        
        _firstOperand = result;
        _isNewInput = true;
        
        UpdateExpressionDisplay();
        UpdateDisplay();

        _expressionInput = "";
    }

    private void OnExponentButtonClicked(object sender, EventArgs e)
    {
        if (double.TryParse(_currentInput, out double result))
        {
            _expressionInput = $"e^({_currentInput}) =";
            UpdateExpressionDisplay();
            
            result = Math.Exp(result);
            _currentInput = result.ToString(CultureInfo.CurrentCulture);
            UpdateDisplay();
            
            _isNewInput = true;
        }
    }
}