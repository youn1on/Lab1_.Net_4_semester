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
    
    // 1. Знайдемо максимальний час подорожі між містами. 

    public TimeSpan GetMaximumTravelTime(int trainId)
    {
        return _database.Schedules.Where(schedule => schedule.TrainId == trainId).Select(schedule => schedule.DateTimeOfArrival - schedule.DateTimeOfDeparture)
            .Max()!;
    }

    // 2. Знайдемо кількість місць у потягах.
    public IEnumerable<(Train, int)> GetNumberOfSeats()
    {
        return _database.Wagons.GroupBy(wagon => wagon.InventaryNumberOfTrain)
            .Select(wagons => (GetTrainById(wagons.Key)!, wagons.Sum(w => w.AmountOfSeats)));
    }


    // 3. Знайдемо розклади з певного міста.
    public IEnumerable<Schedule> GetSchedulesFromTown(string townName)
    {
        Town? town = _database.Towns.FirstOrDefault(t => t.Name == townName);
        return _database.Schedules.Where(s => s.TownFromId == town?.Id)
            .OrderBy(s => s.DateTimeOfDeparture);
    }

    // 4. Знайдемо розклад між містами, час поїздки між якими найменший.
    public Schedule? GetSmallestTimeSchedule()
    {
        return _database.Schedules.MinBy(s => s.DateTimeOfArrival - s.DateTimeOfDeparture);
    }

    // 5. Знайдемо найдовший час зупинки, місто, в якому ця зупинка відбувається та відовідний потяг.
    public IEnumerable<(Train, Town, double)> GetMaxStopTimeTown() 
    {
        return _database.Schedules.GroupBy(s => s.TrainId)
            .Select(g => (GetTrainById(g.Key),
                g.Select(s => (s, g.Where(s2 => s2.TownFromId == s.TownToId && s2.DateTimeOfDeparture.Subtract(s.DateTimeOfArrival) >= new TimeSpan()).MinBy(s2 => s2.DateTimeOfDeparture.Subtract(s.DateTimeOfArrival)))).Select(p =>
                        (p.Item1, p.Item2,
                            p.Item2?.DateTimeOfDeparture.Subtract(p.Item1.DateTimeOfArrival).TotalMinutes))
                    .MaxBy(p => p.Item3))).Select(i =>
                (i.Item1!, GetTownById(i.Item2.Item1.TownToId)!, i.Item2.TotalMinutes!.Value));
    }
    
    // 6. Отримаємо найближчі розклади між двома містами.
    
    public IEnumerable<(Train, Schedule, Schedule)> GetClosestSchedulesBetweenTwoTowns(String A, String B)
    {
        return _database.Schedules.Where(s =>
                _database.Schedules.Where(sch => GetTownById(sch.TownFromId)?.Name == A).Select(sch => sch.TrainId)
                    .Intersect(_database.Schedules.Where(sch => GetTownById(sch.TownToId)?.Name == B)
                        .Select(sch => sch.TrainId)).Contains(s.TrainId) &&
                s.DateTimeOfDeparture > DateTime.Now &&
                (GetTownById(s.TownFromId)!.Name == A || GetTownById(s.TownToId)!.Name == B))
            .GroupBy(sch => sch.TrainId).Select(g => g.Where(s => GetTownById(s.TownFromId)!.Name == A).Select(s =>
                    (GetTrainById(g.Key)!, s,
                        g.Where(s2 =>
                                GetTownById(s2.TownToId)!.Name == B && s2.DateTimeOfArrival > s.DateTimeOfDeparture)
                            .MinBy(s2 => s2.DateTimeOfArrival.Subtract(s.DateTimeOfDeparture))))
                .Where(p => p.Item3 is not null).Select(p => (p.Item1, p.s, p.Item3!))).Where(ien => ien.Any())
            .Select(ien => (ien, ien.Select(p => p.Item3.DateTimeOfArrival.Subtract(p.s.DateTimeOfDeparture)).Min()))
            .Select(ienTimePair =>
                ienTimePair.Item1
                    .Where(p => p.Item3.DateTimeOfArrival.Subtract(p.s.DateTimeOfDeparture) <= ienTimePair.Item2)
                    .MinBy(p => p.s.DateTimeOfDeparture));
    }

    // 7. Знайдемо потяги з хоча б одним люксовим вагоном.

    public IEnumerable<Train> GetLuxTrains()
    {
        return _database.Wagons.Where(w => w.Type == WagonType.Sleeping)
            .Select(w => GetTrainById(w.InventaryNumberOfTrain)!).Distinct();
    }
    
    // 8. Вибираємо унікальні потяги, які прибувають в Київ у вказану дату.

    public IEnumerable<Train> GetKyivTrainsByDate(DateTime date)
    {
        return (from t1 in _database.Schedules
            join t2 in _database.Towns on t1.TownToId equals t2.Id
            where t1.DateTimeOfArrival.Date == date.Date
                  && t2.Name == "Kyiv"
            select GetTrainById(t1.TrainId))!.Distinct();
    }

    // 9. Знайдемо потяги по відповідальній людині.

    public IEnumerable<Train> GetTrainsByResponsiblePerson(Person person)
    {
        return (from t1 in _database.Trains
            join t2 in _database.ResponsiblePeople on t1.ResponsiblePersonId equals t2.Id
            where $"{t2.Name} {t2.Surname}" == person.Name + " " + person.Surname
            select GetTrainById(t1.Id));
    }
    
    // 10. Знайдемо кількість потягів які відправляються з кожної станції в певному часовому проміжку.
    public IEnumerable<(Town, int)> GetAmountOfTrainsFromEachStationInParticularTimeRange(DateTime startTime, DateTime endTime)
    {
        return from s in _database.Schedules
            where s.DateTimeOfDeparture >= startTime
                  && s.DateTimeOfDeparture <= endTime
            group s by s.TownFromId into g
            select ( GetTownById(g.Key)!, g.Count() );
    }
    
    // 11. Отримаємо співвідношення вагонів кожного типу для кожного потягу.

    public IEnumerable<(Train, double, double, double)> GetPercentOfEachWagonType()
    {
        return _database.Wagons.GroupBy(w => w.InventaryNumberOfTrain)
            .Select(g => (
                GetTrainById(g.Key)!,
                (double) g.Count(w => w.Type is WagonType.Reserved) / g.Count(),
                (double) g.Count(w => w.Type == WagonType.Coupe) / g.Count(),
                (double) g.Count(w => w.Type == WagonType.Sleeping) / g.Count()
            ));

    }
    
    // 12. Отримаємо співвідношення місць кожного типу для кожного потягу.
    
    public IEnumerable<(Train, double, double, double)> GetPercentOfEachSeatsType()
    {
        return _database.Wagons.GroupBy(w => w.InventaryNumberOfTrain)
            .Select(g => (
                    GetTrainById(g.Key)!,
                    (double)g.Where(w => w.Type == WagonType.Reserved).Sum(w => w.AmountOfSeats) /
                    g.Sum(w => w.AmountOfSeats),
                    (double)g.Where(w => w.Type == WagonType.Coupe).Sum(w => w.AmountOfSeats) /
                    g.Sum(w => w.AmountOfSeats),
                    (double)g.Where(w => w.Type == WagonType.Sleeping).Sum(w => w.AmountOfSeats) /
                    g.Sum(w => w.AmountOfSeats)
                )
            );
    }
    
    // 13. Знайдемо потяги, в яких більше n-ої кількості плацкартних вагонів.

    public IEnumerable<Train> GetTrainsWithMoreReservedWagonsThan(int n)
    {
        return _database.Trains.Where(t => _database.Wagons.Count(w => w.InventaryNumberOfTrain == t.Id) > n);
    }

    // 14. Знайдемо потяг з найбільшою кількістю місць.

    public Train GetTrainWithMostAmountOfSeats() 
    {
        return _database.Wagons.GroupBy(wagon => wagon.InventaryNumberOfTrain)
            .Select(wagons => (GetTrainById(wagons.Key), wagons.Sum(w => w.AmountOfSeats)))
            .MaxBy(p => p.Item2).Item1!;
    }
    
    // 15. Знайдемо потяг з найбільшою кількістю зупинок.

    public Train GetTrainWithMostStops()
    {
        return _database.Schedules.GroupBy(schedule => schedule.TrainId)
                .Select(g => (GetTrainById(g.Key)!, g.Select(el => el.TownToId).Distinct().Count()))
                .MaxBy(t => t.Item2)
                .Item1;
    }
    
    
    // 16. Знайдемо, скільки потягів проходять через станцію.

    public IEnumerable<(Town, int)> GetTrainAmountThroughEachStation()
    {
        return _database.Schedules.GroupBy(schedule => schedule.TownFromId)
            .Select(g => (GetTownById(g.Key)!, g.Select(el => el.TrainId).Distinct().Count()));
    }

    // 17. Список міст де наразі потяги не курсують.

    public IEnumerable<Town?> GetInactiveTowns()
    {
        return _database.Schedules.GroupBy(s => s.TownFromId).Where(g => !g.Any())
            .Select(g => GetTownById(g.Key));
    }

    // 18. Знайдемо відповідальних осіб, в яких ім'я починається на "M" та виведемо потяги, за які вони відповідають.

    public IEnumerable<(Person, Train)> GetResponsiblePersonStartsWithM()
    {
        return _database.Trains.Where(t => GetPersonById(t.ResponsiblePersonId)!.Name.StartsWith("М"))
            .Select(t => (GetPersonById(t.ResponsiblePersonId)!, t));
    }
    
    // 18. Отримаємо всі міста, через які їде певний потяг.
    
    public IEnumerable<Town> GetTownsByTrain(Train train)
    {
        return _database.Schedules.Where(s => s.TrainId == train.Id).Select(s => GetTownById(s.TownToId)!).Distinct();
    }

    // 19. Середній час подорожі між містами для одного потягу.

    public double GetAverageTimeBetweenCitiesForTrain(Train train)
    {
        return _database.Schedules.Where(s => s.TrainId == train.Id)
            .Average(s => (s.DateTimeOfArrival - s.DateTimeOfDeparture).TotalMinutes);
    }
    
    // 20. Знайдемо людину, відповідальну за потяг з найбільшою кількістю вагонів.

    public Person GetPersonResponsibleForMaxWagonsTrain()
    {
        return _database.ResponsiblePeople
            .FirstOrDefault(p => p.Id == _database.Trains.MaxBy(t => t.AmountOfWagons)!.ResponsiblePersonId)!;
    }
    
    public Town? GetTownById(int id)
    {
        return _database.Towns.FirstOrDefault(t => t.Id == id);
    }
    
    public Train? GetTrainById(int id)
    {
        return _database.Trains.FirstOrDefault(t => t.Id == id);
    }
    
    public Person? GetPersonById(int id)
    {
        return _database.ResponsiblePeople.FirstOrDefault(t => t.Id == id);
    }
    
    public Wagon? GetWagonById(int id)
    {
        return _database.Wagons.FirstOrDefault(t => t.Id == id);
    }
    
    public Schedule? GetScheduleById(int id)
    {
        return _database.Schedules.FirstOrDefault(t => t.Id == id);
    }
    
}