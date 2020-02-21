using MongoDB.Bson;

public class Card
{
    public ObjectId Id { get; set; }
    public string cardname { get; set; }
    public int target { get; set; }
    public double heal { get; set; }
    public int cost { get; set; }
    
}
