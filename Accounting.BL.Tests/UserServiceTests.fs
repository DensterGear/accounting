namespace Accounting.BL.Tests

open Accounting.BL
open Accounting.BL.Domain
open Accounting.BL.UserService
open Microsoft.VisualStudio.TestTools.UnitTesting
open MongoDB.Bson

[<AutoOpen>]
module UserServiceTestsOperation =
    let getTestUser =
            { Id = BsonObjectId(ObjectId.GenerateNewId())
              Name = "John"
              LastName = Some "Doe"
              Email = "john@example.com"
              Age = 25<age>
              Gender = Gender.Male
              City = "New York" }

[<TestClass>]
type UserServiceTests() =
    
    [<TestInitialize>]
    member this.TestInit() =
        userRepository.deleteAll() |> ignore
        
    [<TestCleanup>]
    member this.TestTeardown() =
        userRepository.deleteAll() |> ignore

    [<TestMethod>]
    member this.``GetAll should return all users``() =
        //Arrange
        let user1 = { getTestUser with Id = BsonObjectId(ObjectId.GenerateNewId()) } 
        let user2 = { user1 with Id = BsonObjectId(ObjectId.GenerateNewId()); Email = "john1@example.com" } 

        UserService.create user1 |> ignore
        UserService.create user2 |> ignore

        //Act
        let result = UserService.getAll ()

        //Assert
        Assert.IsTrue ((Seq.length result) = 2)
        
        //Teardown
        userRepository.delete(user1.Id) |> ignore
        userRepository.delete(user2.Id) |> ignore

    [<TestMethod>]
    member this.``Get by id should return specific user``() =
        //Arrange
        let user = { getTestUser with Id = BsonObjectId(ObjectId.GenerateNewId()) }
        UserService.create user |> ignore
        
        //Act
        let actualUser = userRepository.getById user.Id
        
        //Assert
        Assert.AreEqual(user.Id, actualUser.Id)
        
        //Teardown
        userRepository.delete(user.Id) |> ignore
    
    [<TestMethod>]
    member this.``Update user should return specific user with the changes``() =
        //Arrange
        let user = { getTestUser with Id = BsonObjectId(ObjectId.GenerateNewId()) }
        
        UserService.create user |> ignore
        
        let changedUser = { user with City = "Prague" }
        
        //Act
        let actualUser = userRepository.update(user.Id, changedUser)
        
        //Assert
        Assert.AreEqual(changedUser.City, actualUser.City)
        
        //Teardown
        userRepository.delete(user.Id) |> ignore

    [<TestMethod>]
    member this.``Create already existed user should return error``() =
        //Arrange
        let user = { getTestUser with Id = BsonObjectId(ObjectId.GenerateNewId()) }
        
        UserService.create user |> ignore

        //Assert
        Assert.ThrowsException<ValidationError>(fun () -> UserService.create { user with Id = BsonObjectId(ObjectId.GenerateNewId()) } |> ignore) |> ignore
        
        //Teardown
        userRepository.delete(user.Id) |> ignore