#nullable enable

using System;
using JetBrains.Annotations;

namespace Urun4m0r1.RekornTools.Templates
{
    /// <summary>
    /// Template for a disposable base class.
    /// </summary>
    [UsedImplicitly]
    public abstract class BaseIDisposableTemplate : IDisposable
    {
#region IDisposable
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposeManaged)
                    DisposeManagedResources();

                DisposeUnmanagedResources();
            }
        }
#endregion // IDisposable

        // Uncomment this if your class is using unmanaged resources.
        // ~BaseIDisposableTemplate() => Dispose(false);

        private void DisposeManagedResources()
        {
            // TODO: dispose managed state (managed objects)
        }

        private void DisposeUnmanagedResources()
        {
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }
    }

    /// <summary>
    /// Template for a disposable inherited class.
    /// </summary>
    [UsedImplicitly]
    public abstract class InheritedIDisposableTemplate : BaseIDisposableTemplate
    {
#region IDisposable
        private bool _isDisposed;

        protected override void Dispose(bool disposeManaged)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposeManaged)
                    DisposeManagedResources();

                DisposeUnmanagedResources();
            }

            base.Dispose(disposeManaged);
        }
#endregion // IDisposable

        // Uncomment this if your class is using unmanaged resources.
        // ~InheritedIDisposableTemplate() => Dispose(false);

        private void DisposeManagedResources()
        {
            // TODO: dispose managed state (managed objects)
        }

        private void DisposeUnmanagedResources()
        {
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }
    }
}
