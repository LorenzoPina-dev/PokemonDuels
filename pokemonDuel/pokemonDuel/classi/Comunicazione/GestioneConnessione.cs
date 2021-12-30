using pokemonDuel.classi.GestioneFile;
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
        private object synInvia;
        private object synTerm;
        public Giocatore Avversario;

        public bool Termina { get { lock (synTerm) { return termina; } }set { lock (synTerm) { termina = value; } } }
        public GestioneConnessione(TcpClient c)
        {
            synInvia=new object();
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
                    split = m.dati.Split(';');
                    Avversario = new Giocatore();
                    Avversario.Username = split[0];
                    for(int i=1;i<split.Length;i++)
                        Avversario.Deck.Add((Pokemon)StoreInfo.Instance().Pokedex[int.Parse(split[i])].Clone());
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
                        int partenza = M.SistemaIndici(int.Parse(split[0]));
                        int destinazione = M.SistemaIndici(int.Parse(split[1]));
                        M.Muovi(partenza, destinazione);
                    });
                    break;
                case "t":
                    DatiCondivisi.Instance().M.Turno = !DatiCondivisi.Instance().M.Turno;
                    break;
                case "tr":
                    DatiCondivisi.Instance().M.RicominciaGioco();
                    break;
                case "tb":
                    if (bool.Parse(m.dati))
                        DatiCondivisi.Instance().VintoPartita();
                    else
                        DatiCondivisi.Instance().PersoPartita();
                    Avversario = null;
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
                        List<Nodo> mappa = DatiCondivisi.Instance().M.mappa;
                        split = m.dati.Split(';');
                        DatiCondivisi.Instance().A = new Attacco(mappa[int.Parse(split[2])], mappa[int.Parse(split[1])], null, StoreInfo.Instance().Mosse[int.Parse(split[0])]);
                    }
                    else
                    {
                        Attacco a = DatiCondivisi.Instance().A;
                        a.MossaAvversario = StoreInfo.Instance().Mosse[int.Parse(m.dati)];
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
                    Messaggio m = DaElaborare.Dequeue();
                    if (m != null)
                        Elabora(m);
                }
            }
        }

        private void GServer()
        {
            while(!Termina)
            {
                if (DaInviare.Count > 0)
                {
                    Messaggio m = DaInviare.Dequeue();
                    if (m != null)
                    {
                        sw.WriteLine(m.toCsv());
                        sw.Flush();
                    }
                }
            }
            sw.Close();
        }

        private void GClient()
        {
            while(!Termina)
            {
                string s = sr.ReadLine();
                if (s == null)
                    Termina = true;
                else if (s != "")
                {
                    Console.WriteLine(s);
                    DaElaborare.Enqueue(new Messaggio(s));
                }
            }
            sr.Close();
        }
    }
}
