namespace PieceTree;

/// <summary>
/// Provides access to various CharCodes checked in the data structure.
/// </summary>
// Implementation detail: Static class with static readonly values 
// is used instead of an enum because getting an enum's (char) value 
// requires (char) unboxing, which is more computationally expensive
// and less ergonomic than simply if (myChr == CharCode.LineFeed).
// A restricted enum's greater type safety was not judged to outweigh this cost.
public static class CharCodes
{
    public static readonly char CarriageReturn = '\r';
    public static readonly char LineFeed = '\n';
}

/// <summary>
/// Represents a position in the text buffer. 
/// The full Position class is ported from VS Code,
/// providing access to methods not used in the PieceTree text buffer. 
/// </summary>
public class Position
{
    public int Line { get; }
    public int Column { get; }

    public Position(int line, int column)
    {
        Line = line;
        Column = column;
    }

    /// <summary>
    /// Returns a new position with the specifieed line and column.
    /// <summary>
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

    /// <summary>
    /// Returns a new position moved from the previous one by its parameters.
    /// </summary>
    private Position Delta(int moveLine = 0, int moveCol = 0)
    {
        int newLine = this.Line + moveLine;
        int newCol = this.Column + moveCol;
        newLine = newLine < 0 ? 0 : newLine;
        newCol = newCol < 0 ? 0 : newCol;
        return new Position(newLine, newCol);
    }

    /// <summary>
    /// Checks if the first position is before the second. 
    /// Returns false if they are equal.
    /// </summary>
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

    /// <summary>
    /// Checks if the first position is before the second.
    /// Returns true if they are equal.
    /// </summary>
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

    /// <summary>
    /// Calculates the difference between two positions.
    /// This is useful for sorting.
    /// </summary>
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

    // Implementation detail:
    // The VS Code API for the Position class
    // has instance methods which simply call the
    // static method with the same name, which this rewrite follows. 
    // You will not benefit from reading the remainder of this class
    // if you are simply trying to understand the data structure.
    #region InstanceMethods
    public bool IsBefore(Position other)
    {
        return Position.IsBefore(this, other);
    }

    public bool IsBeforeOrEqual(Position other)
    {
        return Position.IsBeforeOrEqual(this, other);
    }
    #endregion

    // Implementation detail:
    // C# does not by default compare class instances by value but by reference.
    // This equals method and the hash code override
    // changes the default comparison behaviour and can be ignored by developers
    // from other languages trying to understand the data structure.
    public override bool Equals(object? obj)
    {
        return obj is Position position &&
               Line == position.Line &&
               Column == position.Column;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Line, Column);
    }
}

public class Range
{
    public Position Start { get; set; }
    public Position End { get; set; }

    public Range(Position start, Position end)
    {
        if (start.IsBeforeOrEqual(end))
        {
            Start = start;
            End = end;
        }
        else
        {
            Start = start;
            End = end;
        }
    }

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

    #region InstanceMethods
    public bool IsEmpty()
    {
        return Range.IsEmpty(this);
    }

    public bool ContainsPosition(Position position)
    {
        return Range.ContainsPosition(this, position);
    }

    public bool ContainsPositionStrict(Position position)
    {
        return Range.ContainsPositionStrict(this, position);
    }

    public bool ContainsRange(Range range)
    {
        return Range.ContainsRange(this, range);
    }

    public bool ContainsRangeStrict(Range range)
    {
        return Range.ContainsRangeStrict(this, range);
    }

    public Range Union(Range range)
    {
        return Range.Union(this, range);
    }

    public Range Intersection(Range range)
    {
        return Range.Intersection(this, range);
    }

    public Range CollapseToStart()
    {
        return Range.CollapseToStart(this);
    }
    #endregion
}

public class BufferCursor
{
    public int Line { get; set; }
    public int Column { get; set; }

    public BufferCursor(int line, int column)
    {
        Line = line;
        Column = column;
    }

}

public class Piece
{
    public readonly int BufferIndex;
    public readonly BufferCursor Start;
    public readonly BufferCursor End;
    public readonly int Length;
    public readonly int LineFeedCount;

