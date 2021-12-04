using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    class ThreadClient
    {

        public ThreadClient()
        {
            
        }

        public void run()
        {
            while (true)
            {
                Messaggio m = DatiCondivisi.Instance().GetDaInviare();
                if (m != null)
                    invia(m);
            }
        }
        public void invia(Messaggio m)
        {
            
        }
    }
}
