using System.Threading.Tasks;
using Dapr.Actors;

namespace DaprActorSample.Actors;

public interface IDoorActor : IActor
{
    Task DoorOpen();
    Task DoorClose();
    Task<bool> IsDoorOpen();
}