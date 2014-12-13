using System;
using System.Collections.Generic;

namespace NewsBoard.Indexer.Utils
{
    public class DescendingComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return -((IComparable) x).CompareTo(y);
        }
    }
}