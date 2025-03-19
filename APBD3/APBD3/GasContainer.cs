namespace APBD3;

public class GasContainer : Container, IHazardNotifier
{
    public float Pressure { get; set; }

    public GasContainer(float height, float netMass, float depth, float maxCapacity, float pressure) : base(height,
        netMass, depth,
        maxCapacity, "G")
    {
        Pressure = pressure;
    }

    public void NotifyAboutHazard()
    {
        Console.WriteLine($"Niebezpieczna sytuacja w kontenerze z gazem ${SerialNumber}");
    }
    public override void EmptyContainer()
    {
        LoadMass *= 0.05f;
    }
    public override string GetContainerInfo()
    {
        return base.GetContainerInfo() + $"\n" +
               $"- Pressure: {Pressure} atm";
    }
}