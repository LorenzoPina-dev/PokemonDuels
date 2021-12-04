using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    class Mossa
    {
        public string nome;
        public int percentuale, danno;

        public Mossa(string csv)
        {
            string[] campi = csv.Split(';');
            nome=campi[0];
            percentuale = int.Parse(campi[1]);
            danno = int.Parse(campi[2]);
        }
        public string ToCsv()
        {
            return nome + ";" + percentuale + ";" + danno;
        }
    }
}
