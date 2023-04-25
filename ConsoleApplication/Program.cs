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
        Console.WriteLine(
            $"Максимальний час подорожi мiж мiстами для потягу {queries.GetTrainById(1)}:\n\t{maximumTravelTime.Hours}h {maximumTravelTime.Minutes}m\n");

        //2
        var seatsInTrains = queries.GetNumberOfSeats();
        Console.WriteLine("Кiлькiсть мiсць у кожному потязi:");
        foreach (var pair in seatsInTrains)
        {
            Console.WriteLine($"\t{pair.Item1} : {pair.Item2} мiсць");
        }

        Console.WriteLine();

        //3
        var schedulesFromTown = queries.GetSchedulesFromTown("Zhytomyr");
        Console.WriteLine("Розклади з мiста Житомира:");
        foreach (var schedule in schedulesFromTown)
        {
            Console.WriteLine(
                $"\tПотяг {queries.GetTrainById(schedule.TrainId)} вiдправляється з " +
                $"{queries.GetTownById(schedule.TownFromId)} {schedule.DateTimeOfDeparture} i " +
                $"прибуває в {queries.GetTownById(schedule.TownToId)} {schedule.DateTimeOfArrival}");
        }

        Console.WriteLine();

        //4
        var smallestTimeSchedule = queries.GetSmallestTimeSchedule();
        Console.WriteLine("Розклад мiж мiстами, час поїздки мiж якими найменший:");
        Console.WriteLine($"\tПотяг {queries.GetTrainById(smallestTimeSchedule.TrainId)} вiдправляється з " +
                          $"{queries.GetTownById(smallestTimeSchedule.TownFromId)} {smallestTimeSchedule.DateTimeOfDeparture} i " +
                          $"прибуває в {queries.GetTownById(smallestTimeSchedule.TownToId)} {smallestTimeSchedule.DateTimeOfArrival}");
        ;
        Console.WriteLine();

        //5 

        var maxStopTimeTown = queries.GetMaxStopTimeTown();
        Console.WriteLine("Максимальний час зупинки:");
        foreach (var element in maxStopTimeTown)
        {
            Console.WriteLine($"\tПотяг {element.Item1} стоїть у мiстi {element.Item2} протягом {element.Item3} хвилин.");
        }

        Console.WriteLine();
        
        //6

        var closestSchedulesBetweenTowns =
            queries.GetClosestSchedulesBetweenTwoTowns("Berdychiv", "Fastiv");
        Console.WriteLine("Найближчi розклади мiж Бердичевом та Фастовом:");
        foreach (var tuple in closestSchedulesBetweenTowns)
        {
            Console.WriteLine($"\tПотяг {tuple.Item1} вiдправиться з мiста {queries.GetTownById(tuple.Item2.TownFromId)}" +
                              $" {tuple.Item2.DateTimeOfDeparture} i прибуде у {queries.GetTownById(tuple.Item3.TownToId)}" +
                              $" {tuple.Item3.DateTimeOfArrival}");
        }
        Console.WriteLine();

        //7

        var atLeastOneLuxWagonTrain = queries.GetLuxTrains();
        Console.WriteLine("Потяги з хоча б одним люксовим вагоном:");
        foreach (var luxTrain in atLeastOneLuxWagonTrain)
        {
            Console.WriteLine($"\t{luxTrain}");
        }

        Console.WriteLine();

        //8 

        DateTime date = new DateTime(2023, 04, 29);
        var distinctTrainsKyivAtDate = queries.GetKyivTrainsByDate(date);
        Console.WriteLine($"Потяги, якi прибувають в Київ {date.Day}.{date.Month}.{date.Year}");
        foreach (var train in distinctTrainsKyivAtDate)
        {
            Console.WriteLine("\t"+train);
        }

        Console.WriteLine();

        //9
        Person person = queries.GetPersonById(1)!;
        var trainsByResponsible = queries.GetTrainsByResponsiblePerson(person);
        Console.WriteLine($"Потяги, за якi вiдповiдає людина {person}:");
        foreach (var train in trainsByResponsible)
        {
            Console.WriteLine("\t"+train);
        }

        Console.WriteLine();

        //10

        DateTime startTime = new DateTime(2023, 04, 28, 11, 30, 00);
        DateTime endTime = new DateTime(2023, 04, 29, 00, 30, 00);
        var amountOfTrainsInTimeRange =
            queries.GetAmountOfTrainsFromEachStationInParticularTimeRange(startTime, endTime);
        Console.WriteLine(
            $"Кiлькiсть потягiв якi вiдправляються з кожної станцiї в промiжку з {startTime} по {endTime}: ");
        foreach (var townAndAmount in amountOfTrainsInTimeRange)
        {
            Console.WriteLine($"\tМiсто:{townAndAmount.Item1} Кiлькiсть потягiв: {townAndAmount.Item2}");
        }

        Console.WriteLine();

        //11

        var trainAndPercentsOfEachWagonType = queries.GetPercentOfEachWagonType();
        Console.WriteLine("Потяги та спiввiдношення рiзних типiв вагонiв у них:");
        foreach (var trainAndPercents in trainAndPercentsOfEachWagonType)
        {
            Console.WriteLine(
                $"\tПотяг {trainAndPercents.Item1}, вiдсоток плацкартних вагонiв:" +
                $" {Math.Round(trainAndPercents.Item2 * 100, 2)}%, вiдсоток вагонiв-купе: {Math.Round(trainAndPercents.Item3 * 100, 2)}%, " +
                $"вiдсоток люксових вагонiв: {Math.Round(trainAndPercents.Item4 * 100, 2)}%");
        }

        Console.WriteLine();

        //12

        var trainAndPercentageOfEachSeatType = queries.GetPercentOfEachSeatsType();
        Console.WriteLine("Потяги та вiдсотки кожного типу мiсць:");
        foreach (var trainAndPercentage in trainAndPercentageOfEachSeatType)
        {
            Console.WriteLine($"\tПотяг {trainAndPercentage.Item1}, вiдсоток плацкартних мiсць:" +
                              $" {Math.Round(trainAndPercentage.Item2 * 100, 2)}%, вiдсоток мiсць-купе: {Math.Round(trainAndPercentage.Item3 * 100, 2)}%, " +
                              $"вiдсоток люксових мiсць: {Math.Round(trainAndPercentage.Item4 * 100, 2)}%");
        }

        Console.WriteLine();

        //13

        var trainsWithMoreThan5ReservedWagons = queries.GetTrainsWithMoreReservedWagonsThan(5);
        Console.WriteLine("Потяги з кiлькiстю плацкартних вагонiв бiльшою за 5:");
        foreach (var train in trainsWithMoreThan5ReservedWagons)
        {
            Console.WriteLine("\t"+train);
        }

        Console.WriteLine();

        //14

        var trainWithMostSeats = queries.GetTrainWithMostAmountOfSeats();
        Console.WriteLine($"Потяг з найбiльщою кiлькiстю мiсць:\n\t{trainWithMostSeats}\n");

        //15

        var trainWithMostAmountOfStops = queries.GetTrainWithMostStops();
        Console.WriteLine($"Потяг з найбiльшою кiлькiстю зупинок:\n\t{trainWithMostAmountOfStops}\n");

        //16

        var townAndAmountThroughEachStation = queries.GetTrainAmountThroughEachStation();
        Console.WriteLine("Кiлькiсть потягiв з кожного мiста");
        foreach (var townAmount in townAndAmountThroughEachStation)
        {
            Console.WriteLine($"\tМiсто: {townAmount.Item1} Кiлькiсть потягiв: {townAmount.Item2}");
        }

        Console.WriteLine();

        //17

        var inactiveTowns = queries.GetInactiveTowns();
        Console.WriteLine("Мiста, в яких потяги наразi не курсують:");
        if (!inactiveTowns.Any())
        {
            Console.WriteLine("\tТаких мiст наразi немає.");
        }

        foreach (var town in inactiveTowns)
        {
            Console.WriteLine("\t"+town);
        }

        Console.WriteLine();

        //18
        var personStartsWithMAndTrainResponsibleFor = queries.GetResponsiblePersonStartsWithM();
        Console.WriteLine("Отримаємо людей, iм`я яких починається на \"М\" та потяги, за якi вони вiдповiдають");
        foreach (var personTrain in personStartsWithMAndTrainResponsibleFor)
        {
            Console.WriteLine($"\t{personTrain.Item1} ({personTrain.Item2})\n");
        }
        Console.WriteLine();
        
        //19
        
        Train train2 = queries.GetTrainById(2);
        var townsTrainGoesThrough = queries.GetTownsByTrain(train2);
        Console.WriteLine($"Мiста, через якi проходить потяг: {train2}");
        foreach (var town in townsTrainGoesThrough)
        {
            Console.WriteLine("\t"+town);
        }
        Console.WriteLine();
        
        //20

        var personResponsibleForMaxWagons = queries.GetPersonResponsibleForMaxWagonsTrain();
        Console.WriteLine($"Людина, вiдповiдальна за найбiльшу кiлькiсть вагонiв:\n\t{personResponsibleForMaxWagons}\n");
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