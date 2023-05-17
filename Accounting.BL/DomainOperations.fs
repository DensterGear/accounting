namespace Accounting.BL.DomainOperations

open Accounting.BL.DataService
open Accounting.BL.Domain

module Validation =
    let userRepository = Repository<User>(userDbCollection)
    
    let checkUserByEmail email =
        let user = userRepository.getByEmail email
        box user = null

    let checkUserById id =
        let user = userRepository.getById id
        not(box user = null)

    let existingByEmail email =
        let exists = checkUserByEmail email
        match exists with
        | false -> raise (ValidationError $"User with email {email} already exists")
        | _ -> ()
        
    let existingById id =
        let exists = checkUserById id
        match exists with
        | true -> ()
        | _ -> raise (ValidationError $"User with id {id} not existing")