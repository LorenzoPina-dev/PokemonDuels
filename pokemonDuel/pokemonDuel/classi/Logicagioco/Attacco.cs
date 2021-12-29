using pokemonDuel.classi.Grafica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Logicagioco
{
    class Attacco
    {
        public Nodo Mio,Avversario;
        public Mossa MossaMia, MossaAvversario;
        public Attacco(Nodo Mio, Nodo Avversario, Mossa MossaMia, Mossa MossaAvversario)
        {
            this.Mio = Mio;
            this.Avversario = Avversario;
            this.MossaMia = MossaMia;
            this.MossaAvversario = MossaAvversario;
        }
        public bool Settato()
        {
            if (Mio != null && Avversario != null && MossaMia != null && MossaAvversario != null)
                return true;
            return false;
        }
        public void EseguiAttacco()
        {
            if (!Settato())
                throw new Exception("attacco non settato");
            if (MossaMia.danno > MossaAvversario.danno)
                DatiCondivisi.Instance().M.RimettiNellaMano(Avversario);
            else if (MossaMia.danno < MossaAvversario.danno)
                DatiCondivisi.Instance().M.RimettiNellaMano(Mio);
            
        }

    }
}
