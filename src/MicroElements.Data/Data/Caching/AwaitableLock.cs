// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroElements.Data.Caching
{
    /// <summary>
    /// AwaitableLock based on <see cref="SemaphoreSlim"/>.
    /// Implements <see cref="IDisposable"/> to use in using block.
    /// It waits for semaphore and releases it on dispose.
    /// </summary>
    internal static class AwaitableLock
    {
        internal readonly struct LockLease : IDisposable
        {
            private readonly SemaphoreSlim _lock;
            public LockLease(SemaphoreSlim @lock) => _lock = @lock;
            public void Dispose() => _lock.Release();
        }

        public static async ValueTask<LockLease> WaitAsyncAndGetLockReleaser(this SemaphoreSlim semaphoreSlim)
        {
            await semaphoreSlim.WaitAsync();
            return new LockLease(semaphoreSlim);
        }

        public static LockLease WaitAndGetLockReleaser(this SemaphoreSlim semaphoreSlim)
        {
            semaphoreSlim.Wait();
            return new LockLease(semaphoreSlim);
        }
    }
}
