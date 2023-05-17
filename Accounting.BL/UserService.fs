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
        userRepository.getAll()
        |> Seq.sortByDescending(fun u -> u.CreatedAt)
    
    [<CompiledName("GetById")>]
    let getById(id: string) =
        let userId = toObjectId id
        Validation.existingById userId
        userRepository.getById userId
    
    [<CompiledName("Create")>]
    let create(user: User) =
        Validation.existingByEmail user.Email
        let user = { user with CreatedAt = DateTime.UtcNow; UpdatedAt = DateTime.UtcNow }
        userRepository.create user
        userRepository.getByEmail user.Email
    
    [<CompiledName("Update")>]
    let update(id: string, user: User) =
        let userId = toObjectId id
        Validation.existingById userId
        let user = { user with UpdatedAt = DateTime.UtcNow }
        userRepository.update (userId, user)
        
    [<CompiledName("Delete")>]
    let delete(id: string) =
        let userId = toObjectId id
        Validation.existingById userId 
        userRepository.delete userId