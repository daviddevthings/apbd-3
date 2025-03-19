namespace APBD3;

public class TaskExtended
{
    private static List<ContainerShip> ships = new();
    private static List<Container> containers = new();

    public static void Main(string[] args)
    {
        var isRunning = true;
        while (isRunning)
        {
            DisplayMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "0": isRunning = false; break;
                case "1": AddShip(); break;
                case "2": RemoveShip(); break;
                case "3": AddContainer(); break;
                case "4": RemoveContainer(); break;
                case "5": LoadContainerOnShip(); break;
                case "6": UnloadContainerFromShip(); break;
                case "7": TransferContainerBetweenShips(); break;
                case "8": LoadCargoIntoContainer(); break;
                case "9": EmptyContainer(); break;
                case "10": ReplaceContainerOnShip(); break;
                case "11": DisplayContainerInfo(); break;
                case "12": DisplayShipInfo(); break;
                default: Console.WriteLine("Nieprawidłowa opcja."); break;
            }

            if (isRunning)
            {
                Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("=== MENU ===\n");

        Console.WriteLine("Lista kontenerowców:");
        if (ships.Count == 0) Console.WriteLine("Brak");
        else
        {
            for (var i = 0; i < ships.Count; i++)
            {
                var ship = ships[i];
                Console.WriteLine(
                    $"{i + 1}. Statek (prędkość={ship.maxSpeed}, maxKontenerów={ship.maxNumberOfContainers}, maxWaga={ship.maxTotalContainerMass})");
            }
        }

        Console.WriteLine("\nLista kontenerów:");
        if (containers.Count == 0) Console.WriteLine("Brak");
        else
        {
            foreach (var container in containers)
            {
                Console.WriteLine($"{container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)");
            }
        }

        Console.WriteLine("\nMożliwe akcje:");
        Console.WriteLine("0. Wyjście");
        Console.WriteLine("1. Dodaj kontenerowiec");
        if (ships.Count > 0) Console.WriteLine("2. Usuń kontenerowiec");
        Console.WriteLine("3. Dodaj kontener");

        if (containers.Count > 0)
        {
            Console.WriteLine("4. Usuń kontener");
            Console.WriteLine("5. Załaduj kontener na statek");
            Console.WriteLine("8. Załaduj towar do kontenera");
            Console.WriteLine("9. Opróżnij kontener");
            Console.WriteLine("11. Wyświetl informacje o kontenerze");
        }

        var anyShipHasContainers = ships.Any(s => s.transportedContainers.Count > 0);
        if (ships.Count > 0 && anyShipHasContainers)
        {
            Console.WriteLine("6. Rozładuj kontener ze statku");
            Console.WriteLine("10. Zamień kontener na statku");
        }

        if (ships.Count > 1 && anyShipHasContainers)
        {
            Console.WriteLine("7. Przenieś kontener między statkami");
        }

        if (ships.Count > 0) Console.WriteLine("12. Wyświetl informacje o statku");

        Console.Write("\nWybierz opcję: ");
    }

    private static void AddShip()
    {
        Console.WriteLine("\n=== Dodaj Kontenerowiec ===");

        var speed = ReadFloat("Podaj maksymalną prędkość (węzły): ");
        var maxContainers = ReadInt("Podaj maksymalną liczbę kontenerów: ");
        var maxWeight = ReadFloat("Podaj maksymalną wagę całkowitą (tony): ");

        ships.Add(new ContainerShip(speed, maxContainers, maxWeight));
        Console.WriteLine("Kontenerowiec dodany pomyślnie!");
    }

    private static void RemoveShip()
    {
        if (ships.Count == 0)
        {
            Console.WriteLine("Brak statków do usunięcia.");
            return;
        }

        Console.WriteLine("\n=== Usuń Kontenerowiec ===");
        for (var i = 0; i < ships.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Statek (kontenery={ships[i].transportedContainers.Count}/{ships[i].maxNumberOfContainers})");
        }

        var shipIndex = ReadInt("Podaj numer statku do usunięcia: ", 1, ships.Count) - 1;

        if (ships[shipIndex].transportedContainers.Count > 0)
        {
            Console.WriteLine("Uwaga: Statek zawiera kontenery. Wszystkie kontenery zostaną zwrócone do puli.");
            containers.AddRange(ships[shipIndex].transportedContainers);
        }

        ships.RemoveAt(shipIndex);
        Console.WriteLine("Statek usunięty pomyślnie!");
    }

    private static void AddContainer()
    {
        Console.WriteLine("\n=== Dodaj Kontener ===");
        Console.WriteLine("Wybierz typ kontenera:");
        Console.WriteLine("1. Kontener na płyny");
        Console.WriteLine("2. Kontener na gazy");
        Console.WriteLine("3. Kontener chłodniczy");

        var containerType = ReadInt("Podaj typ kontenera: ", 1, 3);

        var height = ReadFloat("Podaj wysokość (cm): ");
        var ownMass = ReadFloat("Podaj masę własną (kg): ");
        var depth = ReadFloat("Podaj głębokość (cm): ");
        var maxCapacity = ReadFloat("Podaj maksymalną pojemność (kg): ");

        Container container;

        switch (containerType)
        {
            case 1:
                var isDangerous = ReadBool("Czy jest to kontener na niebezpieczne płyny? (t/n): ");
                container = new LiquidContainer(height, ownMass, depth, maxCapacity, isDangerous);
                break;
            case 2:
                var pressure = ReadFloat("Podaj ciśnienie (atm): ");
                container = new GasContainer(height, ownMass, depth, maxCapacity, pressure);
                break;
            case 3:
                Console.WriteLine("Dostępne typy produktów:");
                Console.WriteLine("- bananas (13.3°C)");
                Console.WriteLine("- chocolate (18°C)");
                Console.WriteLine("- fish (2°C)");
                Console.WriteLine("- meat (-15°C)");
                Console.WriteLine("- ice cream (-18°C)");
                Console.WriteLine("- frozen pizza (-30°C)");
                Console.WriteLine("- cheese (7.2°C)");
                Console.WriteLine("- sausages (5°C)");
                Console.WriteLine("- butter (20.5°C)");
                Console.WriteLine("- eggs (19°C)");

                var productType = ReadString("Podaj typ produktu: ").ToLower();
                var temperature = ReadFloat("Podaj temperaturę (°C): ");

                try
                {
                    container = new CoolingContainer(height, ownMass, depth, maxCapacity, temperature,
                        productType);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}");
                    return;
                }

                break;
            default:
                Console.WriteLine("Nieprawidłowy typ kontenera.");
                return;
        }

        containers.Add(container);
        Console.WriteLine($"Kontener {container.SerialNumber} dodany pomyślnie!");
    }

