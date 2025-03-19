namespace APBD3;

public class CoolingContainer : Container
{
    private Dictionary<string, float> ProductsTemperatures = new();
    public float Temperature { get; set; }
    public string ProductType { get; set; }

    public CoolingContainer(float height, float netMass, float depth, float maxCapacity, float temperature,
        string productType) : base(height, netMass, depth, maxCapacity, "C")
    {
        ProductType = productType;
        ProductsTemperatures.Add("bananas", 13.3f);
        ProductsTemperatures.Add("chocolate", 18f);
        ProductsTemperatures.Add("fish", 2f);
        ProductsTemperatures.Add("meat", -15f);
        ProductsTemperatures.Add("ice cream", -18f);
        ProductsTemperatures.Add("frozen pizza", -30f);
        ProductsTemperatures.Add("cheese", 7.2f);
        ProductsTemperatures.Add("sausages", 5f);
        ProductsTemperatures.Add("butter", 20.5f);
        ProductsTemperatures.Add("eggs", 19f);

        if (!ProductsTemperatures.ContainsKey(productType.ToLower()))
        {
            throw new ArgumentException($"Product type '{productType}' is not supported.");
        }

        float requiredTemperature = ProductsTemperatures[productType.ToLower()];
        if (temperature < requiredTemperature)
        {
            throw new ArgumentException(
                $"Container temperature ({temperature}°C) cannot be lower than required for {productType} ({requiredTemperature}°C).");
        }

        Temperature = temperature;
    }

    public override string GetContainerInfo()
    {
        return base.GetContainerInfo() +
               $"- Product Type: {ProductType}\n" +
               $"- Temperature: {Temperature}°C\n";
    }
}
