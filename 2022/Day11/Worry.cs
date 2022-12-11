namespace Day11;

public struct Worry
{
    public int Mod2 { get; private init; }
    public int Mod3 { get; private init; }
    public int Mod5 { get; private init; }
    public int Mod7 { get; private init; }
    public int Mod11 { get; private init; }
    public int Mod13 { get; private init; }
    public int Mod17 { get; private init; }
    public int Mod19 { get; private init; }

    private Worry(int value)
    {
        Mod2 = value % 2;
        Mod3 = value % 3;
        Mod5 = value % 5;
        Mod7 = value % 7;
        Mod11 = value % 11;
        Mod13 = value % 13;
        Mod17 = value % 17;
        Mod19 = value % 19;
    }

    public static implicit operator Worry(int value) => new(value);

    public static Worry operator +(Worry a, Worry b)
    {
        return new Worry
        {
            Mod2 = (a.Mod2 + b.Mod2) % 2,
            Mod3 = (a.Mod3 + b.Mod3) % 3,
            Mod5 = (a.Mod5 + b.Mod5) % 5,
            Mod7 = (a.Mod7 + b.Mod7) % 7,
            Mod11 = (a.Mod11 + b.Mod11) % 11,
            Mod13 = (a.Mod13 + b.Mod13) % 13,
            Mod17 = (a.Mod17 + b.Mod17) % 17,
            Mod19 = (a.Mod19 + b.Mod19) % 19
        };
    }
    
    public static Worry operator *(Worry a, Worry b)
    {
        return new Worry
        {
            Mod2 = (a.Mod2 * b.Mod2) % 2,
            Mod3 = (a.Mod3 * b.Mod3) % 3,
            Mod5 = (a.Mod5 * b.Mod5) % 5,
            Mod7 = (a.Mod7 * b.Mod7) % 7,
            Mod11 = (a.Mod11 * b.Mod11) % 11,
            Mod13 = (a.Mod13 * b.Mod13) % 13,
            Mod17 = (a.Mod17 * b.Mod17) % 17,
            Mod19 = (a.Mod19 * b.Mod19) % 19
        };
    }
}