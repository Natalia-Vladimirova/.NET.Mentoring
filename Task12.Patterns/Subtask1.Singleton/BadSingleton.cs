namespace Subtask1.Singleton
{
    // The disadvantage - Not thread-safe: more than one instance can be created at the same time
    public class BadSingleton
    {
        private static BadSingleton _instance;

        private BadSingleton() { }

        public static BadSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BadSingleton();
            }

            return _instance;
        }
    }
}
