

public struct GridPosition
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
}
