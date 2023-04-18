using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Schedule : IDbModel
{
    public int Id { get; }
    public int TownFromId { get; set; }
    public int TownToId { get; set; }
    public DateTime DateTimeOfDeparture { get; set; }
    public DateTime DateTimeOfArrival { get; set; }

    public Schedule(int id, int townFromId, int townToId, DateTime dateTimeOfDeparture, 
        DateTime dateTimeOfArrival)
    {
        Id = id;
        TownFromId = townFromId;
        TownToId = townToId;
        DateTimeOfDeparture = dateTimeOfDeparture;
        DateTimeOfArrival = dateTimeOfArrival;
    }
    
    public override string ToString()
    {
        return String.Format("Schedule id: {0} The train departs from TownId{1} on {2} at {3} and arrives to TownId{4}" +
                             " on {5} at {6}", Id, TownFromId, DateTimeOfDeparture.Date,
            DateTimeOfDeparture.TimeOfDay, TownToId, DateTimeOfArrival.Date, DateTimeOfArrival.TimeOfDay);
    }
}