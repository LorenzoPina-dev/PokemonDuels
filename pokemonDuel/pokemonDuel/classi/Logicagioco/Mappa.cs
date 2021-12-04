using pokemonDuel.classi.GestioneFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    class Mappa
    {
        Nodo [] mappa;
        int fine = 3,partenza=22;
        int[] startPosizionamento;
        List<Pokemon> miaMano,altraMano;
        bool turno;
        public Mappa()
        {
            this.mappa = new Nodo[32];
            this.fine = 3;
            this.partenza = 22;
            this.startPosizionamento = new int[] { 25, 31 };
            for (int i = 0; i < mappa.Length; i++)
                mappa[i] = new Nodo(i);
            List<string> collegamenti = LeggiFile.Leggi("./file/Mappa.txt");
            foreach(string s in collegamenti)
            {
                string[] campi = s.Split(';');
                int I = int.Parse(campi[0]),F=int.Parse(campi[1]);
                mappa[I].AddVicino(F);
                mappa[F].AddVicino(I);
            }
        }
    }
}
