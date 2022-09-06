using aspmongo.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace aspmongo.Services;

public class MongoDBServices 
{
    private readonly IMongoCollection<Playlist> _PlaylistCollection;

    public MongoDBServices(IOptions<MongoDbSettings> mongoDBSettings) 
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _PlaylistCollection = database.GetCollection<Playlist> (mongoDBSettings.Value.CollectioName);
    }

    // POST
    public async Task CreateAsync(Playlist playlist){
        await _PlaylistCollection.InsertOneAsync(playlist);
        return;
    }

    // GET
    public async Task<List<Playlist>> GetAsync(){
        return await _PlaylistCollection.Find(new BsonDocument()).ToListAsync();
    }

    // PUT
    public async Task AddToPlaylistAsync(string id, string movieId){
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
        UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("movieId", movieId);
        await _PlaylistCollection.UpdateOneAsync(filter, update);
        return;
    }

    // DELETE
    public async Task DeleteAsync(string id){
        FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
        await _PlaylistCollection.DeleteOneAsync(filter);
        return;
    }

}