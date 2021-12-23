using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Grafica
{
    class GestioneRuota
    {
        public Ruota ruota { get; set; } = null;
        public Mossa _risultato;
        public Mossa Risultato { get { return _risultato; }set { _risultato = value; } }
        float gradi;
        DateTime ultimo;
        public Pokemon Pokemon { get { return ruota.Pokemon; } set { ruota.Pokemon = value; } }
        public GestioneRuota()
        {
            Random random = new Random();
            gradi = (float)random.Next(10000000, 99999999);
            ultimo = DateTime.Now;
        }

        public void Upload()
        {
            if (ruota == null)
                return;

            ruota.Gira((int)(gradi *(DateTime.Now-ultimo).TotalSeconds));
            gradi /=(float)(gradi * (DateTime.Now - ultimo).TotalSeconds);
            ultimo = DateTime.Now;
            if (gradi <= 0)
            {
                Mossa m = ruota.GetRisultato();
                Risultato = m;
                ruota = null;
                //DatiCondivisi.Instance().Avversario.Invia(new Comunicazione.Messaggio("a", m.id + ""));
            }
        }

    }
}
