namespace moto_rent.Features.Riders;
public class Rider
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Cnh { get; set; } = string.Empty;

    public enum CnhCategory
    {
        A,
        B,
        AB
    }
}