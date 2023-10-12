using System;

namespace Sinch.Core
{
    public class AnyOf<T1, T2, T3, T4>
    {
        public Type ActualType { get; }
        
        public T1 First { get; }
        
        public T2 Second { get; }
        
        public T3 Third { get; }
        
        public T4 Fourth { get; }
    }
}