    private static void RemoveContainer()
    {
        if (containers.Count == 0)
        {
            Console.WriteLine("Brak kontenerów do usunięcia.");
            return;
        }

        Console.WriteLine("\n=== Usuń Kontener ===");
        DisplayAvailableContainers();

        var serialNumber = ReadString("Podaj numer seryjny kontenera do usunięcia: ");

        var index =
            containers.FindIndex(c => c.SerialNumber.Equals(serialNumber));
        if (index >= 0)
        {
            containers.RemoveAt(index);
            Console.WriteLine($"Kontener {serialNumber} usunięty pomyślnie!");
        }
        else
        {
            Console.WriteLine($"Kontener {serialNumber} nie znaleziony!");
        }
    }

    private static void LoadContainerOnShip()
    {
        if (ships.Count == 0 || containers.Count == 0)
        {
            Console.WriteLine("Brak statków lub kontenerów.");
            return;
        }

        Console.WriteLine("\n=== Załaduj Kontener na Statek ===");


        for (var i = 0; i < ships.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Statek (kontenery={ships[i].transportedContainers.Count}/{ships[i].maxNumberOfContainers})");
        }

        var shipIndex = ReadInt("Podaj numer statku: ", 1, ships.Count) - 1;
        var ship = ships[shipIndex];


        DisplayAvailableContainers();
        var serialNumber = ReadString("Podaj numer seryjny kontenera do załadowania: ");

        var containerIndex =
            containers.FindIndex(c => c.SerialNumber.Equals(serialNumber));
        if (containerIndex < 0)
        {
            Console.WriteLine($"Kontener {serialNumber} nie znaleziony!");
            return;
        }

        var container = containers[containerIndex];

        try
        {
            ship.LoadNewContainer(container);
            containers.RemoveAt(containerIndex);
            Console.WriteLine($"Kontener {serialNumber} załadowany na statek pomyślnie!");
        }
        catch (OverfillException ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void UnloadContainerFromShip()
    {
        if (ships.Count == 0 || !ships.Any(s => s.transportedContainers.Count > 0))
        {
            Console.WriteLine("Brak statków z kontenerami.");
            return;
        }

        Console.WriteLine("\n=== Rozładuj Kontener ze Statku ===");


        List<ContainerShip> shipsWithContainers = ships.Where(s => s.transportedContainers.Count > 0).ToList();
        for (var i = 0; i < shipsWithContainers.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Statek (kontenery={shipsWithContainers[i].transportedContainers.Count}/{shipsWithContainers[i].maxNumberOfContainers})");
        }

        var shipIndex = ReadInt("Podaj numer statku: ", 1, shipsWithContainers.Count) - 1;
        var ship = shipsWithContainers[shipIndex];


        Console.WriteLine("\nKontenery na tym statku:");
        for (var i = 0; i < ship.transportedContainers.Count; i++)
        {
            var container = ship.transportedContainers[i];
            Console.WriteLine(
                $"{i + 1}. {container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)");
        }

        var containerIndex =
            ReadInt("Podaj numer kontenera do rozładowania: ", 1, ship.transportedContainers.Count) - 1;
        var containerToUnload = ship.transportedContainers[containerIndex];

        ship.RemoveContainer(containerToUnload.SerialNumber);
        containers.Add(containerToUnload);

        Console.WriteLine($"Kontener {containerToUnload.SerialNumber} rozładowany pomyślnie!");
    }

    private static void TransferContainerBetweenShips()
    {
        if (ships.Count < 2 || !ships.Any(s => s.transportedContainers.Count > 0))
        {
            Console.WriteLine("Potrzeba co najmniej dwóch statków, a jeden musi mieć kontenery.");
            return;
        }

        Console.WriteLine("\n=== Przenieś Kontener Między Statkami ===");


        List<ContainerShip> shipsWithContainers = ships.Where(s => s.transportedContainers.Count > 0).ToList();
        Console.WriteLine("Statki źródłowe:");
        for (var i = 0; i < shipsWithContainers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Statek (kontenery={shipsWithContainers[i].transportedContainers.Count})");
        }

        var shipIndex = ReadInt("Podaj numer statku źródłowego: ", 1, shipsWithContainers.Count) - 1;
        var sourceShip = shipsWithContainers[shipIndex];


        Console.WriteLine("\nKontenery na statku źródłowym:");
        for (var i = 0; i < sourceShip.transportedContainers.Count; i++)
        {
            var container = sourceShip.transportedContainers[i];
            Console.WriteLine(
                $"{i + 1}. {container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)");
        }

        var containerIndex = ReadInt("Podaj numer kontenera do przeniesienia: ", 1,
            sourceShip.transportedContainers.Count) - 1;
        var containerToTransfer = sourceShip.transportedContainers[containerIndex];


        Console.WriteLine("\nStatki docelowe:");
        List<ContainerShip> targetShips = ships.Where(s => s != sourceShip).ToList();
        for (var i = 0; i < targetShips.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Statek (kontenery={targetShips[i].transportedContainers.Count}/{targetShips[i].maxNumberOfContainers})");
        }

        var targetShipIndex = ReadInt("Podaj numer statku docelowego: ", 1, targetShips.Count) - 1;
        var targetShip = targetShips[targetShipIndex];

        try
        {
            ContainerShip.TransferContainer(containerToTransfer.SerialNumber, sourceShip, targetShip);
            Console.WriteLine($"Kontener {containerToTransfer.SerialNumber} przeniesiony pomyślnie!");
        }
        catch (OverfillException ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void LoadCargoIntoContainer()
    {
        if (containers.Count == 0)
        {
            Console.WriteLine("Brak dostępnych kontenerów.");
            return;
        }

        Console.WriteLine("\n=== Załaduj Towar do Kontenera ===");
        DisplayAvailableContainers();

        var serialNumber = ReadString("Podaj numer seryjny kontenera: ");

        var index =
            containers.FindIndex(c => c.SerialNumber.Equals(serialNumber));
        if (index < 0)
        {
            Console.WriteLine($"Kontener {serialNumber} nie znaleziony!");
            return;
        }

        var container = containers[index];
        var mass = ReadFloat("Podaj masę ładunku (kg): ");

        try
        {
            container.LoadContainer(mass);
            Console.WriteLine($"Kontener {serialNumber} załadowany {mass}kg pomyślnie!");
        }
        catch (OverfillException ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void EmptyContainer()
    {
        if (containers.Count == 0)
        {
            Console.WriteLine("Brak dostępnych kontenerów.");
            return;
        }

        Console.WriteLine("\n=== Opróżnij Kontener ===");
        DisplayAvailableContainers();

        var serialNumber = ReadString("Podaj numer seryjny kontenera do opróżnienia: ");

        var index =
            containers.FindIndex(c => c.SerialNumber.Equals(serialNumber));
        if (index < 0)
        {
            Console.WriteLine($"Kontener {serialNumber} nie znaleziony!");
            return;
        }

        var container = containers[index];
        container.EmptyContainer();

        Console.WriteLine($"Kontener {serialNumber} opróżniony pomyślnie!");
    }

    private static void ReplaceContainerOnShip()
    {
        if (ships.Count == 0 || !ships.Any(s => s.transportedContainers.Count > 0) || containers.Count == 0)
        {
            Console.WriteLine("Potrzeba co najmniej jednego statku z kontenerem i jednego dostępnego kontenera.");
            return;
        }

        Console.WriteLine("\n=== Zamień Kontener na Statku ===");


        List<ContainerShip> shipsWithContainers = ships.Where(s => s.transportedContainers.Count > 0).ToList();
        Console.WriteLine("Wybierz statek:");
        for (var i = 0; i < shipsWithContainers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Statek (kontenery={shipsWithContainers[i].transportedContainers.Count})");
        }

        var shipIndex = ReadInt("Podaj numer statku: ", 1, shipsWithContainers.Count) - 1;
        var ship = shipsWithContainers[shipIndex];


        Console.WriteLine("\nKontenery na statku do zamiany:");
        for (var i = 0; i < ship.transportedContainers.Count; i++)
        {
            var container = ship.transportedContainers[i];
            Console.WriteLine(
                $"{i + 1}. {container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)");
        }

        var containerIndex =
            ReadInt("Podaj numer kontenera do zamiany: ", 1, ship.transportedContainers.Count) - 1;
        var containerToReplace = ship.transportedContainers[containerIndex];


        Console.WriteLine("\nDostępne kontenery jako zamienniki:");
        DisplayAvailableContainers();

        var replacementSerialNumber = ReadString("Podaj numer seryjny kontenera zamiennika: ");

        var replacementIndex = containers.FindIndex(c =>
            c.SerialNumber.Equals(replacementSerialNumber));
        if (replacementIndex < 0)
        {
            Console.WriteLine($"Kontener {replacementSerialNumber} nie znaleziony!");
            return;
        }

        var replacementContainer = containers[replacementIndex];

        try
        {
            ship.ReplaceContainer(containerToReplace.SerialNumber, replacementContainer);
            containers.RemoveAt(replacementIndex);
            containers.Add(containerToReplace);
            Console.WriteLine(
                $"Kontener {containerToReplace.SerialNumber} zamieniony na {replacementSerialNumber} pomyślnie!");
        }
        catch (OverfillException ex)
        {
            Console.WriteLine($"Błąd: {ex.Message}");
        }
    }

    private static void DisplayContainerInfo()
    {
        if (containers.Count == 0)
        {
            Console.WriteLine("Brak dostępnych kontenerów.");
            return;
        }

        Console.WriteLine("\n=== Wyświetl Informacje o Kontenerze ===");
        DisplayAvailableContainers();

        var serialNumber = ReadString("Podaj numer seryjny kontenera: ");

        var index =
            containers.FindIndex(c => c.SerialNumber.Equals(serialNumber));
        if (index < 0)
        {
            Console.WriteLine($"Kontener {serialNumber} nie znaleziony!");
            return;
        }

        var container = containers[index];
        Console.WriteLine(container.GetContainerInfo());
    }

    private static void DisplayShipInfo()
    {
        if (ships.Count == 0)
        {
            Console.WriteLine("Brak dostępnych statków.");
            return;
        }

        Console.WriteLine("\n=== Wyświetl Informacje o Statku ===");
        for (var i = 0; i < ships.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. Statek (prędkość={ships[i].maxSpeed}, kontenery={ships[i].transportedContainers.Count}/{ships[i].maxNumberOfContainers})");
        }

        var shipIndex = ReadInt("Podaj numer statku: ", 1, ships.Count) - 1;
        var ship = ships[shipIndex];

        Console.WriteLine(ship.GetShipInfo());
    }

    private static void DisplayAvailableContainers()
    {
        Console.WriteLine("Dostępne kontenery:");
        foreach (var container in containers)
        {
            Console.WriteLine($"{container.SerialNumber} ({container.LoadMass}kg / {container.MaxCapacity}kg)");
        }
    }


    private static float ReadFloat(string message, float min = 0)
    {
        while (true)
        {
            Console.Write(message);
            if (float.TryParse(Console.ReadLine(), out float value) && value >= min)
            {
                return value;
            }

            Console.WriteLine($"Nieprawidłowe dane. Podaj liczbę większą lub równą {min}.");
        }
    }

    private static int ReadInt(string message, int min = 0, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(message);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Nieprawidłowe dane. Podaj liczbę z zakresu {min}-{max}.");
        }
    }

    private static string ReadString(string message)
    {
        while (true)
        {
            Console.Write(message);
            var data = Console.ReadLine().Trim();
            if (!string.IsNullOrEmpty(data))
            {
                return data;
            }

            Console.WriteLine("Nieprawidłowe dane. Podaj niepusty ciąg znaków.");
        }
    }

    private static bool ReadBool(string message)
    {
        while (true)
        {
            Console.Write(message);
            var data = Console.ReadLine().Trim().ToLower();
            if (data == "t" || data == "tak")
            {
                return true;
            }

            if (data == "n" || data == "nie")
            {
                return false;
            }

            Console.WriteLine("Nieprawidłowe dane. Podaj 't' lub 'n'.");
        }
    }
}