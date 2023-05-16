namespace Accounting.BL.UserService

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
    
    [<CompiledName("GetById")>]
    let getById(id: string) =
        let userId = toObjectId id
        Validation.existingById userId
        userRepository.getById userId
    
    [<CompiledName("Create")>]
    let create(user: User) =
        Validation.existingByEmail user.Email
        userRepository.create user
        userRepository.getByEmail user.Email
    
    [<CompiledName("Update")>]
    let update(id: string, user: User) =
        let userId = toObjectId id
        Validation.existingById userId
        userRepository.update (userId, user)
        
    [<CompiledName("Delete")>]
    let delete(id: string) =
        let userId = toObjectId id
        Validation.existingById userId 
        userRepository.delete userId