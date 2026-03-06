public class CreatePlayerDto
{
    public string Name { get; set; } = string.Empty;
    public int MarketValue { get; set; }
    public int? TeamId { get; set; }
}