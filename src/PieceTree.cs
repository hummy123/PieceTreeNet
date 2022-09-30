﻿namespace PieceTree;

public partial class Position
{
    public int Line { get; }
    public int Column { get; }

    private Position With(int newLine, int newColumn)
    {
        if (newLine == this.Line && newColumn == this.Column)
        {
            return this;
        }
        else
        {
            return new Position(newLine, newColumn);
        }
    }

    private Position Delta(int moveLine = 0, int moveCol = 0)
    {
        int newLine = this.Line + moveLine;
        int newCol = this.Column + moveCol;
        newLine = newLine < 0 ? 0 : newLine;
        newCol = newCol < 0 ? 0 : newCol;
        return new Position(newLine, newCol);
    }

    public static bool IsBefore(Position a, Position b)
    {
        if (a.Line < b.Line)
        {
            return true;
        }
        if (b.Line < a.Line)
        {
            return false;
        }
        return a.Column < b.Column;
    }

    public static bool IsBeforeOrEqual(Position a, Position b)
    {
        if (a.Line < b.Line)
        {
            return true;
        }
        if (b.Line < a.Line)
        {
            return false;
        }
        return a.Column <= b.Column;
    }

    public static int Compare(Position a, Position b)
    {
        if (a.Line == b.Line)
        {
            return a.Column - b.Column;
        }
        else
        {
            return a.Line - b.Line;
        }
    }
}

public partial class Range
{
    public Position Start { get; set; }
    public Position End { get; set; }

    public static bool IsEmpty(Range range)
    {
        return range.Start == range.End;
    }

    public static bool ContainsPosition(Range range, Position position)
    {
        if (position.Line < range.Start.Line || position.Line > range.End.Line)
        {
            return false;
        }
        if (position.Line == range.Start.Line && position.Column < range.Start.Column)
        {
            return false;
        }
        if (position.Line == range.End.Line && position.Column > range.End.Column)
        {
            return false;
        }
        return true;
    }

    public static bool ContainsPositionStrict(Range range, Position position)
    {
        if (position.Line < range.Start.Line || position.Line > range.End.Line)
        {
            return false;
        }
        if (position.Line == range.Start.Line && position.Column <= range.Start.Column)
        {
            return false;
        }
        if (position.Line == range.End.Line && position.Column >= range.End.Column)
        {
            return false;
        }
        return true;
    }

    public static bool ContainsRange(Range a, Range b)
    {
        if (b.Start.Line < a.Start.Line || b.End.Line < a.Start.Line)
        {
            return false;
        }
        if (b.Start.Line > a.End.Line || b.End.Line > a.End.Line)
        {
            return false;
        }
        if (b.Start.Line == a.Start.Line && b.Start.Column < a.Start.Column)
        {
            return false;
        }
        if (b.End.Line == a.End.Line && b.End.Column > a.End.Column)
        {
            return false;
        }
        return true;
    }

    public static bool ContainsRangeStrict(Range a, Range b)
    {
        if (b.Start.Line < a.Start.Line || b.End.Line < a.Start.Line)
        {
            return false;
        }
        if (b.Start.Line > a.End.Line || b.End.Line > a.End.Line)
        {
            return false;
        }
        if (b.Start.Line == a.Start.Line && b.Start.Column <= a.Start.Column)
        {
            return false;
        }
        if (b.End.Line == a.End.Line && b.End.Column >= a.End.Column)
        {
            return false;
        }
        return true;
    }

    public static Range Union(Range a, Range b)
    {
        Position newStart = a.Start.IsBeforeOrEqual(b.Start) ? a.Start : b.Start;
        Position newEnd = a.End.IsBefore(b.End) ? b.End : a.End;
        return new Range(newStart, newEnd);
    }

    public static Range Intersection(Range a, Range b)
    {
        Position newStart = a.Start.IsBeforeOrEqual(b.Start) ? b.Start : a.Start;
        Position newEnd = a.End.IsBefore(b.End) ? a.End : b.End;
        return new Range(newStart, newEnd);
    }

    public static Range CollapseToStart(Range range)
    {
        return new Range(range.Start, range.Start);
    }

    public static bool AreIntersectingOrTouching(Range a, Range b)
    {
        // Check if `a` is before `b`
        if (a.End.Line < b.Start.Line || (a.End.Line == b.Start.Line && a.End.Column < b.Start.Column))
        {
            return false;
        }
        // Check if `b` is before `a`
        if (b.End.Line < a.Start.Line || (b.End.Line == a.Start.Line && b.End.Column < a.Start.Column))
        {
            return false;
        }

        // These ranges must intersect
        return true;
    }

    public static bool AreIntersecting(Range a, Range b)
    {
        // Check if `a` is before `b`
        if (a.End.Line < b.Start.Line || (a.End.Line == b.Start.Line && a.End.Column <= b.Start.Column))
        {
            return false;
        }
        // Check if `b` is before `a`
        if (b.End.Line < a.Start.Line || (b.End.Line == a.Start.Line && b.End.Column <= a.Start.Column))
        {
            return false;
        }

        // These ranges must intersect
        return true;
    }

