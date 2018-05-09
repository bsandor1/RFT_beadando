using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Termelo_fogyaszto
{

    class Termelo
    {
        int db;
        public Termelo()
        {

            this.db = 10;
        }


        public void Termel()
        {

            for (int i = 0; i < db; i++)
            {
                Monitor.Enter(Program.puffer);
                while (Program.puffer.Count >= 100)
                {
                    Monitor.Wait(Program.puffer);
                }
                int g = Program.rnd.Next(10000, 80000);

                int sleep = Program.rnd.Next(1, 5);
                Thread.Sleep(sleep);
                Program.puffer.Add(g);
                Monitor.Pulse(Program.puffer);
                Monitor.Exit(Program.puffer);

            }
            Console.WriteLine("A {0}-as szál leállt.");
        }
    }
    class Fogyaszto
    {
        int s;

        public Fogyaszto(ref int s)
        {
            this.s = s;
        }

        public void Fogyaszt()
        {

            while (Program.counter > Program.eddigvolt)
            {
                Program.eddigvolt++;
                Monitor.Enter(Program.puffer);
                while (Program.puffer.Count == 0)
                {
                    Monitor.Wait(Program.puffer);
                }

                Console.WriteLine(Program.puffer[0]);
                s += Program.puffer[0];
                Program.puffer.RemoveAt(0);
                this.FejreszKiir();
                int sleep = Program.rnd.Next(3, 8);
                Thread.Sleep(sleep);
                Monitor.Pulse(Program.puffer);
                Monitor.Exit(Program.puffer);

            }

            Console.WriteLine("Fogyasztó szál leállt.");
        }

        public void FejreszKiir()
        {
            if (Program.puffer.Count == 0)
                Console.Title = "Üres";
            else
            {
                if (Program.puffer.Count >= 100)
                    Console.Title = Program.puffer.Count.ToString();

                else
                    Console.Title = Program.puffer.Count.ToString();
            }
        }
    }
        class Program
        {
            static public List<int> puffer = new List<int>();
            static public Random rnd = new Random();
            static public int counter = 10;
            static public int eddigvolt = 0;
            static void Main(string[] args)
            {
                Termelo t = new Termelo();

                int sum = 0;
                Fogyaszto f1 = new Fogyaszto(ref sum);
                Fogyaszto f2 = new Fogyaszto(ref sum);
                Thread szal1 = new Thread(t.Termel);

                Thread szal2 = new Thread(f1.Fogyaszt);
                Thread szal3 = new Thread(f2.Fogyaszt);
                szal1.Start();
                szal2.Start();
                szal3.Start();
                Console.ReadLine();
            }
        }
    }

