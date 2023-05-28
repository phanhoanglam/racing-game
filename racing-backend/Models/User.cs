namespace racing_backend.Models;

public class User
{
    public string ConnectionId { get; set; }
    public string UserName { get; set; }
    public bool IsHost { get; set; } = false;
    public double Persent { get; set; } = 0;
    public double Wpm { get; set; }
    private int _rank;
    public string Rank
    {
        get
        {
            string suffix;
            switch (_rank % 10)
            {
                case 1:
                    suffix = "st";
                    break;
                case 2:
                    suffix = "nd";
                    break;
                case 3:
                    suffix = "rd";
                    break;
                default:
                    suffix = "th";
                    break;
            }
            return _rank + suffix;
        }
        set
        {
            if (value != null)
            { _rank = int.Parse(value); }
        }
    }
}
