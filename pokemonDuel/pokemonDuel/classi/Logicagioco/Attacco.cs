using pokemonDuel.classi.Grafica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pokemonDuel.classi.Logicagioco
{
    class Attacco
    {
        public Nodo Mio,Avversario;
        public Mossa MossaMia, MossaAvversario;
        public Attacco(Nodo Mio, Nodo Avversario, Mossa MossaMia, Mossa MossaAvversario)
        {
            this.Mio = Mio;
            this.Avversario = Avversario;
            if (MossaMia != null)
                this.MossaMia = (Mossa)MossaMia.Clone();
            else
                this.MossaMia = null;
            if (MossaAvversario != null)
                this.MossaAvversario = (Mossa)MossaAvversario.Clone();
            else
                this.MossaAvversario = null;
        }
        public bool Settato()
        {
            if (Mio != null && Avversario != null && MossaMia != null && MossaAvversario != null)
                return true;
            return false;
        }
        public void EseguiAttacco()
        {
            if (!Settato())
                throw new Exception("attacco non settato");
            if (MossaMia.danno > MossaAvversario.danno)
                DatiCondivisi.Instance().M.RimettiNellaMano(Avversario);
            else if (MossaMia.danno < MossaAvversario.danno)
                DatiCondivisi.Instance().M.RimettiNellaMano(Mio);
            
        }
        public void Render()
        {
            Battaglia b = DatiCondivisi.Instance().M.m;
            double H = b.CanvasAttacco.Height, W = b.CanvasAttacco.Width;
            b.Dispatcher.Invoke(delegate {
                double unita = Math.Min(W / 5, H / 5);
                b.CanvasAttacco.Children.Clear();

                Polygon triangoloMio = new Polygon();
                triangoloMio.Points.Add(new Point(W, H * 3 / 4 - 5));
                triangoloMio.Points.Add(new Point(0, H));
                triangoloMio.Points.Add(new Point(W, H));
                triangoloMio.Fill = Brushes.Blue;
                triangoloMio.Stroke = Brushes.LightBlue;
                triangoloMio.StrokeThickness = 5;
                b.CanvasAttacco.Children.Add(triangoloMio);

                Polygon triangoloAvv = new Polygon();
                triangoloAvv.Points.Add(new Point(0, H / 4));
                triangoloAvv.Points.Add(new Point(W, 0));
                triangoloAvv.Points.Add(new Point(0, 0));
                triangoloAvv.Fill = Brushes.DarkRed;
                triangoloAvv.Stroke = Brushes.OrangeRed;
                triangoloAvv.StrokeThickness = 5;
                b.CanvasAttacco.Children.Add(triangoloAvv);

                if (MossaAvversario != null)
                {
                    Label NomeMossa = new Label();
                    NomeMossa.Content = MossaAvversario.nome;
                    Canvas.SetTop(NomeMossa, H / 20);
                    Canvas.SetLeft(NomeMossa, unita + 35);
                    NomeMossa.Foreground = Brushes.OrangeRed;
                    NomeMossa.Background = Brushes.Transparent;
                    NomeMossa.FontSize = 30;
                    NomeMossa.BorderBrush = Brushes.Transparent;
                    b.CanvasAttacco.Children.Add(NomeMossa);

                    Label DannoMossa = new Label();
                    DannoMossa.Content = MossaAvversario.danno + "";
                    Canvas.SetTop(DannoMossa, H / 10);
                    Canvas.SetLeft(DannoMossa, unita + 30);
                    DannoMossa.Foreground = Brushes.OrangeRed;
                    DannoMossa.Background = Brushes.Transparent;
                    DannoMossa.BorderBrush = Brushes.Transparent;
                    DannoMossa.FontSize = 50;
                    b.CanvasAttacco.Children.Add(DannoMossa);
                }
                if (Avversario != null)
                {
                    Rectangle MioPokemon = new Rectangle();
                    MioPokemon.Fill = Avversario.pokemon.Render();
                    MioPokemon.Width = unita;
                    MioPokemon.Height = unita;
                    Canvas.SetTop(MioPokemon, H / 20);
                    Canvas.SetLeft(MioPokemon, 20);
                    b.CanvasAttacco.Children.Add(MioPokemon);
                }
                if (MossaMia != null)
                {
                    Label NomeMossaAltro = new Label();
                    NomeMossaAltro.Content = MossaMia.nome;
                    Canvas.SetBottom(NomeMossaAltro, H / 20);
                    Canvas.SetRight(NomeMossaAltro, unita + 30);
                    NomeMossaAltro.Foreground = Brushes.LightBlue;
                    NomeMossaAltro.Background = Brushes.Transparent;
                    NomeMossaAltro.FontSize = 30;
                    NomeMossaAltro.BorderBrush = Brushes.Transparent;
                    b.CanvasAttacco.Children.Add(NomeMossaAltro);

                    Label DannoMossaAltra = new Label();
                    DannoMossaAltra.Content = MossaMia.danno + "";
                    Canvas.SetBottom(DannoMossaAltra, H / 10);
                    Canvas.SetRight(DannoMossaAltra, unita + 35);
                    DannoMossaAltra.Foreground = Brushes.LightSkyBlue;
                    DannoMossaAltra.Background = Brushes.Transparent;
                    DannoMossaAltra.BorderBrush = Brushes.Transparent;
                    DannoMossaAltra.FontSize = 50;
                    b.CanvasAttacco.Children.Add(DannoMossaAltra);
                }
                if(Mio!=null)
                {
                    Rectangle AltroPokemon = new Rectangle();
                    AltroPokemon.Fill = Mio.pokemon.Render();
                    AltroPokemon.Width = unita;
                    AltroPokemon.Height = unita;
                    Canvas.SetBottom(AltroPokemon, H / 20);
                    Canvas.SetRight(AltroPokemon,20);
                    b.CanvasAttacco.Children.Add(AltroPokemon);
                }
            });
        }

    }
}
