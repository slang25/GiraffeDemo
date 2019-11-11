module GiraffeDemo.ContractTests.Utils

open Newtonsoft.Json
open System.Text.Json
open System.Text.Json.Serialization

let checkCompatiblityWith<'T> input =
    let options = JsonSerializerOptions()
    options.IgnoreNullValues <- false
    options.Converters.Add(JsonFSharpConverter(JsonUnionEncoding.FSharpLuLike))

    let json = JsonSerializer.Serialize(input, options)

    let deserializeSettings = JsonSerializerSettings(NullValueHandling = NullValueHandling.Include, MissingMemberHandling = MissingMemberHandling.Error)
    let result = JsonConvert.DeserializeObject<'T>(json, deserializeSettings)
    result |> ignore
