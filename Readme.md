# Small personal project to evaluate MsgPack for upcoming requirement.
Looking for the most efficient way to transfer data to/from a remote application.
Using https://github.com/neuecc/MessagePack-CSharp

## Step 1 'MsgPackSimpleFixtures' - Can serialize and deserialize a typical dto?
The most efficient payload is achieved by adding MessagePackObject attribute to each class and Key(n) attribute to each property.
Use MessagePackSerializer.ToJson(bytePayload) to view json
## Step 2 'MsgPackApiFixtures' - Can easily send binary payload to webapi endpoint
Output of MesagePackSerializer is a byte array. This is easily handled by a web api endpoint. 
Something like Request.Body.CopyToAsync(memoryStream) -> byte[] body = memoryStream.ToArray() -> MessagePackSerializer.Deserialize<T>(body)
## Step 3 'MstPackSaveToBlob' - Can save to azure blob in binary format and retrieve
Want to be able to dump incoming payloads to blob storage to be processed later by some job

## Other stuff 'BasicEncryption.linq'
Basic encryption utility in linqpad for encryptiong command audit logs that can be shipped with above....