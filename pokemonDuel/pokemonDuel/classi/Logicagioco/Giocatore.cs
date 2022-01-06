using pokemonDuel.classi.GestioneFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    public class Giocatore
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
        public Giocatore(string csv)
        {
            string[] split = csv.Split(';');
            Xp = 0;
            Username = split[0];
            Materiali = int.Parse(split[1]);
            Deck = new List<Pokemon>();
            for(int i=2;i<split.Length;i++)
                Deck.Add((Pokemon)StoreInfo.Instance().Pokedex[int.Parse(split[i])].Clone());
        }
        public string toCsv()
        {
            string mazzo = "";
            foreach (Pokemon p in Deck)
                mazzo += ";" + p.id;
            return Username + ";" + Xp + mazzo;
        }
    }
}
