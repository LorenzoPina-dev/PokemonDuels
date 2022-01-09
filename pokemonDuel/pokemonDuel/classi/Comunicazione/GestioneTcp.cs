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
        List<GestioneConnessione> connessioni;
        private static GestioneTcp instance = null;
        public static GestioneTcp Instance()
        {
            if (instance == null)
                instance = new GestioneTcp();
            return instance;
        }
        private GestioneTcp()
        {
            listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            t = new Thread(run);
            t.Start();
            termina = false;
            connessioni = new List<GestioneConnessione>();
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
                    else
                        connessioni.Add(gc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            listener.Stop();
        }

        public void Disconnetti(GestioneConnessione gr)
        {
            connessioni.Remove(gr);
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
            listener.Server.Close();
            listener.Stop();
            termina = true;
            foreach (GestioneConnessione gc in connessioni)
                gc.Termina = true;
        }

    }
}
