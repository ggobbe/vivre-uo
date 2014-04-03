#if NewAsyncSockets
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Server.Network
{
    public class SocketAsyncEventArgsPool
    {
        private Stack<SocketAsyncEventArgs> m_EventsPool;

        public SocketAsyncEventArgsPool(int numConnection)
        {
            m_EventsPool = new Stack<SocketAsyncEventArgs>(numConnection);
        }

        public void Dispose()
        {
            using (Stack<SocketAsyncEventArgs>.Enumerator enumerator = m_EventsPool.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Dispose();
                }
            }
            m_EventsPool.Clear();
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (m_EventsPool)
            {
                if (m_EventsPool.Count == 0)
                {
                    return new SocketAsyncEventArgs();
                }
                return m_EventsPool.Pop();
            }
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }
            lock (m_EventsPool)
            {
                m_EventsPool.Push(item);
            }
        }

        public int Count
        {
            get
            {
            	return m_EventsPool.Count;
            }
        }
    }
}
#endif