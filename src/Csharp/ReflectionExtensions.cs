using System.Reflection;

namespace Utilities;

public static partial class UtilitiesExtensions
{
    /// <param name="obj">The object to search within.</param>
    extension(object obj)
    {
        /// <summary>
        /// Recursively finds objects of type <typeparamref name="TReturn"/>, where all parent and child objects must be of type <typeparamref name="TRestriction"/>.
        /// </summary>
        /// <typeparam name="TRestriction">The restriction type: all parent and child objects must be of this type.</typeparam>
        /// <typeparam name="TReturn">The type to find.</typeparam>
        /// <param name="visited">A set of already visited objects to prevent infinite loops.</param>
        /// <returns>An enumerable of found objects of type <typeparamref name="TReturn"/>.</returns>
        public IEnumerable<TReturn> FindObjects<TRestriction, TReturn>(HashSet<object>? visited = null)
        {
            visited ??= [];

            // Must be TRestriction and not already visited
            if(obj is not TRestriction || visited.Contains(obj))
            {
                yield break;
            }

            // Mark as visited
            visited.Add(obj);

            // Check if it is TReturn
            if(obj is TReturn direct)
            {
                yield return direct;
            }

            // Iterate through properties that are readable
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            foreach(var prop in properties)
            {
                // Get property value
                object? propValue = prop.GetValue(obj);

                // Skip if property is null
                if(propValue is null)
                {
                    continue;
                }

                // Check if property is IEnumerable (excluding string for performance)
                if(propValue is System.Collections.IEnumerable enumerable and not string)
                {
                    foreach(var ele in enumerable)
                    {
                        // If element is not TRestriction, skip the collection (assume all elements should inherit from TRestriction for performance)
                        if(ele is not TRestriction)
                        {
                            break;
                        }

                        // Recursively check elements in the collection
                        foreach(var nest in FindObjects<TRestriction, TReturn>(ele, visited))
                        {
                            yield return nest;
                        }
                    }
                }
                else
                {
                    // Recursively check the property
                    foreach(var nest in FindObjects<TRestriction, TReturn>(propValue, visited))
                    {
                        yield return nest;
                    }
                }
            }
        }
    }

    /// <param name="objects">The collection of objects to search within.</param>
    extension(IEnumerable<object> objects)
    {
        /// <summary>
        /// Finds objects of type <typeparamref name="TReturn"/>, where all parent and child objects must be of type <typeparamref name="TRestriction"/>.
        /// </summary>
        /// <typeparam name="TRestriction">The restriction type: all parent and child objects must be of this type.</typeparam>
        /// <typeparam name="TReturn">The type to find.</typeparam>
        /// <param name="visited">A set of already visited objects to prevent infinite loops.</param>
        /// <returns>An enumerable of found objects of type <typeparamref name="TReturn"/>.</returns>
        public IEnumerable<TReturn> FindObjects<TRestriction, TReturn>(HashSet<object>? visited = null)
        {
            visited ??= [];
            return objects.SelectMany(obj => FindObjects<TRestriction, TReturn>(obj, visited));
        }
    }

    /// <param name="assembly">The assembly to search.</param>
    extension(Assembly assembly)
    {
        /// <summary>
        /// Gets all types in the assembly that are decorated with the specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to search for.</typeparam>
        /// <returns>An enumerable of types that have the specified attribute.</returns>
        public IEnumerable<Type> GetTypeWithAttribute<TAttribute>() where TAttribute : Attribute =>
            assembly.GetTypes().Where(t => t.GetCustomAttributes<TAttribute>().Any());
    }
}
