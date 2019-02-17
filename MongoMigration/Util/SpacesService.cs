using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace MongoMigration.Util
{
	public class SpacesService
	{
		public SpacesService(IConfiguration configuration)
		{
			_s3 = new AmazonS3Client(configuration.GetValue<string>("Spaces:AccessKey"),
				configuration.GetValue<string>("Spaces:Secret"), new AmazonS3Config
				{
					ServiceURL = configuration.GetValue<string>("Spaces:Endpoint")
				});
			_bucket = configuration.GetValue<string>("Spaces:Bucket");
		}

		private readonly AmazonS3Client _s3;
		private readonly string _bucket;

		public async Task<PutObjectResponse> PutStream(Stream stream, string key)
		{
			return await _s3.PutObjectAsync(new PutObjectRequest
			{
				AutoCloseStream = true,
				InputStream = stream,
				Key = key,
				BucketName = _bucket
			});
		}

		public async Task<PutObjectResponse> Put(byte[] body, string key)
		{
			return await _s3.PutObjectAsync(new PutObjectRequest
			{
				AutoCloseStream = true,
				InputStream = new MemoryStream(body),
				Key = key,
				BucketName = _bucket
			});
		}

		public async Task<PutObjectResponse> PutFile(string path, string key)
		{
			return await _s3.PutObjectAsync(new PutObjectRequest
			{
				AutoCloseStream = true,
				FilePath = path,
				Key = key,
				BucketName = _bucket
			});
		}

		public async Task<GetObjectResponse> Get(string key)
		{
			return await _s3.GetObjectAsync(_bucket, key);
		}

		public async Task<DeleteObjectResponse> Delete(string key)
		{
			return await _s3.DeleteObjectAsync(_bucket, key);
		}
	}
}