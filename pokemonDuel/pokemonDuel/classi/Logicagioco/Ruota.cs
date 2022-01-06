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
        double W, H;
        Pokemon _pokemon;
        int ultimoAngolo;
        List<int> percentuali;
        int d;
        private object synPokemon;
        int ris = -1;
        public Pokemon Pokemon
        {
            get { lock (synPokemon) { return _pokemon; } }
            set
            {
                lock (synPokemon)
                {
                    _pokemon = value;
                    if (_pokemon != null)
                    {
                        ultimoAngolo = 0;
                        ris = -1;
                        percentuali = CalcolaPerc(); Gira(0);
                    }
                }
            }
        }


        public Ruota()
        {
            InitializeComponent();
            synPokemon = new object();
            ultimoAngolo = 0;
            d = (int)Math.Min(W, H);

        }
        public void CambiaDimensioni(double W, double H)
        {
            this.W = W;
            this.H = H;
            d = (int)Math.Min(W, H);
            g = canvas.CreateGraphics();
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
            int somma2 = 0;
            for (int i = 0; i < ris.Count; i++)
            {
                ris[i] = (int)(ris[i] / somma * 360);
                somma2 += ris[i];
            }
            if (somma2 < 360)
                ris[ris.Count - 1] += 360 - somma2;
            return ris;
        }

        public Mossa GetRisultato()
        {
            return StoreInfo.Instance().Mosse[_pokemon.Mosse[ris]];
        }

        public void Gira(int Gradi)     //disegna la ruota ruotata in base ai gradi che li vengono passati come parametro
        {
            try
            {

            lock (synPokemon)
            {
                d = Math.Min(canvas.Width, canvas.Height);
                ris = -1;
                if (Pokemon != null)
                {
                    ultimoAngolo =(ultimoAngolo+ Gradi)%360;
                    g.Clear(Color.White);
                    int Arrivati = ultimoAngolo;
                    for (int i = 0; i < _pokemon.Mosse.Count; i++)
                    {
                        if (((Arrivati + percentuali[i])/360>=1 || Arrivati == 0)&& ris==-1)
                            ris = i;
                        Mossa m = StoreInfo.Instance().Mosse[_pokemon.Mosse[i]];
                        g.FillPie(m.colore, new Rectangle(new Point(0, 0), new Size(d, d)), Arrivati, percentuali[i]);

                        int x, y;
                        int gradi = (Arrivati * 2 + percentuali[i]) / 2;
                        x = (int)(d / 2 + Math.Cos(Conversione.getRad(gradi)) * (d / 2));
                        y = (int)(d / 2 + Math.Sin(Conversione.getRad(gradi)) * (d / 2));
                        if (x > d * 5 / 7)
                            x -= d * 1 / 7;
                        if (y > d * 5 / 7)
                            y -= d * 1 / 7;
                        g.DrawString(m.nome + " " + m.danno, new Font("Arial", 8), Brushes.Black, new Point(x, y));
                        Arrivati += percentuali[i];
                    }
                    Arrivati = ultimoAngolo;
                    for (int i = 0; i < _pokemon.Mosse.Count; i++)
                    {
                        Pen p = new Pen(Color.Black, 2);
                        Mossa m = StoreInfo.Instance().Mosse[_pokemon.Mosse[i]];
                        g.DrawPie(p, new Rectangle(new Point(0, 0), new Size(d, d)), Arrivati, percentuali[i]);
                        Arrivati += percentuali[i];
                    }
                    Pen pen = new Pen(Color.Red, 4);
                    g.DrawLine(pen, (float)(d - 10), d / 2, d + 10, d / 2);
                    }
                }
            }catch (Exception) { }
        }
    }
}
