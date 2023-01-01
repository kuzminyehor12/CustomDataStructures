using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Lab6.BinomialTree
{
    public class BinomialHeap<T>
    {
        public class BinomialNode
        {
            public T Value { get; set; }
            public int Degree { get; set; }

            public BinomialNode Parent { get; set; }
            public BinomialNode Child { get; set; }
            public BinomialNode Sibling { get; set; }
            public BinomialNode(T value, int degree)
            {
                Value = value;
                Degree = degree;
                Parent = null;
                Child = null;
                Sibling = null;
            }
        }

        public BinomialNode Head { get; private set; }
        public BinomialNode MinRoot { get; private set; }
        public int Size { get; private set; }
        private T SmallestValue { get; set; }

        public bool IsEmpty
        {
            get => Head is null;
        }

        public Comparer<T> Comparer { get; }

        public BinomialHeap(T smallestValue, Comparer<T> comparer)
        {
            Head = null;
            MinRoot = null;
            Size = 0;
            SmallestValue = smallestValue;
            Comparer = comparer;
        }

        public BinomialNode Insert(T element)
        {
            var heapWithElement = new BinomialHeap<T>(SmallestValue, Comparer);
            var insertedElement = new BinomialNode(element, 1);

            heapWithElement.Head = insertedElement;
            var newHeap = this.Union(heapWithElement);

            Head = newHeap.Head;
            Size++;
            CountMin();

            return insertedElement;
        }

        public BinomialHeap<T> Union(BinomialHeap<T> heapToUnite)
        {
            var newHeap = MergeHeaps(this, heapToUnite);

            if (newHeap.IsEmpty)
            {
                return newHeap;
            }

            BinomialNode previousRoot = null;
            var root = newHeap.Head;
            var nextRoot = root.Sibling;

            while(nextRoot != null)
            {
                var nextTwoRootsAreNotEqual = root.Degree != nextRoot.Degree;
                var nextThreeRootsAreNotEqual = nextRoot.Sibling != null && nextRoot.Sibling.Degree == nextRoot.Degree;
                var movePointers = nextTwoRootsAreNotEqual || nextThreeRootsAreNotEqual;

                if (movePointers)
                {
                    previousRoot = root;
                    root = nextRoot;
                }
                else
                {
                    if(Comparer.Compare(root.Value, nextRoot.Value) < 0)
                    {
                        root.Sibling = nextRoot.Sibling;
                        LinkTrees(root, nextRoot);
                    }
                    else
                    {
                        if(previousRoot is null)
                        {
                            newHeap.Head = nextRoot;
                        }
                        else
                        {
                            previousRoot.Sibling = nextRoot;
                        }

                        LinkTrees(nextRoot, root);
                        root = nextRoot;
                    }

                    root = nextRoot;
                }

                nextRoot = root.Sibling;
            }

            return newHeap;
        }

        private void LinkTrees(BinomialNode firstTree, BinomialNode secondNode)
        {
            secondNode.Parent = firstTree;
            secondNode.Sibling = firstTree.Child;

            firstTree.Child = secondNode;
            firstTree.Degree++;
        }

        public BinomialHeap<T> MergeHeaps(BinomialHeap<T> firstHeap, BinomialHeap<T> secondHeap)
        {
            if (firstHeap.IsEmpty)
            {
                return secondHeap;
            }

            if (secondHeap.IsEmpty)
            {
                return firstHeap;
            }

            BinomialNode head = null;
            var firstHead = firstHeap.Head;
            var secondHead = secondHeap.Head;

            if(firstHead.Degree < secondHead.Degree)
            {
                head = firstHead;
                firstHead = firstHead.Sibling;
            }
            else
            {
                head = secondHead;
                secondHead = secondHead.Sibling;
            }

            var current = head;
            
            while(firstHead != null && secondHead != null)
            {
                if(firstHead.Degree < secondHead.Degree)
                {
                    current.Sibling = firstHead;
                    firstHead = firstHead.Sibling;
                }
                else
                {
                    current.Sibling = secondHead;
                    secondHead = secondHead.Sibling;
                }
            }

            if (firstHead != null)
            {
                current.Sibling = firstHead;
            }

            if (secondHead != null)
            {
                current.Sibling = secondHead;
            }

            var mergedHeap = new BinomialHeap<T>(SmallestValue, Comparer);
            mergedHeap.Head = head;
            mergedHeap.Size = firstHeap.Size + secondHeap.Size;

            return mergedHeap;
        }

        public void CountMin()
        {
            if (IsEmpty)
            {
                return;
            }

            var current = Head.Sibling;
            var min = Head;

            while(current != null)
            {
                if(Comparer.Compare(current.Value, min.Value) < 0)
                {
                    min = current;
                }

                current = current.Sibling;
            }

            MinRoot = min;
        }

        public BinomialNode ExtractMin()
        {
            var smallestRoot = RemoveSmallestRoot();

            if (smallestRoot is null)
            {
                return smallestRoot;
            }

            Size--;

            if(smallestRoot.Child != null)
            {
                var reversedChildren = new BinomialHeap<T>(SmallestValue, Comparer);
                reversedChildren.Head = ReverseListOfRoots(smallestRoot.Child);
                var newHeap = this.Union(reversedChildren);
                Head = newHeap.Head;
            }

            CountMin();
            return smallestRoot;
        }

        public BinomialNode RemoveSmallestRoot()
        {
            if(IsEmpty)
            {
                return null;
            }

            var current = Head;
            var previous = current;

            var min = current;
            BinomialNode prevMin = null;

            current = current.Sibling;
            while (current != null)
            {
                if (Comparer.Compare(current.Value, min.Value) < 0)
                {
                    min = current;
                    prevMin = previous;
                }

                previous = current;
                current = current.Sibling;
            }

            if(previous is null || prevMin is null)
            {
                Head = Head.Sibling;
            }
            else
            {
                prevMin.Sibling = min.Sibling;
            }

            return min;
        }

        public BinomialNode ReverseListOfRoots(BinomialNode head)
        {
            var current = head;
            BinomialNode previous = null;
            BinomialNode next = null;

            while(current != null)
            {
                next = current.Sibling;
                current.Sibling = previous;
                previous = current;
                current = next;
            }

            return previous;
        }

        public BinomialNode DeleteNode(BinomialNode node)
        {
            DecreaseKey(node, SmallestValue);
            return ExtractMin();
        }

        public bool DecreaseKey(BinomialNode node, T newValue)
        {
            if(Comparer.Compare(node.Value, newValue) < 0)
            {
                return false;
            }

            node.Value = newValue;
            var current = node;
            var parent = current.Parent;

            while(parent != null && Comparer.Compare(current.Value, parent.Value) < 0)
            {
                var temp = parent.Value;
                parent.Value = current.Value;
                current.Value = temp;

                current = parent;
                parent = current.Parent;
            }

            return true;
        }

        public Tuple<int, string> CountTrees()
        {
            int counter = 0;
            var current = Head;

            while(current != null)
            {
                counter++;
                current = current.Sibling;
            }

            return Tuple.Create(counter, Convert.ToString(counter, 2));
        }

        public void Clear()
        {
            Head = null;
            MinRoot = null;
            Size = 0;
        }

        public void Display()
        {
            Console.WriteLine("Binomial Heap:");

            if (IsEmpty)
            {
                Console.WriteLine("Empty");
            }

            Console.WriteLine(Stringify(Head));
        }

        public string Stringify(BinomialNode node)
        {
            StringBuilder builder = new StringBuilder();
            var current = node;

            while (current != null)
            {
                builder.Append(current.Value);
                builder.Append("(");
                builder.Append(Stringify(current.Child));
                builder.Append(")");
                current = current.Sibling;
            }

            return builder.ToString();
        }
    }
}
