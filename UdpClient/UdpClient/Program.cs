using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

using Socket clientSocket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Dgram,
    ProtocolType.Udp);

int port = 5001;

EndPoint serverEndPoint = new IPEndPoint(IPAddress.Loopback, port);

Console.WriteLine($"UDP-клиент запущен. Адрес сервера: {serverEndPoint}");
while (true)
{
    EndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
    
    Console.WriteLine("Вводите сообщения по очереди. Нажмите Enter на пустой строке для выхода.\n");

    Console.Write("> ");
    string? message = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(message)) break;

    byte[] buffer = new byte[1024];
    var bytes = Encoding.UTF8.GetBytes(message);
    clientSocket.SendTo(bytes, serverEndPoint);
    Console.WriteLine($"[Клиент] Датаграмма ({bytes.Length} байт) отправлена на сервер.");

    var bytesRead = clientSocket.ReceiveFrom(buffer, ref remoteIpEndPoint);
    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
    Console.WriteLine($"[Клиент] Получен ответ от {remoteIpEndPoint}: \"{receivedMessage}\"\n");
}

Console.WriteLine("Работа клиента завершена.");

