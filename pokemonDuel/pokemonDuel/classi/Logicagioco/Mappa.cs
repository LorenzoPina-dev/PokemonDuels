using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pokemonDuel.classi.Logicagioco
{
    class Mappa
    {
        public List<Nodo> mappa;
        int Destinazione;
        bool turno;
        List<int> startPosizionamento;
        Canvas myCanvas;
        MainWindow m;
        int partiX, partiY;
        int distanza = 0;
        List<Rectangle> Grafica;
        int PartiPerMano;
        int PedineMappa,PedineMano;
        Nodo Selezionato;
        int Nturno,Tvinti;
        public Giocatore io, altro;
        public Ruota r;
        GestioneRuota gr;
        public int mioAttacco;
        Timer t;

        public Mappa(MainWindow m,Giocatore io,Giocatore altro)
        {
            this.m = m;
            Destinazione = 3;
            PedineMano = 6;
            this.mappa = new List<Nodo>();
            startPosizionamento = new List<int>();
            creaNodi();
            creaCollegamenti();
            myCanvas = m.myCanvas;
            Selezionato = null;
            turno = true;
            Nturno = 0;
            Tvinti = 0;
            this.io = io;
            this.altro = altro;
            m.host.Child = r;
            gr = null;

            r = new Ruota((int)m.host.Width, (int)m.host.Height);
            r.Pokemon = StoreInfo.Instance().Pokedex[10];
            m.host.Child = r;
            RicominciaGioco();

        }
        public int angolo=0;
        public void Gira(int gradi)
        {
            t = new Timer();
            t.Interval = 1000;
            t.Elapsed += T_Elapsed;
            angolo = gradi;
            t.Start();
        }


        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            r.Gira(angolo);
            t.Stop();
            angolo = 0;
        }




        private void Click_Pedina(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle el = (Rectangle)e.Source;
            Nodo cliccato = mappa[int.Parse(el.Name.Split('_')[1])];
            if (!cliccato.presentePokemon)
            {
                if (turno && cliccato.selezionato)
                    Muovi(cliccato);
            }
            else
            {
                if (turno)
                {
                    if (cliccato.pokemon.mio)
                        if (cliccato.indice >= PedineMappa)
                        {
                            HashSet<int> daEvidenziare = new HashSet<int>();
                            foreach (int partenza in startPosizionamento)
                            {
                                HashSet<int> parziale = mappa[partenza].Raggiungibili(mappa, cliccato.pokemon.Salti);
                                foreach (int p in parziale)
                                    daEvidenziare.Add(p);
                            }
                            Mostra(cliccato, daEvidenziare);
                        }
                        else
                            Mostra(cliccato, cliccato.Raggiungibili(mappa, cliccato.pokemon.Salti));
                    else if(Selezionato!=null && Selezionato.vicini.Contains(cliccato.indice))
                        Attacca(cliccato.pokemon);
                }
            }
        }

        private void Attacca(Pokemon cliccato)
        {
            gr = new GestioneRuota();
            gr.ruota = r;
            gr.Pokemon = Selezionato.pokemon;
        }


        public void Upload()
        {
            if (gr != null)
                if (gr.Risultato == null)
                {
                    int ris = gr.Upload();
                    if(ris!=0)
                        Gira(ris);
                }
                else
                { mioAttacco = gr.Risultato.danno;
                    MessageBox.Show(mioAttacco+"");
                    gr = null;
                }
        }

        private void Ridisegna()
        {
            foreach (Nodo n in mappa)
            {
                if (n.presentePokemon)
                {
                    Grafica[n.indice].Fill = n.pokemon.Render();
                    Grafica[n.indice].Stroke =Brushes.Transparent;
                }
                else { 
                    Grafica[n.indice].Fill = Brushes.White;
                Grafica[n.indice].Stroke = Brushes.Black;
            }
            n.selezionato = false;
            }
        }

        private void Muovi(Nodo cliccato)
        {
            cliccato.pokemon = Selezionato.pokemon;
            cliccato.presentePokemon = true;
            Selezionato.pokemon = null;
            Selezionato.presentePokemon = false;
            if(cliccato.indice==Destinazione)
            {
                Nturno++;
                if (turno)
                {
                    Tvinti++;
                    Console.WriteLine("vinto");
                }
                RicominciaGioco();
            }
            Ridisegna();
        }

        private void RicominciaGioco()
        {
            foreach(Nodo n in mappa)
            {
                n.pokemon = null;
                n.presentePokemon = false;
                n.selezionato = false;
            }
            for (int i = 0; i < io.Deck.Count; i++)
            {
                mappa[PedineMappa+PedineMano + i].pokemon = io.Deck[i];
                mappa[PedineMappa+ PedineMano + i].presentePokemon = true;
            }
            for (int i = 0; i < altro.Deck.Count; i++)
            {
                mappa[PedineMappa+ i].pokemon = altro.Deck[i];
                mappa[PedineMappa  + i].presentePokemon = true;
            }
            mappa[20].pokemon = altro.Deck[2];
            mappa[20].presentePokemon = true;
        }

        private void Mostra(Nodo cliccato, HashSet<int> hashSet)
        {
            Ridisegna();
            Selezionato = cliccato;
            foreach (int i in hashSet)
            {
                mappa[i].selezionato = true;
                Grafica[i].Fill = Brushes.Yellow;
            }
        }

        public void creaNodi()
        {
            List<string> righe = File.Leggi("./file/Mappa.csv");
            string[] split = righe[0].Split(';');
            partiX = int.Parse(split[0]);
            partiY = int.Parse(split[1]);
            split = righe[1].Split(';');
            //GrandezzaMano = int.Parse(split[0]);
            PartiPerMano = int.Parse(split[1]);
            foreach (string s in righe[2].Split(';'))
                startPosizionamento.Add(int.Parse(s));
            int x = 0;
            for (int i = 0; i <partiY; i++)
            {
                string[] celle = righe[3 + PartiPerMano+i].Split(';');
                for (int j = 0; j < celle.Length; j++)
                    if (celle[j] == "1")
                    {
                        Nodo n = new Nodo();
                        n.x = j + 1;
                        n.y = i+ PartiPerMano;
                        n.indice = x++;
                        mappa.Add(n);
                    }
            }
            PedineMappa = x;
            for (int i = 0; i < PartiPerMano; i++)
            {
                string[] celle = righe[3 + i].Split(';');
                for (int j = 0; j < celle.Length; j++)
                    if (celle[j] == "1")
                    {
                        Nodo n = new Nodo();
                        n.x = j + 1;
                        n.y = i ;
                        n.indice = x++;
                        mappa.Add(n);
                    }
            }
            for (int i = 0; i < PartiPerMano; i++)
            {
                string[] celle = righe[3 + PartiPerMano + partiY+ i].Split(';');
                for (int j = 0; j < celle.Length; j++)
                    if (celle[j] == "1")
                    {
                        Nodo n = new Nodo();
                        n.x = j + 1;
                        n.y = i + PartiPerMano+partiY;
                        n.indice = x++;
                        mappa.Add(n);
                    }
            }
        }
        public void creaCollegamenti()
        {
            List<string> righe = File.Leggi("./file/Collegamenti.csv");
            foreach (string s in righe)
            {
                string[] estremi = s.Split(';');
                int P = int.Parse(estremi[0]), F = int.Parse(estremi[1]);
                mappa[P].vicini.Add(F);
                mappa[F].vicini.Add(P);
            }
        }
        public void Disegna()
        {
            Rectangle r = new Rectangle();

            r.Width = m.Width;
            r.Height = m.Height;
            r.Fill = Brushes.White;
            myCanvas.Children.Add(r);
            Line l = new Line();
            l.X1 = distanza;
            l.Y1 = 0;
            l.X2 = distanza;
            l.Y2 = 2000;
            l.Stroke = Brushes.Black;
            l.StrokeThickness = 2;
            myCanvas.Children.Add(l);
            DisegnaCollegamenti();
            DisegnaNodi();
        }
        public void DisegnaNodi()
        {
            double dimensioneX = (myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (myCanvas.Height - 40) / (partiY +PartiPerMano*2);
            Grafica = new List<Rectangle>();

            foreach (Nodo n in mappa)
            {
                Rectangle b = new Rectangle();
                b.Stroke = Brushes.Black;
                b.Height = dimensioneY*3/4;
                b.Width = dimensioneX * 3 / 4;
                if (n.presentePokemon)
                {
                    b.Fill = n.pokemon.Render();
                    b.Stroke = Brushes.Transparent;
                }
                else
                    b.Fill = Brushes.White;
                Canvas.SetTop(b, dimensioneY * n.y +dimensioneY / 4);
                Canvas.SetLeft(b, distanza + dimensioneX * n.x + dimensioneX / 4);
                b.Name = "p_" + n.indice;
                b.MouseLeftButtonDown += Click_Pedina;
                Grafica.Add(b);
                myCanvas.Children.Add(b);
            }
        }


        public void DisegnaCollegamenti()
        {
            double dimensioneX = (myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (myCanvas.Height - 40) / (partiY+PartiPerMano*2);
            foreach (Nodo n in mappa)
            {
                foreach (int vicino in n.vicini)
                {
                    Line l = new Line();
                    l.X1 = distanza + n.x * dimensioneX + dimensioneY * 3 / 4;
                    l.Y1 = n.y * dimensioneY + dimensioneY *6 / 7;
                    l.X2 = distanza + mappa[vicino].x * dimensioneX + dimensioneY *3/4;
                    l.Y2 = mappa[vicino].y * dimensioneY + dimensioneY * 6 /7;
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 2;
                    myCanvas.Children.Add(l);
                }
            }
        }
    }
}
