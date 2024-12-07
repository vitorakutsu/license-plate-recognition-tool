namespace ProjEncontraPlaca
{
    public class ElementoEstruturante
    {
        public int[,] matriz;
        public int x;
        public int y;

        public ElementoEstruturante()
        {
            this.matriz = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            this.x = 1;
            this.y = 1;
        }
    }
}