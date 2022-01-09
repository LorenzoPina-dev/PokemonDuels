using pokemonDuel.classi;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
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

namespace pokemonDuel.classi.Componenti
{
    /// <summary>
    /// Logica di interazione per PaginaPokemon.xaml
    /// </summary>
    public partial class PaginaPokemon : UserControl
    {
        public string PedinaVuota =Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png";
        public SortedDictionary<int,Pokemon> tuttiPokemon;
        private Rectangle selezionata;
        int PokemonPerPagina;
        int pagina;
        int parti = 11,partiLista=8,partiMano=2;
        int unita;
        public PaginaPokemon()
        {
            InitializeComponent();
            tuttiPokemon = new SortedDictionary<int, Pokemon>();
            selezionata = null;
            this.Background = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
            Deck.Background = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
        }
        public void CaricaPokemon()
        {
            PokemonPerPagina = (int)(Lista.Width / unita * (Lista.Height / unita))+1;
            DisegnaPokemon(0);
            CaricaMano();
            pagina = 0;

            
        }
        private void CaricaMano()
        {
            Deck.Children.Clear();
            int unita = (int)Math.Min(Width / 6, Deck.Height),x = (int)(Width / 6 - unita) / 2; 
            for (int j = 0; j < 6; j++)
            {
                if (DatiCondivisi.Instance().io.Deck.Count > j)
                {
                    GestioneCanvas.RenderPokemon(Deck, DatiCondivisi.Instance().io.Deck[j], unita, x, 0, false, true);
                }
                else
                {
                    Rectangle i = new Rectangle();
                    i.Fill = new ImageBrush(new BitmapImage(new Uri(PedinaVuota)));
                    i.Margin = new Thickness(x-unita/2, Deck.Height / 6, Deck.Width - x - Width / 6, 0);
                    i.Height = unita;
                    i.Width = Width / 6;
                    Deck.Children.Add(i);
                }
                Rectangle container = new Rectangle();
                container.Fill = Brushes.Transparent;
                container.Width = unita;
                container.Height = unita;
                container.Margin = new Thickness(x, 0, Lista.Width - x - unita, 0);
                container.Name = "D_" + j;
                container.StrokeThickness = 5;
                container.Stroke = Brushes.Transparent;
                container.MouseDown += Cliccata;
                Deck.Children.Add(container);
                x +=(int) Width / 6;
            }
            numPag.Text = pagina + 1 + "";
        }
        public void Cliccata(Rectangle r)
        {
            if (selezionata != null)
                selezionata.Stroke = Brushes.Transparent;
            selezionata = r;

            selezionata.Stroke = Brushes.LightBlue;
        }
        private void Cliccata(object sender, MouseButtonEventArgs e)
        {
            Cliccata((Rectangle)e.Source);
        }
        public void DisegnaPokemon(int ind)
        {
            Lista.Children.Clear();
            int x = 0, y = 0, unita = 100,j=ind;
            var keys = tuttiPokemon.Keys.ToList();
            for(;j< PokemonPerPagina;j++)
            {
                Rectangle container = new Rectangle();
                if (j < keys.Count)
                {
                    Pokemon pok = (Pokemon)tuttiPokemon[keys[ind + j]].Clone();
                    GestioneCanvas.RenderPokemon(Lista, pok, unita, x, y, false, true);
                    container.Name = "P_" + pok.id;
                    container.Fill = Brushes.Transparent;
                }
                else
                    container.Fill = new ImageBrush(new BitmapImage(new Uri(PedinaVuota)));
                container.Width = unita;
                container.Height = unita;
                container.Margin = new Thickness(x, y, Lista.Width - x - unita, Lista.Height - y - unita);
                container.StrokeThickness = 5;
                container.Stroke = Brushes.Transparent;
                container.MouseDown += Cliccata;
                Lista.Children.Add(container);
                x += unita;
                if (x + unita >= Lista.Width)
                {
                    x = 0;
                    y += unita;
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona(e.NewSize.Width,e.NewSize.Height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selezionata != null && selezionata.Name!="")
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
            if (selezionata != null && selezionata.Name != "")
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
            if ((pagina+1) * PokemonPerPagina <= tuttiPokemon.Count)
            {
                pagina++;
                DisegnaPokemon(pagina * PokemonPerPagina);
                numPag.Text = pagina +1+ "";
            }
        }

        internal void Ridimensiona(double Width,double Height)
        {
            Lista.Width = Width;
            Lista.Height = Height * partiLista / parti;
            Deck.Height = Height * partiMano / parti;
            Deck.Width = Width;
            unita = 100;
            CaricaPokemon();
        }
    }
}
