using Lab2.Multilist;
using System;

namespace Lab4.Matrix
{
    public class Matrix
    {
        private readonly MultiList<int> _multiList;
        public int Rows { get; }
        public int Columns { get; }
        public int this[int i, int j]
        {
            get
            {
                if(i < 0 || i > Rows || j < 0 || j > Columns)
                {
                    throw new MatrixException();
                }

                return _multiList.Find(i, j).Data;
            }
            set
            {
                if (i < 0 || i > Rows || j < 0 || j > Columns)
                {
                    throw new MatrixException();
                }

                _multiList.Find(i, j).Data = value;
            }
        }
        public Matrix()
        {
            Rows = 0;
            Columns = 0;
            _multiList = new MultiList<int>();
        }
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _multiList = InitMultilist();
        }
        public MultiList<int> InitMultilist()
        {
            var multiList = new MultiList<int>();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    multiList.Insert(0, i, j);
                }
            }

            return multiList;
        }

        public void Insert(int number, int rowIndex, int columnIndex)
        {
            if(number == 0)
            {
                throw new MatrixException();
            }

            _multiList.Insert(number, rowIndex, columnIndex);
        }

        public void Remove(int rowIndex, int columnIndex)
        {
            _multiList.Find(rowIndex, columnIndex).Data = 0;
        }

        public void Sum(Matrix matrix)
        {
            if(matrix.Rows != this.Rows || matrix.Columns != this.Columns)
            {
                throw new MatrixException();
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] += matrix[i, j];
                }
            }
        }

        public Matrix Transpose()
        {
            var resultMatrix = new Matrix(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    resultMatrix[j, i] = this[i, j];
                }
            }

            return resultMatrix;
        }

        public void Multiply(int number)
        {
            if (number == 0)
            {
                throw new MatrixException();
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] *= number;
                }
            }
        }

        public void Clear()
        {

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = 0;
                }
            }
        }

        public string GetSize()
        {
            return Rows + "x" + Columns;
        }

        public void Display()
        {
            _multiList.Display();
        }
    }
}
