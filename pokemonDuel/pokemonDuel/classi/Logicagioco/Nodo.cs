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

    }
}
