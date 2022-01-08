using pokemonDuel.classi.Componenti;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Timer = System.Timers.Timer;

namespace pokemonDuel.classi.Logicagioco
{
    class Mappa
    {
        public List<Nodo> mappa;
        int Destinazione;
        bool _turno;
        List<int> startPosizionamento;
        int partiX, partiY;
        int distanza = 0;
        List<Rectangle> Grafica;
        int PartiPerMano;
        int PedineMappa, PedineMano;
        Nodo Selezionato;
        public int Nturno, Tvinti;
        public int mioAttacco;
        int turniAPartita = 3;
        private object synTurno;
        Timer t = new Timer();
        int tempoRimasto = 0;
        Label timer;
        public bool Turno
        {
            get { lock (synTurno) { return _turno; } }
            set { lock (synTurno) { _turno = value; if (_turno) { tempoRimasto = 80; t.Start(); } else t.Stop(); } }
        }

        internal void RimettiNellaMano(Nodo perdente)
        {
            int partenza = PedineMappa;
            if (perdente.pokemon.mio)
                partenza += PedineMano;
            for (int i = 0; i < PedineMano; i++)
                if (!mappa[partenza + i].presentePokemon)
                { 
                    Muovi(perdente, mappa[partenza + i]);
                    DatiCondivisi.Instance().main.Dispatcher.Invoke(delegate { Ridisegna(); }); return; }
        }

