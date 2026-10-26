using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

int port = 5000;

using Socket serverSocket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Loopback, port);

serverSocket.Bind(ipendpoint);

serverSocket.Listen(10);

Console.WriteLine("Сервер запущен на порту 5000. Ожидание подключение...");

while (true)
{
    Socket clientSocket = serverSocket.Accept();
    Console.WriteLine($"[Сервер] Подключился клиент: {clientSocket.RemoteEndPoint}");
    ThreadPool.QueueUserWorkItem(_ => HandleClient(clientSocket));
}

void HandleClient(Socket clientSocket)
{
    var clientInfo = clientSocket.RemoteEndPoint;
    byte[] buffer = new byte[1024];
    StringBuilder message = new StringBuilder();

    while (true)
    {
        var receivedBytes = clientSocket.Receive(buffer);

        if (receivedBytes == 0)
        {
            Console.WriteLine($"Клиент [{clientInfo}] отключился.");
            clientSocket.Send(Encoding.UTF8.GetBytes(message.ToString()));
            clientSocket.Close();
            return;
        }

        var receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
        message.Append(receivedMessage);
    }
}