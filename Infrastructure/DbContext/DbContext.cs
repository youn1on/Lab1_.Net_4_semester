using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.DbContext;

public class DbContext
{
    public List<Person> ResponsiblePeople;
    public List<Schedule> Schedules;
    public List<Town> Towns;
    public List<Train> Trains;
    public List<Wagon> Wagons;


    public DbContext(List<Person> responsiblePeople, List<Schedule> schedules, List<Town> towns, List<Train> trains, List<Wagon> wagons)
    {
        ResponsiblePeople = responsiblePeople;
        Schedules = schedules;
        Towns = towns;
        Trains = trains;
        Wagons = wagons;
    }
    
    public DbContext()
    {
        ResponsiblePeople = new();
        Schedules = new();
        Towns = new();
        Trains = new();
        Wagons = new();
    }
}