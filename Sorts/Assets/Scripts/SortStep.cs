using System;
using System.Collections.Generic;

public struct SortStep
{
    public List<IComparable> Process;
    public int Left;
    public int Right;

    public SortStep(List<IComparable> process, int left, int right)
    {
        Process = process;
        Left = left;
        Right = right;
    }
}
