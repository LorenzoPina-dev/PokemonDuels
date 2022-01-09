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

namespace pokemonDuel.classi.Componenti
{
    /// <summary>
    /// Logica di interazione per CaricamentoBattaglia.xaml
    /// </summary>
    public partial class CaricamentoBattaglia : UserControl
    {
        public CaricamentoBattaglia()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));

        }
        public void AddConnessione(GestioneConnessione gr)
        {
            Dispatcher.Invoke(delegate {
                RichiestaConnessione rc = new RichiestaConnessione(gr, this);
                rc.Width = Inviti.Width;
                rc.Height = 50;
                Inviti.Items.Add(rc);
            });
        }
        public void Rimuovi(RichiestaConnessione r)
        {
            Inviti.Items.Remove(r);
            GestioneTcp.Instance().Disconnetti(r.gr);
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
            Inviti.Width = Width;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GestioneTcp.Connetti(IpConnessione.Text);
        }
    }
}
