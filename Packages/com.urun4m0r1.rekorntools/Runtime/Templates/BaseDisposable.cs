#nullable enable

using System;

namespace Urun4m0r1.RekornTools.Templates
{
    public class BaseDisposable : IDisposable
    {
#region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Uncomment this if your class is using unmanaged resources.
        // ~BaseDisposable() => Dispose(false);

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();

            _disposed = true;
        }

        private void DisposeManagedResources()
        {
            // Dispose managed resources here.
        }

        private void DisposeUnmanagedResources()
        {
            // Dispose unmanaged resources here.
        }
#endregion // IDisposable
    }

    public class InheritedDisposable : BaseDisposable
    {
#region IDisposable
        // Uncomment this if your class is using unmanaged resources.
        // ~InheritedDisposable() => Dispose(false);

        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();

            _disposed = true;

            base.Dispose(disposing);
        }

        private void DisposeManagedResources()
        {
            // Dispose managed resources here.
        }

        private void DisposeUnmanagedResources()
        {
            // Dispose unmanaged resources here.
        }
#endregion // IDisposable
    }
}
