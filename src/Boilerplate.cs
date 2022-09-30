namespace PieceTree;

// This file contains boilerplate code that I find
// doesn't help understanding the data structure,
// such as the constructors, the generated equals override
// and the instance functions that just forward a call to
// a static function (added to maintain API compatibility
// with the VS Code implementation).
// Hopefully this makes the main file
// less cluttered and easier to read.

public partial class Position
{
    public Position(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public bool IsBefore(Position other)
    {
        return Position.IsBefore(this, other);
    }

    public bool IsBeforeOrEqual(Position other)
    {
        return Position.IsBeforeOrEqual(this, other);
    }

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

public partial class Range
{
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
}

public partial class BufferCursor
{
    public BufferCursor(int line, int column)
    {
        Line = line;
        Column = column;
    }
}

public partial class Piece
{
    public Piece(int bufferIndex, BufferCursor start, BufferCursor end, int length, int lineFeedCount)
    {
        BufferIndex = bufferIndex;
        Start = start;
        End = end;
        Length = length;
        LineFeedCount = lineFeedCount;
    }
}

public partial class StringBuffer
{
    public StringBuffer(string buffer, int[] lineStarts)
    {
        Buffer = buffer;
        LineStarts = lineStarts;
    }
}

