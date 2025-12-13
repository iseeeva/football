namespace Sobee.Common
{
    public static class AsyncWait
    {
        // https://stackoverflow.com/a/52357854
        /// <summary>Blocks while condition is true or timeout occurs.</summary>
        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        // https://stackoverflow.com/a/52357854
        /// <summary>Blocks until condition is true or timeout occurs.</summary>
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            using var cts = timeout > 0 ? new CancellationTokenSource(timeout) : new CancellationTokenSource();

            while (!condition())
            {
                await Task.Delay(frequency, cts.Token)
                    .ConfigureAwait(false);
            }
        }
    }
}
