using System.Diagnostics.Metrics;
using APBD3;

public class Container
{
    public float LoadMass { get; set; }
    public float Height { get; set; }
    public float NetMass { get; set; }
    public float Depth { get; set; }
    public string SerialNumber { get; set; }
    public float MaxCapacity { get; set; }
    public static int Counter { get; set; } = 0;

    public Container(float height, float netMass, float depth, float maxCapacity, string containerType)
    {
        Height = height;
        NetMass = netMass;
        Depth = depth;
        MaxCapacity = maxCapacity;
        SerialNumber = $"KON-{containerType}-{Counter++}";
        Console.WriteLine($"Created {SerialNumber} ({containerType})");
    }


    public virtual void EmptyContainer()
    {
        LoadMass = 0;
    }

    public virtual void LoadContainer(float mass)
    {
        if (mass+LoadMass > MaxCapacity)
        {
            throw new OverfillException("Max capacity exceeded");
        }

        LoadMass += mass;
        Console.WriteLine($"Loaded {SerialNumber} with {mass}kg");
    }

    public virtual string GetContainerInfo()
    {
        return $"Container {SerialNumber}:\n" +
               $"- Type: {SerialNumber.Split('-')[1]}\n" +
               $"- Dimensions: H:{Height}cm x D:{Depth}cm\n" +
               $"- Net Mass: {NetMass}kg\n" +
               $"- Load Mass: {LoadMass}kg / {MaxCapacity}kg\n";
    }
}