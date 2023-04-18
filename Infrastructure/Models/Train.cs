using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Train : IDbModel
{
    public int InventaryNumber { get; }
    public int ResponsiblePersonId { get; set; } 
    public string TrainNumber { get; set; }
    public int AmountOfWagons { get; set; }
    
    public Train(int inventaryNumber, int responsiblePersonId, string trainNumber, int amountOfWagons)
    {
        InventaryNumber = inventaryNumber;
        ResponsiblePersonId = responsiblePersonId;
        TrainNumber = trainNumber;
        AmountOfWagons = amountOfWagons;
    }

    public override string ToString()
    {
        return String.Format("Train:  {0} \"{1}\" responsible person Id: {2} amount of wagons: {3}",
            TrainNumber, InventaryNumber, ResponsiblePersonId, AmountOfWagons);
    }

    public int Id => InventaryNumber;

    public Train(string csvLine)
    {
        string[] data = csvLine.Split(',');
        if (data.Length != 4 || !int.TryParse(data[0], out int id) || !int.TryParse(data[1], out int responsible) 
            || !int.TryParse(data[3], out int wagons))
            throw new ArgumentException("Incorrect train csv");
        InventaryNumber = id;
        ResponsiblePersonId = responsible;
        TrainNumber = data[2];
        AmountOfWagons = wagons;
    }
}

