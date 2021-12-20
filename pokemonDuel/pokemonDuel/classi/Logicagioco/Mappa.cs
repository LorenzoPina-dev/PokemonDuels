using pokemonDuel.classi.GestioneFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        List<Ellipse> Grafica;
        int PartiPerMano;
        int PedineMappa;
        Nodo Selezionato;
        public Mappa(MainWindow m)
        {
            this.m = m;
            this.mappa = new List<Nodo>();
            startPosizionamento = new List<int>();
            creaNodi();
            creaCollegamenti();
            myCanvas = new Canvas();
            Selezionato = null;
            turno = true;
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
            Pokemon p=new Pokemon();
            p.mio = true;
            p.Salti = 4;
            mappa[34].presentePokemon = true;
            mappa[34].pokemon = p;

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

        public HashSet<int> Raggiungibili(Nodo n, int passi)
        {
            HashSet<int> ris = new HashSet<int>();
            Queue<KeyValuePair<int, int>> daVisitare = new Queue<KeyValuePair<int, int>>();
            daVisitare.Enqueue(new KeyValuePair<int, int>(0, n.indice));
            while (daVisitare.Count > 0)
            {
                KeyValuePair<int, int> pair = daVisitare.Dequeue();
                if (pair.Key >= passi)
                    continue;
                foreach (int vicino in mappa[pair.Value].vicini)
                {
                    if (vicino != n.indice && !ris.Contains(vicino) && !mappa[vicino].presentePokemon)
                    {
                        ris.Add(vicino);
                        daVisitare.Enqueue(new KeyValuePair<int, int>(pair.Key + 1, vicino));
                    }
                }
            }

            return ris;
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
            m.Content = myCanvas;
            m.Show();
        }
        public void DisegnaNodi()
        {
            double dimensioneX = (m.Width - distanza - 40) / (partiX + 2), dimensioneY = (m.Height - 40) / (partiY +PartiPerMano*2);
            Grafica = new List<Ellipse>();
            foreach (Nodo n in mappa)
            {
                Ellipse b = new Ellipse();
                b.Stroke = Brushes.Black;
                b.Height = dimensioneY / 2;
                b.Width = dimensioneX / 2;
                if(n.presentePokemon)
                    b.Fill = Brushes.Yellow;
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

        private void Click_Pedina(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Ellipse el = (Ellipse)e.Source;
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
                                HashSet<int> parziale = Raggiungibili(mappa[partenza], cliccato.pokemon.Salti);
                                foreach (int p in parziale)
                                    daEvidenziare.Add(p);
                            }
                            Mostra(cliccato, daEvidenziare);
                        }
                        else
                            Mostra(cliccato, Raggiungibili(cliccato, cliccato.pokemon.Salti));
                    else
                        Attacca(cliccato);
                }
            }
        }

        private void Attacca(Nodo cliccato)
        {
            throw new NotImplementedException();
        }

        private void Reset()
        {
            foreach(Nodo n in mappa)
            {
                if(n.presentePokemon)
                    Grafica[n.indice].Fill = Brushes.Yellow;
                else
                    Grafica[n.indice].Fill = Brushes.White;
                n.selezionato = false;
            }
        }

        private void Muovi(Nodo cliccato)
        {
            cliccato.pokemon = Selezionato.pokemon;
            cliccato.presentePokemon = true;
            Selezionato.pokemon = null;
            Selezionato.presentePokemon = false;
            Reset();
        }

        private void Mostra(Nodo cliccato,HashSet<int> hashSet)
        {
            Reset();
            Selezionato = cliccato;
            foreach (int i in hashSet)
            {
                mappa[i].selezionato = true;
                Grafica[i].Fill = Brushes.Yellow;
            }
        }

        public void DisegnaCollegamenti()
        {
            double dimensioneX = (m.Width - distanza - 40) / (partiX + 2), dimensioneY = (m.Height - 40) / (partiY+PartiPerMano*2);
            foreach (Nodo n in mappa)
            {
                foreach (int vicino in n.vicini)
                {
                    Line l = new Line();
                    l.X1 = distanza + n.x * dimensioneX + dimensioneY / 2;
                    l.Y1 = n.y * dimensioneY + dimensioneY / 2;
                    l.X2 = distanza + mappa[vicino].x * dimensioneX + dimensioneY / 2;
                    l.Y2 = mappa[vicino].y * dimensioneY + dimensioneY / 2;
                    l.Stroke = Brushes.Black;
                    l.StrokeThickness = 2;
                    myCanvas.Children.Add(l);
                }
            }
        }
    }
}
