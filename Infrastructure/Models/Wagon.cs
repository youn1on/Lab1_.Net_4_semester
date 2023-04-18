using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Wagon : IDbModel
{
    public int Id { get; }
    public int InventaryNumberOfTrain { get; set; }
    public WagonType Type { get; }
    public int AmountOfSeats { get; }

    public Wagon(int id, int inventaryNumberOfTrain, WagonType type, int amountOfSeats)
    {
        Id = id;
        InventaryNumberOfTrain = inventaryNumberOfTrain;
        Type = type;
        AmountOfSeats = amountOfSeats;
    }

    public Wagon(string csvLine)
    {
        string[] data = csvLine.Split(',');
        if (data.Length != 4 || !int.TryParse(data[0], out int id) || !int.TryParse(data[1], out int inventary)
            || !Enum.TryParse(data[2], out WagonType type) || !int.TryParse(data[3], out int seats))
            throw new ArgumentException("Incorrect persons csv");
        Id = id;
        InventaryNumberOfTrain = inventary;
        Type = type;
        AmountOfSeats = seats;
    }
}