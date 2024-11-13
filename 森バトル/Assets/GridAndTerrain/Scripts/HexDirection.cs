
//This script handles different directions relative to a cell. It contains methods that allow other scripts to get certain relative directions.

//A list of possible directions, starting from NorthEast, going clockwise to NorthWest.
public enum HexDirection
{
   NE, E, SE, SW, W, NW
}

public static class HexDirectionExtensions
{
    //Returns the opposite direction.
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    //Returns the next direction counterclockwise from the input.
    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    //Returns the next direction clockwise from the input.
    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}