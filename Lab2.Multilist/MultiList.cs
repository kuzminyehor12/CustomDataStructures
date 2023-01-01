using System;

namespace Lab2.Multilist
{
    public class MultiList<T>
    {
        public class Node
        {
            public T Data { get; set; }
            public Node Child { get; set; }
            public Node Next { get; set; }
            public Node(T data)
            {
                Data = data;
                Child = null;
                Next = null;
            }
            public Node(Node node)
            {
                Data = node.Data;

                if(node.Child != null)
                {
                    Child = new Node(node.Child);
                }

                if(node.Next != null)
                {
                    Next = new Node(node.Next);
                }
            }
        }
        public Node Head { get; private set; }

        public bool IsEmpty
        {
            get => Head is null;
        }

        public int Count { get; set; }

        public MultiList()
        {
            Head = null;
            Count = 0;
        }

        public int CountOnChild(int childIndex)
        {
            var current = Head;
            int countDepth = 0, countOnChild = 0;

            while(current.Child != null && countDepth < childIndex)
            {
                ++countDepth;
                current = current.Child;
            }

            while(current != null)
            {
                ++countOnChild;
                current = current.Next;
            }

            return countOnChild;
        }

        public void Insert(T data, int rowIndex = 0, int columnIndex = 0)
        {
            Node newNode = new Node(data);
           
            if (IsEmpty)
            {
                Head = newNode;
                Count++;
                return;
            }

            int countRow = 0, countColumn = 0;
            var current = Head;

            rowIndex = rowIndex < 0 ? 0 : rowIndex;
            columnIndex = columnIndex < 0 ? 0 : columnIndex;

            while (countRow < rowIndex)
            {
                if (current.Child is null)
                {
                    current.Child = newNode;
                    Count++;
                    return;
                }

                ++countRow;
                current = current.Child;
            }

            while(current.Next != null && countColumn < columnIndex)
            {
                ++countColumn;
                current = current.Next;
            }

            newNode.Next = current.Next;
            current.Next = newNode;
            Count++;
        }

        public void Move(int sourceRowIndex, int sourceColumnIndex, int destRowIndex, int destColumnIndex)
        {
            try
            {
                if (IsEmpty)
                {
                    throw new NullReferenceException("MultiList is empty");
                }

                var source = Find(sourceRowIndex, sourceColumnIndex);

                destRowIndex = destRowIndex < 0 ? 0 : destRowIndex;
                destColumnIndex = destColumnIndex < 0 ? 0 : destColumnIndex;

                Insert(source.Data, destRowIndex, destColumnIndex);
                Remove(sourceRowIndex, sourceColumnIndex);
            }
            catch (Exception ex)
            {
                throw new MultiListException(ex.Message);
            }
        }

        public Node Find(int rowIndex, int columnIndex)
        {
            int countRows = 0, countColumns = 0;
            var current = Head;

            while(countRows < rowIndex)
            {
                if (current.Child is null)
                {
                    throw new IndexOutOfRangeException("Source indexes are invalid!");
                }
                
                ++countRows;
                current = current.Child;
            }

            while(countColumns < columnIndex)
            {
                if (current.Next is null)
                {
                    throw new IndexOutOfRangeException("Source indexes are invalid!");
                }

                ++countColumns;
                current = current.Next;
            }

            return current;
        }

        public T Remove(int rowIndex, int columnIndex)
        {
            try
            {
                if (IsEmpty)
                {
                    throw new NullReferenceException("MultiList is empty");
                }

                var nodeToRemove = Find(rowIndex, columnIndex);
                T data = nodeToRemove.Data;

                if (columnIndex == 0)
                {
                    Node parent = Find(rowIndex - 1, 0);
                    parent.Child = nodeToRemove.Next;
                    Count--;
                }
                else
                {
                    Node prev = Find(rowIndex, columnIndex - 1);
                    prev.Next = nodeToRemove.Next;
                    Count--;
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new MultiListException(ex.Message);
            }
        }

        public void RemoveLevel(int rowIndex)
        {
            try
            {
                if (IsEmpty)
                {
                    throw new NullReferenceException("MultiList is empty");
                }

                var rootToRemove = Find(rowIndex, 0);

                if (rowIndex == 0)
                {
                    Head = Head.Child;
                    Count -= CountOnChild(0);
                }
                else
                {
                    Node prev = Find(rowIndex - 1, 0);
                    prev.Child = rootToRemove.Child;
                    Count -= CountOnChild(rowIndex - 1);
                }
            }
            catch (Exception ex)
            {
                throw new MultiListException(ex.Message);
            }
        }

        public MultiList<T> Copy()
        {
            return new MultiList<T>
            {
                Head = new Node(this.Head),
                Count = this.Count
            };
        }

        public void Clear()
        {
            Head = null;
        }

        public void Display()
        {
            Console.WriteLine("MultiList: ");

            if (IsEmpty)
            {
                Console.WriteLine("Empty");
                return;
            }

            Node current = Head;

            while(current != null)
            {
                var currentColumn = current;

                while(currentColumn != null)
                {
                    Console.Write(currentColumn.Data + " ");
                    currentColumn = currentColumn.Next;
                }

                Console.WriteLine();
                current = current.Child;
            }
        }
    }
}
