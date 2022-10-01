namespace PieceTree;

// This file contains boilerplate code that I find
// doesn't help understanding the data structure:
// namely, the instance functions which just call a static function,
// as seen in the API provided by VS Code's implementation.

public partial class Position
{
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

