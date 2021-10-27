namespace PhysicsEngine.Structs
{
    public class Color
    {
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public int A { get; private set; }
        public float ANorm { get; private set; }
        public float RNorm { get; private set; }
        public float GNorm { get; private set; }
        public float BNorm { get; private set; }

        public static Color White = new Color(255, 255, 255, 255);
        public static Color Black = new Color(0, 0, 0, 255);
        public static Color Red = new Color(255, 0, 0, 255);
        public static Color Green = new Color(0, 255, 0, 255);
        public static Color Blue = new Color(0, 0, 255, 255);
        public static Color Yellow = new Color(255, 255, 0, 255);
        public static Color Cyan = new Color(0, 255, 255, 255);
        public static Color Magenta = new Color(255, 0, 255, 255);
        public static Color VeryDarkBlue = new Color(17, 23, 43, 255);

        public Color(int r, int g, int b, int a)
        {
            R = r;
            G = g;
            B = b;
            A = a;

            ANorm = A / 255f;
            RNorm = R / 255f;
            GNorm = G / 255f;
            BNorm = B / 255f;
        }
    }
}
