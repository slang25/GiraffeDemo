module GiraffeDemo.Server.App

open System
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Giraffe.Serialization
open Newtonsoft.Json
open Serialization
open System.Text.Json
open System.Text.Json.Serialization

type Student =
    { id      : int
      name    : string
      age     : int
      status  : StudentStatus
      courses : List<Course>
    }
and Course =
    { title         : string
      enrolmentDate : DateTime }
and StudentStatus =
    | Active
    | Inactive

let studentHandler : HttpHandler =
    fun next ctx ->
        json { id      = 643
               name    = "Stuart Lang"
               age     = 33
               status  = Active
               courses = [
                   { title         = "Course A"
                     enrolmentDate = DateTime.UtcNow }
                   { title         = "Course B"
                     enrolmentDate = DateTime.UtcNow } ]
        } next ctx

let webApp =
    choose [
        route "/student" >=> GET >=> studentHandler
        RequestErrors.notFound (text "Not Found") ]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore
    let options = JsonSerializerOptions(IgnoreNullValues = true)
    options.Converters.Add(JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike))
    services.AddSingleton<IJsonSerializer>(SystemTextJsonSerializer(options)) |> ignore

[<EntryPoint>]
let main _ =
    WebHost.CreateDefaultBuilder()
        .Configure(configureApp)
        .ConfigureServices(configureServices)
        .Build().Run()
    0