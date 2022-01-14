using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace pokemonDuel.classi.Grafica
{
    class GestioneRuota
    {
        public  Ruota ruota;
        private static GestioneRuota instance = null;
        public Mossa _risultato;
        public Mossa Risultato { get { return _risultato; } set { _risultato = value; } }
        float gradi;
        DateTime ultimo;
        public bool stoppa = true;
        public static GestioneRuota Instance()
        {
            if (instance == null)
                instance = new GestioneRuota();
            return instance;
        }
        private GestioneRuota()
        {
            stoppa = true;
            Risultato = null;
            gradi = 0;
            ruota = new Ruota();
            DatiCondivisi.Instance().b.host.Child = ruota;
            MostraRuota(false);

        }
        public void Start()
        {

            lock (this)
            {
                ruota.Pokemon = DatiCondivisi.Instance().A.Mio.pokemon;
                Random random = new Random();
                Risultato = null;
                gradi = (float)random.Next(1000, 2200);
                ultimo = DateTime.Now;
                stoppa = false;
                MostraRuota(true);
                Thread t = new Thread(Upload);
                t.Start();
                DatiCondivisi.Instance().b.MostraAttacco(true);
            }
        }


        public void Upload()
        {
            while(!stoppa)
            {
                lock (this)
                {
                    float g = (float)(gradi * (DateTime.Now - ultimo).TotalSeconds);
                    gradi -= g;
                    ultimo = DateTime.Now;
                    if (gradi <= 10)
                    {
                        Mossa m = ruota.GetRisultato();
                        Risultato = m;
                        stoppa = true;
                        Attacco a = DatiCondivisi.Instance().A;
                        a.MossaMia = m;
                        MostraRuota(false);
                        if (a.Settato())
                        {
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("a", "" + a.MossaMia.id));
                            a.EseguiAttacco();
                        }
                        else
                        {
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("a", a.Mio.indice + ";" + a.Avversario.indice + ";" + a.MossaMia.id));
                            DatiCondivisi.Instance().b.MostraAttacco(true);
                        }
                    }
                    Gira((int)g);
                }
                Thread.Sleep(100);
            }
        }

        public void Gira(int gradi)
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                ruota.Aggiorna();
                ruota.Gira(gradi);
            });
        }
        public void MostraRuota(bool visibility)
        {
            DatiCondivisi.Instance().b.Dispatcher.Invoke(delegate
            {
                if(DatiCondivisi.Instance().A!=null)
                if (visibility)
                     DatiCondivisi.Instance().b.gestCanvas.MostraAttacca(true);
                    else 
                {
                    DatiCondivisi.Instance().b.gestCanvas.MostraAttacca(false);
                }
            });
        }

    }
}
