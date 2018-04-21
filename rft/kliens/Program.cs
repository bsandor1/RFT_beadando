
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

//Simple client


namespace Client_Basic
{
    class Client_Basic
    {
        static void Connect(String server, String message)
        {

            try
            {
                StreamReader streamR;
                StreamWriter streamW;
                Int32 port = 13000;


                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();

                streamR = new StreamReader(stream);
                streamW = new StreamWriter(stream);
                Console.WriteLine("----------StreamReader--------");
                Console.WriteLine(streamR.ReadLine());

                Console.WriteLine("----------NetworkStream--------");
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Küldött: {0}", message);
                if (stream.CanRead)
                {asd
                    data = new Byte[256];
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Kapott: {0}", responseData);
                }
                else
                {
                    Console.WriteLine("Bocsi ezt nem tudod beolvasni :/.");
                }
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Enterrel tovább léphetsz...");
            Console.ReadKey();
        }
        public static void Main(string[] args)
        {
            while (true)
            {
                string modszer = null;
                int a = 0;
                int b = 0;
                string temp1;
                string temp2;
                Console.WriteLine("Adja meg melyik módszerrel szeretne számolni! (LNKO v. LKKT)");
                modszer = Console.ReadLine().ToUpper();
                if (modszer.Equals("LNKO") || modszer.Equals("LKKT"))
                {
                    Console.WriteLine("Adja meg az első számot");
                    temp1 = Console.ReadLine();
                    try { a = int.Parse(temp1); }
                    catch { a = int.Parse(temp1.Replace('.', ',')); }

                    Console.WriteLine("Adja meg az második számot");
                    temp2 = Console.ReadLine();
                    try { b = int.Parse(temp2); }
                    catch { b = int.Parse(temp2.Replace('.', ',')); }

                    if ((a != null) && (b != null))
                        Connect("127.0.0.1", modszer + "|" + a + "|" + b);
                }
                else if (modszer.Equals("KILEP"))
                    break;
                else
                    Console.WriteLine("Ilyen módszer nincs");
            }
        }
    }
}