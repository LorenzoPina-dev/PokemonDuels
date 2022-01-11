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
    /// Logica di interazione per ShopPokemon.xaml
    /// </summary>
    public partial class ShopPokemon : UserControl
    {

        public string PedinaVuota = Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png";
        private Dictionary<int, Pokemon> tuttiPokemon;
        private Rectangle selezionata;
        int PokemonPerPagina;
        int pagina;
        PaginaPokemon paginaPoK;
        HashSet<int> comprati;
        public ShopPokemon(PaginaPokemon pagina)
        {
            InitializeComponent();
            this.paginaPoK = pagina;
            tuttiPokemon = new Dictionary<int, Pokemon>();
            selezionata = null;
            comprati = new HashSet<int>();
            foreach (KeyValuePair<int, Pokemon> p in StoreInfo.Instance().Pokedex)
            {
                tuttiPokemon.Add(p.Key, p.Value);
                if (p.Value.Materiali <= 250)
                    CompraPokemon(p.Value.id);
            }
            this.Background = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
            DaColorare.Background = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
            paginaPoK.DisegnaPokemon(0);
        }
        private void CaricaPokemon()
        {
            PokemonPerPagina = DisegnaPokemon(0);
            pagina = 0;
            numPag.Text= pagina + 1 + "";
        }
        


        internal void Ridimensiona(double Width, double Height)
        {
            Bottoni.Height = Height / 11;
            Lista.Width = Width;
            Lista.Height = Height -Bottoni.Height;
            TxtMateriali.Content = DatiCondivisi.Instance().io.Materiali+"";
            CaricaPokemon();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona(e.NewSize.Width, e.NewSize.Height);
            CaricaPokemon();
        }
        public void settaVolori(string v)
        {
            string[] split = v.Split(';');
            foreach (string s in split)
            {
                int indice = int.Parse(s);
                if (tuttiPokemon.ContainsKey(indice))
                    CompraPokemon(indice);
            }
            paginaPoK.DisegnaPokemon(0);
        }
        public void CompraPokemon(int indice)
        {
            tuttiPokemon.Remove(indice);
            comprati.Add(indice);
            paginaPoK.tuttiPokemon.Add(indice, (Pokemon)StoreInfo.Instance().Pokedex[indice].Clone());
        }

        private void Prima_Click(object sender, RoutedEventArgs e)
        {
            if (pagina - 1 >= 0)
            {
                pagina--;
                DisegnaPokemon(pagina * PokemonPerPagina);
                numPag.Text = pagina + 1 + "";
            }
        }

        private void Dopo_Click(object sender, RoutedEventArgs e)
        {
            if ((pagina + 1) * PokemonPerPagina <= tuttiPokemon.Count)
            {
                pagina++;
                DisegnaPokemon(pagina * PokemonPerPagina);
                numPag.Text = pagina + 1 + "";
            }
        }

        private int DisegnaPokemon(int ind)
        {
            Lista.Children.Clear();
            int x = 0, y = 0, unita = 150, j = ind;
            var keys = tuttiPokemon.Keys.ToList();
            for (; j < keys.Count; j++)
            {
                Pokemon pok = (Pokemon)tuttiPokemon[keys[j]].Clone();
                GestioneCanvas.RenderPokemon(Lista, pok, unita, x, y, true, true);

                Rectangle container = new Rectangle();
                container.Fill = Brushes.Transparent;
                container.Width = unita;
                container.Height = unita;
                container.Margin = new Thickness(x, y, Lista.Width - x - unita, Lista.Height - y - unita);
                container.Name = "P_" + pok.id;
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

                if (y + unita >= Lista.Height)
                    return j - ind;
            }
            return j - ind;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pokemon p = tuttiPokemon[int.Parse(selezionata.Name.Split('_')[1])];
            if (DatiCondivisi.Instance().io.Materiali >= p.Materiali)
            {
                paginaPoK.tuttiPokemon.Add(p.id, p);
                tuttiPokemon.Remove(p.id);
                paginaPoK.DisegnaPokemon(0);
                comprati.Add(p.id);
                DisegnaPokemon(PokemonPerPagina * pagina);
                DatiCondivisi.Instance().io.Materiali -= p.Materiali;
                TxtMateriali.Content = DatiCondivisi.Instance().io.Materiali + "";
            }
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
        public string Salva()
        {
            string file = ""+comprati.ElementAt(0);
            for (int i = 1; i < comprati.Count; i++)
                file += ";" + comprati.ElementAt(i);
            return file;
        }

    }
}
