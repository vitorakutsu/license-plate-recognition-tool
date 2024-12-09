using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace ProjEncontraPlaca
{
    class Filtros
    {
        private static void Segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
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
            //DesenhaRetangulo(imageBitmapDest, menor, maior, Color.FromArgb(255, 0, 0));
            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        public static void Segmenta8Conectado(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim)
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
                        Segmenta8(imageBitmapSrc, imageBitmapDest, new Point(x, y), listaPini, listaPfim, Color.FromArgb(100, 100, 100));
                }
            }
        }

        private static void DesenhaRetangulo(Bitmap imageBitmapDest, Point menor, Point maior, Color cor)
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

        public static void AplicarMascaraDeslizante(Bitmap placaRegiao, Bitmap imageBitmapDest, Otsu otsu, Rectangle region, ref int cont, List<Point> listaPontosInicioParaFiltrar, List<Point> listaPontosFinalParaFiltrar)
        {
            int larguraMascara = 240;
            bool ok = false;

            List<(Bitmap image, int cont)> melhores = new List<(Bitmap image, int cont)>();

            for (int x = 0; x <= placaRegiao.Width - larguraMascara && !ok; x += 10)
            {
                cont = 0;
                listaPontosInicioParaFiltrar.Clear();
                listaPontosFinalParaFiltrar.Clear();
                
                Rectangle mascara = new Rectangle(x, 0, larguraMascara, placaRegiao.Height);
                Bitmap subImagem = placaRegiao.Clone(mascara, placaRegiao.PixelFormat);
                Bitmap melhor = new Bitmap(subImagem);

                otsu.ConvertToGrayDMA(subImagem);
                int otsuThreshold = otsu.getOtsuThreshold(subImagem);
                otsu.threshold(subImagem, otsuThreshold);

                List<Point> listaPiniMascara = new List<Point>();
                List<Point> listaPfimMascara = new List<Point>();

                Filtros.Segmenta8Conectado(subImagem, subImagem, listaPiniMascara, listaPfimMascara);

                foreach (var ini in listaPiniMascara)
                {
                    int altura = listaPfimMascara[listaPiniMascara.IndexOf(ini)].Y - ini.Y;
                    int largura = listaPfimMascara[listaPiniMascara.IndexOf(ini)].X - ini.X;

                    if (altura > 15 && altura < 27 && largura > 3 && largura < 35)
                    {
                        cont++;
                        
                        listaPontosInicioParaFiltrar.Add(ini);
                        listaPontosFinalParaFiltrar.Add(listaPfimMascara[listaPiniMascara.IndexOf(ini)]);

                        Filtros.DesenhaRetangulo(subImagem, ini, listaPfimMascara[listaPiniMascara.IndexOf(ini)], Color.Green);

                        if (cont == 7)
                            subImagem.Save("C:\\Users\\VITOR\\Documents\\PlacasDeCarros\\Placas\\MelhorPlaca.png", ImageFormat.Png);;

                        using (Graphics g = Graphics.FromImage(imageBitmapDest))
                        {
                            Rectangle targetRegion = new Rectangle(x, region.Y, subImagem.Width, subImagem.Height);
                            g.DrawImage(subImagem, targetRegion);
                        }
                    }
                }

                melhores.Add((melhor, cont));

                subImagem.Dispose();

                if (cont >= 7)
                    ok = true;
            }

            melhores.Sort((a, b) => b.cont.CompareTo(a.cont));
            int i = 0;
      
            ok = false;

            if (cont < 7)
            {
                listaPontosInicioParaFiltrar.Clear();
                listaPontosFinalParaFiltrar.Clear();
                foreach (var melhor in melhores)
                {
                    Bitmap melhorImagem = melhor.image;
                    int alturaBase = 30;
                    for (int y = 0; y <= placaRegiao.Height - alturaBase && !ok; y += 5) // Incremento de 1
                    {
                        cont = 0;
                        listaPontosInicioParaFiltrar.Clear();
                        listaPontosFinalParaFiltrar.Clear();
                        
                        int alturaReal = Math.Min(alturaBase, melhorImagem.Height - y);
                        Rectangle mascaraAltura = new Rectangle(0, y, melhorImagem.Width, alturaReal);
                        Bitmap subImagemAltura = melhorImagem.Clone(mascaraAltura, melhorImagem.PixelFormat);

                        otsu.ConvertToGrayDMA(subImagemAltura);
                        int otsuThreshold = otsu.getOtsuThreshold(subImagemAltura);
                        otsu.threshold(subImagemAltura, otsuThreshold);

                        List<Point> listaPiniAltura = new List<Point>();
                        List<Point> listaPfimAltura = new List<Point>();

                        Filtros.Segmenta8Conectado(subImagemAltura, subImagemAltura, listaPiniAltura, listaPfimAltura);

                        foreach (var ini in listaPiniAltura)
                        {
                            int altura = listaPfimAltura[listaPiniAltura.IndexOf(ini)].Y - ini.Y;
                            int largura = listaPfimAltura[listaPiniAltura.IndexOf(ini)].X - ini.X;

                            if (altura > 15 && altura < 27 && largura > 3 && largura < 39)
                            {
                                cont++;
                                
                                listaPontosInicioParaFiltrar.Add(ini);
                                listaPontosFinalParaFiltrar.Add(listaPfimAltura[listaPiniAltura.IndexOf(ini)]);

                                Filtros.DesenhaRetangulo(subImagemAltura, ini, listaPfimAltura[listaPiniAltura.IndexOf(ini)], Color.Green);

                                using (Graphics g = Graphics.FromImage(imageBitmapDest))
                                {
                                    Rectangle targetRegion = new Rectangle(region.X, y, subImagemAltura.Width, subImagemAltura.Height);
                                    g.DrawImage(subImagemAltura, targetRegion);
                                }
                            }
                        }

                        subImagemAltura.Dispose();

                        if (cont >= 7) // Parar se todos os caracteres forem encontrados
                            ok = true;
                    }
                }
            }
        }

        public static void EncontraPlaca(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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
            List<Point> listaPontosInicioMascaraDeslizante = new List<Point>();
            List<Point> listaPontosFinalMascaraDeslizante = new List<Point>();

            Bitmap placaRegiaoDilatada = null;
            Bitmap placaJanelaDeslizante = null;
            Bitmap subImagem = null;

            int cont = 0, altura, largura, qtdLinhasDivisao = 3, tentativas = 0, caso = 0;
            bool primeiraIteracao = true;
            
            Otsu otsu = new Otsu();
            
            otsu.ConvertToGrayDMA(imageBitmapDest);
            int otsuThreshold = otsu.getOtsuThreshold(imageBitmapDest);
            otsu.threshold(imageBitmapDest, otsuThreshold);
            
            while (cont <= 7 && tentativas < 6)
            {
                if (primeiraIteracao)
                {
                    caso = 0;
                    Bitmap imageBitmap = (Bitmap)imageBitmapDest.Clone();
                    Filtros.Segmenta8Conectado(imageBitmap, imageBitmapDest, listaPini, listaPfim);
                    
                    for (int i = 0; i < listaPini.Count; i++)
                    {
                        altura = listaPfim[i].Y - listaPini[i].Y;
                        largura = listaPfim[i].X - listaPini[i].X;

                        if (altura > 15 && altura < 25 && largura > 3 && largura < 39)
                        {
                            _listaPini.Add(listaPini[i]);
                            _listaPfim.Add(listaPfim[i]);

                            listaPontosInicioParaFiltrar.Add(listaPini[i]);
                            listaPontosFinalParaFiltrar.Add(listaPfim[i]);
                        
                            Filtros.DesenhaRetangulo(imageBitmapDest, listaPini[i], listaPfim[i], Color.Green);
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
                            caso = 1;
                            
                            listaPontosInicioParaFiltrar.Clear();
                            listaPontosFinalParaFiltrar.Clear();
                            
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
                                    cont = 0;
                                    // Criar a região da placa corretamente
                                    Rectangle region = new Rectangle(x, y, width, height);
                                    Bitmap placaRegiao = imageBitmapSrc.Clone(region, imageBitmapSrc.PixelFormat);

                                    // Aplica Otsu e segmentação na região da placa
                                    otsu.ConvertToGrayDMA(placaRegiao);
                                    otsuThreshold = otsu.getOtsuThreshold(placaRegiao);
                                    otsu.threshold(placaRegiao, otsuThreshold);

                                    placaRegiaoDilatada = (Bitmap)placaRegiao.Clone();

                                    listaPini.Clear();
                                    listaPfim.Clear();

                                    Filtros.Segmenta8Conectado(placaRegiaoDilatada, placaRegiaoDilatada, listaPini, listaPfim);

                                    for (int i = 0; i < listaPini.Count; i++)
                                    {
                                        altura = listaPfim[i].Y - listaPini[i].Y;
                                        largura = listaPfim[i].X - listaPini[i].X;

                                        if (altura > 15 && altura < 27 && largura > 3 && largura < 39)
                                        {
                                            listaPontosInicioParaFiltrar.Add(listaPini[i]);
                                            listaPontosFinalParaFiltrar.Add(listaPfim[i]);

                                            Filtros.DesenhaRetangulo(placaRegiaoDilatada, listaPini[i], listaPfim[i], Color.Green);


                                            using (Graphics g = Graphics.FromImage(imageBitmapDest))
                                            {
                                                Rectangle targetRegion = new Rectangle(region.X, region.Y, placaRegiaoDilatada.Width, placaRegiaoDilatada.Height);
                                                g.DrawImage(placaRegiaoDilatada, targetRegion);
                                            }

                                            cont++;
                                        }
                                    }

                                    // Fallback para aplicar a máscara deslizante
                                    if (cont < 7)
                                    {
                                        caso = 2;
                                        placaJanelaDeslizante = imageBitmapSrc.Clone(region, imageBitmapSrc.PixelFormat);
                                        Console.WriteLine("Tentando máscara deslizante...");
                                        cont = 0;
                                        AplicarMascaraDeslizante(placaJanelaDeslizante, imageBitmapDest, otsu, region, ref cont, listaPontosInicioMascaraDeslizante, listaPontosFinalMascaraDeslizante);
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
                            caso = 3;
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

                                subImagem = imageBitmapSrc.Clone(retangulo, imageBitmapSrc.PixelFormat);
                                
                                otsu.ConvertToGrayDMA(subImagem);
                                otsuThreshold = otsu.getOtsuThreshold(subImagem);
                                otsu.threshold(subImagem, otsuThreshold);
                                Bitmap subImagemClone = (Bitmap)subImagem.Clone();
                                
                                Segmenta8Conectado(subImagem, subImagemClone, subListaPini, subListaPfim);
                                
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
                                            Filtros.DesenhaRetangulo(subImagem, subListaPini[j], subListaPfim[j], Color.Green);
                                        }

                                        cont++;
                                    }
                                }

                                if (cont < 7)
                                {
                                    caso = 2;
                                    cont = 0;
                                    placaJanelaDeslizante = imageBitmapSrc.Clone(retangulo, imageBitmapSrc.PixelFormat);
                                    Console.WriteLine("Tentando máscara deslizante...");
                                    AplicarMascaraDeslizante(placaJanelaDeslizante, imageBitmapDest, otsu, retangulo, ref cont, listaPontosInicioMascaraDeslizante, listaPontosFinalMascaraDeslizante);
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

            switch (caso)
            {
                case 0:
                    ReconheceCaracter(imageBitmapDest, _listaPini, _listaPfim, caso);
                    break;
                case 1:
                    ReconheceCaracter(placaRegiaoDilatada, listaPontosInicioParaFiltrar, listaPontosFinalParaFiltrar, caso);
                    break;
                case 2:
                    placaJanelaDeslizante =
                        (Bitmap)Image.FromFile("C:\\Users\\VITOR\\Documents\\PlacasDeCarros\\Placas\\MelhorPlaca.png");
                    ReconheceCaracter(placaJanelaDeslizante, listaPontosInicioMascaraDeslizante,
                        listaPontosFinalMascaraDeslizante, caso);
                    break;
                case 3:
                    ReconheceCaracter(subImagem, listaPontosInicioParaFiltrar, listaPontosFinalParaFiltrar, caso);
                    break;
            }
        }

        public static void FiltrarListaPontosCaracteres(ref List<Point> pontosIniciais, ref List<Point> pontosFinais)
        {
            // Tolerância para verificar se os pontos estão na mesma linha (em pixels)
            const int toleranciaY = 3;

            // Lista temporária para armazenar os pontos filtrados
            List<(Point inicial, Point final)> pontosFiltrados = new List<(Point inicial, Point final)>();

            // Itera por cada ponto inicial
            for (int i = 0; i < pontosIniciais.Count; i++)
            {
                // Conta quantos outros pontos estão alinhados com o ponto atual
                int alinhadosPeloY = 0;

                for (int j = 0; j < pontosIniciais.Count; j++)
                {
                    if (i != j)
                    {
                        if (Math.Abs(pontosIniciais[i].Y - pontosIniciais[j].Y) <= toleranciaY)
                        {
                            alinhadosPeloY++;
                        }
                        else
                        {
                            alinhadosPeloY--;
                        }
                    }
                }
                
                if (alinhadosPeloY >= 1 || pontosIniciais.Count == 1)
                {
                    pontosFiltrados.Add((pontosIniciais[i], pontosFinais[i]));
                }
            }

            // Atualiza as listas originais com os pontos filtrados
            pontosIniciais = pontosFiltrados.Select(p => p.inicial).ToList();
            pontosFinais = pontosFiltrados.Select(p => p.final).ToList();
        }

        public static void ReconheceCaracter(Bitmap imageBase, List<Point> pontosIniciais, List<Point> pontosFinais, int caso)
        {
            
            if(pontosIniciais.Count() > 7)
            {
                FiltrarListaPontosCaracteres(ref pontosIniciais, ref pontosFinais);
            }
            Otsu otsu = new Otsu();

            ClassificacaoCaracteres cl_numeros = new ClassificacaoCaracteres(30, 40, 1, 'S');
            ClassificacaoCaracteres cl_letras = new ClassificacaoCaracteres(30, 40, 2, 'S');

            Console.WriteLine(new string('\n', 50)); // Limpa o console ao iniciar
            Console.WriteLine("Reconhecendo a placa");

            // Simula uma animação de progresso
            for (int progress = 0; progress < 5; progress++)
            {
                Console.Write(".");
                Thread.Sleep(500); // Aguarda 500ms para criar o efeito de carregamento
            }
            Console.WriteLine("\n");

            for (int i = 0; i < pontosIniciais.Count; i++)
            {
                Point pontoInicial = pontosIniciais[i];
                Point pontoFinal = pontosFinais[i];

                int x = Math.Max(0, pontoInicial.X);
                int y = Math.Max(0, pontoInicial.Y);
                int width = Math.Min(imageBase.Width - x, pontoFinal.X - pontoInicial.X + 1);
                int height = Math.Min(imageBase.Height - y, pontoFinal.Y - pontoInicial.Y + 1);

                if (width > 0 && height > 0)
                {
                    Rectangle region = new Rectangle(x, y, width, height);
                    Bitmap subImagem = imageBase.Clone(region, imageBase.PixelFormat);

                    subImagem.Save("C:\\Users\\VITOR\\Documents\\PlacasDeCarros\\Caracteres\\Caractere" + i + ".png", ImageFormat.Png);

                    otsu.ConvertToGrayDMA(subImagem);
                    int otsuThreshold = otsu.getOtsuThreshold(subImagem);
                    otsu.threshold(subImagem, otsuThreshold);

                    subImagem.Save("C:\\Users\\VITOR\\Documents\\PlacasDeCarros\\Caracteres\\CaractereOtsu" + i + ".png", ImageFormat.Png);

                    if (i < 3)
                    {
                        String transicao = cl_letras.retornaTransicaoHorizontal(subImagem);
                        Console.Write(cl_letras.reconheceCaractereTransicao_2pixels(transicao));
                    }
                    else
                    {
                        if (i == 3) Console.Write("-");
                        String transicao = cl_numeros.retornaTransicaoHorizontal(subImagem);
                        Console.Write(cl_numeros.reconheceCaractereTransicao_2pixels(transicao));
                    }
                }
            }

            Console.WriteLine("\nReconhecimento concluído!");
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

        public static void Threshold(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
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
    }
}
