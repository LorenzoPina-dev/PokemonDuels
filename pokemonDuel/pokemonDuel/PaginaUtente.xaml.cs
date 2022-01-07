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
    /// Logica di interazione per PaginaUtente.xaml
    /// </summary>
    public partial class PaginaUtente : UserControl
    {
        Pokemon selezionato;
        public PaginaUtente()
        {
            InitializeComponent();
            TxtNome.Text= DatiCondivisi.Instance().io.Username;
            TxtXp.Content = DatiCondivisi.Instance().io.Xp;
            selezionato = null;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Profilo.Height = 40;
            Mano.Height = e.NewSize.Height -40;
            Mano.Width = e.NewSize.Width;
        }
        public void disegna()
        {
            if (!(Mano.Width is double.NaN || Mano.Height is double.NaN))
            {
                Mano.Children.Clear();
                Rectangle primopok = new Rectangle();
                primopok.Width = 250;
                primopok.Height = 250;
                if (selezionato != null)
                    primopok.Fill = selezionato.Render();
                else if (DatiCondivisi.Instance().io.Deck.Count > 0)
                {
                    primopok.Fill = DatiCondivisi.Instance().io.Deck[0].Render(); 
                    Canvas.SetLeft(primopok, Mano.Width / 2 - primopok.Width / 2);
                    Canvas.SetTop(primopok, Mano.Height * 2 / 7 - primopok.Height / 2);
                }
                else
                { 
                    
                    primopok.Fill = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png")));
                    Canvas.SetLeft(primopok, Mano.Width / 2 - primopok.Width / 2-10);
                    Canvas.SetTop(primopok, Mano.Height * 2 / 7 - primopok.Height / 2+80);
                }
                Mano.Children.Add(primopok);
                int partenza = 0;
                int unita = 125;
                for (int i = 0; i < 3; i++)
                {
                    Rectangle pokemon = new Rectangle();
                    pokemon.Width = unita;
                    pokemon.Height = unita;
                    pokemon.MouseDown += Pokemon_MouseDown;
                    if(DatiCondivisi.Instance().io.Deck.Count>i)
                    pokemon.Fill = DatiCondivisi.Instance().io.Deck[i].Render();
                    else
                        pokemon.Fill= new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png")));
                    pokemon.Name = "P_" + i;
                    double x = Mano.Width / 2 + Math.Cos(Conversione.getRad(partenza)) * primopok.Width, y = Mano.Height* 2/7 - Math.Sin(Conversione.getRad(partenza)) * primopok.Height;
                    Canvas.SetRight(pokemon, x);
                    Canvas.SetTop(pokemon, y);
                    Mano.Children.Add(pokemon);

                    Rectangle Secondo = new Rectangle();
                    Secondo.Width = unita;
                    Secondo.Height = unita;
                    Secondo.MouseDown += Pokemon_MouseDown;
                    if (DatiCondivisi.Instance().io.Deck.Count > 5-i)
                        Secondo.Fill = DatiCondivisi.Instance().io.Deck[5-i].Render();
                    else
                        Secondo.Fill = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/file/Pedine/BaseVuota.png")));
                    Secondo.Name = "P_" + (5-i);
                    Canvas.SetLeft(Secondo, x);
                    Canvas.SetTop(Secondo, y);
                    Mano.Children.Add(Secondo);
                    partenza -= 30;
                    if (i == 2)
                        partenza -= 30;
                }
            }
        }

        private void Pokemon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle source = (Rectangle)e.Source;
            int indice = int.Parse(source.Name.Split('_')[1]);
            if (DatiCondivisi.Instance().io.Deck.Count > indice)
            {
                selezionato = DatiCondivisi.Instance().io.Deck[indice];
                source.Stroke = Brushes.LightBlue;
                source.StrokeThickness = 3;
                disegna();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DatiCondivisi.Instance().io.Username = TxtNome.Text;
        }

        public void AggiornaXp(int xp)
        {
            Dispatcher.Invoke(delegate
            {
                TxtXp.Content = xp;
            });
        }
    }
}
