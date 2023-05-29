using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using racing_backend.Models;

namespace racing_backend.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, string> textOfRoom = new Dictionary<string, string>();
    private static Dictionary<string, List<User>> rooms = new Dictionary<string, List<User>>();

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        string connectionId = Context.ConnectionId;

        var roomReceiveMsg = rooms.Where(kv => kv.Value.Any(u => u.ConnectionId == connectionId))
            .Select(r => r.Key).ToList();

        foreach (var room in rooms)
        {
            room.Value.RemoveAll(u => u.ConnectionId == connectionId);
            if (room.Value.Count() == 0)
            {
                textOfRoom.Remove(room.Key);
                rooms.Remove(room.Key);
            }
        }

        await Clients.Groups(roomReceiveMsg).SendAsync("Disconnected", connectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task CreateRoom(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            await ReceiveError("Username invalid!!!");
        }
        var connectionId = Context.ConnectionId;
        var idRoom = new Random().Next(0, 1000000).ToString("D6");
        while (rooms.ContainsKey(idRoom))
        {
            await CreateRoom(userName);
        }
        await Groups.AddToGroupAsync(connectionId, idRoom);
        var user = new User
        {
            ConnectionId = connectionId,
            UserName = userName,
            IsHost = true,
        };
        rooms.Add(idRoom, new List<User> { user });
        await SendMessageToClient(idRoom, connectionId, rooms.FirstOrDefault(x => x.Key == idRoom).Value);
    }

    public async Task JoinRoom(string idRoom, string userName)
    {
        var connectionId = Context.ConnectionId;
        if (!rooms.Any(r => r.Key == idRoom))
        {
            await ReceiveError("Room id does not exist");
            return;
        }
        await Groups.AddToGroupAsync(connectionId, idRoom);
        var users = rooms.FirstOrDefault(x => x.Key == idRoom).Value;
        users.Add(new User
        {
            ConnectionId = connectionId,
            UserName = userName
        });
        await SendMessageToClient(idRoom, connectionId, rooms.FirstOrDefault(x => x.Key == idRoom).Value);
    }

    public async Task StartGame(string idRoom)
    {
        var connectionId = Context.ConnectionId;
        rooms.Values.SelectMany(list => list)
            .ToList()
            .ForEach(user =>
            {
                user.Persent = 0;
                user.Rank = 0;
            });
        await GenerateText(idRoom);
        await SendMessageToClient(idRoom, connectionId, rooms.FirstOrDefault(x => x.Key == idRoom).Value, textOfRoom.FirstOrDefault(x => x.Key == idRoom).Value);
    }

    public async Task UpdatePercent(int currentWordIndex, string connectionId, string idRoom)
    {
        var text = textOfRoom.FirstOrDefault(x => x.Key == idRoom).Value;
        var curentPercent = PercentCompleted(currentWordIndex, text.Length);
        List<User> roomList = rooms[idRoom];
        var user = roomList.FirstOrDefault(item => item.ConnectionId == connectionId);
        if (user != null)
        {
            user.Persent = Convert.ToInt32(curentPercent);
        }
        if (user.Persent == 100)
        {
            var rank = roomList.OrderByDescending(x => x.Rank).FirstOrDefault();
            user.Rank = rank.Rank + 1;
        }

        await SendMessageToClient(idRoom, connectionId, roomList, text);
    }

    private async Task SendMessageToClient(string idRoom, string connectionId, List<User> users, string? text = null)
    {
        await Clients.Group(idRoom).SendAsync("ReceiveDataGame", new
        {
            idRoom,
            connectionId,
            usersOfRoom = users,
            text
        });
    }

    private async Task ReceiveError(string errorMsg)
    {
        await Clients.Caller.SendAsync("ReceiveError", errorMsg);
    }

    private double PercentCompleted(int count, int totalChars)
    {
        double percentage = (double)count / totalChars * 100;
        return percentage;
    }

    public async Task GenerateText(string idRoom)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiKey = "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";

            HttpClient httpClient = new HttpClient();
            string prompt = "I need text from 10 to 20 words with no special characters.";

            string apiUrl = "https://api.openai.com/v1/engines/text-davinci-003/completions";

            var requestBody = new
            {
                prompt,
                max_tokens = 500,
                temperature = 0.7,
                top_p = 1,
            };

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await httpClient.PostAsJsonAsync(apiUrl, requestBody);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            var objectData1 = JsonConvert.DeserializeObject<OpenAIResponseModel>(responseData);
            //var data = objectData1?.Choices.First().Text.Trim();
            var data = "This is example text for typing game.";
            if (rooms.ContainsKey(idRoom))
            {
                textOfRoom[idRoom] = data;
            }
            else
            {
                textOfRoom.Add(idRoom, data);
            }
        }
    }
}

public class OpenAIResponseModel
{
    public List<OpenAIChoice> Choices { get; set; }
}

public class OpenAIChoice
{
    public string Text { get; set; }
}
