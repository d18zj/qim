﻿using System;

namespace Qim
{
    /// <summary>
    ///     Represents that the derived classes are disposable objects.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private bool _isDisposed = false;
        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or
        ///     resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ExplicitDispose();
        }

        public bool IsDisposed => _isDisposed;

        #endregion

        #region Finalization Constructs

        /// <summary>
        ///     Finalizes the object.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">
        ///     A <see cref="System.Boolean" /> value which indicates whether
        ///     the object should be disposed explicitly.
        /// </param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        ///     Provides the facility that disposes the object in an explicit manner,
        ///     preventing the Finalizer from being called after the object has been
        ///     disposed explicitly.
        /// </summary>
        protected void ExplicitDispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}