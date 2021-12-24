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
        int d;
        public Pokemon Pokemon
        {
            get { return _pokemon; }
            set{ _pokemon = value;
                if (_pokemon != null)
                {
                percentuali = CalcolaPerc(); Gira(0);
                } }
        }


        public Ruota(int W, int H)
        {
            InitializeComponent();
            this.W = W;
            this.H = H;
            g = canvas.CreateGraphics();
            ultimoAngolo = 0;
            d = Math.Min(W, H);

        }
        private List<int> CalcolaPerc()
        {
            List<int> ris = new List<int>();
            float somma = 0;
            foreach (int per in _pokemon.Mosse)
            {
                ris.Add(StoreInfo.Instance().Mosse[per].percentuale);
                somma += StoreInfo.Instance().Mosse[per].percentuale;
            }
            for (int i = 0; i < ris.Count; i++)
                ris[i] =(int)( ris[i] / somma * 360);
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

            if (Pokemon != null)
            {
                ultimoAngolo = Gradi;
                g.Clear(Color.White);
                int Arrivati = Gradi;
                for (int i = 0; i < _pokemon.Mosse.Count; i++)
                {
                    Mossa m = StoreInfo.Instance().Mosse[_pokemon.Mosse[i]];
                    g.FillPie(m.colore, new Rectangle(new Point(0, 0), new Size(d, d)), Arrivati, percentuali[i]);

                    int x, y;
                    //x =(int)( d / 2 + (Math.Cos(Util.getRad(Arrivati)) + Math.Cos(Util.getRad(Arrivati + percentuali[i])))/2 * (d / 2) -30);
                    //y = (int)(d / 2 + (Math.Sin(Util.getRad(Arrivati))+ Math.Sin(Util.getRad(Arrivati + percentuali[i])))/2 * (d / 2) - 5);
                    int gradi = (Arrivati * 2 + percentuali[i]) / 2;
                    x = (int)(d / 2 + Math.Cos(Util.getRad(gradi)) * (d / 2));
                    y = (int)(d / 2 + Math.Sin(Util.getRad(gradi)) * (d / 2));
                    if (x > d * 5 / 7)
                        x -= d * 1 / 7;
                    if (y > d * 5 / 7)
                        y -= d * 1 / 7;
                    g.DrawString(m.nome + " " + m.danno, new Font("Arial", 8), Brushes.Black, new Point(x, y));
                    Arrivati += percentuali[i];
                }
                Arrivati = Gradi;
                for (int i = 0; i < _pokemon.Mosse.Count; i++)
                {
                    Mossa m = StoreInfo.Instance().Mosse[_pokemon.Mosse[i]];
                    Pen p = new Pen(Color.Black, 2);
                    g.DrawPie(p, new Rectangle(new Point(0, 0), new Size(d, d)), Arrivati, percentuali[i]);
                    Arrivati += percentuali[i];
                }
            }
        }

    }
}
