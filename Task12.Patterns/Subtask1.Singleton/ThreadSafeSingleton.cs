namespace Subtask1.Singleton
{
    // Implementation - double-check locking
    // Advantages:
    // Thread-safe
    // Lazy

    // The volatile keyword indicates that a field might be modified by multiple threads that are executing at the same time.
    // Fields that are declared volatile are not subject to compiler optimizations that assume access by a single thread.
    public sealed class ThreadSafeSingleton
    {
        private static volatile ThreadSafeSingleton _instance;
        private static readonly object _syncRoot = new object();

        private ThreadSafeSingleton() { }

        public static ThreadSafeSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ThreadSafeSingleton();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
