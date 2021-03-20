using System;
using System.Collections.Concurrent;

namespace Xramcire.Dokumentieren.Services
{
    //
    //  This is an adpatation of code found here: 
    //  https://www.codeproject.com/Tips/1190802/File-Locking-in-a-Multi-Threaded-Environment
    //
    internal class DocumentLock
    {
        public int Count = 0;
    }

    internal class DocumentLockManager
    {
        private static ConcurrentDictionary<string, DocumentLock> documentLocks = new ConcurrentDictionary<string, DocumentLock>();

        private static DocumentLock GetLock(string documentName)
        {
            DocumentLock documentLock = null;
            if (documentLocks.TryGetValue(documentName.ToLower(), out documentLock))
            {
                documentLock.Count++;
                return documentLock;
            }
            else
            {
                documentLock = new DocumentLock();
                documentLocks.TryAdd(documentName.ToLower(), documentLock);
                documentLock.Count++;
                return documentLock;
            }
        }

        public static void GetLock(string documentName, Action action)
        {
            lock (GetLock(documentName))
            {
                action();
                Unlock(documentName);
            }
        }

        public static T GetLock<T>(string documentName, Func<T> func)
        {
            lock (GetLock(documentName))
            {
                T ret = func();
                Unlock(documentName);
                return ret;
            }
        }

        private static void Unlock(string documentName)
        {
            DocumentLock documentLock = null;
            if (documentLocks.TryGetValue(documentName.ToLower(), out documentLock))
            {
                documentLock.Count--;
                if (documentLock.Count == 0)
                {
                    documentLocks.TryRemove(documentName.ToLower(), out documentLock);
                }
            }
        }
    }
}
