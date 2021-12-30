using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
    /// Logica di interazione per CaricamentoBattaglia.xaml
    /// </summary>
    public partial class CaricamentoBattaglia : UserControl
    {
        public Battaglia Campo;
        public CaricamentoBattaglia()
        {
            InitializeComponent();
            Campo = new Battaglia();
            Campo.Height = Height;
            Campo.Width = Width;
            Inviti.Background = Brushes.Red;
            finestra.Children.Add(Campo);
        }
        public void AddConnessione(GestioneConnessione gr)
        {
            Dispatcher.Invoke(delegate {
                Inviti.Items.Add(new RichiestaConnessione(gr, this));
            });
        }
        public void Rimuovi(RichiestaConnessione r)
        {
            Inviti.Items.Remove(r);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona();
        }

        public void Ridimensiona()
        {
            Campo.Width = Width;
            Campo.Height = Height;
            Campo.Ridimensiona(Campo.Width, Campo.Height);
            Inviti.Height = Height * 8 / 9;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GestioneTcp.Connetti(IpConnessione.Text);
        }
        public void MostraMappa()
        {
            Campo.Visibility = Visibility.Visible;
            GestInviti.Visibility = Visibility.Hidden;
        }
        public void MostraInviti()
        {
            Campo.Visibility = Visibility.Hidden;
            GestInviti.Visibility = Visibility.Visible;
        }
    }
}
