using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.Grafica;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace pokemonDuel.classi
{
    class DatiCondivisi
    {
        private static DatiCondivisi instance = null;
        //Attributi per la logica di gioco
        public Mappa M;
        private GestioneConnessione _avversario;
        public MainWindow main;
        public Giocatore io,altro;
        public Attacco A;
        public GestioneTcp gt;
        public CaricamentoBattaglia caricamento;
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
        private static object syn = new object();

        //////////////////////////////////////////////////////////////////////

        public static DatiCondivisi Instance()
        {
            lock (syn)
            {
                if (instance == null)
                    instance = new DatiCondivisi();
                return instance;
            }
        }
        private DatiCondivisi()
        {
            syncComunicazione = new object();
            _avversario = null;
            M = null;
            io = new Giocatore();
            altro = new Giocatore();
            gt = new GestioneTcp();
        }
        public void InviaMessaggio(Messaggio m)
        {
            if (Avversario != null)
                Avversario.Invia(m);
            else
                throw new Exception("Avversario inesistente");
        }

        internal void MostraRichiestaBattaglia(GestioneConnessione gestioneConnessione)
        {
            caricamento.AddConnessione(gestioneConnessione);
        }

        public void VintoPartita()
        {

        }
        public void PersoPartita()
        {

        }

        internal void AvviaPartita()
        {
            main.Dispatcher.Invoke(delegate
            {
                main.MostraPartita();
                M.Disegna();
                M.RicominciaGioco();
            });
        }
        public void Upload()
        {
            
        }


    }
}
