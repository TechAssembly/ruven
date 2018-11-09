
using System.Collections.Generic;
using System.Linq;

public class AvailableRooms
{
    readonly Dictionary<string, List<RoomData>> rooms = new Dictionary<string, List<RoomData>>();

    public void UpdateRoomList(string roomName, List<RoomData> roomList)
    {
        rooms[roomName] = roomList;
    }

    public List<RoomData> GetAllRooms()
    {
        return rooms.Values.SelectMany(r => r).ToList();
    }
}