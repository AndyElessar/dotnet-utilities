namespace Utilities;

// Extension methods for System.Threading.Tasks.Task and System.Threading.Tasks.ValueTask<br/>
// From <see href="https://github.com/brminnick/AsyncAwaitBestPractices"/>, licensed under MIT License.
public static partial class UtilitiesExtensions
{
    private static Action<Exception>? _onException;
    private static bool _shouldAlwaysRethrowException;

    /// <param name="task">Task.</param>
    extension(Task task)
    {
        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="configureAwaitOptions">Options to control behavior when awaiting</param>
        public void SafeFireAndForget(in ConfigureAwaitOptions configureAwaitOptions, in Action<Exception>? onException = null) =>
            HandleSafeFireAndForget(task, configureAwaitOptions, onException);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="configureAwaitOptions">Options to control behavior when awaiting</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(in ConfigureAwaitOptions configureAwaitOptions, in Action<TException>? onException = null) where TException : Exception =>
            HandleSafeFireAndForget(task, configureAwaitOptions, onException);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(in Action<Exception>? onException = null, in bool continueOnCapturedContext = false) =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(in Action<TException>? onException = null, in bool continueOnCapturedContext = false) where TException : Exception =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(Action<Exception>? onException, bool continueOnCapturedContext = false) =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(Action<TException>? onException, bool continueOnCapturedContext = false) where TException : Exception =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="configureAwaitOptions">Options to control behavior when awaiting</param>
        public void SafeFireAndForget(ConfigureAwaitOptions configureAwaitOptions, Action<Exception>? onException = null) =>
            task.SafeFireAndForget(in configureAwaitOptions, in onException);

        /// <summary>
        /// Safely execute the Task without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="configureAwaitOptions">Options to control behavior when awaiting</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(ConfigureAwaitOptions configureAwaitOptions, Action<TException>? onException = null) where TException : Exception =>
            task.SafeFireAndForget(in configureAwaitOptions, in onException);
    }

