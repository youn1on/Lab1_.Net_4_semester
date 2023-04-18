using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Person : IDbModel
{
    public int Id { get; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }

    public Person(int id, string name, string surname, string patronymic)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    public Person(string csvString)
    {
        string[] data = csvString.Split(',');
        if (data.Length != 4 || !int.TryParse(data[0], out int id))
            throw new ArgumentException("Incorrect persons csv");
        Id = id;
        Surname = data[1];
        Name = data[2];
        Patronymic = String.IsNullOrEmpty(data[3]) ? null : data[3];
    }
    
    public override string ToString()
    {
        return String.Format("Responsible person: {0} {1} {2} responsible person Id: {3}", Surname, Name, Patronymic, Id);
    }
}