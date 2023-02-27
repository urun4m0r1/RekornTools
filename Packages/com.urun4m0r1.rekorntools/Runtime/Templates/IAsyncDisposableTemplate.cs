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
        private bool _isDisposing;
        private bool _isDisposed;

        // Uncomment this if your class is using unmanaged resources.
        // ~BaseIAsyncDisposableTemplate() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (!_isDisposing)
            {
                _isDisposing = true;

                if (disposeManaged && !_isAsyncDisposed)
                {
                    _isDisposed = true;
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
            }
        }
#endregion // IDisposable

#region IAysncDisposable
        private bool _isAsyncDisposing;
        private bool _isAsyncDisposed;

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_isAsyncDisposing)
            {
                _isAsyncDisposing = true;

                if (!_isDisposed)
                {
                    _isAsyncDisposed = true;
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

        private bool _isDisposing;
        private bool _isDisposed;

        protected override void Dispose(bool disposeManaged)
        {
            if (!_isDisposing)
            {
                _isDisposing = true;

                if (disposeManaged && !_isAsyncDisposed)
                {
                    _isDisposed = true;
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
            }

            base.Dispose(disposeManaged);
        }
#endregion // IDisposable

#region IAysncDisposable
        private bool _isAsyncDisposing;
        private bool _isAsyncDisposed;

        protected override async ValueTask DisposeAsyncCore()
        {
            if (!_isAsyncDisposing)
            {
                _isAsyncDisposing = true;

                if (!_isDisposed)
                {
                    _isAsyncDisposed = true;
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
