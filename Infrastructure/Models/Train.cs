using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Train : IDbModel
{
    public int InventaryNumber { get; }
    public int ResponsiblePersonId { get; set; } 
    public string Number { get; set; }
    public int AmountOfWagons { get; set; }
    
    public Train(int inventaryNumber, int responsiblePersonId, string number, int amountOfWagons)
    {
        InventaryNumber = inventaryNumber;
        ResponsiblePersonId = responsiblePersonId;
        Number = number;
        AmountOfWagons = amountOfWagons;
    }

    public override string ToString()
    {
        return String.Format("Train:  {0} \"{1}\" responsible person Id: {2} amount of wagons: {3}",
            Number, InventaryNumber, ResponsiblePersonId, AmountOfWagons);
    }

    public int Id => InventaryNumber;
}

