using pokemonDuel.classi.Comunicazione;
using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private bool stoppa = true;
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
                gradi = (float)random.Next(4000, 5200);
                ultimo = DateTime.Now;
                stoppa = false;
                MostraRuota(true);
            }
        }

        public int Upload()
        {
            lock (this)
            {
                if ((DateTime.Now - ultimo).TotalMilliseconds < 140 || ruota == null || DatiCondivisi.Instance().A == null)
                    return 0;
                float g = (float)(gradi * (DateTime.Now - ultimo).TotalSeconds) / 3;
                gradi -= g;
                ultimo = DateTime.Now;
                if (gradi <= 10)
                {
                    Mossa m = ruota.GetRisultato();
                    Risultato = m;
                    stoppa = true;
                    DatiCondivisi.Instance().A.MossaMia = m;
                    DatiCondivisi.Instance().A.MossaAvversario =StoreInfo.Instance().Mosse[DatiCondivisi.Instance().A.Avversario.pokemon.Mosse[0]];
                    MostraRuota(false);
                    Attacco a = DatiCondivisi.Instance().A;
                    if (a.Settato())
                    {
                        DatiCondivisi.Instance().Avversario.Invia(new Messaggio("a", "" + a.MossaMia.id));
                        a.EseguiAttacco(); 
                    }
                    else
                        DatiCondivisi.Instance().Avversario.Invia(new Messaggio("a", "" + a.Mio.indice + a.Avversario.indice + a.MossaMia.id));
                    return 0;
                }
                return (int)g;
            }
        }
        
        public void Gira(int gradi)
        {
            ruota.Gira(gradi);
        }
        public void MostraRuota(bool visibility)
        {
            if (visibility)
                DatiCondivisi.Instance().M.m.host.Visibility = Visibility.Visible;
            else
                DatiCondivisi.Instance().M.m.host.Visibility = Visibility.Hidden;
        }

    }
}
