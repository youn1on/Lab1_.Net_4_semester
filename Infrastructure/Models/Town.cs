using System.Runtime.InteropServices.ComTypes;
using Infrastructure.Interfaces;

namespace Infrastructure.Models;

public class Town : IDbModel
{
    public int Id { get; }
    public string Name { get; set; }

    public Town(int id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public override string ToString()
    {
        return String.Format("Town: {0} Town Id: {1}", Name, Id);
    }
}