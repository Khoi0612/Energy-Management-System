using EMS.Core.Interfaces;
using EMS.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Core.Services
{
    internal class EnergyMonitoringService : IDisposable
    {
        private readonly IEnergyReadingRepository _repository;
        private readonly BlockingCollection<EnergyReading> _queue;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task _processingTask;
        private bool _disposed;

        public EnergyMonitoringService(IEnergyReadingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _queue = new BlockingCollection<EnergyReading>();
            _cancellationTokenSource = new CancellationTokenSource();
            _processingTask = Task.Run((Func<Task>)ProcessQueueAsync);
        }

        public void EnqueueReading(EnergyReading reading)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(EnergyMonitoringService));

            _queue.Add(reading);
        }

        private async Task ProcessQueueAsync()
        {
            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    EnergyReading reading;
                    if (_queue.TryTake(out reading, Timeout.Infinite, _cancellationTokenSource.Token))
                    {
                        try
                        {
                            await _repository.SaveReadingAsync(reading);
                        }
                        catch (Exception ex)
                        {
                            // Log error but continue processing
                            Debug.WriteLine($"Error saving reading: {ex.Message}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown, ignore
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
                    _queue.CompleteAdding();
                    try
                    {
                        _processingTask.Wait();
                    }
                    catch (AggregateException)
                    {
                        // Ignore task cancellation exceptions
                    }
                    _queue.Dispose();
                    _cancellationTokenSource.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
