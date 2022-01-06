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
        public PaginaUtente()
        {
            InitializeComponent();
            TxtNome.Text= DatiCondivisi.Instance().io.Username;
            TxtXp.Content = DatiCondivisi.Instance().io.Xp;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Profilo.Height = 40;
            Mano.Height = e.NewSize.Height -40;
            Mano.Width = e.NewSize.Width;
        }
        public void disegna()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DatiCondivisi.Instance().io.Username = TxtNome.Text;
        }

        public void AggiornaXp(int xp)
        {
            TxtXp.Content = xp;
        }
    }
}
