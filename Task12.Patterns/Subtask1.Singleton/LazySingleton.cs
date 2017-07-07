using System;

namespace Subtask1.Singleton
{
    // Implementation - using system class Lazy<T>
    // Advantages:
    // Simple
    // Thread-safe
    // Lazy
    public sealed class LazySingleton
    {
        private static readonly Lazy<LazySingleton> _instance = new Lazy<LazySingleton>(() => new LazySingleton());

        private LazySingleton() { }

        public static LazySingleton Instance => _instance.Value;
    }
}
