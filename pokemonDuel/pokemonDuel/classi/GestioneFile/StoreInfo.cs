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
        public Dictionary<int, Pokemon> Pokedex;
        public Dictionary<int,Mossa> Mosse;
        static StoreInfo instance=null;
        public static StoreInfo Instance()
        {
            if (instance == null)
                instance = new StoreInfo();
            return instance;
        }
        private StoreInfo()
        {
            Mosse= new Dictionary<int,Mossa>();
            List<string> Tmosse = GestFile.Leggi("./file/Mosse.csv");
            foreach (string m in Tmosse)
            {
                Mossa mossa = new Mossa(m);
                Mosse.Add(mossa.id,mossa);
            }
            Pokedex = new Dictionary<int, Pokemon>();
            List<string> pokemon = GestFile.Leggi("./file/Pokemon.csv");
            foreach (string s in pokemon)
            {
                try
                {
                    Pokemon p = new Pokemon(s);
                    if(p.Instance(s))
                        Pokedex.Add(p.id,p);
                }catch(Exception)
                { }
            }
        }
    }
}
