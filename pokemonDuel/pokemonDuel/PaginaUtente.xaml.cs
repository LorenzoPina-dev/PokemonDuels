using pokemonDuel.classi;
using System;
using System.Collections.Generic;
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
        public int Xp { get { return DatiCondivisi.Instance().io.Xp; } }
        public string Username { get { return DatiCondivisi.Instance().io.Username; } }
        public PaginaUtente()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Profilo.Height = e.NewSize.Height/ 4;
            Mano.Height = e.NewSize.Height * 3 / 4;
            Mano.Width = e.NewSize.Width;
        }
        public void disegna()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DatiCondivisi.Instance().io.Username = TxtNome.Text;
        }
    }
}
