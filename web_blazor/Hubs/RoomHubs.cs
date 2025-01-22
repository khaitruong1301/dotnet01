
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class RoomHubs:Hub{

    public RoomService _roomService;
    public RoomHubs(RoomService roomService){
        _roomService = roomService;
        
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"connected client-id: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex){
        await base.OnDisconnectedAsync(ex);
    }

}