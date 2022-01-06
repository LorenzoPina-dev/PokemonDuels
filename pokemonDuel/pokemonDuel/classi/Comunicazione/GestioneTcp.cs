using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace pokemonDuel.classi.Comunicazione
{
    class GestioneTcp
    {
        TcpListener listener;
        Thread t;
        private bool termina;
        public GestioneTcp()
        {
            listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            t = new Thread(run);
            t.Start();
            termina = false;
        }
        public void run()
        {
            try
            {
                while (!termina)
                {
                    TcpClient c = listener.AcceptTcpClient();
                    GestioneConnessione gc = new GestioneConnessione(c);
                    if (DatiCondivisi.Instance().Avversario != null)
                        gc.Termina = true;
                }
            }
            catch (Exception)
            {
            }
            listener.Stop();
        }

        public static void Connetti(string ip)
        {
            if (DatiCondivisi.Instance().Avversario == null)
            {
                Giocatore io = DatiCondivisi.Instance().io;
                TcpClient c = new TcpClient(ip, 12345);
                GestioneConnessione gc = new GestioneConnessione(c);
                gc.Invia(new Messaggio("c", io.toCsv()));
            }
        }
        public void stop()
        {
            listener.Stop();
            termina = true;
        }

    }
}
