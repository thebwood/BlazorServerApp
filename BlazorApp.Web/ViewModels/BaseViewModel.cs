using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BlazorApp.Web.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private bool _isLoading;
    private string? _errorMessage;
    private string? _successMessage;

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsLoading
    {
        get => _isLoading;
        protected set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        protected set
        {
            if (_errorMessage != value)
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public string? SuccessMessage
    {
        get => _successMessage;
        protected set
        {
            if (_successMessage != value)
            {
                _successMessage = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasSuccess => !string.IsNullOrEmpty(SuccessMessage);

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            OnPropertyChanged(propertyName);
        }
    }

    public void ClearMessages()
    {
        ErrorMessage = null;
        SuccessMessage = null;
    }

    public void SetError(string error)
    {
        ErrorMessage = error;
        SuccessMessage = null;
    }

    public void SetSuccess(string message)
    {
        SuccessMessage = message;
        ErrorMessage = null;
    }

    protected async Task ExecuteAsync(Func<Task> action, string? errorPrefix = null)
    {
        try
        {
            IsLoading = true;
            ClearMessages();
            await action();
        }
        catch (Exception ex)
        {
            var prefix = string.IsNullOrEmpty(errorPrefix) ? "An error occurred" : errorPrefix;
            SetError($"{prefix}: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected async Task<T?> ExecuteAsync<T>(Func<Task<T>> action, string? errorPrefix = null)
    {
        try
        {
            IsLoading = true;
            ClearMessages();
            return await action();
        }
        catch (Exception ex)
        {
            var prefix = string.IsNullOrEmpty(errorPrefix) ? "An error occurred" : errorPrefix;
            SetError($"{prefix}: {ex.Message}");
            return default;
        }
        finally
        {
            IsLoading = false;
        }
    }
}
