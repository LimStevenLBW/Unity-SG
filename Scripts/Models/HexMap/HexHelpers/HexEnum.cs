public enum HexDirection
{
    NE, E, SE, SW, W, NW
}

//Connection Types
public enum HexEdgeType
{
    Flat, Slope, Cliff
}

/*
 * An extension method is a static method inside a static class that behaves like an instance method of some type. 
 * That type could be anything, a class, an interface, a struct, a primitive value, or an enum. 
 * The first argument of an extension method needs to have the this keyword. It defines the type and instance value that the method will operate on.
 * Does this allow us to add methods to everything? 
 * Yes, just like you could write any static method that has any type as its argument. Is this a good idea? When used in moderation, it can be.
 * It is a tool that has its uses, but wielding it with abandon will produce an unstructured mess.
*/
public static class HexDirectionExtensions
{
 

    //Retrieves the opposite hexdirection enum
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
    public static HexDirection Previous2(this HexDirection direction)
    {
        direction -= 2;
        return direction >= HexDirection.NE ? direction : (direction + 6);
    }

    public static HexDirection Next2(this HexDirection direction)
    {
        direction += 2;
        return direction <= HexDirection.NW ? direction : (direction - 6);
    }
}