using pokemonDuel.classi;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logica di interazione per PaginaPokemon.xaml
    /// </summary>
    public partial class PaginaPokemon : UserControl
    {
        public string PedinaVuota =Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png";
        private Dictionary<int,Pokemon> tuttiPokemon;
        private Image selezionata;
        int PokemonPerPagina;
        int pagina;
        int parti = 11,partiLista=8,partiMano=2;

        public PaginaPokemon()
        {
            InitializeComponent();
            tuttiPokemon = new Dictionary<int, Pokemon>();
            foreach (KeyValuePair<int,Pokemon> p in StoreInfo.Instance().Pokedex)
                tuttiPokemon.Add(p.Key,p.Value);
            selezionata = null;
        }
        private void CaricaPokemon()
        {
            PokemonPerPagina=DisegnaPokemon(0);
            CaricaMano();
            pagina = 0;
            
        }
        private void CaricaMano()
        {
            Deck.Children.Clear();
            int x = 0, unita = (int)(Deck.Width / 6);
            for (int j = 0; j < 6; j++)
            {
                Image i = new Image();
                if (DatiCondivisi.Instance().io.Deck.Count > j)
                    i.Source = new BitmapImage(new Uri(DatiCondivisi.Instance().io.Deck[j].UrlTexture));
                else
                    i.Source = new BitmapImage(new Uri(PedinaVuota));
                i.Name = "D_" + j;
                i.MouseDown += Cliccata;
                i.Width = unita;
                i.Height = (int)Math.Min(unita, Deck.Height);
                i.Margin = new Thickness(x, Deck.Height / 6, Deck.Width - x - unita, 0);
                x += unita;
                Deck.Children.Add(i);
            }
            numPag.Text = pagina + 1 + "";
        }

        private void Cliccata(object sender, MouseButtonEventArgs e)
        {
            selezionata = (Image)e.Source;
        }
        private int DisegnaPokemon(int ind)
        {
            Lista.Children.Clear();
            int x = 0, y = 0, unita = 75,j=ind;
            var keys = tuttiPokemon.Keys.ToList();
            for(;j< keys.Count;j++)
            {
                Pokemon pok = (Pokemon)tuttiPokemon[keys[j]].Clone();
                Image i = new Image();
                i.Source = new BitmapImage(new Uri(pok.UrlTexture));
                i.Width = unita;
                i.Height = unita;
                i.Margin = new Thickness(x, y, Lista.Width - x - unita, Lista.Height - y - unita);
                i.Name = "P_" + pok.id;
                x += unita;
                i.MouseDown += Cliccata;
                if (x + unita >= Lista.Width)
                {
                    x = 0;
                    y += unita;
                }
                Lista.Children.Add(i);
                if (y + unita >= Lista.Height)
                    return j-ind;
            }
            return j - ind;
        }

        private void Lista_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona(Width, Height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selezionata != null)
            {
                string[] split = selezionata.Name.Split('_');
                if (split[0] == "P")
                {
                    Pokemon p = (Pokemon)StoreInfo.Instance().Pokedex[int.Parse(split[1])].Clone();
                    if (DatiCondivisi.Instance().io.Deck.Count < 6)
                    {
                        p.mio = true;
                        DatiCondivisi.Instance().io.Deck.Add(p);
                        tuttiPokemon.Remove(p.id);
                        DisegnaPokemon(pagina * PokemonPerPagina);
                        CaricaMano();
                        selezionata = null;
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selezionata != null)
            {
                string[] split = selezionata.Name.Split('_');
                if (split[0] != "D")
                    return;
                Pokemon p = DatiCondivisi.Instance().io.Deck[int.Parse(split[1])];
                if (DatiCondivisi.Instance().io.Deck.Contains(p))
                {
                    DatiCondivisi.Instance().io.Deck.Remove(p);
                    tuttiPokemon.Add(p.id, p);
                    DisegnaPokemon(pagina * PokemonPerPagina);
                    CaricaMano();
                    numPag.Text = pagina + 1 + "";
                }
                selezionata = null;
            }
        }

        private void Prima_Click(object sender, RoutedEventArgs e)
        {
            if (pagina - 1 >= 0)
            {
                pagina--;
                DisegnaPokemon(pagina * PokemonPerPagina);
                numPag.Text = pagina+1+"";
            }
        }

        private void Dopo_Click(object sender, RoutedEventArgs e)
        {
            if ((pagina + 1) * PokemonPerPagina <= tuttiPokemon.Count)
            {
                pagina++;
                DisegnaPokemon(pagina * PokemonPerPagina);
                numPag.Text = pagina +1+ "";
            }
        }

        internal void Ridimensiona(double width, double height)
        {
            Lista.Width = width;
            Lista.Height = height * partiLista / parti;
            Deck.Height = height * partiMano / parti;
            Deck.Width = width;
            CaricaPokemon();
        }
    }
}
