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
        public GestioneRuota()
        {
            Random random = new Random();
            gradi = (float)random.Next(10000000, 99999999);
            ultimo = DateTime.Now;
        }

        public int Upload()
        {
            if ((DateTime.Now - ultimo).TotalMilliseconds < 60)
                return 0;
            if (ruota == null || Pokemon==null)
                throw new Exception("non esiste la ruota");
            float g = (float)(gradi * (DateTime.Now - ultimo).TotalSeconds);
            gradi -=g;
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
