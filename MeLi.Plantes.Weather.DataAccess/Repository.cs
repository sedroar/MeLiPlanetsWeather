using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.DataAccess
{
    public class Repository<T> where T : BaseEntity
    {
        private readonly IConfiguration configuration;

        private readonly IMongoClient mongoClient;
        private IMongoDatabase database;
        private IMongoCollection<T> collection;

        private UpdateOptions updateOptions => new UpdateOptions { IsUpsert = false };

        public Repository(IConfiguration configuration, IMongoClient mongoClient)
        {
            this.configuration = configuration;
            this.mongoClient = mongoClient;
            GetDatabase();
            GetCollection();
        }

        private void GetDatabase()
        {
            database = mongoClient.GetDatabase(configuration["MongoDbDatabaseName"]);
        }

        private void GetCollection()
        {
            collection = database
                .GetCollection<T>(typeof(T).Name);
        }

        public virtual async Task<List<T>> Fetch()
        {
            return await (await collection.FindAsync<T>(Builders<T>.Filter.Empty).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> Insert(T entity)
        {
            await collection.InsertOneAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public async Task<IEnumerable<T>> InsertMany(IEnumerable<T> entities)
        {
            await collection.InsertManyAsync(entities);
            return entities;
        }

        public async Task<bool> Delete(string id)
        {
            var eqFilterDefinition = Builders<T>.Filter.Eq(d => d.Id, ObjectId.Parse(id));
            var deleteResult = await collection.DeleteOneAsync(eqFilterDefinition).ConfigureAwait(false);
            return deleteResult.DeletedCount > 0;
        }

        public virtual async Task<List<T>> Find(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            var documents = await collection.FindAsync<T>(filter).ConfigureAwait(false);

            return await documents.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<List<T>> Find(FilterDefinition<T> filter)
        {
            var documents = await collection.FindAsync<T>(filter).ConfigureAwait(false);

            return await documents.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetById(string id)
        {
            var eqFilterDefinition = Builders<T>.Filter.Eq(d => d.Id, ObjectId.Parse(id));
            return await (await collection.FindAsync(eqFilterDefinition).ConfigureAwait(false)).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<T> Update(T entity)
        {
            var eqFilterDefinition = Builders<T>.Filter.Eq(d => d.Id, entity.Id);
            var updateResult = await collection.ReplaceOneAsync(eqFilterDefinition, entity, updateOptions).ConfigureAwait(false);
            return entity;
        }
    }
}
