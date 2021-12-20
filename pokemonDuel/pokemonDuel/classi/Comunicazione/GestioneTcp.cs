using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    class GestioneTcp
    {
        TcpListener listener;
        public GestioneTcp()
        {
            listener = new TcpListener(IPAddress.Loopback, 12345);
            listener.Start();
            Thread t = new Thread(run);
            t.Start();
        }
        public void run()
        {
            while(true)
            {
                TcpClient c = listener.AcceptTcpClient();
                GestioneConnessione gc = new GestioneConnessione(c);
                try
                {
                    Console.WriteLine(c.ToString());
                    DatiCondivisi.Instance().Avversario = gc;
                }catch(Exception)
                {
                    gc.Termina = true;
                }
            }
        }

    }
}
