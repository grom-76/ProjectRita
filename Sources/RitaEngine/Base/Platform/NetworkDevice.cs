namespace RitaEngine.Platform ;


using System.Net;
using System.Net.Sockets;
using System.Text;

// https://docs.microsoft.com/fr-fr/dotnet/api/system.net.sockets.udpclient?view=net-5.0
[SkipLocalsInit, StructLayout(LayoutKind.Sequential )]      
public static class NetDevice
{
    public static void Init()
    {
        
    }
    /// <summary>
    /// Simply send message to adress and port (throw exception)
    /// </summary>
    /// <param name="message"></param>
    /// <param name="address"></param>
    /// <param name="port"></param>
    public static void Send(string message,string address,int port=11000 )
    {
        using UdpClient udpClient = new(port);
        try
        {
            udpClient.Connect(address, port);
            // Sends a message to the host to which you have connected.
            // Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            udpClient.Send(Encoding.ASCII.GetBytes(message), message.Length * sizeof(byte));

            udpClient.Close();
        }
        catch (System.Exception)
        {
            
            throw;
        }
        finally 
        {
            udpClient.Close();
        }
    }
    
    /// <summary>
    /// Simple receive data from server at fixed ip
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public static byte[] ReceiveAnyIP(int port=11000)
    {
        UdpClient udpClient = new(port);
        try
        {
            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new(IPAddress.Any, port);

            // Blocks until a message returns on this socket from a remote host.
            byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            //string returnData = Encoding.ASCII.GetString(receiveBytes);

            udpClient.Close();
            return receiveBytes;
        }
        catch (System.Exception)
        {
            udpClient.Close();
            throw;
        }
    }

}

// public class LiteNetLibSample
// {
//     public void Server()
//     {
//         EventBasedNetListener listener = new EventBasedNetListener();
//         NetManager server = new NetManager(listener);
//         server.Start(9050 /* port */);

//         listener.ConnectionRequestEvent += request =>
//         {
//             if(server.ConnectedPeersCount < 10 /* max connections */)
//                 request.AcceptIfKey("SomeConnectionKey");
//             else
//                 request.Reject();
//         };

//         listener.PeerConnectedEvent += peer =>
//         {
//             Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
//             NetDataWriter writer = new NetDataWriter();                 // Create writer class
//             writer.Put("Hello client!");                                // Put some string
//             peer.Send(writer, DeliveryMethod.ReliableOrdered);             // Send with reliability
//         };

//         while (!Console.KeyAvailable)
//         {
//             server.PollEvents();
//             Thread.Sleep(15);
//         }

//         server.Stop();
//     }

//     public void Client()
//     {
//         EventBasedNetListener listener = new EventBasedNetListener();
//         NetManager client = new NetManager(listener);
//         client.Start();
//         client.Connect("localhost" /* host ip or name */, 9050 /* port */, "SomeConnectionKey" /* text key or NetDataWriter */);
//         listener.NetworkReceiveEvent += (fromPeer, dataReader, channel, deliveryMethod) =>
//         {
//             Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
//             dataReader.Recycle();
//         };

//         while (!Console.KeyAvailable)
//         {
//             client.PollEvents();
//             Thread.Sleep(15);
//         }

//         client.Stop();
//     }
// }

// public class EchoMessagesTest 
// {
//     private static int _messagesReceivedCount = 0;

//     private class ClientListener : INetEventListener
//     {
//         public void OnPeerConnected(NetPeer peer)
//         {
//             Console.WriteLine("[Client] connected to: {0}:{1}", peer.EndPoint.Address, peer.EndPoint.Port);

//             NetDataWriter dataWriter = new NetDataWriter();
//             for (int i = 0; i < 5; i++)
//             {
//                 dataWriter.Reset();
//                 dataWriter.Put(0);
//                 dataWriter.Put(i);
//                 peer.Send(dataWriter, DeliveryMethod.ReliableUnordered);

//                 dataWriter.Reset();
//                 dataWriter.Put(1);
//                 dataWriter.Put(i);
//                 peer.Send(dataWriter, DeliveryMethod.ReliableOrdered);

//                 dataWriter.Reset();
//                 dataWriter.Put(2);
//                 dataWriter.Put(i);
//                 peer.Send(dataWriter, DeliveryMethod.Sequenced);

//                 dataWriter.Reset();
//                 dataWriter.Put(3);
//                 dataWriter.Put(i);
//                 peer.Send(dataWriter, DeliveryMethod.Unreliable);

//                 dataWriter.Reset();
//                 dataWriter.Put(4);
//                 dataWriter.Put(i);
//                 peer.Send(dataWriter, DeliveryMethod.ReliableSequenced);
//             }

//             //And test fragment
//             byte[] testData = new byte[13218];
//             testData[0] = 192;
//             testData[13217] = 31;
//             peer.Send(testData, DeliveryMethod.ReliableOrdered);
//         }

//         public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
//         {
//             Console.WriteLine("[Client] disconnected: " + disconnectInfo.Reason);
//         }

