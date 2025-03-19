namespace APBD3;

public class LiquidContainer : Container, IHazardNotifier
{
    private bool isHazardous;

    public LiquidContainer(float height, float netMass, float depth, float maxCapacity, bool isHazardous) : base(height,
        netMass, depth,
        maxCapacity, "L")
    {
        this.isHazardous = isHazardous;
    }

    public void NotifyAboutHazard()
    {
        Console.WriteLine($"Niebezpieczna sytuacja w kontenerze z płynem ${SerialNumber}");
    }

    public override void LoadContainer(float mass)
    {
        if (mass + LoadMass > MaxCapacity)
        {
            throw new OverfillException("Max capacity exceeded");
        }

        if ((isHazardous && mass > 0.5 * MaxCapacity) || mass > 0.9 * MaxCapacity)
        {
            NotifyAboutHazard();
            isHazardous = true;
        }

        LoadMass += mass;
        Console.WriteLine($"Loaded {SerialNumber} with {mass}kg");
    }

    public override string GetContainerInfo()
    {
        return base.GetContainerInfo() + "\n" +
               $"- Hazardous Cargo: {(isHazardous ? "Yes" : "No")}";
    }
}