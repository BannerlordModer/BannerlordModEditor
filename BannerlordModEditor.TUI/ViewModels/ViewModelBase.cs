using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BannerlordModEditor.TUI.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class Command : IDisposable
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private EventHandler? _canExecuteChanged;

        public Command(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public Command(Action execute, Func<bool>? canExecute = null)
        {
            _execute = () => Task.Run(execute);
            _canExecute = canExecute;
        }

        public bool CanExecute()
        {
            return _canExecute?.Invoke() ?? true;
        }

        public async Task ExecuteAsync()
        {
            if (CanExecute())
            {
                await _execute();
            }
        }

        public void Execute()
        {
            if (CanExecute())
            {
                _execute().GetAwaiter().GetResult();
            }
        }

        public void NotifyCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged
        {
            add { _canExecuteChanged += value; }
            remove { _canExecuteChanged -= value; }
        }

        public void Dispose()
        {
            // 清理事件处理器
            _canExecuteChanged = null;
        }
    }
}