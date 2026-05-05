namespace OverrloadingOperator
{
    public class  Point
    {

        public int X { get; set; }

        public int Y { get; set; }


        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

            public static Point operator -(Point p1, Point p2)
            {
                return new Point(p1.X - p2.X, p1.Y - p2.Y);
            }
    
            public override string ToString()
            {
                return $"({X}, {Y})";
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Point p1 = new Point(2, 3);
            Point p2 = new Point(4, 5);

            Point sum = p1 + p2;

            Console.WriteLine(sum.ToString());

           
        }
    }
}
