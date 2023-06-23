using System;
using System.Threading.Tasks;
using Dapr.Actors.Runtime;
using DaprActorSample.Actors;
using Microsoft.Extensions.Logging;

namespace DaprActorSample.ActorsHost.Actors;

public class DoorActor : Actor, IDoorActor, IRemindable
{
    private const string StateName = "door_data";

    public DoorActor(ActorHost host) : base(host)
    {
    }

    protected override async Task OnActivateAsync()
    {
        await base.OnActivateAsync();

        var stateExists = await StateManager.ContainsStateAsync(StateName);
        if (!stateExists)
        {
            var data = new DoorData {IsDoorOpen = true};
            await StateManager.SetStateAsync(StateName, data);
        }
    }

    public async Task DoorClose()
    {
        var state = await StateManager.GetStateAsync<DoorData>(StateName);
        state.IsDoorOpen = false;
        await StateManager.SetStateAsync(StateName, state);

        await UnregisterReminderAsync("control-loop");
    }

    public async Task DoorOpen()
    {
        var state = await StateManager.GetStateAsync<DoorData>(StateName);
        state.IsDoorOpen = true;
        await StateManager.SetStateAsync(StateName, state);

        await RegisterReminderAsync("control-loop", null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
    }

    public async Task<bool> IsDoorOpen()
    {
        var airConState = await StateManager.GetStateAsync<DoorData>(StateName);
        return airConState.IsDoorOpen;
    }

    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        var doorState = await StateManager.GetStateAsync<DoorData>(StateName);
        Logger.LogInformation(doorState.IsDoorOpen ? "Door Open." : "Door Closed.");
    }
}

public class DoorData
{
    public bool IsDoorOpen { get; set; }
}