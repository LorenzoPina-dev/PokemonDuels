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
        public Mossa Risultato { get { return _risultato; }set { _risultato = value; } }
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
            Battaglia b = DatiCondivisi.Instance().caricamento.Campo;
            ruota = new Ruota((int)b.host.Width, (int)b.host.Height);
            b.host.Child = ruota;
            MostraRuota(false);

        }
        public void Start()
        {
            lock (this)
            {
                ruota.Pokemon = DatiCondivisi.Instance().A.Mio.pokemon;
                Random random = new Random();
                Risultato = null;
                gradi = (float)random.Next(4000, 5200);
                ultimo = DateTime.Now;
                stoppa = false;
                MostraRuota(true);
                Thread t = new Thread(Upload);
                t.Start();
            }
        }


        public void Upload()
        {
            while(!stoppa)
            {
                lock (this)
                {
                    float g = (float)(gradi * (DateTime.Now - ultimo).TotalSeconds) / 3;
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
                            DatiCondivisi.Instance().Avversario.Invia(new Messaggio("a", a.Mio.indice + ";" + a.Avversario.indice + ";" + a.MossaMia.id));
                    }
                    Gira((int)g);
                }
                Thread.Sleep(10);
            }
        }

        public void Gira(int gradi)
        {
            DatiCondivisi.Instance().M.m.Dispatcher.Invoke(delegate
            {
                ruota.Gira(gradi);
            });
        }
        public void MostraRuota(bool visibility)
        {
            DatiCondivisi.Instance().M.m.Dispatcher.Invoke(delegate
            {
                if (visibility)
                    DatiCondivisi.Instance().M.m.host.Visibility = Visibility.Visible;
                else
                    DatiCondivisi.Instance().M.m.host.Visibility = Visibility.Hidden;
            });
        }

    }
}
