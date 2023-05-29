namespace racing_backend.Models;

public class User
{
    public string ConnectionId { get; set; }
    public string UserName { get; set; }
    public bool IsHost { get; set; } = false;
    public double Persent { get; set; } = 0;
    public double Wpm { get; set; }
    private int _rank = 0;
    public int Rank
    {
        get { return _rank; }
        set
        {
            _rank = value;
            CalculateRankDisplay(value);
        }
    }
    public string? RankDisplay { get; set; }

    private void CalculateRankDisplay(int? value)
    {
        string suffix;
        switch (value)
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
        RankDisplay = value + suffix;
    }
}
