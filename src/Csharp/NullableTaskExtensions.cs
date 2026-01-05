namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension(Task? task)
    {
        public Task As() => task ?? Task.CompletedTask;
    }

    extension(ValueTask? task)
    {
        public ValueTask As() => task ?? ValueTask.CompletedTask;
    }

    extension<T>(Task<T>? task) where T : struct
    {
        public async ValueTask<T?> AsNullable() => task is not null ? await task : null;
    }

    extension<T>(Task<T>? task) where T : class
    {
        public async ValueTask<T?> As() => task is not null ? await task : null;
    }

    extension<T>(ValueTask<T>? task) where T : struct
    {
        public async ValueTask<T?> AsNullable() => task.HasValue ? await task.Value : null;
    }

    extension<T>(ValueTask<T>? task) where T : class
    {
        public async ValueTask<T?> As() => task.HasValue ? await task.Value : null;
    }
}
