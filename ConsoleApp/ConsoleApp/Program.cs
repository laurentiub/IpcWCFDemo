using System;
using System.ServiceModel;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var myClient = new Client();

            var duplexProxy = new DuplexChannelFactory<IServer>(myClient, new NetNamedPipeBinding(), "net.pipe://localhost/test");
            var duplexConnection =  duplexProxy.CreateChannel();
            duplexConnection.Connect();

            Console.ReadKey();            
        }
    }

    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IServer
    {
        [OperationContract]
        void Connect();

        [OperationContract]
        bool Test();
    }

    [ServiceContract]
    public interface IClient
    {
        [OperationContract]
        bool Response(int x);
    }

    public class Client : IClient
    {
        public bool Response(int x)
        {
            Console.WriteLine("Response method called");
            return (x % 2 == 0);
        }
    }

}
