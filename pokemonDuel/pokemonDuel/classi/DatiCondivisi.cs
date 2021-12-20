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
        private GestioneConnessione _avversario;
        public GestioneConnessione Avversario
        {
            get { lock (syncComunicazione) { return _avversario; } }
            set { lock (syncComunicazione) {
                    if (_avversario == null)
                        _avversario = value;
                    else
                        throw new Exception("Avversario già esistente");
                } }
        }
        private object syncComunicazione;

        //////////////////////////////////////////////////////////////////////

        public static DatiCondivisi Instance()
        {
            if (instance == null)
                instance = new DatiCondivisi();
            return instance;
        }
        private DatiCondivisi()
        {
            syncComunicazione = new object();
            _avversario = null;
            M = null;
        }
        public void InviaMessaggio(Messaggio m)
        {
            if (Avversario != null)
                Avversario.Invia(m);
            else
                throw new Exception("Avversario inesistente");
        }
    }
}
