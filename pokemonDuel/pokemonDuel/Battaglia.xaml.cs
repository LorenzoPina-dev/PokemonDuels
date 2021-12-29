using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pokemonDuel
{
    /// <summary>
    /// Logica di interazione per Battaglia.xaml
    /// </summary>
    public partial class Battaglia : UserControl
    {
        public Battaglia()
        {
            InitializeComponent();
            myCanvas.Width = this.Width;
            myCanvas.Height = Height;
            host.Width = Width / 2;
            host.Height = Height / 2;
            Random rand= new Random();
            List<Pokemon> deck = new List<Pokemon>();
            Giocatore altro = new Giocatore();
            for (int i = 0; i < 6; i++)
            {
                Pokemon p = (Pokemon)StoreInfo.Instance().Pokedex[rand.Next(0, StoreInfo.Instance().Pokedex.Count)].Clone();
                p.mio = false;
                deck.Add(p);
            }
            altro = new Giocatore();
            altro.Deck = deck;
            altro.Username = "pippo";
            DatiCondivisi.Instance().altro = altro;
            DatiCondivisi.Instance().M = new Mappa(this);
            CompositionTarget.Rendering += Upload;

        }

        int i = 0;
        private void Upload(object sender, EventArgs e)
        {
            DatiCondivisi.Instance().Upload();
            //DatiCondivisi.Instance().M.Upload();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DatiCondivisi.Instance().M.Disegna();
        }

        private void myCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            myCanvas.Width = Width;
            myCanvas.Height = Height;
            double unita = Math.Min(Width / 2, Height / 2);
            host.Width = unita;
            host.Height = unita;
            host.Margin = new Thickness(0, Height - unita, Width -unita,0);
            DatiCondivisi.Instance().M.Disegna();
            GestioneRuota.Instance().ruota.CambiaDimensioni(host.Width, host.Height);
        }
    }
}
