using System.Text;

namespace Day25;

public static class Snafu
{
    public static long ToDecimal(string snafu)
    {
        int value = 0;
        for (int p = 0; p < snafu.Length; p++)
        {
            int index = snafu.Length - p - 1;
            value += snafu[index] switch
            {
                '=' => -2 * (int)Math.Pow(5, p),
                '-' => -1 * (int)Math.Pow(5, p),
                '0' => 0 * (int)Math.Pow(5, p),
                '1' => 1 * (int)Math.Pow(5, p),
                '2' => 2 * (int)Math.Pow(5, p),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return value;
    }

    public static string Sum(IEnumerable<string> snafus)
    {
        return snafus.Aggregate("0", Add);
    }

    public static string Add(string a, string b)
    {
        int length = Math.Max(a.Length, b.Length);
        a = a.PadLeft(length, '0');
        b = b.PadLeft(length, '0');
        
        var output = new StringBuilder();
        var carry = '0';
        for (int p = 0; p < length; p++)
        {
            int i = length - p - 1;
            var (carry1, result1) = Add(carry, a[i]);
            var (carry2, result2) = Add(result1, b[i]);
            
            carry = Add(carry1, carry2).Result;
            output.Insert(0, result2);
        }
        
        output.Insert(0, carry);
        while(output[0] == '0')
            output.Remove(0, 1);

        return output.ToString();
    }
    
    private static (char Carry, char Result) Add(char a, char b)
    {
        return (a, b) switch
        {
            ('=', '=') => ('-', '1'),
            ('=', '-') => ('-', '2'),
            ('=', '0') => ('0', '='),
            ('=', '1') => ('0', '-'),
            ('=', '2') => ('0', '0'),

            ('-', '=') => ('-', '2'),
            ('-', '-') => ('0', '='),
            ('-', '0') => ('0', '-'),
            ('-', '1') => ('0', '0'),
            ('-', '2') => ('0', '1'),

            ('0', '=') => ('0', '='),
            ('0', '-') => ('0', '-'),
            ('0', '0') => ('0', '0'),
            ('0', '1') => ('0', '1'),
            ('0', '2') => ('0', '2'),

            ('1', '=') => ('0', '-'),
            ('1', '-') => ('0', '0'),
            ('1', '0') => ('0', '1'),
            ('1', '1') => ('0', '2'),
            ('1', '2') => ('1', '='),

            ('2', '=') => ('0', '0'),
            ('2', '-') => ('0', '1'),
            ('2', '0') => ('0', '2'),
            ('2', '1') => ('1', '='),
            ('2', '2') => ('1', '-'),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static (char Charry, char Result) Multiply(char a, char b)
    {
        return (a, b) switch
        {
            ('=', '=') => ('1', '-'),
            ('=', '-') => ('0', '2'),
            ('=', '0') => ('0', '0'),
            ('=', '1') => ('0', '='),
            ('=', '2') => ('-', '1'),

            ('-', '=') => ('0', '2'),
            ('-', '-') => ('0', '1'),
            ('-', '0') => ('0', '0'),
            ('-', '1') => ('0', '-'),
            ('-', '2') => ('0', '='),

            ('0', '=') => ('0', '0'),
            ('0', '-') => ('0', '0'),
            ('0', '0') => ('0', '0'),
            ('0', '1') => ('0', '0'),
            ('0', '2') => ('0', '0'),

            ('1', '=') => ('0', '='),
            ('1', '-') => ('0', '-'),
            ('1', '0') => ('0', '0'),
            ('1', '1') => ('0', '1'),
            ('1', '2') => ('0', '2'),

            ('2', '=') => ('-', '1'),
            ('2', '-') => ('0', '='),
            ('2', '0') => ('0', '0'),
            ('2', '1') => ('0', '2'),
            ('2', '2') => ('1', '-'),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}