        public Mappa()
        {
            synTurno = new object();
            Destinazione = 3;
            PedineMano = 6;
            this.mappa = new List<Nodo>();
            startPosizionamento = new List<int>();
            creaNodi();
            creaCollegamenti();
            _turno = true;
            t.Interval = 1000;
            timer = new Label();
            t.Elapsed += T_Elapsed;
            Grafica = new List<Rectangle>();
        }
        public void Riavvia()
        {
            Nturno = 0;
            Tvinti = 0;
            Selezionato = null;
            Disegna();
            Ridisegna();
         }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (tempoRimasto > 0)
            {
                tempoRimasto--;
                if (tempoRimasto == 0)
                {
                    DatiCondivisi.Instance().Avversario.Invia(new Messaggio("t", ""));
                    Turno = !Turno;
                }
                DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate {
                    string minuti = tempoRimasto / 60 + "", secondo = tempoRimasto % 60 + "";
                    if (minuti.Length <= 1)
                        minuti = "0" + minuti;
                    if (secondo.Length <= 1)
                        secondo = "0" + secondo;
                    timer.Content = minuti + ":" + secondo;
                });
            }
        }

        private void Click_Pedina(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Rectangle el = (Rectangle)e.Source;
            Nodo cliccato = mappa[int.Parse(el.Name.Split('_')[1])];
            if (!cliccato.presentePokemon)
            {
                if (Turno && cliccato.selezionato)
                {
                    Muovi(Selezionato, cliccato);
                    DatiCondivisi.Instance().Avversario.Invia(new Messaggio("m", Selezionato.indice + ";" + cliccato.indice));
                    DatiCondivisi.Instance().Avversario.Invia(new Messaggio("t", ""));
                    ControllaVincitore(cliccato.indice);
                    Turno = !Turno;
                }
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
                                if (!mappa[partenza].presentePokemon)
                                {
                                    HashSet<int> parziale = mappa[partenza].Raggiungibili(mappa, cliccato.pokemon.Salti);
                                    foreach (int p in parziale)
                                        daEvidenziare.Add(p);
                                }
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
            DatiCondivisi.Instance().A = new Attacco(Selezionato, Attaccato, null, null);
            GestioneRuota.Instance().Start();
        }



        

        private void Ridisegna()
        {
            DisegnaMappa();
            if (!mappa[24].presentePokemon)
                Grafica[24].Fill = Brushes.LightBlue;
            if (!mappa[3].presentePokemon)
                Grafica[3].Fill = Brushes.OrangeRed;
        }

        public void Muovi(int idPartenza,int idDetinazione)
        {
            DatiCondivisi.Instance().caricamento.Dispatcher.Invoke(delegate
            {
                Muovi(mappa[SistemaIndici(idPartenza)], mappa[SistemaIndici(idDetinazione)]);
            });
        }

        public void Muovi(Nodo partenza, Nodo destinazione)
        {
            lock (this)
            {
                destinazione.pokemon = partenza.pokemon;
                destinazione.presentePokemon = true;
                partenza.pokemon = null;
                partenza.presentePokemon = false;
            }
            DatiCondivisi.Instance().main.Dispatcher.Invoke(delegate { Ridisegna(); });
        }

        public void ControllaVincitore(int destinazione)
        {
            if (destinazione == Destinazione)
            {
                Nturno++;
                if (Turno)
                {
                    Tvinti++;
                    if (Nturno == turniAPartita)
                    {
                        if (Tvinti > Nturno - Tvinti)
                        {
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tb", "1"));
                            DatiCondivisi.Instance().TerminaPartita(true);
                        }
                        else
                        {
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tb", "0"));
                            DatiCondivisi.Instance().TerminaPartita(true);
                        }
                    }
                    else
                    {
                        DatiCondivisi.Instance().TermineRound(true);
                        DatiCondivisi.Instance().Avversario.Invia(new Messaggio("tr", "1"));
                    }
                    Console.WriteLine("vinto");
                }
                RicominciaGioco();
            }
            Ridisegna();
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
            DatiCondivisi.Instance().main.Dispatcher.Invoke(delegate { Ridisegna(); });
        }

        private void Mostra(Nodo cliccato, HashSet<int> hashSet)        //evidenzia le caselle raggiungibili
        {
            Ridisegna();
            Selezionato = cliccato;
            foreach (int i in hashSet)
            {
                mappa[i].selezionato = true;
                Grafica[i].Fill = Brushes.Yellow;
            }
        }

        public void creaNodi()      //crea i nodi logici
        {
            List<string> righe = GestioneFile.GestFile.Leggi("./file/Mappa.csv");
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
        public void creaCollegamenti()      //crea i collegamenti logici
        {
            List<string> righe = GestioneFile.GestFile.Leggi("./file/Collegamenti.csv");
            foreach (string s in righe)
            {
                string[] estremi = s.Split(';');
                int P = int.Parse(estremi[0]), F = int.Parse(estremi[1]);
                mappa[P].vicini.Add(F);
                mappa[F].vicini.Add(P);
            }
        }
        public void Disegna()       //metodo per ridisegnare tutta la mappa
        {
            Battaglia m = DatiCondivisi.Instance().b;
            m.Dispatcher.Invoke(delegate
            {
                m.CanvasGiocatore.Children.Clear();
                DisegnaGiocatori(m);
                DisegnaMappa();
            });
        }
        public void DisegnaMappa()
        {
            Battaglia m = DatiCondivisi.Instance().b;
            m.myCanvas.Children.Clear();
            DisegnaCollegamenti(m);
            DisegnaNodi(m);
        }

        private void DisegnaGiocatori(Battaglia m)      //disegna la barra in alto con le informazioni dei 2 utenti e il timer
        {
            Rectangle mio = new Rectangle();
            mio.Width = m.CanvasGiocatore.Width / 2;
            mio.Height = m.CanvasGiocatore.Height;
            mio.Fill = Brushes.Blue;
            Canvas.SetLeft(mio, 0);
            Canvas.SetTop(mio, 0);
            m.CanvasGiocatore.Children.Add(mio);
            Rectangle altro = new Rectangle();
            altro.Width = m.CanvasGiocatore.Width / 2;
            altro.Height = m.CanvasGiocatore.Height;
            altro.Fill = Brushes.Red;
            Canvas.SetRight(altro, 0);
            Canvas.SetTop(altro, 0);
            m.CanvasGiocatore.Children.Add(altro);
            Label nomeMio = new Label();
            nomeMio.Content = DatiCondivisi.Instance().io.Username;
            nomeMio.Foreground = Brushes.White;
            nomeMio.FontSize = 25;
            Canvas.SetLeft(nomeMio, 30);
            Canvas.SetTop(nomeMio, 0);
            m.CanvasGiocatore.Children.Add(nomeMio);
            Label nomeAltro = new Label();
            nomeAltro.Content = DatiCondivisi.Instance().altro.Username;
            nomeAltro.Foreground = Brushes.White;
            nomeAltro.FontSize = 25;
            Canvas.SetRight(nomeAltro, 30);
            Canvas.SetTop(nomeAltro, 0);
            m.CanvasGiocatore.Children.Add(nomeAltro);
            Label XpMio = new Label();
            XpMio.Content = DatiCondivisi.Instance().io.Xp;
            XpMio.Foreground = Brushes.White;
            XpMio.FontSize = 25;
            Canvas.SetLeft(XpMio, 40);
            Canvas.SetTop(XpMio, 30);
            m.CanvasGiocatore.Children.Add(XpMio);
            Label XPAltro = new Label();
            XPAltro.Content = DatiCondivisi.Instance().altro.Xp;
            XPAltro.Foreground = Brushes.White;
            XPAltro.FontSize = 25;
            Canvas.SetRight(XPAltro, 40);
            Canvas.SetTop(XPAltro, 30);
            m.CanvasGiocatore.Children.Add(XPAltro);
            int altezza = 50, lunghezza = 150,offset=10;
            Polygon Contenitore = new Polygon();
            Contenitore.Points.Add(new Point((lunghezza-20)/2, 0));
            Contenitore.Points.Add(new Point( lunghezza/2, altezza/2));
            Contenitore.Points.Add(new Point((lunghezza - 20) / 2, altezza));
            Contenitore.Points.Add(new Point(-(lunghezza - 20) / 2, altezza));
            Contenitore.Points.Add(new Point(-lunghezza / 2, altezza / 2));
            Contenitore.Points.Add(new Point(-(lunghezza - 20) / 2, 0));
            Canvas.SetTop(Contenitore, offset);
            Canvas.SetLeft(Contenitore, m.CanvasGiocatore.Width / 2);
            Contenitore.Fill = Brushes.Black;
            m.CanvasGiocatore.Children.Add(Contenitore);
            timer.Content = tempoRimasto;
            timer.Foreground = Brushes.Yellow;
            timer.FontSize = 25;
            Canvas.SetTop(timer, offset + 10);
            Canvas.SetLeft(timer, m.CanvasGiocatore.Width / 2 - 35);
            m.CanvasGiocatore.Children.Add(timer);
            Label RoundVinti = new Label();
            RoundVinti.Content = Tvinti;
            RoundVinti.FontSize = 20;
            RoundVinti.Background = Brushes.Transparent;
            RoundVinti.Foreground = Brushes.White;
            Canvas.SetTop(RoundVinti, 20);
            Canvas.SetRight(RoundVinti, m.CanvasGiocatore.Width / 2 + (lunghezza - 20) / 2+10);
            m.CanvasGiocatore.Children.Add(RoundVinti);
            Label RoundPersi = new Label();
            RoundPersi.Content =Nturno-Tvinti;
            RoundPersi.FontSize = 20;
            RoundPersi.Background = Brushes.Transparent;
            RoundPersi.Foreground = Brushes.White;
            Canvas.SetTop(RoundPersi, 20);
            Canvas.SetLeft(RoundPersi, m.CanvasGiocatore.Width / 2 + (lunghezza - 20) / 2 + 10);
            m.CanvasGiocatore.Children.Add(RoundPersi);
        }

        public void DisegnaNodi(Battaglia m)        //per disegnare le caselle della mappa
        {
            double dimensioneX = (m.myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (m.myCanvas.Height - 40) / (partiY + PartiPerMano * 2);
            Grafica.Clear();

            foreach (Nodo n in mappa)
            {
                Rectangle b = new Rectangle();
                b.Stroke = Brushes.Black;
                b.Height = dimensioneY * 3 / 4;
                b.Width = dimensioneX * 3 / 4;
                if (n.presentePokemon)
                {
                    GestioneCanvas.RenderPokemon(m.myCanvas,n.pokemon, dimensioneX * 3 / 4, dimensioneY * 3 / 4, dimensioneX * n.x + dimensioneX / 4, dimensioneY * n.y + dimensioneY / 4,  false,false);
                    if (n.pokemon.mio)
                        b.Stroke = Brushes.LightBlue;
                    else
                        b.Stroke = Brushes.OrangeRed;
                    b.Fill = Brushes.Transparent;
                }
                else
                    b.Fill = Brushes.White;
                Canvas.SetTop(b, dimensioneY * n.y + dimensioneY / 4);
                Canvas.SetLeft(b, distanza + dimensioneX * n.x + dimensioneX / 4);
                Canvas.SetZIndex(b, 10);
                b.Name = "p_" + n.indice;
                b.MouseDown += Click_Pedina;
                Grafica.Add(b);
                m.myCanvas.Children.Add(b);
            }
        }


        public void DisegnaCollegamenti(Battaglia m)        //per disegnare le linee della mappa
        {
            double dimensioneX = (m.myCanvas.Width - distanza - 40) / (partiX + 2), dimensioneY = (m.myCanvas.Height - 40) / (partiY + PartiPerMano * 2);
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
                    m.myCanvas.Children.Add(l);
                }
            }
        }
        public int SistemaIndici(int daSistemare)       //perchè il destinatario manderà le coordinate in base al suo sistema di riferimento e questo metodo fa interagire i 2 sistemi
        {
            if (daSistemare >= PedineMappa)
                daSistemare -= PedineMano;
            if (daSistemare < PedineMappa)
                daSistemare = PedineMappa-1 - daSistemare;
            return daSistemare;
        }
    }
}
