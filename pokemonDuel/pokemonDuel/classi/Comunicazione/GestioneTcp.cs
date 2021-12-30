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
        public GestioneTcp()
        {
            listener = new TcpListener(IPAddress.Any, 54321);
            listener.Start();
            t = new Thread(run);
            t.Start();
        }
        public void run()
        {
            while(true)
            {
                TcpClient c = listener.AcceptTcpClient();
                GestioneConnessione gc = new GestioneConnessione(c);
                if(DatiCondivisi.Instance().Avversario!=null)
                    gc.Termina = true;
            }
            listener.Stop();
        }

        public static void Connetti(string ip)
        {
            if (DatiCondivisi.Instance().Avversario == null)
            {
                Giocatore io = DatiCondivisi.Instance().io;
                TcpClient c = new TcpClient(ip, 54321);
                GestioneConnessione gc = new GestioneConnessione(c);
                string s="";
                foreach (Pokemon p in io.Deck)
                    s += ";" + p.id;
                gc.Invia(new Messaggio("c", io.Username + s));
            }
        }
        public void stop()
        {
            listener.Stop();
            t.Abort();
        }

    }
}
