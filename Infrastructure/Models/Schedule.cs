using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Schedule : IDbModel
{
    public int Id { get; }
    public int TownFromId { get; set; }
    public int TownToId { get; set; }
    public DateTime DateTimeOfDeparture { get; set; }
    public DateTime DateTimeOfArrival { get; set; }
    public int TrainId { get; set; }

    public Schedule(int id, int townFromId, int townToId, DateTime dateTimeOfDeparture, 
        DateTime dateTimeOfArrival, int trainId)
    {
        Id = id;
        TownFromId = townFromId;
        TownToId = townToId;
        DateTimeOfDeparture = dateTimeOfDeparture;
        DateTimeOfArrival = dateTimeOfArrival;
        TrainId = trainId;
    }
    
    public override string ToString()
    {
        return String.Format("Schedule id: {0} The train departs from TownId{1} on {2} at {3} and arrives to TownId{4}" +
                             " on {5} at {6}", Id, TownFromId, DateTimeOfDeparture.Date,
            DateTimeOfDeparture.TimeOfDay, TownToId, DateTimeOfArrival.Date, DateTimeOfArrival.TimeOfDay);
    }

    public Schedule(string csvLine)
    {
        string[] data = csvLine.Split(',');
        if (data.Length != 5 || !int.TryParse(data[0], out int id)  || !int.TryParse(data[1], out int from)
            || !int.TryParse(data[2], out int to)  || !DateTime.TryParse(data[3], out DateTime departure) 
            || !DateTime.TryParse(data[4], out DateTime arrival) || !int.TryParse(data[5], out int train))
            throw new ArgumentException("Incorrect schedules csv");
        Id = id;
        TownFromId = from;
        TownToId = to;
        DateTimeOfDeparture = departure;
        DateTimeOfArrival = arrival;
        TrainId = train;
    }
}