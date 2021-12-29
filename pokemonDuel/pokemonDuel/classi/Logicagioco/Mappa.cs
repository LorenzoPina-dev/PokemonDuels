using pokemonDuel.classi.Comunicazione;
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
        bool _turno;
        public bool Turno
        {
            get { lock (synTurno) { return _turno; } }
            set { lock (synTurno) { _turno = value; } }
        }
        List<int> startPosizionamento;
        Canvas myCanvas;
        public Battaglia m;
        int partiX, partiY;
        int distanza = 0;
        List<Rectangle> Grafica;
        int PartiPerMano;
        int PedineMappa, PedineMano;
        Nodo Selezionato;
        int Nturno, Tvinti;
        GestioneRuota gr;
        public int mioAttacco;
        int turniAPartita = 3;
        private object synTurno;

        internal void RimettiNellaMano(Nodo perdente)
        {
            int partenza = PedineMappa;
            if (perdente.pokemon.mio)
                partenza += PedineMano;
            for (int i=0;i<PedineMano;i++)
                if(!mappa[partenza + i].presentePokemon)
                    Muovi(perdente,mappa[partenza + i]);
        }

        public Mappa(Battaglia m)
        {
            this.m = m;
            synTurno = new object();
            Destinazione = 3;
            PedineMano = 6;
            this.mappa = new List<Nodo>();
            startPosizionamento = new List<int>();
            creaNodi();
            creaCollegamenti();
            myCanvas = m.myCanvas;
            Selezionato = null;
            Turno = true;
            Nturno = 0;
            Tvinti = 0;
            gr = null;

        }
       
        private void Click_Pedina(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle el = (Rectangle)e.Source;
            Nodo cliccato = mappa[int.Parse(el.Name.Split('_')[1])];
            if (!cliccato.presentePokemon)
            {
                if (Turno && cliccato.selezionato)
                    Muovi(Selezionato,cliccato);
            }
            else
            {
                if (Turno)
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
                    else if (Selezionato != null && Selezionato.vicini.Contains(cliccato.indice))
                        GiraRuota(cliccato);
                }
            }
        }
        public void GiraRuota(Nodo Attaccato)
        {
            DatiCondivisi.Instance().A=new Attacco(Selezionato, Attaccato, null, null);
            GestioneRuota.Instance().Start();
        }



        

        private void Ridisegna()
        {
            foreach (Nodo n in mappa)
            {
                if (n.presentePokemon)
                {
                    if (n.pokemon == null)
                        n.presentePokemon = false;
                    Grafica[n.indice].Fill = n.pokemon.Render();
                    Grafica[n.indice].Stroke = Brushes.Transparent;
                }
                else
                {
                    Grafica[n.indice].Fill = Brushes.White;
                    Grafica[n.indice].Stroke = Brushes.Black;
                }
                n.selezionato = false;
            }
        }

        public void Muovi(int idPartenza,int idDetinazione)
        {
            Muovi(mappa[idPartenza], mappa[idDetinazione]);
        }

        public void Muovi(Nodo partenza, Nodo destinazione)
        {
            lock (this)
            {
                destinazione.pokemon = partenza.pokemon;
                destinazione.presentePokemon = true;
                partenza.pokemon = null;
                partenza.presentePokemon = false;
                if (destinazione.indice == Destinazione)
                {
                    Nturno++;
                    if (Turno)
                    {
                        Tvinti++;
                        DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tr", "1"));
                        if (Nturno == turniAPartita)
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tb", "1"));
                        Console.WriteLine("vinto");
                    }
                    RicominciaGioco();
                }
                if(partenza.indice>PedineMappa)
                    DatiCondivisi.Instance().Avversario.Invia(new Messaggio("s", partenza.indice + ";" + destinazione.indice));
                else
                    DatiCondivisi.Instance().Avversario.Invia(new Messaggio("m", partenza.indice + ";" + destinazione.indice));
                DatiCondivisi.Instance().Avversario.Invia(new Messaggio("t", ""));
                Turno = !Turno;
                Ridisegna();
            }
        }

        public void RicominciaGioco()
        {
            Giocatore io = DatiCondivisi.Instance().io;
            Giocatore altro = DatiCondivisi.Instance().altro;
            foreach (Nodo n in mappa)
            {
                n.pokemon = null;
                n.presentePokemon = false;
                n.selezionato = false;
            }
            for (int i = 0; i < io.Deck.Count; i++)
            {
                mappa[PedineMappa + PedineMano + i].pokemon = io.Deck[i];
                mappa[PedineMappa + PedineMano + i].presentePokemon = true;
            }
            for (int i = 0; i < altro.Deck.Count; i++)
            {
                mappa[PedineMappa + i].pokemon = altro.Deck[i];
                mappa[PedineMappa + i].presentePokemon = true;
            }
            Ridisegna();
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
            for (int i = 0; i < partiY; i++)
            {
                string[] celle = righe[3 + PartiPerMano + i].Split(';');
                for (int j = 0; j < celle.Length; j++)
                    if (celle[j] == "1")
                    {
                        Nodo n = new Nodo();
                        n.x = j + 1;
                        n.y = i + PartiPerMano;
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
                        n.y = i;
                        n.indice = x++;
                        mappa.Add(n);
                    }
            }
            for (int i = 0; i < PartiPerMano; i++)
            {
                string[] celle = righe[3 + PartiPerMano + partiY + i].Split(';');
                for (int j = 0; j < celle.Length; j++)
                    if (celle[j] == "1")
                    {
                        Nodo n = new Nodo();
                        n.x = j + 1;
                        n.y = i + PartiPerMano + partiY;
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
            RicominciaGioco();
        }
        public void DisegnaNodi()
        {
            double dimensioneX = (myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (myCanvas.Height - 40) / (partiY + PartiPerMano * 2);
            Grafica = new List<Rectangle>();

            foreach (Nodo n in mappa)
            {
                Rectangle b = new Rectangle();
                b.Stroke = Brushes.Black;
                b.Height = dimensioneY * 3 / 4;
                b.Width = dimensioneX * 3 / 4;
                if (n.presentePokemon)
                {
                    b.Fill = n.pokemon.Render();
                    b.Stroke = Brushes.Transparent;
                }
                else
                    b.Fill = Brushes.White;
                Canvas.SetTop(b, dimensioneY * n.y + dimensioneY / 4);
                Canvas.SetLeft(b, distanza + dimensioneX * n.x + dimensioneX / 4);
                b.Name = "p_" + n.indice;
                b.MouseLeftButtonDown += Click_Pedina;
                Grafica.Add(b);
                myCanvas.Children.Add(b);
            }
        }


        public void DisegnaCollegamenti()
        {
            double dimensioneX = (myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (myCanvas.Height - 40) / (partiY + PartiPerMano * 2);
            foreach (Nodo n in mappa)
            {
                foreach (int vicino in n.vicini)
                {
                    Line l = new Line();
                    l.X1 = distanza + n.x * dimensioneX + dimensioneY * 3 / 4;
                    l.Y1 = n.y * dimensioneY + dimensioneY * 6 / 7;
                    l.X2 = distanza + mappa[vicino].x * dimensioneX + dimensioneY * 3 / 4;
                    l.Y2 = mappa[vicino].y * dimensioneY + dimensioneY * 6 / 7;
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 2;
                    myCanvas.Children.Add(l);
                }
            }
        }
    }
}
