using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Util
{
    public class Conversione
    {
        public static double getRad(int gradi)
        {
            return gradi / 180f * Math.PI;
        }
    }
    public enum Finestra
    {
        Utente,Pokemon,Inviti,Battaglia,Shop
    }
}
