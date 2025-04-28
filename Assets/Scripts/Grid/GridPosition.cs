

using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int X;
    public int Z;

    public GridPosition(int x, int z)
    {
        this.X = x;
        this.Z = z;
    }

    public override string ToString()
    {
        return $"X: {X}, Z: {Z}";
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.X == b.X && a.Z == b.Z;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               X == position.X &&
               Z == position.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Z);
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }
}
