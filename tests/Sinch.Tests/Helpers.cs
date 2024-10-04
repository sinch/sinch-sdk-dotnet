using System;
using System.Globalization;
using System.Reflection;

namespace Sinch.Tests
{
    internal static class Helpers
    {
        public static T GetPrivateProperty<T, TInstance>(TInstance instance, string propName)
        {
            var prop = typeof(TInstance).GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Instance);
            var getter = prop!.GetGetMethod(true);
            return (T)getter!.Invoke(instance, null)!;
        }

        public static T GetPrivateField<T, TInstance>(TInstance instance, string fieldName)
        {
            var field = instance!.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field!.GetValue(instance)!;
        }
        
        // Unused, but can be useful in future for tricky testing
        public static T InvokePrivateMethod<T, TInstance>(TInstance instance, string methodName, object[] parameters)
        {
            MethodInfo dynMethod = instance.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (dynMethod == null)
            {
                throw new InvalidOperationException($"{methodName} is not found on a type ${instance.GetType().Name}");
            }

            var returns = dynMethod.Invoke(instance, parameters);
            return (T)returns;
        }

        public static DateTime ParseUtc(string time)
        {
            return DateTime.Parse(time, CultureInfo.InvariantCulture).ToUniversalTime();
        }
    }
}
