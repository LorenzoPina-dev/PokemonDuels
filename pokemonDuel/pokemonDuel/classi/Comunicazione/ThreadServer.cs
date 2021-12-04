using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    class ThreadServer
    {
        public ThreadServer()
        {
        }

        public void run()
        {
            while (true)
                DatiCondivisi.Instance().AddDaElaborare(Ricevi());
        }
        public Messaggio Ricevi()
        {
            byte[] dataReceived;
            return new Messaggio(Encoding.ASCII.GetString(dataReceived));
        }
    }
}
