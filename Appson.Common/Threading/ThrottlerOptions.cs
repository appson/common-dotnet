namespace Appson.Common.Threading
{
    public class ThrottlerOptions
    {
        public int DelayMillis { get; set; } = 100;

        public float RatePerSecond { get; set; } = 20;

        public int MaxBurstSize { get; set; } = 10;

        public int InitialQuota { get; set; } = 0;
    }
}