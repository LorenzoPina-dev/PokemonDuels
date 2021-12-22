using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.GestioneFile
{
    class StoreInfo
    {
        public List<Pokemon> Pokedex;
        public List<Mossa> Mosse;
        static StoreInfo instance=null;
        public static StoreInfo Instance()
        {
            if (instance == null)
                instance = new StoreInfo();
            return instance;
        }
        private StoreInfo()
        {
            Mosse= new List<Mossa>();
            List<string> Tmosse = File.Leggi("./file/Mosse.csv");
            foreach (string m in Tmosse)
            {
                Mossa mossa = new Mossa(m);
                Mosse.Add(mossa);
            }
            Pokedex = new List<Pokemon>();
            List<string> pokemon = File.Leggi("./file/Pokemon.csv");
            foreach (string s in pokemon)
            {
                try
                {
                    Pokemon p = new Pokemon(s);
                    if(p.Instance(s))
                        Pokedex.Add(p);
                }catch(Exception)
                { }
            }
        }
    }
}
