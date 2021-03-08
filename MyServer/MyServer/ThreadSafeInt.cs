using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServer
{
    //线程安全的整型
    public class ThreadSafeInt
    {
        private int value;
        public ThreadSafeInt(int value)
        {
            this.value = value;
        }
        //增加并获取
        public int Add_Get()
        {
            lock(this)
            {
                value++;
                return value;
            }
        }
        //减少并获取
        public int Reduce_Get()
        {
            lock(this)
            {
                value--;
                return value;
            }
        }
        //获取
        public int Get()
        {
            lock(this)
            {
                return value;
            }
        }
    }
}
