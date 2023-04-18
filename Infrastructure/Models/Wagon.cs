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

}