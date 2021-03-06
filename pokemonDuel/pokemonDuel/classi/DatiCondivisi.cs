using pokemonDuel.classi.Componenti;
using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
using pokemonDuel.classi.Logicagioco;
using pokemonDuel.classi.Util;
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
        public CaricamentoBattaglia caricamento;
        public Battaglia b;
        Timer t;
        public GestioneConnessione Avversario
        {
            get { lock (syncComunicazione) { return _avversario; } }
            set { lock (syncComunicazione) {
                    if (_avversario == null || value==null)
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
            t = new Timer();
            t.Interval = 1000;
            t.Elapsed += T_Elapsed;
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(tempoRimanenteRund > 0)
            {
                tempoRimanenteRund--;
                if (tempoRimanenteRund == 0)
                {

                    b.gestCanvas.Svuota();
                    b.MostraMappa();
                    RidisegnaPartita();
                    t.Stop();
                }
            }
        }

        private void RidisegnaPartita()
        {
            main.Dispatcher.Invoke(delegate
            {
                main.MostraFinestra(Finestra.Battaglia);
                M.Disegna();
                M.RicominciaGioco();
            });
        }

        public void InviaMessaggio(Messaggio m)
        {
            if (Avversario != null)
                Avversario.Invia(m);
            else
                throw new Exception("Avversario inesistente");
        }

        public void MostraRichiestaBattaglia(GestioneConnessione gestioneConnessione)
        {
            caricamento.AddConnessione(gestioneConnessione);

        }

        public void TermineRound(bool vinto)
        {
            b.Dispatcher.Invoke(delegate
            {
                b.gestCanvas.RenderFineRound(vinto);
                b.MostraUtil();
            });
            tempoRimanenteRund = 5;
            t.Start();
        }
        public void TerminaPartita(bool vinto)
        {
            Random r = new Random();
            int xp = r.Next(20, 50),materiale=r.Next(50,60);
            if (vinto)
            {
                io.Xp += xp;
                io.Materiali += materiale;
            }
            b.Dispatcher.Invoke(delegate {
                b.gestCanvas.RenderFine(vinto,xp,materiale);
                b.MostraUtil();
            });
            //Avversario.Termina = true;
            Avversario = null;
            M.Stop();
        }
        public void AvviaPartita()
        {
            b.Dispatcher.Invoke(delegate
            {
                M.Riavvia();
                b.MostraMappa();
                RidisegnaPartita();
            });
        }

    }
}
