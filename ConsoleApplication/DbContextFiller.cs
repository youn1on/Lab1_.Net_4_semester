using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace ConsoleApplication;

public static class DbContextFiller
{
    private const string PersonsSource = "data/Persons.csv";
    private const string TrainsSource = "data/Trains.csv";
    private const string SchedulesSource = "data/Schedules.csv";
    private const string TownsSource = "data/Towns.csv";
    private const string WagonsSource = "data/Wagons.csv";

    private static List<T> GetModels<T>(string sourcePath, Func<string, T> parserFunc) where T : IDbModel
    {
        using StreamReader sr = new StreamReader(sourcePath);
        List<T> data = new List<T>();
        foreach (var csvLine in sr.ReadToEnd().Split('\n'))
        {
            data.Add(parserFunc(csvLine));
        }

        return data;
    }

    public static List<Person> GetPersons()
    {
        return GetModels(PersonsSource, csvLine => new Person(csvLine));
    }
    
    public static List<Schedule> GetSchedules()
    {
        return GetModels(SchedulesSource, csvLine => new Schedule(csvLine));
    }
    
    public static List<Town> GetTowns()
    {
        return GetModels(TownsSource, csvLine => new Town(csvLine));
    }
    
    public static List<Train> GetTrains()
    {
        return GetModels(TrainsSource, csvLine => new Train(csvLine));
    }
    
    public static List<Wagon> GetWagons()
    {
        return GetModels(WagonsSource, csvLine => new Wagon(csvLine));
    }
}