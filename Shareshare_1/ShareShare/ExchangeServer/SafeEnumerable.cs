using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace shareshare.PriceServer
{

    public class SafeEnumerator<T> : IEnumerator<T>
    {
        // this is the (thread-unsafe)
        // enumerator of the underlying collection
        private readonly IEnumerator<T> m_Inner;
        // this is the object we shall lock on. 
      //  private readonly object m_Lock;

        private readonly ReaderWriterLockSlim m_lock;

        public SafeEnumerator(IEnumerator<T> inner, ReaderWriterLockSlim @lock)
        {
            m_Inner = inner;
            m_lock = @lock;
            // entering lock in constructor
            m_lock.EnterReadLock();
            //Monitor.Enter(m_lock);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            // .. and exiting lock on Dispose()
            // This will be called when foreach loop finishes
            if (m_lock.IsReadLockHeld) m_lock.ExitReadLock();
        }

        #endregion

        #region Implementation of IEnumerator

        // we just delegate actual implementation
        // to the inner enumerator, that actually iterates
        // over some collection

        public bool MoveNext()
        {
            return m_Inner.MoveNext();
        }

        public void Reset()
        {
            m_Inner.Reset();
        }

        public T Current
        {
            get { return m_Inner.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion
    }

    public class SafeEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> m_Inner;
        private readonly ReaderWriterLockSlim m_Lock;

        public SafeEnumerable(IEnumerable<T> inner, ReaderWriterLockSlim @lock)
        {
            m_Lock = @lock;
            m_Inner = inner;
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return new SafeEnumerator<T>(m_Inner.GetEnumerator(), m_Lock);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
