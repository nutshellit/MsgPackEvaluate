using MessagePack;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MsgPackLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace MsgPackApiFixtures
{
    public class MsgPackSaveToBlob
    {
        [Fact]
        public async Task CanSave()
        {
            var ct = ComplexType1.Sample();
            var bytes = MessagePackSerializer.Serialize(ct);

            string connStr = "UseDevelopmentStorage=true";
            string containerName = "mobileuploads";
            CloudStorageAccount account = CloudStorageAccount.Parse(connStr);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            await container.SetPermissionsAsync(containerPermissions);
            string blobName = Guid.NewGuid().ToString();
            CloudBlockBlob photo = container.GetBlockBlobReference(blobName);
            await photo.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            string id =  photo.Uri.ToString();

            //download
            CloudBlockBlob photo1 = container.GetBlockBlobReference(blobName);
            byte[] bytesD = Array.Empty<byte>();
            using (var ms = new MemoryStream())
            {
                await photo1.DownloadToStreamAsync(ms);
                bytesD = ms.ToArray();
            }

            var ctDes = MessagePackSerializer.Deserialize<ComplexType1>(bytesD);
            Assert.Equal(ct.Simple1.First().Id, ctDes.Simple1.First().Id);

        }
    }
}
