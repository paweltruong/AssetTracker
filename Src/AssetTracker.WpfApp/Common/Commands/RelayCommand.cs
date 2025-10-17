using System.ComponentModel;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.Commands
{
    public interface IRelayCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }

    /// <summary>
    /// A command that relays its functionality to other objects by invoking delegates.
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }
    }

    /// <summary>
    /// Strongly typed version of RelayCommand
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : IRelayCommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object? parameter) => CanExecute((T)parameter);

        public void Execute(object? parameter) => Execute((T)parameter);

        public bool CanExecute(T parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(T parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public interface IAsyncRelayCommand : IRelayCommand
    {
        Task ExecuteAsync();
    }

    /// <summary>
    /// RelayCommand for asynchronous delegates
    /// </summary>
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
