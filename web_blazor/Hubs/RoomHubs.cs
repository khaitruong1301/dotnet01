
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

public class RoomHubs:Hub{

    public RoomService _roomService;
    public ProductService _productService;
    public RoomHubs(RoomService roomService, ProductService prodService){
        _roomService = roomService;
        _productService = prodService;
        
    }
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        Console.WriteLine($"connected client-id: {Context.ConnectionId}");
        //Sau khi connect thì gửi dữ liệu cho tất cả client danh sách phòng
        // _productService.GetAllProductApi();
       
        //Gọi api = server 
        List<RoomVM> lstRoom = RoomService.lstRoom;
        await Clients.All.SendAsync("GetAllRoom",lstRoom);
        // await Clients.All.SendAsync("GetAllProduct",_productService.lstProduct);

        
    }
    //Tạo ra 1 api hub trả về danh sách các phòng
    
    public async Task AddRoom(){
        //Tạo 1 room mới
        RoomVM newRoom = new RoomVM();
        RoomService.lstRoom.Add(newRoom); //Thêm 1 room vào list
        Console.WriteLine($"{Context.ConnectionId}");
        //Phát lại toàn client lst room mới
        await Clients.All.SendAsync("GetAllRoom",RoomService.lstRoom);
        
    }

    public async Task AddNewProd(ProductAddNew prod){
        
        //gọi api thêm
        //Gọi api getAll 
        await Clients.All.SendAsync("GetAllRoom",RoomService.lstRoom);
        
    }
    


    public override async Task OnDisconnectedAsync(Exception? ex){
        Console.WriteLine($"disconnectd client-id: {Context.ConnectionId}");

        await base.OnDisconnectedAsync(ex);
    }

}