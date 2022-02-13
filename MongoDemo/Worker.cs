using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDemo
{
    class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }



        
        public async Task<string> MongoDbCrud( string docId, string t, string v)

        {
            try
            {
                var _collection = MongoDBConnect.GetCollection<Doc>();
                // get document
                var id = new ObjectId(docId);
                Doc @doc = await _collection.Find(x => x._id == id).FirstOrDefaultAsync();
                // update document
                var filter = Builders<Doc>.Filter.Eq(s => s._id, id);
                var update = Builders<Doc>.Update.Set(s => s._t, t);
                var result = await _collection.UpdateOneAsync(filter, update);
                // update document and change type to string
                var filter2 = new BsonDocument("_v", new BsonDocument("$type", BsonType.String));
                var update2 = Builders<Doc>.Update.Set(s => s._v, v);
                var result2 = await _collection.UpdateOneAsync(filter, update);
                return "Sucess";
            }
            catch (Exception)
            {

                return "Fail";
            }
           
        }
    }

    class Doc
    {
        public ObjectId _id { get; set; }
        public string _v { get; set; }
        public string _t { get; set; }
    }
        class MongoDBConnect
    {
        private readonly IConfiguration configuration;
        private static string _mongoDbURL;
        private static string _dbName;
        private static string _collectionName;
        public MongoDBConnect(IConfiguration configuration)
        {
            this.configuration = configuration;
            _mongoDbURL = configuration.GetValue<string>("MongoDbURL");
            _dbName = configuration.GetValue<string>("DbName");
            _collectionName = configuration.GetValue<string>("CollectionName");
        }


        public static IMongoDatabase dB { get; set; }
        internal static MongoClient ConnectToDB(string mongoDbURL)
        {
            return new MongoClient(_mongoDbURL);
        }



        internal static IMongoDatabase GetDataBase()
        {
            var client = new MongoClient(_mongoDbURL);
            dB = client.GetDatabase(_dbName);
            return dB;
        }

        internal static IMongoCollection<Doc> GetCollection<T>()
        {
            return dB.GetCollection<Doc>(_collectionName);
        }

    }
}
