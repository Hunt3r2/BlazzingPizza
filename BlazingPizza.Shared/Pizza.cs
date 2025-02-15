using System.Text.Json.Serialization;

namespace BlazingPizza;

/// <summary>
///    /// Represents a customized pizza as part of an order
/// </summary>
public class Pizza
{
    public const int DefaultSize = 12;
    public const int MinimumSize = 9;
    public const int MaximumSize = 17;
    public List<PizzaTopping> Toppings { get; set; } = new();

    //Para donar
    public bool Donar { get; set; }

    public int Id { get; set; }

    public int OrderId { get; set; }

    public PizzaSpecial? Special { get; set; }

    public int SpecialId { get; set; }

    public int Size { get; set; }


    public decimal GetBasePrice()
    {
        if(Special == null) throw new NullReferenceException($"{nameof(Special)} was null when calculating Base Price.");
        return ((decimal)Size / (decimal)DefaultSize) * Special.BasePrice;
    }

    public decimal GetTotalPrice()
    {
        var precio = Special?.BasePrice ?? 0;
        precio += Toppings.Sum(t => t.Topping?.Price ?? 0);
        precio += Size switch { 32 => 2, 40 => 4, _ => 0 };
        precio += Donar ? 1 : 0;
        return precio;
    }

    public string GetFormattedTotalPrice()
    {
        decimal precioBase = GetBasePrice();
        decimal precioComplementos = Toppings.Sum(t => t.Topping!.Price);

        //si el usuario ha optado por donar, se añade 1€ al precio
        if (Donar)
        {
            //aumentar el precio en 1€ si se dona
            precioBase += 1.0m; 
        }

        //el precio total formateado
        decimal totalPrice = precioBase + precioComplementos;
        return totalPrice.ToString("0.00");
    }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(Pizza))]
public partial class PizzaContext : JsonSerializerContext { }