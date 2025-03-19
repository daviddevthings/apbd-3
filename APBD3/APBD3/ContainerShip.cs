namespace APBD3;

public class ContainerShip
{
    public List<Container> transportedContainers { get; set; } = new();
    public float maxSpeed { get; set; }
    public int maxNumberOfContainers { get; set; }
    public float maxTotalContainerMass { get; set; }

    public ContainerShip(float maxSpeed, int maxNumberOfContainers, float maxTotalContainerMass)
    {
        this.maxSpeed = maxSpeed;
        this.maxNumberOfContainers = maxNumberOfContainers;
        this.maxTotalContainerMass = maxTotalContainerMass;
    }

    public void LoadNewContainer(Container container)
    {
        var totalMass = transportedContainers.Sum(container1 => container1.NetMass + container1.LoadMass);
        if (totalMass + container.NetMass + container.LoadMass > maxTotalContainerMass * 1000)
        {
            throw new OverfillException("Total mass exceeded");
        }

        if (transportedContainers.Count + 1 > maxNumberOfContainers)
        {
            throw new OverfillException("Too many containers");
        }

        transportedContainers.Add(container);
    }

    public void LoadNewContainers(List<Container> containers)
    {
        var totalMass = transportedContainers.Sum(container => container.NetMass + container.LoadMass);
        var newContainersTotalMass = containers.Sum(container => container.NetMass + container.LoadMass);

        if (totalMass + newContainersTotalMass > maxTotalContainerMass * 1000)
        {
            throw new OverfillException("Total mass exceeded");
        }

        if (transportedContainers.Count + containers.Count > maxNumberOfContainers)
        {
            throw new OverfillException("Too many containers");
        }

        transportedContainers.AddRange(containers);
    }

    public void RemoveContainer(string containerSerialNumber)
    {
        var removedItems =
            transportedContainers.RemoveAll(container => container.SerialNumber == containerSerialNumber);
        if (removedItems == 0)
        {
            Console.WriteLine($"{containerSerialNumber} not found");
        }
        else
        {
            Console.WriteLine($"{containerSerialNumber} removed");
        }
    }

    public void ReplaceContainer(string containerSerialNumber, Container container)
    {
        var totalMass = transportedContainers.Sum(container1 => container1.NetMass + container1.LoadMass);
        if (totalMass + container.LoadMass + container.NetMass > maxTotalContainerMass * 1000)
        {
            throw new OverfillException("Total mass exceeded");
        }

        int index = transportedContainers.FindIndex(container1 => container1.SerialNumber == containerSerialNumber);
        if (index >= 0)
        {
            transportedContainers[index] = container;
        }
    }

    public static void TransferContainer(string containerSerialNumber, ContainerShip cs1, ContainerShip cs2)
    {
        Container? c = cs1.transportedContainers.Find(container => container.SerialNumber == containerSerialNumber);
        if (c != null)
        {
            cs2.LoadNewContainer(c);
            cs1.RemoveContainer(c.SerialNumber);
        }
    }

    public string GetShipInfo()
    {
        float totalContainerMass = transportedContainers.Sum(c => c.NetMass + c.LoadMass);

        string info = $"Container Ship Information:\n" +
                      $"- Max Speed: {maxSpeed} knots\n" +
                      $"- Container Capacity: {transportedContainers.Count}/{maxNumberOfContainers}\n" +
                      $"- Weight: {totalContainerMass / 1000:F2} tons / {maxTotalContainerMass} tons\n";

        if (transportedContainers.Count == 0)
        {
            info += "No containers loaded.\n";
        }
        else
        {
            info += $"Containers ({transportedContainers.Count}):\n";
            foreach (var container in transportedContainers)
            {
                info += $"- {container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)\n";
            }
        }

        return info;
    }
}