    public Piece(int bufferIndex, BufferCursor start, BufferCursor end, int length, int lineFeedCount)
    {
        BufferIndex = bufferIndex;
        Start = start;
        End = end;
        Length = length;
        LineFeedCount = lineFeedCount;
    }
}

public class LineStarts
{
    public List<uint> Lines { get; }
    public int Cr { get; }
    public int Lf { get; }
    public int CrLf { get; }
    public bool IsBasicAscii { get; }

    public LineStarts(List<uint> lines, int cr, int lf, int crLf, bool isBasicAscii)
    {
        Lines = lines;
        Cr = cr;
        Lf = lf;
        CrLf = crLf;
        IsBasicAscii = isBasicAscii;
    }

    public List<uint> CreateLineStartsFast(string str, bool isReadOnly = true)
    {
        List<uint> r = new() { 0u };
        int rLength = 1;

        for (int i = 0; i < str.Length; i++)
        {
            var chr = str[i];

            if (chr == CharCodes.CarriageReturn)
            {
                var nextLoopNum = i + 1;
                if (str.Length > nextLoopNum && str[nextLoopNum] == CharCodes.LineFeed)
                {
                    // \r\n case
                    r[rLength++] = (uint)i + 2;
                    i++; // skip \n
                }
                else
                {
                    // \r case
                    r[rLength++] = (uint)i + 1;
                }
            }
            else if (chr == CharCodes.LineFeed)
            {
                r[rLength++] = (uint)i + 1;
            }
        }

        if (isReadOnly)
        {
            // create a defensive copy of the list where writes don't matter
            return r.Skip(0).ToList();
        }
        else
        {
            return r;
        }
    }

    public LineStarts CreateLineStarts(List<uint> tempLineStarts, string str)
    {
        List<uint> r = new() { 0u };
        int rLength = 1;
        int cr = 0;
        int lf = 0;
        int crlf = 0;
        bool isBasicAscii = true;

        for (int i = 0; i < str.Length; i++)
        {
            char chr = str[i];

            if (chr == CharCodes.CarriageReturn)
            {
                var nextLoopNum = i + 1;
                if (str.Length > nextLoopNum && str[nextLoopNum] == CharCodes.LineFeed)
                {
                    // \r\n case
                    crlf++;
                    r[rLength++] = (uint)i + 2;
                    i++; // skip \n
                }
                else
                {
                    // \r case
                    cr++;
                    r[rLength++] = (uint)i + 1;
                }
            }
            else if (chr == CharCodes.LineFeed)
            {
                lf++;
                r[rLength++] = (uint)i + 1;
            }
            else
            {
                if (isBasicAscii)
                {

                }
            }
        }
    }
}

public class StringBuffer
{
    public string Buffer { get; set; }
    public List<int> LineStarts { get; set; }

    public StringBuffer(string buffer, List<int> lineStarts)
    {
        Buffer = buffer;
        LineStarts = lineStarts;
    }
}

public enum NodeColour { Red, Black }

