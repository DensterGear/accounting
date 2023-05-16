namespace Accounting.BL.UserService

open Accounting.BL.DataService
open Accounting.BL.Domain
open Accounting.BL.DomainOperations
open Microsoft.FSharp.Core

[<AutoOpen>]
module UserServiceOperations =
    let userRepository = Repository<User>(userDbCollection) 

module UserService =
    let getAll(): User seq =
        userRepository.getAll()
        
    let getGetById(id: string) =
        let userId = toObjectId id
        Validation.existingById userId
        userRepository.getById userId
    
    let create(user: User) =
        Validation.existingByEmail user.Email
        userRepository.create user
        userRepository.getByEmail user.Email
        
    let update(id: string, user: User) =
        let userId = toObjectId id
        Validation.existingById userId
        userRepository.update (userId, user)
        
    let delete(id: string) =
        let userId = toObjectId id
        Validation.existingById userId 
        userRepository.delete userId