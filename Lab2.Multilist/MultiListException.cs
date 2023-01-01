using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Multilist
{
    public class MultiListException : Exception
    {
        public MultiListException() : base()
        {

        }

        public MultiListException(string msg) : base(msg)
        {

        }

        public MultiListException(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}
