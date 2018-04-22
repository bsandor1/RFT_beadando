using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using ClassLibrary1;


namespace szerver
{
    class ClientComm
    {
        protected NetworkStream stream;
        protected StreamReader streamR;
        protected StreamWriter streamW;


        public ClientComm(TcpClient incoming)
        {
            stream = incoming.GetStream();
            streamR = new StreamReader(stream);
            streamW = new StreamWriter(stream);
        }
        public void CommStart()
        {
            bool end = false;
            bool delete_thr = true;

            try
            {
                streamW.WriteLine("Üdvözöllek ! Én vagyok a legszörnyűbb rémálmod . Megcsináltam amit kértél ... de lehet hogy mást ;) ! ");
                streamW.Flush();

                while (!end)
                {
                    Byte[] bytes = new Byte[256];
                    String data = null;
                    Console.WriteLine("Kapcsolódva!");
                    data = null;
                    int i;
                    string work = "Valami történt";
                    Console.WriteLine(work);
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Kapott: {0}", data);
                        if (data.Split('|')[0].Equals("LNKO"))
                            data = "Az LNKO=" + Matek.LNKO(int.Parse(data.Split('|')[1]), int.Parse(data.Split('|')[2]));
                        else if (data.Split('|')[0].Equals("LKKT"))
                            data = "Az LKKT=" + Matek.LKKT(int.Parse(data.Split('|')[1]), int.Parse(data.Split('|')[2]));
                        else data = "Nem volt jó a bemenet";
                        data = data.ToUpper();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);


                    }
                    end = true;
                }
            }
            catch (Exception e)
            {
                if (e is ThreadAbortException)
                    delete_thr = false;
                Console.WriteLine("Lekapcsolódva");
            }
            if (delete_thr)
            {
                lock (Listerner.running_threads)
                {
                    Thread thisss = Thread.CurrentThread;
                    int i = Server_Basic.Listerner.running_threads.IndexOf(thisss);
                    if (i != -1) Listerner.running_threads.RemoveAt(i);
                    Console.WriteLine("Lekapcsolódva");

                }
            }
            stream.Close();
        }
    }

    class Listerner
    {
        static TcpListener server = null;
        static Thread connections = null;

        public static List<Thread> running_threads = new List<Thread>();

        static void connectionReciever()
        {
            while (true)
            {
                if (server.Pending())
                {
                    TcpClient client = server.AcceptTcpClient();
                    ClientComm c = new ClientComm(client);
                    Thread t = new Thread(c.CommStart);
                    lock (running_threads)
                    {
                        running_threads.Add(t);
                    }
                    t.Start();
                }

            }
        }
        public static void Main()
        {
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();
                connections = new Thread(connectionReciever);
                connections.Start();
                ConsoleKeyInfo cki;
                Console.TreatControlCAsInput = true;
                Console.WriteLine("Szerver nyitott!");
                Console.WriteLine("Az ESC gombbal bezárhatod: \n");
                do
                {
                    cki = Console.ReadKey();
                } while (cki.Key != ConsoleKey.Escape);



                connections.Abort();
            }

            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
                Console.WriteLine("The server is halott.");

            }
            Console.WriteLine("\n Nyomj Entert a továbblépéshez...");
            Console.ReadLine();
        }
    }

}
