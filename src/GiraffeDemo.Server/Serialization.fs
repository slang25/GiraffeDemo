module GiraffeDemo.Server.Serialization
open System
open System.IO
open Giraffe.Serialization
open System.Text.Json
open System.Threading.Tasks

type SystemTextJsonSerializer (options: JsonSerializerOptions) =

        static member DefaultOptions =
           JsonSerializerOptions(
               PropertyNamingPolicy = JsonNamingPolicy.CamelCase
           )

        interface IJsonSerializer with
            member __.SerializeToString (x : 'T) =
                System.Text.Json.JsonSerializer.Serialize(x,  options)

            member __.SerializeToBytes (x : 'T) =
                System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(x, options)

            member __.SerializeToStreamAsync (x : 'T) (stream : Stream) =
                System.Text.Json.JsonSerializer.SerializeAsync(stream, x, options)

            member __.Deserialize<'T> (json : string) : 'T =
                System.Text.Json.JsonSerializer.Deserialize<'T>(json, options)

            member __.Deserialize<'T> (bytes : byte array) : 'T =
                System.Text.Json.JsonSerializer.Deserialize<'T>(Span<_>.op_Implicit(bytes.AsSpan()), options)

            member __.DeserializeAsync<'T> (stream : Stream) : Task<'T> =
                System.Text.Json.JsonSerializer.DeserializeAsync<'T>(stream, options).AsTask()