public class TreeNode
{
    public TreeNode Parent { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    public NodeColour Colour { get; set; }

    public Piece Piece { get; set; }
    public int LeftSize { get; set; }
    public int LfLeft { get; set; }

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

public class NodePosition
{
    public TreeNode Node { get; set; }
    public int Remainder { get; set; }
    public int NodeStartLineNumber { get; set; }

    public NodePosition(TreeNode node, int remainder, int nodeStartLineNumber)
    {
        Node = node;
        Remainder = remainder;
        NodeStartLineNumber = nodeStartLineNumber;
    }
}

public class CacheEntry
{
    public TreeNode Node { get; set; }
    public int NodeStartOffset { get; set; }
    public int? NodeStartLineNumber { get; set; }

    public CacheEntry(TreeNode node, int nodeStartOffset, int? nodeStartLineNumber)
    {
        Node = node;
        NodeStartOffset = nodeStartOffset;
        NodeStartLineNumber = nodeStartLineNumber;
    }
}

public class LastVisitedLine
{
    public int LineNumber { get; set; }
    public string Value { get; set; }

    public LastVisitedLine(int lineNumber, string value)
    {
        LineNumber = lineNumber;
        Value = value;
    }
}

public class PieceTreeSearchCache
{
    public int Limit { get; set; }
    public List<CacheEntry> Cache { get; set; }

    public PieceTreeSearchCache(int limit, List<CacheEntry> cache)
    {
        Limit = limit;
        Cache = new List<CacheEntry>();
    }

    public CacheEntry? Get(int offset)
    {
        for (int i = Cache.Count - 1; i >= 0; i--)
        {
            var nodePos = Cache[i];
            if (nodePos.NodeStartOffset <= offset && nodePos.NodeStartOffset + nodePos.Node.Piece.Length >= offset)
            {
                return nodePos;
            }
        }
        return null;
    }

    public CacheEntry? Get2(int lineNumber)
    {
        for (int i = Cache.Count; i >= 0; i++)
        {
            var nodePos = Cache[i];
            if (nodePos.NodeStartLineNumber is not null)
            {
                if (nodePos.NodeStartLineNumber < lineNumber && nodePos.Node.Piece.LineFeedCount >= lineNumber)
                {
                    return nodePos;
                }
            }
        }
        return null;
    }

    public void Set(CacheEntry nodePos)
    {
        if (Cache.Count >= Limit)
        {
            Cache = Cache.Skip(1).ToList();
        }
        Cache.Add(nodePos);
    }

    public void Validate(int offset)
    {
        bool hasInvalidVal = false;
        // Create copy of cache which has same values but is different object)
        List<CacheEntry?> temp = new();
        foreach (var item in Cache)
        {
            temp.Add(new CacheEntry(item.Node, item.NodeStartOffset, item.NodeStartLineNumber));
        }
        for (int i = 0; i < temp.Count; i++)
        {
            var nodePos = temp[i];
            if (nodePos is not null)
            {
                if ((nodePos.Node.Parent == nodePos.Node.Parent.Left && nodePos.Node.Parent == nodePos.Node.Parent.Right || nodePos.NodeStartOffset >= offset))
                {
                    temp[i] = null;
                    hasInvalidVal = true;
                    continue;
                }
            }

            if (hasInvalidVal)
            {
                List<CacheEntry> newArr = new();
                foreach (var entry in temp)
                {
                    if (entry is not null)
                    {
                        newArr.Add(entry);
                    }
                }
                Cache = newArr;
            }
        }
    }
}

public class PieceTreeBase
{
    public TreeNode? Root { get; set; }
    public List<StringBuffer> Buffers { get; set; }
    public int LineCount { get; set; }
    public int Length { get; set; }
    public string Eol { get; set; }
    public int EolLength { get; set; }
    public bool EolNormalised { get; set; }
    public BufferCursor LastChangedBufferPos { get; set; }
    public PieceTreeSearchCache SearchCache { get; set; }
    public LastVisitedLine LastVisitedLine { get; set; }

    public PieceTreeBase(List<StringBuffer> chunks, string eol, bool eolNormalised)
    {
        Create(chunks, eol, eolNormalised);
    }

    private void Create(List<StringBuffer> chunks, string eol, bool eolNormalised)
    {
        Buffers = new List<StringBuffer>()
        {
            new StringBuffer("", new List<int> {0})
        };
        LastChangedBufferPos = new BufferCursor(0, 0);
        Root = null;
        LineCount = 1;
        Length = 0;
        Eol = eol;
        EolLength = eol.Length;
        EolNormalised = eolNormalised;

        TreeNode? lastNode = null;

        for (int i = 0; i < chunks.Count; i++)
        {
            var len = chunks.Count;
            if (chunks[i].Buffer.Length > 0)
            {
                // Need to add LineStarts class somewhere.
                // chunks[i].LineStarts = LineStarts.CreateLineStartsFast(chunks[i].Buffer);
            }
        }
    }
}

public class PieceTreeSnapshot
{
    public Piece[] Pieces { get; set; }
    public int Index { get; set; }
    public PieceTreeBase Tree { get; set; }
    public string Bom { get; set; }
}

enum DefaultEndOfLine { Lf = 1, CrLf = 2 }

public class PieceTreeTextBufferFactory
{
    public List<StringBuffer> Chunks { get; set; }
    public String Bom { get; set; }
    public int Cr { get; set; };
    public int Lf { get; set; }
    public int CrLf { get; set; }
    public bool NoramliseEol { get; set; }
}

public class PieceTreeTextBufferBuilder
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
