namespace Appson.Common.Threading
{
    public class ThrottlerOptions
    {
        /// <summary>
        /// Specifies the delay in each iteration of the thread, effectively setting the resolution of the throttling. 
        /// </summary>
        /// <remarks>
        /// The throttler thread will sleep for the amount specified in this parameter in each iteration, and then releases the semaphore
        /// potentially multiple times at once, depending on the rate.
        /// For example, if the rate is 50 per second, and DelayMillis is set to 100ms, it means that the throttler will try to release 
        /// the semaphore 5 times at once, every 100 milliseconds.
        /// Lower values will result in a smoother throttling but increases CPU usage and sensitivity to available CPU on the system.
        /// </remarks>
        public int DelayMillis { get; set; } = 100;

        /// <summary>
        /// Expected rate of semaphore release per second. Can be a fraction like 0.1 per second.
        /// </summary>
        /// <remarks>
        /// Should be a positive floating point number. 
        /// Due to the throttling calculation method, larger rates may have more error, and rates larger than
        /// 500k per second are not acceptable because they can have more than 2.5% inherent calculation error, in addition
        /// to the errors that may be introduced as a result of thread scheduling or fluctuations on CPU availability.
        /// </remarks>
        public double RatePerSecond { get; set; } = 20;

        /// <summary>
        /// Maximum size of the semaphore.
        /// </summary>
        /// <remarks>
        /// This parameter effectively specifies the maximum number of "wait"s that can happen at once. This will happen when all
        /// throttled threads are blocked elsewhere (perhaps waiting for a queue or data to become available) and then all of them
        /// are activated at once. In such a case, the MaxBurstSize number of threads will be allowed to pass the semaphore's wait
        /// immidietally, and the rest will be throttled according to the specified rate.
        /// Note that throttler does not guarantee this maximum burst, and it is considered as "best effort". In some rare cases,
        /// there can be bursts larger than the number specified. This is mainly an optimization choice to avoid too much extra
        /// concurrency control.
        /// </remarks>
        public int MaxBurstSize { get; set; } = 10;

        /// <summary>
        /// Initial number on the semaphore, allowing an initial burst.
        /// </summary>
        /// <remarks>
        /// Should be less than or equal to MaxBurstSize.
        /// </remarks>
        public int InitialQuota { get; set; } = 0;
    }
}