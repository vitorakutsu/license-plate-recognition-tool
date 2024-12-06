using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ProjEncontraPlaca
{
    class Otsu
    {
        //função é usada para calcular os valores q na equação
        private float Px(int ini, int fim, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = ini; i <= fim; i++)
                sum = sum + hist[i];

            return (float)sum;
        }

        //função é usada para calcular os valores médios na equação (mu)
        private float Mx(int ini, int fim, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = ini; i <= fim; i++)
                sum = sum + i * hist[i];

            return (float)sum;
        }

        //busca a posição do maior elemento no vetor
        private int findMax(float[] vec, int n)
        {
            float maior = 0;
            int pos=0;
            int i;

            for (i = 1; i < n - 1; i++)
            {
                if (vec[i] > maior)
                {
                    maior = vec[i];
                    pos = i;
                }
            }
            return pos;
        }

        //calcula o histograma da imagem
        unsafe private void getHistogram(byte* p, int width, int height, int stride, int[] hist)
        {
            hist.Initialize();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * 3; j += 3)
                {
                    hist[p[i * stride + j]]++;
                }
            }
        }

        //encontra threshold OTSU
        public int getOtsuThreshold(Bitmap bmp)
        {
            byte t=0;
	        float[] vet=new float[256];
            int[] hist=new int[256];
            vet.Initialize();

	        float p1,p2,p12;
	        int k;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();

                getHistogram(p,bmp.Width,bmp.Height,bmData.Stride, hist);

                //percorrer todos os valores t possíveis e maximiza a variação entre as classes
                for (k = 1; k < 255; k++)
                {
                    p1 = Px(0, k, hist);
                    p2 = Px(k + 1, 255, hist);
                    p12 = p1 * p2;
                    if (p12 == 0) 
                        p12 = 1;
                    float diff=(Mx(0, k, hist) * p2) - (Mx(k + 1, 255, hist) * p1);
                    vet[k] = (float)diff * diff / p12;
                }
            }
            bmp.UnlockBits(bmData);

            t = (byte)findMax(vet, 256);

            return t;
        }

        public void ConvertToGrayDMA(Bitmap imageBitmap)
        {
            int width = imageBitmap.Width;
            int height = imageBitmap.Height;
            int pixelSize = 3;
            int gs;

            // Bloqueia os dados do bitmap
            BitmapData bitmapData = imageBitmap.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapData.Stride - (width * pixelSize);

            unsafe
            {
                byte* ptr = (byte*)bitmapData.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(ptr);     // está armazenado dessa forma: b g r 
                        g = *(ptr + 1);
                        r = *(ptr + 2);

                        // Calcula o tom de cinza
                        gs = (int)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                        // Define o mesmo valor para os três canais
                        *(ptr++) = (byte)gs;
                        *(ptr++) = (byte)gs;
                        *(ptr++) = (byte)gs;
                    }
                    ptr += padding;
                }
            }

            // Desbloqueia os dados do bitmap
            imageBitmap.UnlockBits(bitmapData);
        }


        public void threshold(Bitmap bmp, int thresh)
        { 
            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*)(void*)bmData.Scan0.ToPointer();
                int h= bmp.Height;
                int w = bmp.Width;
                int ws = bmData.Stride;

                for (int i = 0; i < h; i++)
                {
                    byte *row=&p[i*ws];
                    for (int j = 0; j < w * 3; j += 3)
                    {
                        row[j] = (byte)((row[j] > (byte)thresh) ? 255 : 0);
                        row[j+1] = (byte)((row[j+1] > (byte)thresh) ? 255 : 0);
                        row[j + 2] = (byte)((row[j + 2] > (byte)thresh) ? 255 : 0);
                    }
                }
            }
            bmp.UnlockBits(bmData);
        }
    }
}

