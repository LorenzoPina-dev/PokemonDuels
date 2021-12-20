using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    class GestioneConnessione
    {
        private StreamWriter sw;
        private StreamReader sr;
        private bool termina;
        private Queue<Messaggio> DaInviare;
        private Queue<Messaggio> DaElaborare;
        private object synInvia;
        private object synTerm;
        public bool Termina { get { lock (synTerm) { return termina; } }set { lock (synTerm) { termina = value; } } }
        public GestioneConnessione(TcpClient c)
        {
            synInvia=new object();
            synTerm = new object();
            sw = new StreamWriter(c.GetStream());
            sr = new StreamReader(c.GetStream());
            termina = false;
            DaInviare = new Queue<Messaggio>();
            DaElaborare = new Queue<Messaggio>();
            Thread client = new Thread(GClient);
            Thread Server = new Thread(GServer);
            Thread Logica = new Thread(GLogica);
            client.Start();
            Server.Start();
            Logica.Start();
        }


        private Messaggio Elabora(Messaggio m)
        {
            Messaggio ris = new Messaggio("");
            /*switch(m.scelta)
            {
                
            }*/
            return ris;
        }

        public void Invia(Messaggio m)
        {
            lock (synInvia)
            {
                DaInviare.Enqueue(m);
            }
        }

        private void GLogica()
        {
            while (!Termina)
            {
                if (DaElaborare.Count > 0)
                {
                    Messaggio m = DaElaborare.Dequeue();
                    if (m != null)
                        Invia(Elabora(m));
                }
            }
        }

        private void GServer()
        {
            while(!Termina)
            {
                if (DaInviare.Count > 0)
                {
                    Messaggio m = DaInviare.Dequeue();
                    if (m != null)
                    {
                        sw.WriteLine(m.toCsv());
                        sw.Flush();
                    }
                }
            }
            sw.Close();
        }

        private void GClient()
        {
            while(!Termina)
            {
                string s = sr.ReadLine();
                if (s == null)
                    Termina = true;
                else if (s != "")
                {
                    Console.WriteLine(s);
                    DaElaborare.Enqueue(new Messaggio(s));
                }
            }
            sr.Close();
        }
    }
}
