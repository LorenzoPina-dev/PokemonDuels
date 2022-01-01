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
            caricamento.Width = Width;
            caricamento.Height =Height;
            caricamento.Ridimensiona();
            Mappa.Children.Add(paginaPokemon);
        }

        public void MostraMappa()
        {
            Dispatcher.Invoke(delegate
            {
                caricamento.Visibility = Visibility.Visible;
                App.Visibility = Visibility.Visible;
                Mappa.Visibility = Visibility.Hidden;
                caricamento.MostraInviti();
            });
        }
        public void MostraPartita()
        {
            Dispatcher.Invoke(delegate
            {
                caricamento.Visibility = Visibility.Visible;
                App.Visibility = Visibility.Hidden;
                caricamento.MostraMappa();
                DatiCondivisi.Instance().M.Disegna();
                DatiCondivisi.Instance().M.RicominciaGioco();
            });
        }
        
        public void MostraApp()
        {
            Dispatcher.Invoke(delegate
            {
                caricamento.Visibility = Visibility.Hidden;
                App.Visibility = Visibility.Visible;
                Mappa.Visibility = Visibility.Visible;
                caricamento.MostraInviti();
            });
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
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DatiCondivisi.Instance().gt.stop();
        }
    }
}