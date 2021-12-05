using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    class ThreadLogica
    {
        public void run()
        {
            while (true)
            {
                Messaggio m = DatiCondivisi.Instance().GetDaElaborare();
                if (m != null)
                    DatiCondivisi.Instance().AddDaInviare(Elabora(m));
            }
        }

        private Messaggio Elabora(Messaggio m)
        {
            Messaggio ris=new Messaggio("");
            /*switch(m.scelta)
            {
                
            }*/
            return ris;
        }
    }
}
