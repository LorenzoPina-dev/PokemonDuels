using pokemonDuel.classi.GestioneFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pokemonDuel.classi.Logicagioco
{
    public partial class Ruota : UserControl
    {
        Graphics g;
        int W, H;
        Pokemon _pokemon;
        int ultimoAngolo;
        List<int> percentuali;
        public Pokemon Pokemon
        {
            get { return _pokemon; }
            set{ _pokemon = value; percentuali = CalcolaPerc(); Gira(0); }
        }


        public Ruota(int W, int H)
        {
            InitializeComponent();
            this.W = W;
            this.H = H;
            g = canvas.CreateGraphics();
            ultimoAngolo = 0;

        }
        private List<int> CalcolaPerc()
        {
            List<int> ris = new List<int>();
            foreach (int per in _pokemon.Mosse)
                ris.Add(StoreInfo.Instance().Mosse[per].percentuale);
            return ris;
        }

        public Mossa GetRisultato()
        {
            int temp = ultimoAngolo;
            int i = 0;
            while ((temp -= percentuali[i]) > 0) i++;
            return StoreInfo.Instance().Mosse[_pokemon.Mosse[i]];
        }

        public void Gira(int Gradi)
        {
            ultimoAngolo = Gradi;
            g.Clear(Color.White);
            int d = Math.Min(W - 10, H - 10);
            int Arrivati = Gradi;
            foreach (int per in percentuali)
            {
                g.FillPie(Brushes.Yellow, new Rectangle(new Point(10, 10), new Size(d, d)), Arrivati, Arrivati + per);
                Arrivati += per;
            }
            Arrivati = Gradi;
            foreach (int per in _pokemon.Mosse)
            {
                Pen p = new Pen(Color.Black, 2);
                g.DrawPie(p, new Rectangle(new Point(10, 10), new Size(d, d)), Arrivati, Arrivati + per);
                Arrivati += per;
            }
            g.Flush();
        }

    }
}
