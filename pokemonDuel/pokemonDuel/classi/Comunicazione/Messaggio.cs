using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    public class Messaggio
    {
        public string scelta, dati;
        public Messaggio(string scelta, string dati)
        {
            this.scelta = scelta;
            this.dati = dati;
        }
        public Messaggio(string csv)
        {
            int separatore = csv.IndexOf(';');
            this.scelta = csv.Substring(0,separatore);
            this.dati = csv.Substring(separatore+1,csv.Length-separatore-1);
        }
        public string toCsv()
        {
            return scelta + ";" + dati;
        }
    }
}
