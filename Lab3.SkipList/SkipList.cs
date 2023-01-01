using System;
using System.Collections.Generic;

namespace Lab3.SkipList
{
    public class SkipList<TValue>
    {
        public class Node
        {
            public List<Node> Next { get; set; }
            public TValue Value { get; set; }
            public int Key { get; set; }
            public Node(int key, TValue value = default)
            {
                Key = key;
                Value = value;
                Next = new List<Node>();
            }

            public Node(Node node)
            {
                Key = node.Key;
                Value = node.Value;
                Next = new List<Node>(node.Next.Count + 1);
                for (int i = 0; i < Next.Count; i++)
                {
                    Next[i] = node.Next[i];
                }
            }
        }

        private const double Probability = 0.5;
        public Node Head { get; private set; }
        public Node Tail { get; private set; }
        public int ActualLevel { get; private set; }
        public Func<int, int> DefineMaxLevel { get; private set; }
        public bool IsCustomFunctioned { get; set; }
        public int MaxLevel { get; private set; }
        public int Size { get; private set; }

        public SkipList(int maxValue, Func<int, int> func = default)
        {
            Head = new Node(int.MaxValue);
            Tail = new Node(int.MaxValue);
            Head.Next.Add(Tail);
            MaxLevel = maxValue;
            Size = 0;
            ActualLevel = 0;

            if(func == default)
            {
                DefineMaxLevel = new Func<int, int>(_ => MaxLevel > 0 ? MaxLevel - 1 : int.MaxValue);
                IsCustomFunctioned = false;
            }
            else
            {
                DefineMaxLevel = func;
                IsCustomFunctioned = true;
            }
        }

        public bool Insert(int key, TValue value)
        {
            if (Insert(key, value, Head, ActualLevel, new List<Node>()))
            {
                Size++;

                if (IsCustomFunctioned)
                {
                    MaxLevel = Size;
                }
                    
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Insert(int key, TValue value, Node node, int level, List<Node> changes)
        {

            for (int i = level; i > 0; i--)
            {
                if (node.Next[i].Key < key)
                {
                    return Insert(key, value, node.Next[i], i, changes);
                }
                else
                {
                    changes.Add(node);
                }
            }

            if (node.Next[0].Key < key)
            {
                return Insert(key, value, node.Next[0], 0, changes);
            }
            else if (node.Next[0].Key == key)
            {
                return false;
            }
            else
            {
                changes.Add(node);
                changes.Reverse();
                GenerateNode(key, value, changes);
                return true;
            }

        }
        private void GenerateNode(int key, TValue value, List<Node> changes)
        {
            var random = new Random();
            double probability = 0;
            int level = 0;
            int i = 0;

            Node newNode = new Node(key, value);
            newNode.Next.Clear();

            while (probability <= Probability && level < changes.Count)
            {
                newNode.Next.Add(changes[level].Next[level]);
                changes[level].Next[level] = newNode;
                i++;
                probability = random.NextDouble();
                level++;
            }

            int num = DefineMaxLevel(MaxLevel);

            while (probability <= Probability && i < num)
            {
                Head.Next.Add(newNode);
                newNode.Next.Add(Tail);
                i++;
                probability = random.NextDouble();

            }

            ActualLevel = Head.Next.Count - 1;
        }

        public Node Find(int key)
        {
            int level = ActualLevel;
            Node current = Head;

            while (level >= 0)
            {
                if (current.Next[level].Key > key)
                {
                    level--;
                }
                else if (current.Next[level].Key < key)
                {
                    current = current.Next[level];
                }
                else
                {
                    return current.Next[level];
                }
            }

            return null;
        }
        public void Remove(int key)
        {
            var previousNodes = GetPrevious(key, Head);
            Node current = Find(key);
            int level = previousNodes.Count;
            previousNodes.Reverse();

            for (int i = 0; i < level; i++)
            {
                previousNodes[i].Next[i] = current.Next[i];
            }

            Size--;
        }
        private List<Node> GetPrevious(int key, Node node)
        {
            var previousNodes = new List<Node>();
            int level = ActualLevel;
            Node current = node;

            while (level >= 0)
            {
                if (current.Next[level].Key > key)
                {
                    level--;
                }
                else if (current.Next[level].Key < key)
                {
                    current = current.Next[level];
                }
                else
                {
                    previousNodes.Add(current);
                    level--;
                }
            }

            return previousNodes;
        }

        public SkipList<TValue> Copy()
        {
            return new SkipList<TValue>(int.MaxValue)
            {
                Head = this.Head,
                Tail = this.Tail,
                ActualLevel = this.ActualLevel,
                MaxLevel = this.MaxLevel,
                DefineMaxLevel = this.DefineMaxLevel,
                Size = this.Size
            };
        }

        public void Clear()
        {
            Head = new Node(int.MaxValue);
            Tail = new Node(int.MaxValue);
            Head.Next.Add(Tail);
            Size = 0;
            ActualLevel = 0;
        }

        public void Display()
        {
            Console.WriteLine("\nActual Level: " + this.ActualLevel);
            Console.WriteLine("Max Level: " + this.MaxLevel);
            Console.WriteLine("SkipList:");
            Node current = Head.Next[0];

            while (current.Key != int.MaxValue)
            {
                Console.Write(current.Key + ":" + current.Value + ' ');
                current = current.Next[0];
            }

            Console.WriteLine();
        }
    }
}
