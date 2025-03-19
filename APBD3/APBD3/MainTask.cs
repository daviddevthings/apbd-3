namespace APBD3;

public class MainTask
{
    public static void Main(string[] args)
    {
        //Stworzenie kontenera danego typu
        var liquidContainer = new LiquidContainer(250, 500, 600, 20000, false);

        var gasContainer = new GasContainer(220, 450, 600, 15000, 2.5f);

        var coolingContainer1 = new CoolingContainer(280, 600, 650, 25000, 14.0f, "bananas");

        var coolingContainer2 = new CoolingContainer(280, 600, 650, 25000, -15.0f, "ice cream");

        //Załadowanie ładunku do danego kontenera
        liquidContainer.LoadContainer(15000);

        gasContainer.LoadContainer(10000);

        coolingContainer1.LoadContainer(20000);

        coolingContainer2.LoadContainer(18000);
        Console.WriteLine();

        var containerShip1 = new ContainerShip(25.0f, 10, 500);
        var containerShip2 = new ContainerShip(20.0f, 5, 250);

        //Załadowanie kontenera na statek
        containerShip1.LoadNewContainer(liquidContainer);
        containerShip1.LoadNewContainer(gasContainer);
        Console.WriteLine(containerShip1.GetShipInfo());

        //Załadowanie listy kontenerów na statek
        List<Container> containers = new List<Container> { coolingContainer1, coolingContainer2 };
        containerShip2.LoadNewContainers(containers);
        Console.WriteLine(containerShip2.GetShipInfo());

        //Usunięcie kontenera ze statku
        containerShip1.RemoveContainer("KON-L-0");
        Console.WriteLine(containerShip1.GetShipInfo());

        //Rozładowanie kontenera
        Console.WriteLine(coolingContainer1.GetContainerInfo());
        coolingContainer1.EmptyContainer();
        Console.WriteLine(coolingContainer1.GetContainerInfo());

        //Zastąpienie kontenera na statku o danym numerze innym kontenerem
        var newLiquidContainer = new LiquidContainer(250, 520, 600, 22000,false);
        newLiquidContainer.LoadContainer(15000);
        containerShip1.ReplaceContainer("KON-G-1", newLiquidContainer);
        Console.WriteLine(containerShip1.GetShipInfo());

        //Możliwość przeniesienie kontenera między dwoma statkami
        ContainerShip.TransferContainer("KON-L-4", containerShip1, containerShip2);
        Console.WriteLine(containerShip1.GetShipInfo());
        Console.WriteLine(containerShip2.GetShipInfo());
    }
}