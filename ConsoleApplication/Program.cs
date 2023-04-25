using Infrastructure.DbContext;
using Infrastructure.Models;

namespace ConsoleApplication;

public static class Program
{
    public static void Main()
    {
        var database = InitializeDB();
        Queries queries = new Queries(database);
        
        //1
        var maximumTravelTime = queries.GetMaximumTravelTime(1);
        Console.WriteLine($"Максимальний час подорожi мiж мiстами для потягу {queries.GetTrainById(1)}\n{maximumTravelTime}\n");

        //2
        var seatsInTrains = queries.GetNumberOfSeats();
        Console.WriteLine("Кiлькiсть мiсць у кожному потязi:");
        foreach (var pair in seatsInTrains)
        {
            Console.WriteLine($"{pair.Item1} : {pair.Item2} мiсць");
        }
        Console.WriteLine();
        
        //3
        var schedulesFromTown = queries.GetSchedulesFromTown("Zhytomyr");
        Console.WriteLine("Розклади з мiста Житомира:");
        foreach (var schedule in schedulesFromTown)
        {
            Console.WriteLine(
                $"Потяг {queries.GetTrainById(schedule.TrainId)} вiдправляється з " +
                $"{queries.GetTownById(schedule.TownFromId)} {schedule.DateTimeOfDeparture} i " +
                $"прибуває в {queries.GetTownById(schedule.TownToId)} {schedule.DateTimeOfArrival}");
        }

        Console.WriteLine();
        
        //4
        var smallestTimeSchedule = queries.GetSmallestTimeSchedule();
        Console.WriteLine("Розклад мiж мiстами, час поїздки мiж якими найменший");
        Console.WriteLine(  $"Потяг {queries.GetTrainById(smallestTimeSchedule.TrainId)} вiдправляється з " +
                            $"{queries.GetTownById(smallestTimeSchedule.TownFromId)} {smallestTimeSchedule.DateTimeOfDeparture} i " +
                            $"прибуває в {queries.GetTownById(smallestTimeSchedule.TownToId)} {smallestTimeSchedule.DateTimeOfArrival}");;
        Console.WriteLine();
     
        //5 

        var maxStopTimeTown = queries.GetMaxStopTimeTown();
        Console.WriteLine("Максимальний час зупинки");
        foreach (var element in maxStopTimeTown)
        {
            Console.WriteLine($"Потяг {element.Item1} стоїть у мiстi {element.Item2} протягом {element.Item3} хвилин.");
        }

        Console.WriteLine();
        //6 #TODO

        //7

        var atLeastOneLuxWagonTrain = queries.GetLuxTrains();
        Console.WriteLine("Потяги з хоча б одним люксовим вагоном");
        foreach (var luxTrain in atLeastOneLuxWagonTrain)
        {
            Console.WriteLine($"{luxTrain}");
        }

        Console.WriteLine();
        
        //8 
        
        DateTime date = new DateTime(2023, 04, 22);
        var distinctTrainsKyivAtDate = queries.GetKyivTrainsByDate(date);
        Console.WriteLine($"Потяги, якi прибувають в Київ {date.Day}.{date.Month}.{date.Year}");
        foreach (var train in distinctTrainsKyivAtDate)
        {
            Console.WriteLine(train);
        }
        
        //9
        Person person = queries.GetPersonById(1)!;
        var trainsByResponsible = queries.GetTrainsByResponsiblePerson(person);
        Console.WriteLine($"Потяги, за якi вiдповiдає людина {person}:");
        foreach (var train in trainsByResponsible)
        {
            Console.WriteLine(train);
        }

        Console.WriteLine();
        
        //10

        DateTime startTime = new DateTime(2023, 04, 21, 11, 30, 00);
        DateTime endTime = new DateTime(2023, 04, 22, 00, 30, 00);
        var amountOfTrainsInTimeRange =
            queries.GetAmountOfTrainsFromEachStationInParticularTimeRange(startTime, endTime);
        Console.WriteLine($"Кiлькiсть потягiв якi вiдправляються з кожної станцiї в промiжку з {startTime} по {endTime}: ");
        foreach (var townAndAmount in amountOfTrainsInTimeRange)
        {
            Console.WriteLine($"Мiсто:{townAndAmount.Item1} Кiлькiсть потягiв: {townAndAmount.Item2}");
        }

        Console.WriteLine();
        
        //11

        var trainAndPercentsOfEachWagonType = queries.GetPercentOfEachWagonType();
        Console.WriteLine("Потяги та спiввiдношення рiзних типiв вагонiв у них:");
        foreach (var trainAndPercents in trainAndPercentsOfEachWagonType)
        {
            Console.WriteLine(
                $"Потяг {trainAndPercents.Item1}, вiдсоток плацкартних вагонiв:" +
                $" {Math.Round(trainAndPercents.Item2*100, 2)}%, вiдсоток вагонiв-купе: {Math.Round(trainAndPercents.Item3*100, 2)}%, " +
                $"вiдсоток люксових вагонiв: {Math.Round(trainAndPercents.Item4*100, 2)}%");
        }

        Console.WriteLine();

        //12

        var trainAndPercentageOfEachSeatType = queries.GetPercentOfEachSeatsType();
        Console.WriteLine("Потяги та вiдсотки кожного типу мiсць:");
        foreach (var trainAndPercentage in trainAndPercentageOfEachSeatType)
        {
            Console.WriteLine($"Потяг {trainAndPercentage.Item1}, вiдсоток плацкартних мiсць:" +
                              $" {Math.Round(trainAndPercentage.Item2*100, 2)}%, вiдсоток мiсць-купе: {Math.Round(trainAndPercentage.Item3*100, 2)}%, " +
                              $"вiдсоток люксових мiсць: {Math.Round(trainAndPercentage.Item4*100, 2)}%");
        }

        Console.WriteLine();
        
        //13

        var trainsWithMoreThan5ReservedWagons = queries.GetTrainsWithMoreReservedWagonsThan(5);
        Console.WriteLine("Потяги з бiльшою кiлькiстю плацкартних вагонiв за 5:");
        foreach (var train in trainsWithMoreThan5ReservedWagons) 
        {
            Console.WriteLine(train);
        }

        Console.WriteLine();
        
        //14

        var trainWithMostSeats = queries.GetTrainWithMostAmountOfSeats();
        Console.WriteLine($"Потяг з найбiльщою кiлькiстю мiсць:\n{trainWithMostSeats}\n");
        
        //15

        var trainWithMostAmountOfStops = queries.GetTrainWithMostStops();
        Console.WriteLine($"Потяг з найбiльшою кiлькiстю зупинок:\n{trainWithMostAmountOfStops}\n");
        
        //16

        var townAndAmountThroughEachStation = queries.GetTrainAmountThroughEachStation();
        Console.WriteLine("Кiлькiсть потягiв з кожного мiста");
        foreach (var townAmount in townAndAmountThroughEachStation)
        {
            Console.WriteLine($"Мiсто: {townAmount.Item1} Кiлькiсть потягiв: {townAmount.Item2}");
        }

        Console.WriteLine();
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