using System.Drawing;

namespace ProjEncontraPlaca
{
    public class Utils
    {
        public static bool Branco(Color cor)
        {
           return cor.R == 255 && cor.G == 255 && cor.B == 255; 
        }
        
        public static bool Preto(Color cor)
        {
            return cor.R == 0 && cor.G == 0 && cor.B == 0;
        }
    }
}