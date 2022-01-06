using pokemonDuel.classi.GestioneFile;
using pokemonDuel.classi.Grafica;
using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pokemonDuel.classi.Comunicazione
{
    public class GestioneConnessione
    {
        private StreamWriter sw;
        private StreamReader sr;
        private bool termina;
        private Queue<Messaggio> DaInviare;
        private Queue<Messaggio> DaElaborare;
        private object synElabora,synInvia;
        private object synTerm;
        public Giocatore Avversario;

        public bool Termina { get { lock (synTerm) { return termina; } }set { lock (synTerm) { termina = value; } } }
        public GestioneConnessione(TcpClient c)
        {
            synInvia = new object();
            synElabora = new object();
            synTerm = new object();
            sw = new StreamWriter(c.GetStream());
            sr = new StreamReader(c.GetStream());
            termina = false;
            DaInviare = new Queue<Messaggio>();
            DaElaborare = new Queue<Messaggio>();
            Thread client = new Thread(GClient);
            Thread Server = new Thread(GServer);
            Thread Logica = new Thread(GLogica);
            client.Start();
            Server.Start();
            Logica.Start();
        }


        private void Elabora(Messaggio m)
        {
            string[] split;
            switch (m.scelta)
            {
                case "c":
                    Avversario = new Giocatore(m.dati);
                    if (DatiCondivisi.Instance().Avversario == null)
                        DatiCondivisi.Instance().MostraRichiestaBattaglia(this);
                    else
                        Invia(new Messaggio("n", ""));
                    break;
                case "m":
                    split = m.dati.Split(';');
                    Mappa M = DatiCondivisi.Instance().M;
                    DatiCondivisi.Instance().caricamento.Dispatcher.Invoke(delegate
                    {
                        M.Muovi(int.Parse(split[0]), int.Parse(split[1]));
                    });
                    break;
                case "t":
                    DatiCondivisi.Instance().M.Turno = !DatiCondivisi.Instance().M.Turno;
                    break;
                case "tr":
                    DatiCondivisi.Instance().TermineRound(int.Parse(m.dati)==0);
                    DatiCondivisi.Instance().M.Nturno++;
                    if (int.Parse(m.dati) == 0)
                        DatiCondivisi.Instance().M.Tvinti++;
                    break;
                case "tb":
                    DatiCondivisi.Instance().TerminaPartita(int.Parse(m.dati) == 0);
                    Termina = true;
                    break;
                case "y":
                    if(m.dati.Contains(';'))
                    {
                        Avversario = new Giocatore();
                        split = m.dati.Split(';');
                        Avversario.Username = split[0];
                        for (int i = 1; i < split.Length; i++)
                            Avversario.Deck.Add((Pokemon)StoreInfo.Instance().Pokedex[int.Parse(split[i])].Clone());
                        if (DatiCondivisi.Instance().Avversario == null)
                        {
                            Invia(new Messaggio("y", ""));
                            DatiCondivisi.Instance().M.Turno = false;
                            DatiCondivisi.Instance().AvviaPartita();
                        }
                        else
                            Invia(new Messaggio("n", ""));
                    }
                    if (DatiCondivisi.Instance().Avversario == null)
                    {
                        DatiCondivisi.Instance().altro = Avversario;
                        DatiCondivisi.Instance().Avversario = this;
                        DatiCondivisi.Instance().AvviaPartita();
                    }
                    break;
                case "n":
                    Termina = true;
                    break;
                case "a":
                    if (m.dati.Contains(';'))
                    {
                        Mappa mappa = DatiCondivisi.Instance().M;
                        split = m.dati.Split(';');
                        DatiCondivisi.Instance().A = new Attacco(mappa.mappa[mappa.SistemaIndici(int.Parse(split[1]))], mappa.mappa[mappa.SistemaIndici(int.Parse(split[0]))], null, StoreInfo.Instance().Mosse[int.Parse(split[2])]);
                        DatiCondivisi.Instance().main.Dispatcher.Invoke(delegate
                        {
                            GestioneRuota.Instance().Start();
                        });
                    }
                    else
                    {
                        Attacco a = DatiCondivisi.Instance().A;
                        a.MossaAvversario = (Mossa)StoreInfo.Instance().Mosse[int.Parse(m.dati)].Clone();
                        if (a.Settato())
                            a.EseguiAttacco();
                    }
                    break;
            }
        }

        public void Invia(Messaggio m)
        {
            lock (synInvia)
            {
                DaInviare.Enqueue(m);
            }
        }

        private void GLogica()
        {
            while (!Termina)
            {
                if (DaElaborare.Count > 0)
                {
                    Messaggio m;
                    lock (synElabora)
                    {
                        m = DaElaborare.Dequeue();
                    }
                    if (m != null)
                        Elabora(m);
                }
            }
        }

        private void GServer()
        {
            try
            {
                while (!Termina || DaInviare.Count != 0)
                {
                    if (DaInviare.Count > 0)
                    {
                        Messaggio m;
                        lock (synInvia)
                        {
                            m = DaInviare.Dequeue();
                        }
                        if (m != null)
                        {
                            sw.WriteLine(m.toCsv());
                            sw.Flush();
                        }
                    }
                }
            }catch(Exception)
            {}
            sw.Close();
        }

        private void GClient()
        {
            try { 
            while(!Termina)
            {
                string s = sr.ReadLine();
                if (s == null)
                    Termina = true;
                else if (s != "")
                {
                    Console.WriteLine(s);
                    Messaggio m = new Messaggio(s);
                    lock (synElabora)
                    {
                        DaElaborare.Enqueue(m);
                    }
                }
            }
        }catch(Exception)
            {}
    sr.Close();
        }
    }
}
