using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.Grafica;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        public Battaglia b;
        Timer t;
        int tempoRimanente;
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
        private int tempoRimanenteRund;

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
            t = new Timer();
            tempoRimanente = 0;
            t.Interval = 1000;
            t.Elapsed += T_Elapsed;
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(tempoRimanente>0)
            {
                tempoRimanente--;
                if (tempoRimanente==0)
                {

                    Avversario.Termina = true;
                    Avversario = null;
                    main.MostraApp();
                    altro = null;
                }
            }
            if(tempoRimanenteRund > 0)
            {
                tempoRimanenteRund--;
                if (tempoRimanenteRund == 0)
                {
                    b.gestCanvas.Svuota();
                    M.RicominciaGioco();
                }
            }
            if (tempoRimanenteRund == 0 && tempoRimanente == 0)
                t.Stop();
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

        public void TermineRound(bool vinto)
        {
            //Random r = new Random();
            //int xp = r.Next(20, 50),materiale=r.Next(50,60);
            b.gestCanvas.RenderFineRound(vinto/*,xp,materiale*/);
            b.MostraUtil();
            tempoRimanenteRund = 5;
            t.Start();
        }
        public void TerminaPartita(bool vinto)
        {
            b.gestCanvas.RenderFineRound(vinto/*,xp,materiale*/);
            tempoRimanente = 30;
            t.Start();
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
