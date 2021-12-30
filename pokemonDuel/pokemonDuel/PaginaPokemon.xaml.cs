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
        private int W, H;
        private List<Pokemon> tuttiPokemon;
        private Pokemon selezionata;
        private Giocatore g;
        public PaginaPokemon()
        {
            InitializeComponent();
            this.W = (int)Width;
            this.H = (int) Height;
            tuttiPokemon = new List<Pokemon>();
            foreach (Pokemon p in StoreInfo.Instance().Pokedex.Values)
                tuttiPokemon.Add(p);
            selezionata = null;
            g = new Giocatore();
            CaricaPokemon();
        }
        private void CaricaPokemon()
        {
            Lista.Children.Clear();
            Deck.Children.Clear();
            int x = 0,y=0,unita=75;
            foreach (Pokemon p in tuttiPokemon)
            {
                Pokemon pok = (Pokemon)p.Clone();
                Image i = new Image();
                i.Source = new BitmapImage(new Uri(pok.UrlTexture));
                i.Width = unita;
                i.Height = unita;
                i.Margin = new Thickness(x, y, W-x-unita, H*3.4/6-y-unita);
                i.Name = "P_" + pok.id;
                x += unita;
                i.MouseDown += Cliccata;
                if (x+unita >= W)
                {
                    x = 0;
                    y += unita;
                }
                Lista.Children.Add(i);
                if (y+unita >= H*3.4/6)
                    break;
            }

            x = 0;unita = W / 6;
            for (int j= 0;j < 6;j++)
            {
                Image i = new Image();
                if(DatiCondivisi.Instance().io.Deck.Count>j)
                    i.Source = new BitmapImage(new Uri(DatiCondivisi.Instance().io.Deck[j].UrlTexture));
                else
                    i.Source = new BitmapImage(new Uri(PedinaVuota));
                i.Name = "D_" + j;
                i.MouseDown += Cliccata;
                i.Width = unita;
                i.Height = (int)Math.Min(unita, H /6);
                i.Margin = new Thickness(x, H/24, W - x - unita, 0);
                x += unita;
                Deck.Children.Add(i);
            }
        }

        private void Cliccata(object sender, MouseButtonEventArgs e)
        {
            Image temp = (Image)e.Source;
            string[] split = temp.Name.Split('_');
            if (split[0] == "P")
                selezionata = (Pokemon)StoreInfo.Instance().Pokedex[int.Parse(temp.Name.Split('_')[1])];
            else if (split[0] == "D")
                selezionata = g.Deck[int.Parse(split[1])];
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
            if (DatiCondivisi.Instance().io.Deck.Count < 6 && selezionata!=null)
            {
                Pokemon p = (Pokemon)selezionata.Clone();
                p.mio = true;
                DatiCondivisi.Instance().io.Deck.Add(p);
                tuttiPokemon.Remove(selezionata);
                CaricaPokemon();
                selezionata = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selezionata!=null && DatiCondivisi.Instance().io.Deck.Contains(selezionata))
            {
                DatiCondivisi.Instance().io.Deck.Remove(selezionata);
                tuttiPokemon.Add(selezionata);
                CaricaPokemon(); 
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DatiCondivisi.Instance().io.Username = Username.Text;
        }

        internal void Ridimensiona(double width, double height)
        {
            W = (int)width;
            H = (int)height;
            CaricaPokemon();
        }
    }
}
