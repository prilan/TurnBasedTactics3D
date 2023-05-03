using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("\"({x}, {y})\"")]
public struct Int2
{
    public int x;
    public int y;

    public Int2(int xVal, int yVal)
    {
        x = xVal;
        y = yVal;
    }

    public static Int2 operator +(Int2 value1, Int2 value2)
    {
        return new Int2(value1.x + value2.x, value1.y + value2.y);
    }

    public static Int2 operator -(Int2 value1, Int2 value2)
    {
        return new Int2(value1.x - value2.x, value1.y - value2.y);
    }

    public static Vector2 operator *(Int2 value1, float value2)
    {
        return new Vector2(value1.x * value2, value1.y * value2);
    }

    public static implicit operator Vector2(Int2 value)
    {
        return new Vector2 (value.x, value.y);
    }

    public override string ToString()
    {
        base.ToString();

        return "(" + x + ", " + y + ")";
    }
}