//         public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
//         {
//             Console.WriteLine("[Client] error! " + socketErrorCode);
//         }

//         public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
//         {
//             if (reader.AvailableBytes == 13218)
//             {
//                 Console.WriteLine("[{0}] TestFrag: {1}, {2}",
//                     peer.NetManager.LocalPort,
//                     reader.RawData[reader.UserDataOffset],
//                     reader.RawData[reader.UserDataOffset + 13217]);
//             }
//             else
//             {
//                 int type = reader.GetInt();
//                 int num = reader.GetInt();
//                 _messagesReceivedCount++;
//                 Console.WriteLine("[{0}] CNT: {1}, TYPE: {2}, NUM: {3}, MTD: {4}", peer.NetManager.LocalPort, _messagesReceivedCount, type, num, deliveryMethod);
//             }
//         }

//         public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
//         {

//         }

//         public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
//         {

//         }

//         public void OnConnectionRequest(ConnectionRequest request)
//         {

//         }
//     }

//     private class ServerListener : INetEventListener
//     {
//         public NetManager Server=null!;

//         public void OnPeerConnected(NetPeer peer)
//         {
//             Console.WriteLine("[Server] Peer connected: " + peer.EndPoint);
//             foreach (var netPeer in Server)
//             {
//                 if(netPeer.ConnectionState == ConnectionState.Connected)
//                     Console.WriteLine("ConnectedPeersList: id={0}, ep={1}", netPeer.Id, netPeer.EndPoint);
//             }
//         }

//         public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
//         {
//             Console.WriteLine("[Server] Peer disconnected: " + peer.EndPoint + ", reason: " + disconnectInfo.Reason);
//         }

//         public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
//         {
//             Console.WriteLine("[Server] error: " + socketErrorCode);
//         }

//         public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
//         {
//             //echo
//             peer.Send(reader.GetRemainingBytes(), deliveryMethod);

//             //fragment log
//             if (reader.AvailableBytes == 13218)
//             {
//                 Console.WriteLine("[Server] TestFrag: {0}, {1}",
//                     reader.RawData[reader.UserDataOffset],
//                     reader.RawData[reader.UserDataOffset + 13217]);
//             }
//         }

//         public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
//         {
//             Console.WriteLine("[Server] ReceiveUnconnected: {0}", reader.GetString(100));
//         }

//         public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
//         {

//         }

//         public void OnConnectionRequest(ConnectionRequest request)
//         {
//             var acceptedPeer = request.AcceptIfKey("gamekey");
//             Console.WriteLine("[Server] ConnectionRequest. Ep: {0}, Accepted: {1}",
//                 request.RemoteEndPoint,
//                 acceptedPeer != null);
//         }
//     }

//     private ClientListener _clientListener=null!;
//     private ServerListener _serverListener=null!;
//     private const int Port = 9050;

//     public void Run()
//     {
//         Console.WriteLine("=== Echo Messages Test ===");
//         //Server
//         _serverListener = new ServerListener();

//         NetManager server = new NetManager(_serverListener);

//         if (!server.Start(Port))
//         {
//             Console.WriteLine("Server start failed");
//             Console.ReadKey();
//             return;
//         }
//         _serverListener.Server = server;

//         //Client
//         _clientListener = new ClientListener();

//         NetManager client1 = new NetManager(_clientListener)
//         {
//             SimulationMaxLatency = 1500,
//             //SimulateLatency = true,
//         };
//         //client1
//         if (!client1.Start())
//         {
//             Console.WriteLine("Client1 start failed");
//             return;
//         }
//         client1.Connect("127.0.0.1", Port, "gamekey");

//         NetManager client2 = new NetManager(_clientListener)
//         {
//             //SimulateLatency = true,
//             SimulationMaxLatency = 1500
//         };

//         client2.Start();
//         client2.Connect("::1", Port, "gamekey");

//         while (!Console.KeyAvailable)
//         {
//             client1.PollEvents();
//             client2.PollEvents();
//             server.PollEvents();
//             Thread.Sleep(15);
//         }

//         client1.Stop();
//         client2.Stop();
//         server.Stop();
//         Console.ReadKey();
//         Console.WriteLine("ServStats:\n BytesReceived: {0}\n PacketsReceived: {1}\n BytesSent: {2}\n PacketsSent: {3}",
//             server.Statistics.BytesReceived,
//             server.Statistics.PacketsReceived,
//             server.Statistics.BytesSent,
//             server.Statistics.PacketsSent);
//         Console.WriteLine("Client1Stats:\n BytesReceived: {0}\n PacketsReceived: {1}\n BytesSent: {2}\n PacketsSent: {3}",
//             client1.Statistics.BytesReceived,
//             client1.Statistics.PacketsReceived,
//             client1.Statistics.BytesSent,
//             client1.Statistics.PacketsSent);
//         Console.WriteLine("Client2Stats:\n BytesReceived: {0}\n PacketsReceived: {1}\n BytesSent: {2}\n PacketsSent: {3}",
//             client2.Statistics.BytesReceived,
//             client2.Statistics.PacketsReceived,
//             client2.Statistics.BytesSent,
//             client2.Statistics.PacketsSent);
//         Console.WriteLine("Press any key to exit");
//         Console.ReadKey();
//     }
// }

