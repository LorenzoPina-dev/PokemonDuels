using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    public class Mossa
    {
        public int id;
        public string nome,desc;
        public int percentuale, danno;
        public  Brush colore;
        public Mossa(string csv)
        {
            string[] campi = csv.Split(';');
            id = int.Parse(campi[0]);
            nome=campi[1];
            if (campi[2].Contains("Purple"))
                colore = Brushes.Purple;
            else if (campi[2].Contains("Blue"))
                colore = Brushes.Blue;
            else if (campi[2].Contains("Red"))
                colore = Brushes.Red;
            else if (campi[2].Contains("Gold"))
                colore = Brushes.Gold;
            else
                colore = Brushes.White;
            desc = campi[3];
            percentuale = int.Parse(campi[5]);
            danno = int.Parse(campi[4]);
        }
        public string ToCsv()
        {
            return nome + ";" + percentuale + ";" + danno;
        }
    }
}
