using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

using Socket serverSocket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Dgram,
    ProtocolType.Udp);

int port = 5001;

IPEndPoint IPEndPoint = new(IPAddress.Loopback, port);

serverSocket.Bind(IPEndPoint);

Console.WriteLine($"UDP-сервер запущен на порту {port}. Ожидание датаграмм...");
while (true)
{
    EndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
    byte[] buffer = new byte[1024];
    var bytesRead = serverSocket.ReceiveFrom(buffer, ref remoteIPEndPoint);
    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine($"[Сервер] Получено от {remoteIPEndPoint}: {receivedMessage}");
    serverSocket.SendTo(buffer, 0, bytesRead, SocketFlags.None, remoteIPEndPoint);
    Console.WriteLine($"[Сервер] Ответ отправлен обратно на {remoteIPEndPoint}\n");
}