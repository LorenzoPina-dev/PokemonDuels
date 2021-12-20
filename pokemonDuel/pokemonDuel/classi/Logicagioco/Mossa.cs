using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    class Mossa
    {
        public int id;
        public string nome,desc;
        public int percentuale, danno;

        public Mossa(string csv)
        {
            string[] campi = csv.Split(';');
            id = int.Parse(campi[0]);
            nome=campi[1];
            desc = campi[2];
            percentuale = int.Parse(campi[4]);
            danno = int.Parse(campi[3]);
        }
        public string ToCsv()
        {
            return nome + ";" + percentuale + ";" + danno;
        }
    }
}
