using pokemonDuel.classi;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
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
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pokemonDuel.classi.Componenti
{
    /// <summary>
    /// Logica di interazione per Battaglia.xaml
    /// </summary>
    public partial class Battaglia : UserControl
    {
        public WindowsFormsHost host;
        public GestioneCanvas gestCanvas;
        public Battaglia()
        {
            InitializeComponent();
            host = new WindowsFormsHost();
            DatiCondivisi.Instance().b = this;
            DatiCondivisi.Instance().M = new Mappa();
            gestCanvas = new GestioneCanvas(CanvasEventi);
        }

        int i = 0;
        private void Ridimensiona(double Width,double Height)
        {
            if (!(Width is double.NaN && Height is double.NaN))
            {
                CanvasGiocatore.Width = Width;
                CanvasGiocatore.Height = Height / 12;
                myCanvas.Width = Width;
                myCanvas.Height = Height- CanvasGiocatore.Height;
                double unita = Math.Min(Width *3/4, Height *3/4);
                host.Width = unita;
                host.Height = unita;
                CanvasEventi.Width = Width;
                CanvasEventi.Height = Height;
                if (DatiCondivisi.Instance().A != null)
                    gestCanvas.MostraAttacca(true);
                GestioneRuota.Instance().ruota.CambiaDimensioni(unita, unita);
            }
        }
        public void MostraAttacco(bool MostraRuota)
        {
            Dispatcher.Invoke(delegate
            {
                Attacco.Visibility = Visibility.Visible;
                CanvasEventi.Visibility = Visibility.Visible;
                host.Visibility = Visibility.Visible;
                gestCanvas.MostraAttacca(MostraRuota);
            });
        }
        public void MostraUtil()
        {
            Dispatcher.Invoke(delegate
            {
                Attacco.Visibility = Visibility.Visible;
                CanvasEventi.Visibility = Visibility.Visible;
                host.Visibility = Visibility.Hidden;
            });
        }
        public void MostraMappa()
        {
            Dispatcher.Invoke(delegate
            {
                Attacco.Visibility = Visibility.Hidden;
                myCanvas.Visibility = Visibility.Visible;
                CanvasEventi.Visibility = Visibility.Hidden;
            });
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona(e.NewSize.Width,e.NewSize.Height);
        }
    }
}
