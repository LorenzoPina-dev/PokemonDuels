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
        private int _xp;
        public string Username;
        public int Xp { get { return _xp; } set { _xp = value; DatiCondivisi.Instance().main.AggiornaXp(_xp); } }
        public int Materiali;
        public List<Pokemon> Deck;

        public Giocatore()
        {
            _xp = 0;
            Username = "";
            Materiali = 500;
            Deck = new List<Pokemon>();
        }
        public Giocatore(string csv,bool mieiDati)
        {
            string[] split = csv.Split(';');
            Username = split[0];
            _xp = int.Parse(split[1]);
            Materiali = int.Parse(split[2]);
            Deck = new List<Pokemon>();
            for(int i = 3; i < split.Length; i++)
            {
                Pokemon p = (Pokemon)StoreInfo.Instance().Pokedex[int.Parse(split[i])].Clone();
                p.mio = mieiDati;
                Deck.Add(p);
            }
        }
        public string toCsv()
        {
            string mazzo = "";
            foreach (Pokemon p in Deck)
                mazzo += ";" + p.id;
            return Username + ";" + Xp+";"+Materiali + mazzo;
        }
    }
}
