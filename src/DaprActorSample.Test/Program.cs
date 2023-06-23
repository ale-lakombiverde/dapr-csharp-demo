using Dapr.Actors;
using Dapr.Actors.Client;
using DaprActorSample.Actors;
using System;
using System.Threading.Tasks;

namespace DaprActorSample.Person
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var actorId = new ActorId("1");
            var doorProxy = ActorProxy.Create<IDoorActor>(actorId, "DoorActor");
            Console.WriteLine("Close Door");
            await doorProxy.DoorClose();
            Console.WriteLine("Open Door");
            await doorProxy.DoorOpen();
            var isOpen = await doorProxy.IsDoorOpen();
            if (isOpen)
            {
                Console.WriteLine("Test: Door is open");
            }
        }
    }
}
