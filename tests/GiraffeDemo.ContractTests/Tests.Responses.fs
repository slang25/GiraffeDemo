module GiraffeDemo.ContractTests.Tests.Responses

open Expecto
open GiraffeDemo.ContractTests.Utils
open GiraffeDemo

// These tests will fail if we have added or changed the shape of the API response that has not been reflected in the OpenAPI file
[<Tests>]
let tests =
    testList "Check Response Types" [
        testProperty "Check Student" <| fun (response:Server.App.Student) ->
            response |> checkCompatiblityWith<Client.Student>
    ]
