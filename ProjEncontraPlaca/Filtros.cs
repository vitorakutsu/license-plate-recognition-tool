using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjEncontraPlaca
{
    class Filtros
    {
        private static void segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
        {
            Point menor = new Point(), maior = new Point(), patual = new Point();
            Queue<Point> fila = new Queue<Point>();
            menor.X = maior.X = ini.X;
            menor.Y = maior.Y = ini.Y;
            fila.Enqueue(ini);
            while (fila.Count != 0)
            {
                patual = fila.Dequeue();
                imageBitmapSrc.SetPixel(patual.X, patual.Y, Color.FromArgb(255, 0, 0));
                imageBitmapDest.SetPixel(patual.X, patual.Y, cor_pintar);

                if (patual.X < menor.X)
                    menor.X = patual.X;
                if (patual.X > maior.X)
                    maior.X = patual.X;
                if (patual.Y < menor.Y)
                    menor.Y = patual.Y;
                if (patual.Y > maior.Y)
                    maior.Y = patual.Y;

                if (patual.X > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X - 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X - 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y - 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y - 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y - 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X < imageBitmapSrc.Width - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.X < imageBitmapSrc.Width - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X + 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X + 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y < imageBitmapSrc.Height - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y < imageBitmapSrc.Height - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y + 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y + 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y + 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }

            }
            
            if (menor.X > 0)
                menor.X--;
            if (maior.X < imageBitmapSrc.Width - 1)
                maior.X++;
            if (menor.Y > 0)
                menor.Y--;
            if (maior.Y < imageBitmapSrc.Height - 1)
                maior.Y++;
            desenhaRetangulo(imageBitmapDest, menor, maior, Color.FromArgb(255, 0, 0));
            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        public static void segmentar8conectado(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    if (r == 0)
                        segmenta8(imageBitmapSrc, imageBitmapDest, new Point(x, y), listaPini, listaPfim, Color.FromArgb(100, 100, 100));
                }
            }
        }

        private static void desenhaRetangulo(Bitmap imageBitmapDest, Point menor, Point maior, Color cor)
        {
            for (int x = menor.X; x <= maior.X; x++)
            {
                imageBitmapDest.SetPixel(x, menor.Y, cor);
                imageBitmapDest.SetPixel(x, maior.Y, cor);
            }
            for (int y = menor.Y; y <= maior.Y; y++)
            {
                imageBitmapDest.SetPixel(menor.X, y, cor);
                imageBitmapDest.SetPixel(maior.X, y, cor);
            }
        }
        
        public static void encontra_placa(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            List<Point> listaPini = new List<Point>();
            List<Point> listaPfim = new List<Point>();
            List<Point> listaPontosInicialDivisaoImagem = new List<Point>();
            List<Point> listaPontosFinalDivisaoImagem = new List<Point>();
            List<Point> listaPontosInicioParaFiltrar = new List<Point>();
            List<Point> listaPontosFinalParaFiltrar = new List<Point>();
            List<Point> _listaPini = new List<Point>();
            List<Point> _listaPfim = new List<Point>();
            List<Point> subListaPini = new List<Point>();
            List<Point> subListaPfim = new List<Point>();

            int cont = 0, altura, largura, qtdLinhasDivisao = 3, tentativas = 0;
            bool primeiraIteracao = true;
            
            Otsu otsu = new Otsu();

            // Convertendo a imagem para escala de cinza e aplicando o limiar de Otsu
            otsu.ConvertToGrayDMA(imageBitmapDest);
            int otsuThreshold = otsu.getOtsuThreshold(imageBitmapDest);
            otsu.threshold(imageBitmapDest, otsuThreshold);

            // Loop principal para encontrar a placa
            while (cont <= 7 && tentativas < 6)
            {
                if (primeiraIteracao)
                {
                    Bitmap imageBitmap = (Bitmap)imageBitmapDest.Clone();
                    Filtros.segmentar8conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);
                    
                    for (int i = 0; i < listaPini.Count; i++)
                    {
                        altura = listaPfim[i].Y - listaPini[i].Y;
                        largura = listaPfim[i].X - listaPini[i].X;

                        if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                        {
                            _listaPini.Add(listaPini[i]);
                            _listaPfim.Add(listaPfim[i]);

                            listaPontosInicioParaFiltrar.Add(listaPini[i]);
                            listaPontosFinalParaFiltrar.Add(listaPfim[i]);
                        
                            Filtros.desenhaRetangulo(imageBitmapDest, listaPini[i], listaPfim[i], Color.Green);
                            cont++;
                        }
                    }
                }
                else
                {
                    if (cont != 7)
                    {
                        if (cont >= 1 && _listaPini.Count > 0 && _listaPfim.Count > 0)
                        {
                            Point pontoInicial = new Point(0, _listaPini[0].Y - 10);
                            Point pontoFinal = new Point(imageBitmapDest.Width - 1, _listaPfim[_listaPfim.Count - 1].Y);
                            
                            int x = Math.Max(0, pontoInicial.X);
                            int y = Math.Max(0, pontoInicial.Y);
                            int width = Math.Min(imageBitmapSrc.Width - x, pontoFinal.X - pontoInicial.X + 1);
                            int height = Math.Min(imageBitmapSrc.Height - y, pontoFinal.Y - pontoInicial.Y + 1);

                            if (width > 0 && height > 0)
                            {
                                try
                                {
                                    // Criar a região da placa corretamente
                                    Rectangle region = new Rectangle(x, y, width, height);
                                    Bitmap placaRegiao = imageBitmapSrc.Clone(region, imageBitmapSrc.PixelFormat);
                                    
                                    placaRegiao.Save("C:\\Users\\Pedro Filitto\\Downloads\\RegiaoPlaca.png", ImageFormat.Png);
                                    
                                    otsu.ConvertToGrayDMA(placaRegiao);
                                    otsuThreshold = otsu.getOtsuThreshold(placaRegiao);
                                    otsu.threshold(placaRegiao, otsuThreshold);
                                    
                                    placaRegiao.Save("C:\\Users\\Pedro Filitto\\Downloads\\RegiaoPlacaOtsu.png", ImageFormat.Png);
                                    
                                    Bitmap placaRegiaoDilatada = (Bitmap)placaRegiao.Clone();
                                    
                                    //DilatarImagem(placaRegiao, placaRegiaoDilatada, new ElementoEstruturante());
                                    
                                    placaRegiaoDilatada.Save("C:\\Users\\Pedro Filitto\\Downloads\\RegiaoPlacaDilatada.png", ImageFormat.Png);

                                    listaPini.Clear();
                                    listaPfim.Clear();

                                    Filtros.segmentar8conectado(placaRegiaoDilatada, placaRegiaoDilatada, listaPini, listaPfim);
                                    
                                    placaRegiao.Save("C:\\Users\\Pedro Filitto\\Downloads\\RegiaoPlacaSegmentada.png", ImageFormat.Png);
                                    
                                    for (int i = 0; i < listaPini.Count; i++)
                                    {
                                        altura = listaPfim[i].Y - listaPini[i].Y;
                                        largura = listaPfim[i].X - listaPini[i].X;

                                        if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                                        {
                                            listaPontosInicioParaFiltrar.Add(listaPini[i]);
                                            listaPontosFinalParaFiltrar.Add(listaPfim[i]);
                                            
                                            Filtros.desenhaRetangulo(placaRegiaoDilatada, listaPini[i], listaPfim[i], Color.Green);
                                            
                                            placaRegiaoDilatada.Save("C:\\Users\\Pedro Filitto\\Downloads\\RegiaoPlacaFiltrada.png", ImageFormat.Png);
                                            
                                            using (Graphics g = Graphics.FromImage(imageBitmapDest))
                                            {
                                                Rectangle targetRegion = new Rectangle(region.X, region.Y, placaRegiaoDilatada.Width, placaRegiaoDilatada.Height);
                                                g.DrawImage(placaRegiaoDilatada, targetRegion);
                                            }
                                            
                                            cont++;
                                        }
                                    }
                                    
                                    placaRegiao.Dispose();
                                }
                                catch (OutOfMemoryException ex)
                                {
                                    Console.WriteLine("Erro de memória ao clonar a região: " + ex.Message);
                                }
                            }
                        }
                        else
                        {
                            DividirImagemEmEspacosIguais(imageBitmapSrc, imageBitmapSrc.Height / qtdLinhasDivisao, listaPontosInicialDivisaoImagem, listaPontosFinalDivisaoImagem);

                            // Itera por cada divisão criada
                            for (int i = 0; i < listaPontosInicialDivisaoImagem.Count; i++)
                            {
                                // Define o retângulo da subimagem com base nos pontos da divisão
                                Rectangle retangulo = new Rectangle(
                                    listaPontosInicialDivisaoImagem[i].X,
                                    listaPontosInicialDivisaoImagem[i].Y,
                                    listaPontosFinalDivisaoImagem[i].X - listaPontosInicialDivisaoImagem[i].X,
                                    listaPontosFinalDivisaoImagem[i].Y - listaPontosInicialDivisaoImagem[i].Y
                                );

                                Bitmap subImagem = imageBitmapSrc.Clone(retangulo, imageBitmapSrc.PixelFormat);
                                
                                otsu.ConvertToGrayDMA(subImagem);
                                otsuThreshold = otsu.getOtsuThreshold(subImagem);
                                otsu.threshold(subImagem, otsuThreshold);
                                Bitmap subImagemClone = (Bitmap)subImagem.Clone();
                                
                                segmentar8conectado(subImagem, subImagemClone, subListaPini, subListaPfim);
                                
                                for (int j = 0; j < subListaPini.Count; j++)
                                {
                                    altura = subListaPfim[j].Y - subListaPini[j].Y;
                                    largura = subListaPfim[j].X - subListaPini[j].X;

                                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                                    {
                                        _listaPini.Add(subListaPini[j]);
                                        _listaPfim.Add(subListaPfim[j]);
                                        
                                        listaPontosInicioParaFiltrar.Add(subListaPini[j]);
                                        listaPontosFinalParaFiltrar.Add(subListaPfim[j]);
                                        
                                        //Validar se o pixel que irá ser pintado está dentro dos limites da imagem
                                        if (subListaPfim[j].X < subImagemClone.Width &&
                                            subListaPfim[j].Y < subImagemClone.Height)
                                        {
                                            Filtros.desenhaRetangulo(subImagem, subListaPini[j], subListaPfim[j], Color.Green);
                                        }
                                        
                                        using (Graphics g = Graphics.FromImage(imageBitmapDest))
                                        {
                                            Rectangle targetRegion = new Rectangle(retangulo.X, retangulo.Y, subImagem.Width, subImagem.Height);
                                            g.DrawImage(subImagem, targetRegion);
                                        }
                                    }
                                }
                            }

                            // Se nenhum dígito foi encontrado, aumenta a quantidade de divisões
                            if (cont == 0)
                            {
                                qtdLinhasDivisao++;
                            }
                        }
                    }
                }
                
                if (cont != 7)
                {
                    Console.WriteLine("Não foi possível identificar todos os dígitos da placa.");
                }
                
                primeiraIteracao = false;
                tentativas++;
            }

            //FiltrarListaPontosCaracteres(listaPontosInicioParaFiltrar, listaPontosFinalParaFiltrar);
        }

        public static void DilatarImagem(Bitmap image, Bitmap imageDest, ElementoEstruturante elementoEstruturante)
        {
            for (int y = elementoEstruturante.y; y < image.Height - elementoEstruturante.matriz.GetLength(0) + 1; y++)
            {
                for (int x = elementoEstruturante.x; x < image.Width - elementoEstruturante.matriz.GetLength(1) + 1; x++)
                {
                    if (MatchMask(image, elementoEstruturante, x, y))
                    {
                        imageDest.SetPixel(x, y, Color.Black);
                    }
                }
            }
        }

        public static bool MatchMask(Bitmap imagem, ElementoEstruturante elementoEstruturante, int x, int y)
        {
            int x2, y2;

            x2 = x - elementoEstruturante.x;
            y2 = y - elementoEstruturante.y;
            
            for (int i = 0; i < elementoEstruturante.matriz.GetLength(0); i++)
            {
                for (int j = 0; j < elementoEstruturante.matriz.GetLength(1); j++)
                {
                    if (elementoEstruturante.matriz[i, j] == 1 && Utils.Preto(imagem.GetPixel(x2 + j, y2 + i)))
                        return true;
                }
            }

            return false;
        }


        public static void DividirImagemEmEspacosIguais(Bitmap imageBitmap, int espacosEntreLinhas, List<Point> listaPontosInicial, List<Point> listaPontosFinal)
        {
            listaPontosFinal.Clear();
            listaPontosInicial.Clear();
            
            int alturaImagem = imageBitmap.Height;
            int larguraImagem = imageBitmap.Width;
            
            int qtdDivisoes = alturaImagem / espacosEntreLinhas;
            
            for (int i = 0; i < qtdDivisoes; i++)
            {
                Point pontoInicial = new Point(0, i * espacosEntreLinhas);
                
                Point pontoFinal = new Point(larguraImagem, (i + 1) * espacosEntreLinhas);
                
                if (i == qtdDivisoes - 1 && pontoFinal.Y > alturaImagem)
                {
                    pontoFinal.Y = alturaImagem;
                }
                
                listaPontosInicial.Add(pontoInicial);
                listaPontosFinal.Add(pontoFinal);
            }
            
            if (alturaImagem % espacosEntreLinhas != 0)
            {
                Point pontoInicialExtra = new Point(0, qtdDivisoes * espacosEntreLinhas);
                Point pontoFinalExtra = new Point(larguraImagem, alturaImagem);

                listaPontosInicial.Add(pontoInicialExtra);
                listaPontosFinal.Add(pontoFinalExtra);
            }
        }

        //sem acesso direto a memoria
        public static void threshold(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.1140 + g * 0.5870 + b * 0.2990);
                    if (gs > 127)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void countour(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int x, y, x2, y2, aux_cR;
            bool flag;

            Color corB = Color.FromArgb(255, 255, 255);
            for (y = 0; y < height; y++)
                for (x = 0; x < width; x++)
                    imageBitmapDest.SetPixel(x, y, corB);

            Bitmap imageBranca = (Bitmap)imageBitmapDest.Clone();

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width - 1; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);
                    Color cor2 = imageBitmapSrc.GetPixel(x + 1, y);
                    if (cor.R == 255 && cor2.R == 0 && imageBitmapDest.GetPixel(x + 1, y).R == 255)
                    {
                        Bitmap imageAux = (Bitmap)imageBranca.Clone();
                        x2 = x;
                        y2 = y;
                        do
                        {
                            Color p0, p1, p2, p3, p4, p5, p6, p7;
                            imageBitmapDest.SetPixel(x2, y2, Color.FromArgb(0, 0, 0));
                            imageAux.SetPixel(x2, y2, Color.FromArgb(1, 1, 1));

                            p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                            p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                            p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                            p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                            p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                            p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                            p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                            p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);

                            if (p1.R == 255 && p0.R == 255 && p2.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p3.R == 255 && p4.R == 0 && p2.R == 255)
                            {
                                x2 = x2 - 1;
                                y2 = y2 - 1;
                            }
                            else
                             if (p5.R == 255 && p4.R == 255 && p6.R == 0)
                            {
                                x2 = x2 - 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p7.R == 255 && p6.R == 255 && p0.R == 0)
                            {
                                x2 = x2 + 1;
                                y2 = y2 + 1;
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R != 2)
                            {
                                x2 = x2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p1 = imageBitmapSrc.GetPixel(x2 + 1, y2 - 1);
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p0.R == 255 && p2.R == 0 && p1.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p0.R == 255 && p2.R == 0 && p1.R == 0 && imageAux.GetPixel(x2 + 1, y2).R == 2)
                                x2 = x2 + 1;
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R != 2)
                            {
                                y2 = y2 - 1;
                                flag = true;
                                do
                                {
                                    p2 = imageBitmapSrc.GetPixel(x2, y2 - 1);
                                    p3 = imageBitmapSrc.GetPixel(x2 - 1, y2 - 1);
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p2.R == 255 && p4.R == 0 && p3.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p2.R == 255 && p4.R == 0 && p3.R == 0 && imageAux.GetPixel(x2, y2 - 1).R == 2)
                                y2 = y2 - 1;
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R != 2)
                            {
                                x2 = x2 - 1;
                                flag = true;
                                do
                                {
                                    p4 = imageBitmapSrc.GetPixel(x2 - 1, y2);
                                    p5 = imageBitmapSrc.GetPixel(x2 - 1, y2 + 1);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p4.R == 255 && p5.R == 0 && p6.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        x2 = x2 - 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p4.R == 255 && p5.R == 0 && p6.R == 0 && imageAux.GetPixel(x2 - 1, y2).R == 2)
                                x2 = x2 - 1;
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R != 2)
                            {
                                y2 = y2 + 1;
                                flag = true;
                                do
                                {
                                    p0 = imageBitmapSrc.GetPixel(x2 + 1, y2);
                                    p6 = imageBitmapSrc.GetPixel(x2, y2 + 1);
                                    p7 = imageBitmapSrc.GetPixel(x2 + 1, y2 + 1);
                                    aux_cR = imageAux.GetPixel(x2, y2).R;
                                    if (p6.R == 255 && p0.R == 0 && p7.R == 0 && aux_cR == 1)
                                    {
                                        imageAux.SetPixel(x2, y2, Color.FromArgb(2, 2, 2));
                                        y2 = y2 + 1;
                                    }
                                    else
                                        flag = false;
                                } while (flag);
                            }
                            else
                            if (p6.R == 255 && p0.R == 0 && p7.R == 0 && imageAux.GetPixel(x2, y2 + 1).R == 2)
                                y2 = y2 + 1;
                        }
                        while (x != x2 || y != y2);
                    }
                }
            }
        }

        public static void brancoPreto(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    if (gs > 220)
                        gs = 255;
                    else
                        gs = 0;

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }
    }
}
