using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        PaginaPokemon paginaPokemon;
        public MainWindow()
        {
            InitializeComponent();
            DatiCondivisi.Instance().main = this;
            DatiCondivisi.Instance().caricamento = caricamento;
            paginaPokemon = new PaginaPokemon();
            paginaPokemon.Background = Brushes.Gray;
            paginaPokemon.Width = Width;
            paginaPokemon.Height = Height * 8.5 / 10;
            paginaPokemon.Margin = new Thickness(0, 0, 0, 0);
            Mappa.Children.Add(paginaPokemon);
        }

        public void MostraMappa()
        {
            caricamento.Visibility = Visibility.Visible;
            Mappa.Visibility = Visibility.Hidden;
        }
        public void MostraApp()
        {
            caricamento.Visibility = Visibility.Hidden;
            Mappa.Visibility = Visibility.Visible;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            paginaPokemon.Width = Width;
            paginaPokemon.Height = Height*8.5/10;
            caricamento.Height = Height;
            caricamento.Width = Width;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MostraMappa();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MostraApp();
        }
    }
}



/*List<Pokemon> deck = new List<Pokemon>();
            Random rand = new Random();

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
            CompositionTarget.Rendering += Upload;
            */



/*int i = 0;
private void Upload(object sender, EventArgs e)
{
    if (DatiCondivisi.Instance().M == null || DatiCondivisi.Instance().M.r.Pokemon==null)
        return;
    DatiCondivisi.Instance().M.Upload();
}*/