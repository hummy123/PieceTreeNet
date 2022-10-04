namespace PieceTree;

public partial class Position
{
    public int Line { get; }
    public int Column { get; }

    public Position(int line, int column)
    {
        Line = line;
        Column = column;
    }

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
}

public partial class BufferCursor
{
    public int Line { get; set; }
    public int Column { get; set; }

    public BufferCursor(int line, int column)
    {
        Line = line;
        Column = column;
    }

}

public partial class Piece
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
            var len = str.Length;
            var chr = str[i];

            if (chr == EndOfLine.Cr[0])
            {
                var nextLoopNum = i + 1;
                if (str.Length > nextLoopNum && str[nextLoopNum] == EndOfLine.Lf[0])
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
            else if (chr == EndOfLine.Lf[0])
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

    public LineStarts CreateLineStarts(List<uint> r, string str)
    {

    }
}

public partial class StringBuffer
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

public partial class TreeNode
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

// Because C# doesn't have ergonomics for enums with non-integral values,
// we use a static class to represent EndOfLine.
// This is not type-safe in the sense that the compiler will not give
// warnings for usage that does not align with the design intent.
public static class EndOfLine
{
    public static string Cr = "\r";
    public static string Lf = "\n";
    public static string CrLf = "\r\n";
}

public partial class PieceTreeBase
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
