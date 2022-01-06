using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Media;
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
        public void MostraAttacca()
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                Svuota();
                RenderAttacco();
            });
        }
        public void RenderFine(bool vinto, int xp/*,int materiali*/)
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
                Canvas.SetTop(TornaHome, canvas.Height/2 +60);
                TornaHome.Width = 150;
                TornaHome.Height = 60;
                TornaHome.FontSize = 20;
                TornaHome.VerticalContentAlignment = VerticalAlignment.Center;
                TornaHome.HorizontalContentAlignment = HorizontalAlignment.Center;
                canvas.Children.Add(TornaHome);

                Label Xp = new Label();
                Xp.Content ="XP +"+ xp;
                Xp.Foreground = Brushes.White;
                Xp.Background = Brushes.Transparent;
                Xp.FontSize = 25;
                Canvas.SetLeft(Xp, canvas.Width / 2 - 40);
                Canvas.SetTop(Xp, canvas.Height / 2);
                canvas.Children.Add(Xp);
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

        private void RenderAttacco()
        {
            Attacco a = DatiCondivisi.Instance().A;
            double H =canvas.Height, W =canvas.Width;
            double unita = Math.Min(W / 5, H / 5) ;

            WindowsFormsHost host = DatiCondivisi.Instance().b.host;
            Canvas.SetBottom(host, 0);
            Canvas.SetLeft(host, 0);
            canvas.Children.Add(host);

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
    }
}
