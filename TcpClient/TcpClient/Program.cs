using System.Net;
using System.Net.Sockets;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

using Socket clientSocket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

int port = 5000;

IPEndPoint ipendpoint = new(IPAddress.Loopback, port);

clientSocket.Connect(ipendpoint);

Console.WriteLine("Подключение к серверу установлено!");
Console.WriteLine("Вводите сообщения по очереди. Чтобы завершить ввод и получить ответ, нажмите Enter на пустой строке:");

while (true)
{
    Console.Write("> ");
    string? message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(message)) break;

    var messageBytes = Encoding.UTF8.GetBytes(message);

    clientSocket.Send(messageBytes);
}

clientSocket.Shutdown(SocketShutdown.Send);

byte[] buffer = new byte[1024];

Console.WriteLine("Передача данных завершена. Ожидание ответа от сервера...");

var bytesRead = clientSocket.Receive(buffer);

var received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

Console.WriteLine("\n--- Ответ от сервера ---"); 
Console.WriteLine(received); 
Console.WriteLine("-----------------------------");

clientSocket.Close();

Console.WriteLine("Соединение успешно закрыто.");
