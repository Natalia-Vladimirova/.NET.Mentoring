using PerformanceCounterHelper;

namespace MvcMusicStore.Infrastructure
{
    public class CounterInstance
    {
        public CounterHelper<PerformanceCounters> CounterHelper { get; }

        public CounterInstance()
        {
            CounterHelper = PerformanceHelper.CreateCounterHelper<PerformanceCounters>();
        }
    }
}