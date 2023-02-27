#nullable enable

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Urun4m0r1.RekornTools.Templates
{
    /// <summary>
    /// Template for an async disposable base class.
    /// </summary>
    [UsedImplicitly]
    public abstract class BaseIAsyncDisposableTemplate : IDisposable, IAsyncDisposable
    {
#region IDisposable
        private bool _disposing;
        private bool _disposed;

        // Uncomment this if your class is using unmanaged resources.
        // ~BaseIAsyncDisposableTemplate() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposing)
            {
                _disposing = true;

                if (disposing && !_asyncDisposed)
                {
                    _disposed = true;
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
            }
        }
#endregion // IDisposable

#region IAysncDisposable
        private bool _asyncDisposing;
        private bool _asyncDisposed;

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_asyncDisposing)
            {
                _asyncDisposing = true;

                if (!_disposed)
                {
                    _asyncDisposed = true;
                    await DisposeManagedResourcesAsync().ConfigureAwait(false);
                }
            }
        }
#endregion // IAsyncDisposable

        private void DisposeManagedResources()
        {
            // TODO: dispose managed state (managed objects)
        }

        private void DisposeUnmanagedResources()
        {
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }

        private async ValueTask DisposeManagedResourcesAsync()
        {
            // TODO: async dispose managed state (managed objects)
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Template for an async disposable inherited class.
    /// </summary>
    [UsedImplicitly]
    public abstract class InheritedIAsyncIDisposableTemplate : BaseIAsyncDisposableTemplate
    {
#region IDisposable
        // Uncomment this if your class is using unmanaged resources.
        // ~InheritedIAsyncIDisposableTemplate() => Dispose(false);

        private bool _disposing;
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (!_disposing)
            {
                _disposing = true;

                if (disposing && !_asyncDisposed)
                {
                    _disposed = true;
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
            }

            base.Dispose(disposing);
        }
#endregion // IDisposable

#region IAysncDisposable
        private bool _asyncDisposing;
        private bool _asyncDisposed;

        protected override async ValueTask DisposeAsyncCore()
        {
            if (!_asyncDisposing)
            {
                _asyncDisposing = true;

                if (!_disposed)
                {
                    _asyncDisposed = true;
                    await DisposeManagedResourcesAsync().ConfigureAwait(false);
                }
            }

            await base.DisposeAsyncCore().ConfigureAwait(false);
        }
#endregion // IAsyncDisposable

        private void DisposeManagedResources()
        {
            // TODO: dispose managed state (managed objects)
        }

        private void DisposeUnmanagedResources()
        {
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }

        private async ValueTask DisposeManagedResourcesAsync()
        {
            // TODO: async dispose managed state (managed objects)
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
