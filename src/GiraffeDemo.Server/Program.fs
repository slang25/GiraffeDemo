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
    { Id      : int
      Name    : string
      Age     : int
      Status  : StudentStatus
      Courses : List<Course>
    }
and Course =
    { Title         : string
      EnrolmentDate : DateTime }
and StudentStatus =
    | Active
    | Inactive

let studentHandler : HttpHandler =
    fun next ctx ->
        json { Id      = 643
               Name    = "Stuart Lang"
               Age     = 33
               Status  = Active
               Courses = [
                   { Title         = "Course A"
                     EnrolmentDate = DateTime.UtcNow }
                   { Title         = "Course B"
                     EnrolmentDate = DateTime.UtcNow } ]
        } next ctx

let webApp =
    choose [
        route "/student" >=> GET >=> studentHandler
        RequestErrors.notFound (text "Not Found") ]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore
    let options = JsonSerializerOptions(IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
    options.Converters.Add(JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike))
    services.AddSingleton<IJsonSerializer>(SystemTextJsonSerializer(options)) |> ignore

[<EntryPoint>]
let main _ =
    WebHost.CreateDefaultBuilder()
        .Configure(configureApp)
        .ConfigureServices(configureServices)
        .Build().Run()
    0