// See https://aka.ms/new-console-template for more information
using SharedLib;

Console.WriteLine("Enter server ip:");
var server = Console.ReadLine();

ConnectionMgr connectionMgr = new ConnectionMgr(server);
connectionMgr.TestConnect();

Console.ReadLine();
