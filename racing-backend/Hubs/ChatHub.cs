using System.Text;
using Microsoft.AspNetCore.SignalR;
using racing_backend.Models;

namespace racing_backend.Hubs;

public class ChatHub : Hub
{
    private readonly HttpClient _httpClient = new HttpClient();
    //private string Text = "Sometimes you are too ashamed to leave. That was true now. And sometimes you need too much to know the facts, and so humbly and stupidly you stay.";
    private string Text = "Sometimes you are too ashamed to leave.";
    private static Dictionary<string, List<User>> rooms = new Dictionary<string, List<User>>();
    public async Task CreateRoom(string userName)
    {
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
                user.Rank = null;
            });
        await SendMessageToClient(idRoom, connectionId, rooms.FirstOrDefault(x => x.Key == idRoom).Value, Text);
    }

    public async Task UpdatePercent(int currentWordIndex, string connectionId, string idRoom)
    {
        var curentPercent = PercentCompleted(currentWordIndex);
        //List<User> roomList = rooms[idRoom];
        //int index = roomList.FindIndex(item => item.ConnectionId == connectionId);
        //roomList[index] = (roomList[index].Item1, roomList[index].Item2, Convert.ToInt32(curentPercent));
        //rooms[idRoom] = roomList;
        List<User> roomList = rooms[idRoom];
        var user = roomList.FirstOrDefault(item => item.ConnectionId == connectionId);
        if (user != null)
        {
            user.Persent = Convert.ToInt32(curentPercent);
        }

        //await Clients.Group(idRoom).SendAsync("UpdatedPercent", new
        //{
        //    usersOfRoom = rooms.FirstOrDefault(x => x.Key == idRoom).Value,
        //    connectionId
        //});
        await SendMessageToClient(idRoom, connectionId, rooms.FirstOrDefault(x => x.Key == idRoom).Value, Text);
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

    private double PercentCompleted(int count)
    {
        int totalChars = Text.Length;
        double percentage = (double)count / totalChars * 100;
        return percentage;
    }

    public async Task<string> GenerateText()
    {
        try
        {
            string apiUrl = "https://api.openai.com/v1/engines/text-davinci-003/completions";
            string prompt = "Generate a text with meaningful content.";
            int maxTokens = 30;
            int n = 1;

            // Gửi yêu cầu API
            var requestBody = new
            {
                prompt = prompt,
                max_tokens = maxTokens,
                n = n
            };

            string requestBodyJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer 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");
            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = System.Text.Json.JsonSerializer.Deserialize<dynamic>(responseContent);
                string generatedText = jsonResponse.choices[0].text;

                return generatedText;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