    public static int CompareUsingStarts(Range a, Range b)
    {
        if (!a.IsEmpty() && !b.IsEmpty())
        {
            if (a.Start.Line == b.Start.Line)
            {
                if (a.Start.Column == b.Start.Column)
                {
                    if (a.End.Line == b.End.Line)
                    {
                        return a.End.Column - b.End.Column;
                    }
                    return a.End.Line - b.End.Line;
                }
                return a.Start.Column - b.Start.Column;
            }
            return a.Start.Line - b.Start.Line;
        }
        int aExists = (a.IsEmpty() ? 1 : 0);
        int bExists = (b.IsEmpty() ? 1 : 0);
        return aExists - bExists;
    }

    public static int CompareUsingEnds(Range a, Range b)
    {
        if (a.End.Line == b.End.Line)
        {
            if (a.End.Column == b.End.Column)
            {
                if (a.Start.Line == b.Start.Line)
                {
                    return a.Start.Column - b.Start.Column;
                }
                return a.Start.Line - b.Start.Line;
            }
            return a.End.Column - b.End.Column;
        }
        return a.End.Line - b.End.Line;
    }
    public static bool SpansMultipleLines(Range range)
    {
        if (range.Start.Column < range.End.Column)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public partial class BufferCursor
{
    public int Line { get; set; }
    public int Column { get; set; }
}

public partial class Piece
{
    public readonly int BufferIndex;
    public readonly BufferCursor Start;
    public readonly BufferCursor End;
    public readonly int Length;
    public readonly int LineFeedCount;
}

public partial class StringBuffer
{
    public string Buffer { get; set; }
    public int[] LineStarts { get; set; }
}

public enum NodeColour { Red, Black }

public partial class TreeNode
{
    public TreeNode Parent { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    public Piece Piece { get; set; }
    public int LeftSize { get; set; }
    public int LfLeft { get; set; }
    public NodeColour Colour { get; set; }

    public TreeNode(Piece piece, NodeColour colour)
    {
        Piece = piece;
        Colour = colour;
        LeftSize = 0;
        LfLeft = 0;
        Parent = this;
        Left = this;
        Right = this;
    }

    public static TreeNode Leftmost(TreeNode node)
    {
        while (node.Left != node)
        {
            node = node.Left;
        }
        return node;
    }
    public static TreeNode Rightmost(TreeNode node)
    {
        while (node.Right != node)
        {
            node = node.Right;
        }
        return node;
    }
    public TreeNode Next()
    {
        if (this.Right != this)
        {
            return Leftmost(this);
        }

        var node = this;

        while (node.Parent != node)
        {
            if (node.Parent.Left == node)
            {
                break;
            }
            node = node.Parent;
        }

        if (node.Parent == node)
        {
            return node;
        }
        else
        {
            return node.Parent;
        }
    }

    public TreeNode Prev()
    {
        if (this.Left != this)
        {
            return Rightmost(this);
        }

        var node = this;

        while (node.Parent != node)
        {
            if (node.Parent.Right == node)
            {
                break;
            }
            node = node.Parent;
        }

        if (node.Parent == node)
        {
            return node;
        }
        else
        {
            return node.Parent;
        }
    }

    public void Detach()
    {
        this.Parent = this;
        this.Left = this;
        this.Right = this;
    }

}

public partial class NodePosition
{
    public TreeNode Node { get; set; }
    public int Remainder { get; set; }
    public int NodeStartLineNumber { get; set; }
}

public partial class CacheEntry
{
    public TreeNode Node { get; set; }
    public int NodeStartOffset { get; set; }
    public int NodeStartLineNumber { get; set; }
}

public partial class LastVisitedLine
{
    public int LineNumber { get; set; }
    public string Value { get; set; }
}

public partial class PieceTreeSearchCache
{
    public int Limit { get; set; }
    public List<CacheEntry> Cache { get; set; }
}

public partial class PieceTreeBase
{
    public TreeNode Root { get; set; }
    public List<StringBuffer> Buffers { get; set; }
    public int LineCount { get; set; }
    public int Length { get; set; }
    public string Eol { get; set; }
    public int EolLength { get; set; }
    public bool EolNormalised { get; set; }
    public BufferCursor LastChangedBufferPos { get; set; }
    public PieceTreeSearchCache SearchCache { get; set; }
    public LastVisitedLine LastVisitedLine { get; set; }
}

public partial class PieceTreeSnapshot
{
    public Piece[] Pieces { get; set; }
    public int Index { get; set; }
    public PieceTreeBase Tree { get; set; }
    public string Bom { get; set; }
}

enum DefaultEndOfLine { Lf = 1, CrLf = 2 }

public partial class PieceTreeTextBufferFactory
{
    public List<StringBuffer> Chunks { get; set; }
    public String Bom { get; set; }
    public int Cr { get; set; };
    public int Lf { get; set; }
    public int CrLf { get; set; }
    public bool NoramliseEol { get; set; }
}

public partial class PieceTreeTextBufferBuilder
{
    public List<StringBuffer> Chunks { get; set; }
    public string Bom { get; set; }
    public bool HasPreviousChar { get; set; }
    public int PreviousChar { get; set; }
    public List<int> TempLineStarts { get; set; }
    public int Cr { get; set; }
    public int Lf { get; set; }
    public int CrLf { get; set; }
}