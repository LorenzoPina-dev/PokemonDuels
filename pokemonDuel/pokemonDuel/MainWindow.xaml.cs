using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using pokemonDuel.classi.Componenti;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using pokemonDuel.classi.Util;
using System.Windows.Controls;

namespace pokemonDuel
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CaricamentoBattaglia caricamento;
        PaginaPokemon pokemon;
        PaginaUtente utente;
        ShopPokemon shop;
        public MainWindow()
        {

            InitializeComponent();
            utente = new PaginaUtente();
            pokemon = new PaginaPokemon();
            caricamento = new CaricamentoBattaglia();
            shop = new ShopPokemon(pokemon);
            finestre.Children.Add(caricamento);
            finestre.Children.Add(pokemon);
            finestre.Children.Add(utente);
            finestre.Children.Add(shop);
            Width = SystemParameters.FullPrimaryScreenWidth;
            Height =SystemParameters.FullPrimaryScreenHeight;
            DatiCondivisi.Instance().main = this;
            DatiCondivisi.Instance().caricamento = caricamento;
            List<string> Giocatore = GestFile.LeggiInfoGiocatore();
            if(Giocatore.Count>0)
                DatiCondivisi.Instance().io = new Giocatore(Giocatore[0], true);
            if (Giocatore.Count > 1)
                shop.settaVolori(Giocatore[1]);
            GestioneTcp.Instance();
            Queue<Panel> elementi = new Queue<Panel>();
            elementi.Enqueue(content);
            while (elementi.Count != 0)
                foreach (UIElement el in elementi.Dequeue().Children)
                    if (el is ShopPokemon)
                        elementi.Enqueue(((ShopPokemon)el).Bottoni);
                    else if (el is PaginaUtente)
                        elementi.Enqueue(((PaginaUtente)el).Profilo);
                    else if (el is CaricamentoBattaglia)
                        elementi.Enqueue(((CaricamentoBattaglia)el).top);
                    else if(el is PaginaPokemon)
                        elementi.Enqueue(((PaginaPokemon)el).Bottoni);
                    else if (el is Control)
                    {
                        ((Control)el).Background = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
                        ((Control)el).Foreground = Brushes.White;
                    }
                    else if (el is Panel)
                        elementi.Enqueue((Panel)el);
        }
        private void NascondiTutto()
        {
            App.Visibility = Visibility.Hidden;
            battaglia.Visibility = Visibility.Hidden;
            caricamento.Visibility = Visibility.Hidden;
            pokemon.Visibility = Visibility.Hidden;
            utente.Visibility = Visibility.Hidden;
            shop.Visibility = Visibility.Hidden;
        }

        public void MostraFinestra(Finestra fin)
        {
            Dispatcher.Invoke(delegate
            {
                NascondiTutto();
                switch (fin)
                {
                    case Finestra.Utente:
                        App.Visibility = Visibility.Visible;
                        utente.Visibility = Visibility.Visible;
                        utente.disegna();
                        break;
                    case Finestra.Pokemon:
                        App.Visibility = Visibility.Visible;
                        pokemon.Visibility = Visibility.Visible;
                        break;
                    case Finestra.Inviti:
                        App.Visibility = Visibility.Visible;
                        caricamento.Visibility = Visibility.Visible;
                        break;
                    case Finestra.Battaglia:
                        battaglia.Visibility = Visibility.Visible;
                        break;
                    case Finestra.Shop:
                        App.Visibility = Visibility.Visible;
                        shop.Visibility = Visibility.Visible;
                        shop.TxtMateriali.Content = DatiCondivisi.Instance().io.Materiali + "";
                        break;
                }
            });
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = e.NewSize.Width;
            Height = e.NewSize.Height;
            Bottoni.Height = Height / 13;
            finestre.Width = Width;
            finestre.Height = Height-Bottoni.Height;
            caricamento.Height = Height;
            caricamento.Width = Width;
            pokemon.Height= Height - Bottoni.Height;
            shop.Height = Height - Bottoni.Height;
            shop.Width = Width;
            pokemon.Width = Width;
            battaglia.Width = Width;
            battaglia.Height = Height;
            MostraFinestra(Finestra.Utente);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DatiCondivisi.Instance().Avversario != null)
                DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tb", "0"));
            GestioneTcp.Instance().stop();
            GestFile.SalvaInfoGiocatore(DatiCondivisi.Instance().io.toCsv()+"\r\n"+shop.Salva());
        }


        private void Deck_Button(object sender, RoutedEventArgs e)
        {
            MostraFinestra(Finestra.Pokemon);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MostraFinestra(Finestra.Inviti);
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            MostraFinestra(Finestra.Utente);
        }
        private void Shop_Click(object sender, RoutedEventArgs e)
        {
            MostraFinestra(Finestra.Shop);
        }

        public void AggiornaXp(int xp)
        {
            utente.AggiornaXp(xp);
        }

    }
}