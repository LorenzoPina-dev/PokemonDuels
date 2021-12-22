using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    class Giocatore
    {
        public int Xp;
        public string Username;
        public int Materiali;
        public List<Pokemon> Deck;

        public Giocatore()
        {
            Xp = 0;
            Username = "";
            Materiali = 0;
            Deck = new List<Pokemon>();
        }
    }
}
