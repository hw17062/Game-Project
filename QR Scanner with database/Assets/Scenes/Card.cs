using MongoDB.Bson;

public class Card
{
    public ObjectId Id { get; set; }
    public string cardname { get; set; }
    public string type { get; set; }
    public int target { get; set; }
    public double value { get; set; }
    public int ap { get; set; }
    
}
