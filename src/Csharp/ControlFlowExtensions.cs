namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension<T>(T val)
    {
        public T If(bool condition, Func<T, T> match) =>
            condition ? match(val) : val;

        public T If(bool condition, Func<T, T> match, Func<T, T> unmatch) =>
            condition ? match(val) : unmatch(val);

        public TResult If<TResult>(bool condition, Func<T, TResult> match, Func<T, TResult> unmatch) =>
            condition ? match(val) : unmatch(val);

        public T If(bool condition, Action<T> match)
        {
            if(condition)
                match(val);

            return val;
        }

        public T If(bool condition, Action<T> match, Action<T> unmatch)
        {
            if(condition)
                match(val);
            else
                unmatch(val);

            return val;
        }


        public T While(Func<T, bool> condition, Func<T, T> action)
        {
            while(condition(val))
            {
                val = action(val);
            }
            return val;
        }

        public T While(Func<T, bool> condition, Action<T> action)
        {
            while(condition(val))
            {
                action(val);
            }
            return val;
        }

        public T DoWhile(Func<T, bool> condition, Func<T, T> action)
        {
            do
            {
                val = action(val);
            } while(condition(val));

            return val;
        }

        public T DoWhile(Func<T, bool> condition, Action<T> action)
        {
            do
            {
                action(val);
            } while(condition(val));

            return val;
        }
    }
}
