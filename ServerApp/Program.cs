using System;
using System.ServiceModel;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var myServer = new Server();

            var host = new ServiceHost(myServer);
            host.AddServiceEndpoint(typeof(IServer), new NetNamedPipeBinding(), "net.pipe://localhost/test");

            host.Open();

            Console.WriteLine("Server started.");
            Console.WriteLine("Press any key to callback...");
            Console.ReadKey();

            myServer.Context.Response(4);

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

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Server : IServer
    {
        public IClient Context { get; set; }

        public void Connect()
        {
            Context = OperationContext.Current.GetCallbackChannel<IClient>();
        }

        public bool Test()
        {
            if(Context == null)
                Context = OperationContext.Current.GetCallbackChannel<IClient>();

            Console.WriteLine("Test method called");
            return true;
        }
    }

    [ServiceContract]
    public interface IClient
    {
        [OperationContract]
        bool Response(int x);
    }
}
