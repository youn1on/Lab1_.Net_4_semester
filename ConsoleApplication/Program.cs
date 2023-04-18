using Infrastructure.DbContext;

namespace ConsoleApplication;

public static class Program
{
    public static void Main()
    {
        var database = InitializeDB();
    }

    public static DbContext InitializeDB()
    {
        var persons = DbContextFiller.GetPersons();
        var schedules = DbContextFiller.GetSchedules();
        var towns = DbContextFiller.GetTowns();
        var trains = DbContextFiller.GetTrains();
        var wagons = DbContextFiller.GetWagons();

        return new DbContext(persons, schedules, towns, trains, wagons);
    } 
}