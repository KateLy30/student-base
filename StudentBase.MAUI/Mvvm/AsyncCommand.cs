using System.Windows.Input;

namespace StudentBase.MAUI.Mvvm
{
    public class AsyncCommand : ICommand
    {
        // Делегат, который содержит асинхронную логику команды.
        // При выполнении команды мы вызываем этот Func и ожидаем Task.
        private readonly Func<object?, Task> _execute;

        // Опциональный делегат, который определяет, может ли команда выполняться.
        // Если null — считается, что команда всегда может выполняться (если не выполняется уже).
        private readonly Func<object?, bool>? _canExecute;

        // Флаг, указывающий, выполняется ли команда в данный момент.
        // Используется, чтобы предотвратить повторный запуск одновременно.
        private bool _isExecuting;

        // Событие ICommand, которое уведомляет UI, что состояние возможности выполнения изменилось.
        public event EventHandler? CanExecuteChanged;

        // Конструктор для асинхронного делегата, принимающего параметр.
        // Передаёт внешний canExecute напрямую.
        public AsyncCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Удобный перегруженный конструктор для делегата без параметра.
        // Внутри оборачиваем Func<Task> в Func<object?, Task>, а canExecute в Func<object?, bool>.
        public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = _ => execute();
            if (canExecute != null)
                _canExecute = _ => canExecute();
        }

        // Реализация ICommand.CanExecute
        // Команда доступна тогда, когда она не выполняется (_isExecuting == false)
        // и при этом пользовательский _canExecute (если задан) возвращает true.
        public bool CanExecute(object? parameter)
            => !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);


        // начинаем выполнение асинхронной операции и корректно обрабатываем состояние _isExecuting,
        // а также вызываем CanExecuteChanged до и после выполнения, чтобы UI обновил состояние кнопок и т.п.
        public async void Execute(object? parameter)
        {
            // Если команда сейчас не может выполняться — выходим.
            if (!CanExecute(parameter))
                return;
            try
            {
                // Помечаем, что команда выполняется и уведомляем об изменении.
                _isExecuting = true;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                // Выполняем асинхронное действие и ждём его завершения.
                await _execute(parameter);
            }
            finally
            {
                // В любом случае снимаем флаг выполнения и уведомляем UI.
                _isExecuting = false;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // Метод для ручного уведомления внешних подписчиков, что состояние CanExecute изменилось.
        // Полезно вызывать, если условие canExecute зависит от внешних данных.
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
