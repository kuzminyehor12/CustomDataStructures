using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1.Deque
{
    public class DequeException : Exception
    {
        public DequeException()
        {
            
        }

        public DequeException(string msg) : base(msg)
        {
            
        }
    }
}
