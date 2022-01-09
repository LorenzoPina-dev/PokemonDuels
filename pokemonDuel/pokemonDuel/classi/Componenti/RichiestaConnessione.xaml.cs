using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.Logicagioco;
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

namespace pokemonDuel.classi.Componenti
{
    /// <summary>
    /// Logica di interazione per RichiestaConnessione.xaml
    /// </summary>
    public partial class RichiestaConnessione : UserControl
    {
        public GestioneConnessione gr;
        CaricamentoBattaglia caricamento;
        public RichiestaConnessione( GestioneConnessione gr,CaricamentoBattaglia caricamento)
        {
            InitializeComponent();
            this.gr = gr;
            Username.Content = gr.Avversario.Username;
            this.caricamento = caricamento;

            foreach (UIElement el in content.Children)
                if (el is Control)
                {
                    ((Control)el).Background = new SolidColorBrush(Color.FromArgb(255, 120, 120, 120));
                    ((Control)el).Foreground = Brushes.White;
                }
        }

        private void Accetta_Click(object sender, RoutedEventArgs e)
        {
            Giocatore io = DatiCondivisi.Instance().io;
            string s = "";
            foreach (Pokemon m in io.Deck)
                s += ";" + m.id;
            gr.Invia(new Messaggio("y", io.Username + s));
            caricamento.Rimuovi(this);
        }

        private void Nega_Click(object sender, RoutedEventArgs e)
        {
            caricamento.Rimuovi(this);
            gr.Invia(new Messaggio("n", ""));
        }
    }
}
