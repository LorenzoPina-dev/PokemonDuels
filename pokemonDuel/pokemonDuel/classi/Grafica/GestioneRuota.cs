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
        public Ruota ruota { get; set; } = null;
        public Mossa _risultato;
        public Mossa Risultato { get { return _risultato; }set { _risultato = value; } }
        float gradi;
        DateTime ultimo;
        public Pokemon Pokemon { get { return ruota.Pokemon; } set { ruota.Pokemon = value; } }
        float divisore;
        public GestioneRuota()
        {
            Random random = new Random();
            gradi = (float)random.Next(4000, 5200);
            ultimo = DateTime.Now;
            divisore = 1;
        }

        public int Upload()
        {
            if ((DateTime.Now - ultimo).TotalMilliseconds < 140)
                return 0;
            if (ruota == null || Pokemon==null)
                throw new Exception("non esiste la ruota");
            float g = (float)(gradi * (DateTime.Now - ultimo).TotalSeconds)/3;
            gradi -= g;
            //gradi /=divisore;
            //divisore+=0.002f;
            ultimo = DateTime.Now;
            if (gradi <= 2)
            {
                Mossa m = ruota.GetRisultato();
                Risultato = m;
                Pokemon = null;
                MessageBox.Show(Risultato.nome + Risultato.danno);
                //DatiCondivisi.Instance().Avversario.Invia(new Comunicazione.Messaggio("a", m.id + ""));
                return 0;
            }
            return (int)g;
        }

    }
}
