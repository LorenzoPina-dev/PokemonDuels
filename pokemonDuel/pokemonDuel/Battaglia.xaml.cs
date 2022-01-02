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

namespace pokemonDuel
{
    /// <summary>
    /// Logica di interazione per Battaglia.xaml
    /// </summary>
    public partial class Battaglia : UserControl
    {
        public Battaglia()
        {
            InitializeComponent();
            DatiCondivisi.Instance().M = new Mappa(this);
            CompositionTarget.Rendering += Upload;

        }

        int i = 0;
        private void Upload(object sender, EventArgs e)
        {
            DatiCondivisi.Instance().Upload();
            //DatiCondivisi.Instance().M.Upload();
        }
        public void Ridimensiona()
        {
            myCanvas.Width = Width;
            myCanvas.Height = Height;
            double unita = Math.Min(Width / 2, Height / 2);
            host.Width = unita;
            host.Height = unita;
            CanvasAttacco.Width = Width;
            CanvasAttacco.Height = Height;
            if(DatiCondivisi.Instance().A!=null)
                DatiCondivisi.Instance().A.Render();
            host.Margin = new Thickness(0, Height - unita, Width - unita, 0);
            DatiCondivisi.Instance().M.Disegna();
            GestioneRuota.Instance().ruota.CambiaDimensioni(host.Width, host.Height);
        }
        public void MostraAttacco()
        {
            Attacco.Visibility = Visibility.Visible;
            DatiCondivisi.Instance().A.Render();
        }
        public void MostraMappa()
        {
            Attacco.Visibility = Visibility.Hidden;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Ridimensiona();
        }
    }
}
