using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Common
{
    public abstract class Disposable : IDisposable
    {
        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                FreeManagedObjects();
            }

            FreeUnmanagedObjects();
            disposed = true;
        }

        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Free managed resources.
        /// If something implements IDisposable then it's managed object! Remember
        /// </summary>
        protected virtual void FreeManagedObjects()
        {
        }

        protected virtual void FreeUnmanagedObjects()
        {

        }


    }
}
