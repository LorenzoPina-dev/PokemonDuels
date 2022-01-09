using pokemonDuel.classi.Logicagioco;
using pokemonDuel.classi.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pokemonDuel.classi.Grafica
{
    public class GestioneCanvas
    {
        private Canvas canvas;
        public GestioneCanvas(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public void MostraAttacca(bool MostraRuota)
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                Svuota();
                if (MostraRuota)
                    RenderRuota();
                RenderAttacco();
            });
        }
        public void RenderFine(bool vinto, int xp,int materiali)
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                Svuota();
                Rectangle sfondo = new Rectangle();
                sfondo.Width = canvas.Width;
                sfondo.Height = canvas.Height;
                sfondo.Fill = new SolidColorBrush(Color.FromArgb(175,0,0,0));
                canvas.Children.Add(sfondo);

                Label scritta = new Label();
                if (vinto)
                {
                    scritta.Content = "Vinto";
                    scritta.Foreground = Brushes.LightBlue;

                    Label Xp = new Label();
                    Xp.Content = "XP +" + xp;
                    Xp.Foreground = Brushes.White;
                    Xp.Background = Brushes.Transparent;
                    Xp.FontSize = 25;
                    Canvas.SetLeft(Xp, canvas.Width / 2 - 40);
                    Canvas.SetTop(Xp, canvas.Height / 2);
                    canvas.Children.Add(Xp);

                    Label Materiali = new Label();
                    Materiali.Content = "Materiali +" + materiali;
                    Materiali.Foreground = Brushes.White;
                    Materiali.Background = Brushes.Transparent;
                    Materiali.FontSize = 25;
                    Canvas.SetLeft(Materiali, canvas.Width / 2 - 75);
                    Canvas.SetTop(Materiali, canvas.Height / 2 + 40);
                    canvas.Children.Add(Materiali);
                }
                else
                { 
                    scritta.Content = "Perso";
                    scritta.Foreground = Brushes.OrangeRed;
                }
                scritta.FontSize = 40;
                Canvas.SetLeft(scritta, canvas.Width / 2 - 50);
                Canvas.SetTop(scritta, canvas.Height / 2-60);
                canvas.Children.Add(scritta);

                Button TornaHome = new Button();
                TornaHome.Content = "Torna Home";
                TornaHome.Background = Brushes.LightBlue;
                TornaHome.Foreground = Brushes.Black;
                TornaHome.Click += TornaHome_Click;
                Canvas.SetLeft(TornaHome, canvas.Width / 2 - 75);
                Canvas.SetTop(TornaHome, canvas.Height/2 +100);
                TornaHome.Width = 150;
                TornaHome.Height = 60;
                TornaHome.FontSize = 20;
                TornaHome.VerticalContentAlignment = VerticalAlignment.Center;
                TornaHome.HorizontalContentAlignment = HorizontalAlignment.Center;
                canvas.Children.Add(TornaHome);
            });
        }

        private void TornaHome_Click(object sender, RoutedEventArgs e)
        {
            DatiCondivisi.Instance().main.MostraFinestra(Finestra.Utente);
        }

        public void RenderFineRound(bool vinto)
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                Svuota();
                Label scritta = new Label();
                if (vinto)
                {
                    scritta.Content = "Vinto";
                    scritta.Foreground = Brushes.LightBlue;
                }
                else
                {
                    scritta.Content = "Perso";
                    scritta.Foreground = Brushes.OrangeRed;
                }
                scritta.FontSize = 40;
                Canvas.SetLeft(scritta, canvas.Width / 2 - 50);
                Canvas.SetTop(scritta, canvas.Height / 2);
                canvas.Children.Add(scritta);

            });
        }
        private void RenderRuota()
        {
            WindowsFormsHost host = DatiCondivisi.Instance().b.host;
            Canvas.SetBottom(host, 0);
            Canvas.SetLeft(host, 0);
            canvas.Children.Add(host);
        }

        private void RenderAttacco()
        {
            Attacco a = DatiCondivisi.Instance().A;
            double H =canvas.Height, W =canvas.Width;
            double unita = Math.Min(W / 5, H / 5) ;


            Polygon triangoloMio = new Polygon();
            triangoloMio.Points.Add(new Point(W, H * 3 / 4 - 5));
            triangoloMio.Points.Add(new Point(0, H));
            triangoloMio.Points.Add(new Point(W, H));
            triangoloMio.Fill = Brushes.Blue;
            triangoloMio.Stroke = Brushes.LightBlue;
            triangoloMio.StrokeThickness = 5;
            canvas.Children.Add(triangoloMio);

            Polygon triangoloAvv = new Polygon();
            triangoloAvv.Points.Add(new Point(0, H / 4));
            triangoloAvv.Points.Add(new Point(W, 0));
            triangoloAvv.Points.Add(new Point(0, 0));
            triangoloAvv.Fill = Brushes.DarkRed;
            triangoloAvv.Stroke = Brushes.OrangeRed;
            triangoloAvv.StrokeThickness = 5;
            canvas.Children.Add(triangoloAvv);

            if (a.MossaMia != null)
            {
                Label NomeMossaAltro = new Label();
                NomeMossaAltro.Content = a.MossaMia.nome;
                Canvas.SetBottom(NomeMossaAltro, H / 20);
                Canvas.SetRight(NomeMossaAltro, unita + 30);
                NomeMossaAltro.Foreground = Brushes.LightBlue;
                NomeMossaAltro.Background = Brushes.Transparent;
                NomeMossaAltro.FontSize = 30;
                NomeMossaAltro.BorderBrush = Brushes.Transparent;
                canvas.Children.Add(NomeMossaAltro);

                Label DannoMossaAltra = new Label();
                DannoMossaAltra.Content = a.MossaMia.danno + "";
                Canvas.SetBottom(DannoMossaAltra, H / 10);
                Canvas.SetRight(DannoMossaAltra, unita + 35);
                DannoMossaAltra.Foreground = Brushes.LightSkyBlue;
                DannoMossaAltra.Background = Brushes.Transparent;
                DannoMossaAltra.BorderBrush = Brushes.Transparent;
                DannoMossaAltra.FontSize = 50;
                canvas.Children.Add(DannoMossaAltra);
            }
            if (a.Mio != null)
            {
                Rectangle AltroPokemon = new Rectangle();
                AltroPokemon.Fill = a.Mio.pokemon.Render();
                AltroPokemon.Width = unita;
                AltroPokemon.Height = unita;
                Canvas.SetBottom(AltroPokemon, H / 20);
                Canvas.SetRight(AltroPokemon, 20);
                canvas.Children.Add(AltroPokemon);
            }

            if (a.MossaAvversario != null)
            {
                Label NomeMossa = new Label();
                NomeMossa.Content = a.MossaAvversario.nome;
                Canvas.SetTop(NomeMossa, H / 20);
                Canvas.SetLeft(NomeMossa, unita + 35);
                NomeMossa.Foreground = Brushes.OrangeRed;
                NomeMossa.Background = Brushes.Transparent;
                NomeMossa.FontSize = 30;
                NomeMossa.BorderBrush = Brushes.Transparent;
                canvas.Children.Add(NomeMossa);

                Label DannoMossa = new Label();
                DannoMossa.Content = a.MossaAvversario.danno + "";
                Canvas.SetTop(DannoMossa, H / 10);
                Canvas.SetLeft(DannoMossa, unita + 30);
                DannoMossa.Foreground = Brushes.OrangeRed;
                DannoMossa.Background = Brushes.Transparent;
                DannoMossa.BorderBrush = Brushes.Transparent;
                DannoMossa.FontSize = 50;
                canvas.Children.Add(DannoMossa);
            }
            if (a.Avversario != null)
            {
                Rectangle MioPokemon = new Rectangle();
                MioPokemon.Fill = a.Avversario.pokemon.Render();
                MioPokemon.Width = unita;
                MioPokemon.Height = unita;
                Canvas.SetTop(MioPokemon, H / 20);
                Canvas.SetLeft(MioPokemon, 20);
                canvas.Children.Add(MioPokemon);
            }
        }

        public void Svuota()
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                canvas.Children.Clear();
            });
        }


        public static void RenderPokemon(Canvas canvas, Pokemon pok, double Width, double x, double y, bool mostraCosto, bool MostraNome)
        {
            RenderPokemon(canvas,pok, Width, Width, x, y, mostraCosto, MostraNome);
        }
        public static void RenderPokemon(Canvas canvas, Pokemon pok, double Width, double Height, double x, double y, bool mostraCosto,bool MostraNome)
        {
            double unitaX = Width, unitaY = Height, offset = 0;
            if (mostraCosto)
            {
                unitaX -= 20;
                unitaY -= 20;
                offset = 10;
            }
            Rectangle i = new Rectangle();
            i.Fill = pok.Render();
            i.Width = unitaX;
            i.Height = unitaY;
            Canvas.SetTop(i, y);
            Canvas.SetLeft(i, x + offset); 
            canvas.Children.Add(i);
            Ellipse i2 = new Ellipse();
            i2.Fill = Brushes.Blue;
            i2.Width = Math.Max(unitaX / 4, 25);
            i2.Height = Math.Max(unitaY / 4, 25);
            Canvas.SetTop(i2, y + unitaY - i2.Width);
            Canvas.SetLeft(i2, x + unitaX - i2.Height);
            i2.StrokeThickness = 5;
            i2.Stroke = Brushes.Transparent;
            canvas.Children.Add(i2);
            Label i3 = new Label();
            i3.Content = pok.Salti;
            i3.Foreground = Brushes.White;
            i3.HorizontalContentAlignment = HorizontalAlignment.Center;
            i3.VerticalContentAlignment = VerticalAlignment.Center;
            i3.FontSize = Math.Min(unitaY,unitaY)/100*10;
            if (i3.FontSize < 10)
                i3.FontSize = 10;
            i3.Width = i2.Width;
            i3.Height = i2.Height;
            Canvas.SetTop(i3, y + unitaY - i3.Height);
            Canvas.SetLeft(i3, x + unitaX - i3.Width);
            canvas.Children.Add(i3);
            if (MostraNome)
            {
                Label Nome = new Label();
                Nome.Foreground = Brushes.White;
                Nome.Content = pok.Nome;
                Nome.HorizontalContentAlignment = HorizontalAlignment.Center;
                Nome.VerticalContentAlignment = VerticalAlignment.Center;
                Nome.Width = Width;
                Nome.FontSize = Math.Min(unitaY, unitaY) / 100 * 10;
                if (Nome.FontSize < 10)
                    Nome.FontSize = 10;
                Nome.Height = unitaY / 4;
                Canvas.SetTop(Nome, y + unitaY * 3 / 4);
                Canvas.SetLeft(Nome, x);
                canvas.Children.Add(Nome);
            }
            if (mostraCosto)
            {
                Image IconaMateriali = new Image();
                IconaMateriali.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/file/material.png"));
                IconaMateriali.Width = 15;
                IconaMateriali.Height = 15;
                Canvas.SetTop(IconaMateriali, y + Width - 2 * offset);
                Canvas.SetLeft(IconaMateriali, x);
                canvas.Children.Add(IconaMateriali);
                Label Materiali = new Label();
                Materiali.Foreground = Brushes.White;
                Materiali.Content = pok.Materiali;
                Materiali.VerticalContentAlignment = VerticalAlignment.Center;
                Materiali.Width = Width - IconaMateriali.Width;
                Materiali.FontSize = Math.Min(unitaY, unitaY) / 100 * 10;
                if (Materiali.FontSize < 10)
                    Materiali.FontSize = 10;
                Materiali.Height = unitaY / 4;
                Canvas.SetTop(Materiali, y + Width - 3 * offset);
                Canvas.SetLeft(Materiali, x + IconaMateriali.Width);
                canvas.Children.Add(Materiali);
            }
        }

    }
}
