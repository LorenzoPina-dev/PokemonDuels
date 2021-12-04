using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi
{
    class DatiCondivisi
    {
        private static DatiCondivisi instance = null;
        //Attributi per la logica di gioco
        public Mappa M;
        //////////////////////////////////////////////////////////////////////

        //Attributi e metodi per la comunicazione
        Queue<Messaggio> DaInviare,DaElaborare;
        object synInviare, synElaborare,synIp;
        string _ipDestinatario;
        public string IpDestinatario { get { lock (synIp) { return _ipDestinatario; } } set { lock (synIp) {_ipDestinatario=value; } } }
        ///////////////////////////////////////////////////////////////////////
        public static DatiCondivisi Instance()
        {
            if (instance == null)
                instance = new DatiCondivisi();
            return instance;
        }
        private DatiCondivisi()
        {
            M = new Mappa();
            DaInviare = new Queue<Messaggio>();
            DaElaborare = new Queue<Messaggio>();
            synElaborare = new object();
            synInviare = new object();
            synIp = new object();
        }




        public void AddDaInviare(Messaggio m)
        {
            lock(synInviare)
            {
                DaInviare.Enqueue(m);
            }
        }
        public void AddDaElaborare(Messaggio m)
        {
            lock (synElaborare)
            {
                DaElaborare.Enqueue(m);
            }
        }
        public Messaggio GetDaInviare()
        {
            lock (synInviare)
            {
                return DaInviare.Dequeue();
            }
        }
        public Messaggio GetDaElaborare()
        {
            lock (synElaborare)
            {
                return DaElaborare.Dequeue();
            }
        }
    }
}
