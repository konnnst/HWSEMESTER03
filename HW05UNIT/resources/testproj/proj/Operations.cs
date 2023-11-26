using System.Diagnostics;

namespace Proj;

public class Operations
{
    public static double Plus(int a, int b) => a + b;
    public static double Minus(int a, int b) => a - b;
    public static double Mult(int a, int b) => a * b;
    public static double Div(int a, int b)
    {
        if (b == 0)
        {
            throw new Exception("Zero division");
        }
        else
        {
            return a / b;
        }
    }
}

internal class EntryPoint
{
    private static void Main()
    {
        
    }
}