using pokemonDuel.classi.GestioneFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pokemonDuel.classi.Logicagioco
{
    class Mappa
    {
        public Nodo[] mappa;
        int fine = 3, partenza = 22;
        int[] startPosizionamento;
        List<Pokemon> miaMano, altraMano;
        List<Button> bottoni;
        int nBottoni = 27;
        bool turno;
        int partiWidth,partiHeight;
        public Mappa(int width,int height,Grid g)
        {
            partiWidth = 14;
            partiHeight = 5;
            this.mappa = new Nodo[32];
            this.fine = 3;
            this.partenza = 22;
            for (int i = 0; i < mappa.Length; i++)
                mappa[i] = new Nodo(i);
            List<string> collegamenti = LeggiFile.Leggi("./file/Mappa.csv");
            foreach (string s in collegamenti)
            {
                string[] campi = s.Split(';');
                int I = int.Parse(campi[0]), F = int.Parse(campi[1]);
                mappa[I].AddVicino(F);
                mappa[F].AddVicino(I);
            }
            this.startPosizionamento = new int[] { 25, 31 };
            bottoni = new List<Button>();
            for (int i = 0; i < 27; i++)
            {
                Button b = new Button();
                b.Margin = new System.Windows.Thickness();
                
            }
        }

        private void Pedina_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Pokemon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public List<int> Raggiungibili(Nodo n, int passi)
        {
            List<int> ris = new List<int>();
            Queue<KeyValuePair<int, int>> daVisitare = new Queue<KeyValuePair<int, int>>();
            daVisitare.Enqueue(new KeyValuePair<int, int>(0, n.indice));
            while (daVisitare.Count > 0)
            {
                KeyValuePair<int, int> pair = daVisitare.Dequeue();
                if (pair.Key >= passi)
                    continue;
                foreach (int vicino in mappa[pair.Value].vicini)
                {
                    if (vicino != n.indice && !ris.Contains(vicino) && )
                    {
                        ris.Add(vicino);
                        daVisitare.Enqueue(new KeyValuePair<int, int>(pair.Key + 1, vicino));
                    }
                }
            }

            return ris;
        }
    }
}
