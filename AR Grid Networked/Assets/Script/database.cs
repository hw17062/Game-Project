using UnityEngine;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;

public class database
{
    private IMongoCollection<Card> collection;
    private string connectionString = "mongodb+srv://testAdmin:testPass@homes-bh5m4.gcp.mongodb.net/test?retryWrites=true&w=majority";

    // Start is called before the first frame update
    public database()
    {
        var client = new MongoClient(connectionString);
        var server = client.GetDatabase("test");
        collection = server.GetCollection<Card>("card");
        if (collection == null)
        {
            Debug.Log("error");
        }

    }

    public Card FindCard(string cardName)
    {
        var filter = Builders<Card>.Filter.Eq(x => x.cardname, cardName);
        var r = collection.Find<Card>(filter);

        var c = r.CountDocuments();
        //check if there is a card in the database;
        if (c >= 1)
        {
            Debug.Log("Find " + c + " cards: " + cardName);
            var card = r.First<Card>(); // get the first card result from collection
            Debug.Log(card.cardname + " ap: " + card.ap + "type: " + card.type);
            return card;
        }
        else
        {
            Debug.Log("Can not find card: " + cardName);
            return null;
        }

    }
}
