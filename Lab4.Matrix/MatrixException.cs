using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Matrix
{
    public class MatrixException : Exception
    {
        public MatrixException() : base()
        {

        }

        public MatrixException(string msg) : base(msg)
        {

        }
    }
}
