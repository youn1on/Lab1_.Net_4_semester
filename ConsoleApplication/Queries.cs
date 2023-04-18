using Infrastructure.DbContext;
using Infrastructure.Models;

namespace ConsoleApplication;

public class Queries
{
    private DbContext _database;

    public Queries(DbContext database)
    {
        _database = database;
    }

    public Schedule GetMaximumTravelTime(int trainId)
    {
        return _database.Schedules.Where(schedule => schedule.TrainId == trainId)
            .MaxBy(schedule => schedule.DateTimeOfArrival - schedule.DateTimeOfDeparture)!;
    }

    public List<(int, int)> GetNumberOfSeats()
    {
        return _database.Wagons.GroupBy(wagon => wagon.InventaryNumberOfTrain)
            .Select(wagons => (wagons.Key, wagons.Count())).ToList();
    }

    public List<(int, int)> CountSchedulesFromEachTown()
    {
        return _database.Schedules.GroupBy(s => s.TownFromId)
            .Select(g => (g.Key, g.Count())).ToList();
    }

    public IEnumerable<Schedule> GetSchedulesFromTown(string townName)
    {
        Town? town = _database.Towns.FirstOrDefault(t => t.Name == townName);
        return _database.Schedules.Where(s => s.TownFromId == town?.Id)
            .OrderBy(s => s.DateTimeOfDeparture);
    }
    
}