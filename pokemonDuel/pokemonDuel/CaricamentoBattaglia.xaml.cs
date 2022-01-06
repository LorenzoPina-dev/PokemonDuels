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
        public CaricamentoBattaglia()
        {
            InitializeComponent();
            Inviti.Background = Brushes.Red;
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
            Ridimensiona(e.NewSize.Width,e.NewSize.Height);
        }

        public void Ridimensiona(double Width,double Height)
        {
            this.Height = Height;
            this.Width = Width;
            Inviti.Height = Height- DatiCondivisi.Instance().main.Bottoni.Height;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GestioneTcp.Connetti(IpConnessione.Text);
        }
    }
}