// class BroadcastTest 
// {
//     private class ClientListener : INetEventListener
//     {
//         public NetManager Client=null!;

//         public void OnPeerConnected(NetPeer peer)
//         {
//             Console.WriteLine("[Client {0}] connected to: {1}:{2}", Client.LocalPort, peer.EndPoint.Address, peer.EndPoint.Port);
//         }

//         public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
//         {
//             Console.WriteLine("[Client] disconnected: " + disconnectInfo.Reason);
//         }

//         public void OnNetworkError(IPEndPoint endPoint, SocketError error)
//         {
//             Console.WriteLine("[Client] error! " + error);
//         }

//         public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channel, DeliveryMethod deliveryMethod)
//         {

//         }

//         public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
//         {
//             var text = reader.GetString(100);
//             Console.WriteLine("[Client] ReceiveUnconnected {0}. From: {1}. Data: {2}", messageType, remoteEndPoint, text);
//             if (messageType == UnconnectedMessageType.BasicMessage && text == "SERVER DISCOVERY RESPONSE")
//             {
//                 Client.Connect(remoteEndPoint, "key");
//             }
//         }

//         public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
//         {

//         }

//         public void OnConnectionRequest(ConnectionRequest request)
//         {
//             request.Reject();
//         }
//     }

//     private class ServerListener : INetEventListener
//     {
//         public NetManager Server=null!;

//         public void OnPeerConnected(NetPeer peer)
//         {
//             Console.WriteLine("[Server] Peer connected: " + peer.EndPoint);
//             var peers = Server.ConnectedPeerList;
//             foreach (var netPeer in peers)
//             {
//                 Console.WriteLine("ConnectedPeersList: id={0}, ep={1}", netPeer.Id, netPeer.EndPoint);
//             }
//         }

//         public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
//         {
//             Console.WriteLine("[Server] Peer disconnected: " + peer.EndPoint + ", reason: " + disconnectInfo.Reason);
//         }

//         public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
//         {
//             Console.WriteLine("[Server] error: " + socketErrorCode);
//         }

//         public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
//         {

//         }

//         public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
//         {
//             Console.WriteLine("[Server] ReceiveUnconnected {0}. From: {1}. Data: {2}", messageType, remoteEndPoint, reader.GetString(100));
//             NetDataWriter writer = new NetDataWriter();
//             writer.Put("SERVER DISCOVERY RESPONSE");
//             Server.SendUnconnectedMessage(writer, remoteEndPoint);
//         }

//         public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
//         {

//         }

//         public void OnConnectionRequest(ConnectionRequest request)
//         {
//             request.AcceptIfKey("key");
//         }
//     }

//     private ClientListener _clientListener1=null!;
//     private ClientListener _clientListener2=null!;
//     private ServerListener _serverListener=null!;

//     public void Run()
//     {
//         Console.WriteLine("=== Broadcast Test ===");
//         //Server
//         _serverListener = new ServerListener();

//         NetManager server = new NetManager(_serverListener)
//         {
//             BroadcastReceiveEnabled = true,
//             IPv6Mode = IPv6Mode.DualMode
//         };
//         if (!server.Start(9050))
//         {
//             Console.WriteLine("Server start failed");
//             Console.ReadKey();
//             return;
//         }
//         _serverListener.Server = server;

//         //Client
//         _clientListener1 = new ClientListener();

//         NetManager client1 = new NetManager(_clientListener1)
//         {
//             UnconnectedMessagesEnabled = true,
//             SimulateLatency = true,
//             SimulationMaxLatency = 1500,
//             IPv6Mode = IPv6Mode.DualMode
//         };
//         _clientListener1.Client = client1;
//         if (!client1.Start())
//         {
//             Console.WriteLine("Client1 start failed");

//             return;
//         }

//         _clientListener2 = new ClientListener();
//         NetManager client2 = new NetManager(_clientListener2)
//         {
//             UnconnectedMessagesEnabled = true,
//             SimulateLatency = true,
//             SimulationMaxLatency = 1500,
//             IPv6Mode = IPv6Mode.DualMode
//         };

//         _clientListener2.Client = client2;
//         client2.Start();

//         //Send broadcast
//         NetDataWriter writer = new NetDataWriter();

//         writer.Put("CLIENT 1 DISCOVERY REQUEST");
//         client1.SendBroadcast(writer, 9050);
//         writer.Reset();

//         writer.Put("CLIENT 2 DISCOVERY REQUEST");
//         client2.SendBroadcast(writer, 9050);

//         while (!Console.KeyAvailable)
//         {
//             client1.PollEvents();
//             client2.PollEvents();
//             server.PollEvents();
//             Thread.Sleep(15);
//         }

//         client1.Stop();
//         client2.Stop();
//         server.Stop();
//         Console.ReadKey();
//         Console.WriteLine("Press any key to exit");
//         Console.ReadKey();
//     }
// }
