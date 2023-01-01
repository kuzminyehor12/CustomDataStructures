using System;

namespace Lab1.Deque
{
    public class Deque<T>
    {
        public class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
                Prev = null;
            }
        }

        public Node Head { get; private set; }

        public bool IsEmpty
        {
            get => Head is null;
        }

        public T First
        {
            get
            {
                Node headNode = Head;
                return headNode.Data;
            }
        }

        public T Last
        {
            get
            {
                Node lastNode = ForwardToLast();
                return lastNode.Data;
            }
        }

        public int Length
        {
            get
            {
                int count = 0;
                Node temp = Head;

                while (temp != null)
                {
                    temp = temp.Next;
                    ++count;
                }

                return count;
            }
        }

        public Deque()
        {
            Head = null;
        }

        public void PushFront(T data)
        {
            Node newNode = new Node(data);
            newNode.Next = Head;
            newNode.Prev = null;

            if (!IsEmpty)
            {
                Head.Prev = newNode;
            }

            Head = newNode;
        }

        public void PushBack(T data)
        {
            Node newNode = new Node(data);

            if (IsEmpty)
            {
                newNode.Prev = null;
                Head = newNode;
                return;
            }

            Node lastNode = ForwardToLast();
            lastNode.Next = newNode;
            newNode.Prev = lastNode;
        }

        private Node ForwardToLast()
        {
            Node temp = Head;

            while (temp.Next != null)
            {
                temp = temp.Next;
            }

            return temp;
        }

        public T PopFront()
        {
            if (IsEmpty)
            {
                throw new DequeException("Cannot remove element out of empty deque!");
            }

            Head.Next.Prev = null;
            T data = Head.Data;
            Head = Head.Next;
            return data;
        }

        public T PopBack()
        {
            if (IsEmpty)
            {
                throw new DequeException("Cannot remove element out of empty deque!");
            }

            Node lastNode = ForwardToLast();
            T data = lastNode.Data;
            lastNode = lastNode.Prev;
            lastNode.Next = null;
            return data;
        }

        public void SwapFirstAndLast()
        {
            if (IsEmpty)
            {
                throw new DequeException("Cannot swap elements in empty deque!");
            }

            Node lastNode = ForwardToLast();

            if (Length == 1)
            {
                return;
            }

            if(Length == 2)
            {
                Node temp = Head;
                temp.Prev = lastNode;
                Head = lastNode;
                Head.Next = temp;

                Head.Prev = null;
                temp.Next = null;
                return;
            }
           
            Node lastPrev = lastNode.Prev;
            Node headNext = Head.Next;

            Head.Next = null;
            lastNode.Prev = null;

            Head.Prev = lastPrev;
            lastNode.Next = headNext;

            lastPrev.Next = Head;
            headNext.Prev = lastNode;

            lastNode = lastPrev.Next;
            Head = headNext.Prev;
        }

        public void Reverse()
        {
            if (IsEmpty)
            {
                throw new DequeException("Cannot reverse empty deque!");
            }

            Deque<T> deque = new Deque<T>();

            Node temp = ForwardToLast();
            while (temp != null)
            {
                deque.PushBack(temp.Data);
                temp = temp.Prev;
            }

            Head = deque.Head;
        }

        public bool Contains(T data)
        {
            Node temp = Head;

            while (temp.Next != null)
            {
                if (temp.Data.Equals(data))
                {
                    return true;
                }

                temp = temp.Next;
            }

            return false;
        }

        public void Clear()
        {
            Head = null;
        }

        public void Display()
        {
            Console.WriteLine("Deque: ");

            if (IsEmpty)
            {
                Console.WriteLine("Empty");
                return;
            }

            Node temp = Head;

            while (temp != null)
            {
                Console.Write(temp.Data + " ");
                temp = temp.Next;
            }

            Console.WriteLine();
        }
    }
}