    /// <param name="task">ValueTask.</param>
    extension(ValueTask task)
    {
        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(in Action<Exception>? onException = null, in bool continueOnCapturedContext = false) =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(in Action<TException>? onException = null, in bool continueOnCapturedContext = false) where TException : Exception =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(Action<Exception>? onException, bool continueOnCapturedContext = false) =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(Action<TException>? onException, bool continueOnCapturedContext = false) where TException : Exception =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);
    }

    /// <typeparam name="T">The return value of the ValueTask.</typeparam>
    /// <param name="task">ValueTask.</param>
    extension<T>(ValueTask<T> task)
    {
        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(in Action<Exception>? onException = null, in bool continueOnCapturedContext = false) =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(in Action<TException>? onException = null, in bool continueOnCapturedContext = false) where TException : Exception =>
            HandleSafeFireAndForget(task, continueOnCapturedContext, onException);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the ValueTask, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        public void SafeFireAndForget(Action<Exception>? onException, bool continueOnCapturedContext = false) =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);

        /// <summary>
        /// Safely execute the ValueTask without waiting for it to complete before moving to the next line of code; commonly known as "Fire And Forget". Inspired by John Thiriet's blog post, "Removing Async Void": https://johnthiriet.com/removing-async-void/.
        /// </summary>
        /// <param name="onException">If an exception is thrown in the Task, <c>onException</c> will execute. If onException is null, the exception will be re-thrown</param>
        /// <param name="continueOnCapturedContext">If set to <c>true</c>, continue on captured context; this will ensure that the Synchronization Context returns to the calling thread. If set to <c>false</c>, continue on a different context; this will allow the Synchronization Context to continue on a different thread</param>
        /// <typeparam name="TException">Exception type. If an exception is thrown of a different type, it will not be handled</typeparam>
        public void SafeFireAndForget<TException>(Action<TException>? onException, bool continueOnCapturedContext = false) where TException : Exception =>
            task.SafeFireAndForget(in onException, in continueOnCapturedContext);
    }

    /// <summary>
    /// Initialize SafeFireAndForget
    ///
    /// Warning: When <c>true</c>, there is no way to catch this exception, and it will always result in a crash. Recommended only for debugging purposes.
    /// </summary>
    /// <param name="shouldAlwaysRethrowException">If set to <c>true</c>, after the exception has been caught and handled, the exception will always be rethrown.</param>
    public static void Initialize(in bool shouldAlwaysRethrowException = false) => _shouldAlwaysRethrowException = shouldAlwaysRethrowException;

    /// <summary>
    /// Initialize SafeFireAndForget
    ///
    /// Warning: When <c>true</c>, there is no way to catch this exception, and it will always result in a crash. Recommended only for debugging purposes.
    /// </summary>
    /// <param name="shouldAlwaysRethrowException">If set to <c>true</c>, after the exception has been caught and handled, the exception will always be rethrown.</param>
    public static void Initialize(bool shouldAlwaysRethrowException) => Initialize(in shouldAlwaysRethrowException);

    /// <summary>
    /// Remove the default action for SafeFireAndForget
    /// </summary>
    public static void RemoveDefaultExceptionHandling() => _onException = null;

    /// <summary>
    /// Set the default action for SafeFireAndForget to handle every exception
    /// </summary>
    /// <param name="onException">If an exception is thrown in the Task using SafeFireAndForget, <c>onException</c> will execute</param>
    public static void SetDefaultExceptionHandling(in Action<Exception> onException)
    {
        ArgumentNullException.ThrowIfNull(onException);

        _onException = onException;
    }

    /// <summary>
    /// Set the default action for SafeFireAndForget to handle every exception
    /// </summary>
    /// <param name="onException">If an exception is thrown in the Task using SafeFireAndForget, <c>onException</c> will execute</param>
    public static void SetDefaultExceptionHandling(Action<Exception> onException) => SetDefaultExceptionHandling(in onException);

    private static async void HandleSafeFireAndForget<TException>(ValueTask valueTask, bool continueOnCapturedContext, Action<TException>? onException) where TException : Exception
    {
        try
        {
            await valueTask.ConfigureAwait(continueOnCapturedContext);
        }
        catch(TException ex) when(_onException is not null || onException is not null)
        {
            HandleException(ex, onException);

            if(_shouldAlwaysRethrowException)
                throw;
        }
    }

    private static async void HandleSafeFireAndForget<T, TException>(ValueTask<T> valueTask, bool continueOnCapturedContext, Action<TException>? onException) where TException : Exception
    {
        try
        {
            await valueTask.ConfigureAwait(continueOnCapturedContext);
        }
        catch(TException ex) when(_onException is not null || onException is not null)
        {
            HandleException(ex, onException);

            if(_shouldAlwaysRethrowException)
                throw;
        }
    }

    private static async void HandleSafeFireAndForget<TException>(Task task, bool continueOnCapturedContext, Action<TException>? onException) where TException : Exception
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch(TException ex) when(_onException is not null || onException is not null)
        {
            HandleException(ex, onException);

            if(_shouldAlwaysRethrowException)
                throw;
        }
    }

    private static async void HandleSafeFireAndForget<TException>(Task task, ConfigureAwaitOptions configureAwaitOptions, Action<TException>? onException) where TException : Exception
    {
        try
        {
            await task.ConfigureAwait(configureAwaitOptions);
        }
        catch(TException ex) when(_onException is not null || onException is not null)
        {
            HandleException(ex, onException);

            if(_shouldAlwaysRethrowException)
                throw;
        }
    }

    private static void HandleException<TException>(in TException exception, in Action<TException>? onException) where TException : Exception
    {
        if(exception is OperationCanceledException or TaskCanceledException)
            return;

        _onException?.Invoke(exception);
        onException?.Invoke(exception);
    }
}
