using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyServer
{
    public delegate void ExecuteDelegate();
    public class SingleExecute
    {
        private static object ob = new object();
        private static SingleExecute instance = null;
        public static SingleExecute Instance
        {
            get
            {
                lock(ob)
                {
                    if (instance == null)
                    {
                        instance = new SingleExecute();
                    }
                    return instance;
                }
                
            }
        }
        private object objLock = new object();
        //互斥锁
        private Mutex mutex;
        
        public SingleExecute()
        {
            mutex = new Mutex();
        }
        //单线程执行逻辑
        public void Execute(ExecuteDelegate executeDelegate)
        {
            lock(objLock)
            {
                mutex.WaitOne();
                executeDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
