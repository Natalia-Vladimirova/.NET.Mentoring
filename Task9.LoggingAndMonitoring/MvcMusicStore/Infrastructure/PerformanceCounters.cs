using System.Diagnostics;
using PerformanceCounterHelper;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory("MvcMusicStore Counters", PerformanceCounterCategoryType.SingleInstance, "")]
    public enum PerformanceCounters
    {
        [PerformanceCounter("Log in", "", PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogIn,

        [PerformanceCounter("Log off", "", PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogOff,

        [PerformanceCounter("Checkout", "", PerformanceCounterType.NumberOfItems32)]
        SuccessfulCheckout
    }
}