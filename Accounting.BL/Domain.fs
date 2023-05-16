module Accounting.BL.Domain

open Microsoft.FSharp.Core
open MongoDB.Bson

[<Measure>]
type age

[<Literal>]
let userDbCollection = "users"

exception SourceError of string
exception SystemError of string
exception ValidationError of string

type Result<'T> =
    Success of 'T
    | Failure of error:string

type Configuration =
    { DatabaseConnectionString: string
      DatabaseName: string }

type Gender =
    | Male = 1
    | Female = 2

[<CLIMutable>]
type User =
    {
      Id: BsonObjectId
      Name: string
      LastName: Option<string>
      Email: string
      Age: int<age>
      Gender: Gender
      City: string }
