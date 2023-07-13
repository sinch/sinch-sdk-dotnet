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
    }
}
