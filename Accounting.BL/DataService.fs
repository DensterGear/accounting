namespace Accounting.BL.DataService

open Accounting.BL.Domain
open MongoDB.Bson
open MongoDB.Driver
open Microsoft.Extensions.Configuration
open System

[<AutoOpen>]
module DataServiceOperations =
    let toObjectId id =
        try
            BsonObjectId(ObjectId.Parse(id))
        with
            | ex -> raise (SystemError ex.Message)
            
    let equalIdFilter (id: BsonObjectId) =
        Builders<'T>.Filter.Eq("_id", id)

    let loadConfiguration () =
        try 
            let configBuilder = ConfigurationBuilder()
                                    .SetBasePath(AppContext.BaseDirectory)
                                    .AddJsonFile("appsettings.json", optional=false, reloadOnChange=true)
            configBuilder.Build()
        with
            | ex -> raise (SourceError ex.Message)
            
type Repository<'T>(collectionName: string) =
    let config = loadConfiguration()
    let client = MongoClient(config.GetSection("Database:ConnectionString").Value)
    let collection = client.GetDatabase(config.GetSection("Database:DatabaseName").Value).GetCollection<'T>(collectionName)
    member this.getAll() = collection.AsQueryable().ToEnumerable()

    member this.    getById(id: BsonObjectId) =
        collection.Find(equalIdFilter id).SingleOrDefault()

    member this.getByEmail(email: string) =
        let filter = Builders<'T>.Filter.Eq("Email", email)
        collection.Find(filter).SingleOrDefault()

    member this.create(entity: 'T) =
        collection.InsertOne(entity)

    member this.update(id: BsonObjectId, entity: 'T) =
        collection.ReplaceOne(equalIdFilter id, entity) |> ignore
        collection.Find(equalIdFilter id).SingleOrDefault()

    member this.delete(id: BsonObjectId) =
        collection.DeleteOne(equalIdFilter id)
        
    member this.deleteAll() = async {
        let! deleteResult = collection.DeleteManyAsync(Builders<'T>.Filter.Empty) |> Async.AwaitTask
        return deleteResult
    }
        
