using System.Collections.Generic;

namespace Nananami.Lib.Messaging
{
    public class MessageTray<T>
    {
        public void Post(string message, T messageState)
        {
            if (!m_messages.ContainsKey(message))
            {
                m_messages[message] = new Queue<T>();
            }
            m_messages[message].Enqueue(messageState);
        }

        public bool IsMessagePosted(string message)
        {
            if (!m_messages.ContainsKey(message) || m_messages[message].Count == 0)
            {
                return false;
            }
            return true;
        }

        public T Query(string message)
        {
            if (IsMessagePosted(message))
            {
                return m_messages[message].Dequeue();
            }

            throw new KeyNotFoundException($"{message} was not posted.");
        }

        public bool TryQuery(string message, out T result)
        {
            if (IsMessagePosted(message))
            {
                result = m_messages[message].Dequeue();
                return true;
            }
            result = default!;
            return false;
        }

        private Dictionary<string, Queue<T>> m_messages = new Dictionary<string, Queue<T>>();
    }
}