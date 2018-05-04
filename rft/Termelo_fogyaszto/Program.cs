using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class Program
    {
        static void Main(string[] args)
        {
            Termelo t = new Termelo();
            Console.ReadLine();
        }
    }
}
