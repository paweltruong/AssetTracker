using System.ComponentModel;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => _execute((T)parameter);

        public event EventHandler? CanExecuteChanged;
    }

    public interface IAsyncRelayCommand : ICommand
    {
        Task ExecuteAsync();
        void RaiseCanExecuteChanged();
    }

    public class AsyncRelayCommand : IAsyncRelayCommand, INotifyPropertyChanged
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExecuting)));
                    RaiseCanExecuteChanged();
                }
            }
        }

        public bool CanExecute(object? parameter) =>
            !IsExecuting && (_canExecute?.Invoke() ?? true);

        public async void Execute(object? parameter) => await ExecuteAsync();

        public async Task ExecuteAsync()
        {
            if (CanExecute(null))
            {
                try
                {
                    IsExecuting = true;
                    await _execute();
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }

        public event EventHandler? CanExecuteChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaiseCanExecuteChanged() =>
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
