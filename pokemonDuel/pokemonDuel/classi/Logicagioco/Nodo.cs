using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace pokemonDuel.classi.Logicagioco
{
    class Nodo
    {
        public int indice;
        public List<int> vicini;
        public int x, y;
        public bool presentePokemon;
        public Pokemon pokemon;
        public bool selezionato;
        public Nodo()
        {
            vicini = new List<int>();
            x = 0;
            y = 0;
        }
        public Nodo(int i)
        {
            vicini = new List<int>();
            indice = i;
            x = 0;
            y = 0;
        }
        public void AddVicino(int n)
        {
            vicini.Add(n);
        }
        

        public HashSet<int> Raggiungibili(List<Nodo> mappa, int passi)
        {
            HashSet<int> ris = new HashSet<int>();
            Queue<KeyValuePair<int, int>> daVisitare = new Queue<KeyValuePair<int, int>>();
            daVisitare.Enqueue(new KeyValuePair<int, int>(0, this.indice));
            while (daVisitare.Count > 0)
            {
                KeyValuePair<int, int> pair = daVisitare.Dequeue();
                if (pair.Key >= passi)
                    continue;
                foreach (int vicino in mappa[pair.Value].vicini)
                {
                    if (vicino != indice && !ris.Contains(vicino) && !mappa[vicino].presentePokemon)
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
