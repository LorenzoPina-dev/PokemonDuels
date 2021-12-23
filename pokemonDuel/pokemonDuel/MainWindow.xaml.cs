using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Pokemon> deck = new List<Pokemon>();
            Random rand = new Random();
            DatiCondivisi.Instance();
            StoreInfo.Instance();
            for (int i = 0; i < 6; i++)
            {
                Pokemon p = (Pokemon)StoreInfo.Instance().Pokedex[rand.Next(0, StoreInfo.Instance().Pokedex.Count)].Clone();
                p.mio = true;
                deck.Add(p);
            }
            Giocatore io = new Giocatore();
            io.Deck = deck;
            io.Username = "pippo";
            deck = new List<Pokemon>();
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

            DatiCondivisi.Instance().M = new Mappa(this,io,altro);
            
            GestioneTcp gt = new GestioneTcp();
            HashSet<int> ris=DatiCondivisi.Instance().M.mappa[0].Raggiungibili(DatiCondivisi.Instance().M.mappa, 2);
            DatiCondivisi.Instance().M.Disegna();
            foreach (int r in ris)
                Console.WriteLine(r);
            Ruota ruota = new Ruota((int)host.Width, (int)host.Height);
            DatiCondivisi.Instance().M.r = ruota;
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DatiCondivisi.Instance().M.Disegna();
        }
    }
}
