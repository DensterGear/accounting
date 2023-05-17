namespace Accounting.BL.UserService

open System
open Accounting.BL.DataService
open Accounting.BL.Domain
open Accounting.BL.DomainOperations
open Microsoft.FSharp.Core

[<AutoOpen>]
module UserServiceOperations =
    let userRepository = Repository<User>(userDbCollection) 

module UserService =
    
    [<CompiledName("GetAll")>]
    let getAll(): User seq =
        async {
            let! users = userRepository.getAllAsync() |> Async.AwaitTask
            return users |> Seq.sortByDescending(fun x -> x.CreatedAt)
        } |> Async.RunSynchronously
    
    [<CompiledName("GetById")>]
    let getById(id: string) =
        let userId = toObjectId id
        Validation.existingById userId
        async {
            let! user = userRepository.getByIdAsync(userId) |> Async.AwaitTask
            return user
        } |> Async.RunSynchronously
        
    
    [<CompiledName("Create")>]
    let create(user: User) =
        Validation.existingByEmail user.Email
        let user = { user with CreatedAt = DateTime.UtcNow; UpdatedAt = DateTime.UtcNow }
        async {
            userRepository.createAsync user |> Async.AwaitTask |> ignore
            let! user = userRepository.getByEmailAsync user.Email |> Async.AwaitTask
            return user
        } |> Async.RunSynchronously
    
    [<CompiledName("Update")>]
    let update(id: string, user: User) =
        let userId = toObjectId id
        Validation.existingById userId
        let user = { user with UpdatedAt = DateTime.UtcNow }
        async {
            let! userUpdate = userRepository.updateAsync (userId, user) |> Async.AwaitTask
            return userUpdate
        } |> Async.RunSynchronously
        
        
    [<CompiledName("Delete")>]
    let delete(id: string) =
        let userId = toObjectId id
        Validation.existingById userId
        async {
            userRepository.deleteAsync userId |> Async.AwaitTask |> ignore
        } |> Async.RunSynchronously
        