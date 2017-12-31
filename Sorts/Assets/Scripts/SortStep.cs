using System;
using System.Collections.Generic;
using UnityEngine;

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

public struct SortElement
{
    public Vector3 Position;
    public int Number;

    public SortElement(Vector3 position, int number)
    {
        Position = position;
        Number = number;
    }
}
