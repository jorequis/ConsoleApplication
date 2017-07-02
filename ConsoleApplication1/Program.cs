using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimularClientePong();
        }

        public static void ServerUDP()
        {
            byte a = 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");
            a <<= 1;
            Console.Write(Convert.ToString(a, 2).PadLeft(8, '0') + " " + a + "\n");

            UdpClient udpServer = new UdpClient(8001);

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 8001);
                var data = udpServer.Receive(ref remoteEP); // listen on port 11000
                Console.Write("receive data from " + remoteEP.ToString());
                udpServer.Send(new byte[] { 1 }, 1, remoteEP); // reply back
            }
        }

        public static void ClienteUDP()
        {
            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("88.24.19.217"), 9001); // endpoint where server is listening
            client.Connect(ep);

            // send data
            client.Send(new byte[] { Convert.ToByte(1) }, 1);

            // then receive data
            var receivedData = client.Receive(ref ep);

            Console.Write("receive data from " + ep.ToString());

            Console.Read();
        }

        public static void SimularClientePong()
        {
            UInt32 UID = 0;
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse("88.24.19.217"), 9000);
            
            try
            {
                miPrimerSocket.Connect(miDireccion);
                Console.WriteLine("Conectado con exito");

                byte[] ByRec = new byte[4];
                int a = miPrimerSocket.Receive(ByRec, 0, ByRec.Length, 0);
                Array.Resize(ref ByRec, a);

                UID = BitConverter.ToUInt32(ByRec, 0);
                Console.WriteLine("UID Recibido correctamente: " + UID);

                miPrimerSocket.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: {0}", error.ToString());
            }

            //Trozo UDP

            var client = new UdpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("88.24.19.217"), 9050); // endpoint where server is listening
            client.Connect(ep);

            // send data
            byte[] sendBytes = BitConverter.GetBytes(UID);
            for (int i = 0; i < sendBytes.Length; i++)
                Console.WriteLine("Byte" + i + ": " + Convert.ToString(sendBytes[i], 2).PadLeft(8, '0'));
            client.Send(sendBytes, sendBytes.Length);

            byte[] data = client.Receive(ref ep); // listen on port 11000
            Console.Write("Recieved data: " + data.Length + "\n");
            Console.Write("Recieved data: " + BitConverter.ToSingle(data, 0) + "\n");

            while (true)
            {
                data = client.Receive(ref ep); // listen on port 11000
                Console.Write("Recieved data: " + data.Length + "\n");
                Console.Write("Recieved data: " + BitConverter.ToSingle(data, 0) + ", " + BitConverter.ToSingle(data, 4) + "\n");
            }

            Console.WriteLine("Presione cualquier tecla para terminar");
            Console.ReadLine();
        }

        public static void ConectarComoClienteBucle()
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse("10.0.83.156"), 4000);
            IPAddress miDireccionIP = IPAddress.Parse("10.0.83.156");

            Console.WriteLine("Conectando a " + miDireccion.Address + ":" + miDireccion.Port);

            byte[] ByRec;
            string texto = "";
            byte[] textoEnviar;

            try
            {
                miPrimerSocket.Connect(miDireccion);
                Console.WriteLine("Conectado con exito");

                while (!texto.Equals("exit"))
                {

                    Console.WriteLine("Ingrese el texto a enviar al servidor: ");
                    texto = Console.ReadLine(); //leemos el texto ingresado 
                    textoEnviar = Encoding.Default.GetBytes(texto); //pasamos el texto a array de bytes
                    miPrimerSocket.Send(textoEnviar, 0, textoEnviar.Length, 0); // y lo enviamos

                    Console.WriteLine("Enviado exitosamente");
                }

                miPrimerSocket.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: {0}", error.ToString());
            }
            Console.WriteLine("Presione cualquier tecla para terminar");
            Console.ReadLine();
        }

        public static void ConectarComoCliente()
        {
            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Parse("192.168.1.191"), 9000);

            byte[] ByRec;
            string texto;
            byte[] textoEnviar;

            try
            {
                miPrimerSocket.Connect(miDireccion);
                Console.WriteLine("Conectado con exito");

                Console.WriteLine("Ingrese el texto a enviar al servidor: ");
                texto = Console.ReadLine(); //leemos el texto ingresado 
                textoEnviar = Encoding.Default.GetBytes(texto); //pasamos el texto a array de bytes
                miPrimerSocket.Send(textoEnviar, 0, textoEnviar.Length, 0); // y lo enviamos
                
                ByRec = new byte[4];
                int a = miPrimerSocket.Receive(ByRec, 0, ByRec.Length, 0);
                Array.Resize(ref ByRec, a);
                for (int i = 0; i < 4; i++)
                    Console.WriteLine("Servidor dice: " + Convert.ToString(ByRec[i], 2).PadLeft(8, '0')); //mostramos lo recibido
                Console.WriteLine("Servidor dice: " + (BitConverter.ToUInt32(ByRec, 0))); //mostramos lo recibido

                Console.WriteLine("Enviado exitosamente");

                miPrimerSocket.Close();
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: {0}", error.ToString());
            }
            Console.WriteLine("Presione cualquier tecla para terminar");
            Console.ReadLine();
        }

        public static void ConectarComoServidor()
        {
            byte[] ByRec;
            string texto;
            byte[] textoEnviar;

            Socket miPrimerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint miDireccion = new IPEndPoint(IPAddress.Any, 6901);
            
            try
            {
                miPrimerSocket.Bind(miDireccion);
                miPrimerSocket.Listen(1);

                Console.WriteLine("Escuchando...");
                Socket Escuchar = miPrimerSocket.Accept();
                Console.WriteLine("Conectado con exito");

                ByRec = new byte[255];
                int a = Escuchar.Receive(ByRec, 0, ByRec.Length, 0);
                Array.Resize(ref ByRec, a);
                Console.WriteLine("Cliente dice: " + Encoding.Default.GetString(ByRec)); //mostramos lo recibido
                
                Console.WriteLine("Ingrese el texto a enviar al cliente: ");
                texto = Console.ReadLine(); //leemos el texto ingresado 
                textoEnviar = Encoding.Default.GetBytes(texto); //pasamos el texto a array de bytes
                Escuchar.Send(textoEnviar, 0, textoEnviar.Length, 0); // y lo enviamos

                miPrimerSocket.Close();

            }
            catch (Exception error)
            {
                Console.WriteLine("Error: {0}", error.ToString());
            }
            Console.WriteLine("Presione cualquier tecla para terminar");
            Console.ReadLine();

        }
    }
}
