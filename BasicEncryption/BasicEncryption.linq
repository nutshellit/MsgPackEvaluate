<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	var dto = new Dto{Id = Guid.NewGuid(), Name = "name123", Address = "myadd"};
	string orgTxt = JsonConvert.SerializeObject(dto);
	string encTxt = AES.Encrypt(orgTxt, "12345");
	encTxt.Dump();
	
	string deccryp = AES.Decrypt(encTxt, "12345");
	deccryp.Dump();

	string xx = "tScxeoPdaG9lbHGiZRwMJVqKzZe6WaUxOQq3I7BgZAcPp6IglXwC2UDgCI1Ap0J5WbEkhsKtRCWMkdRymPuud+a4A/q3aDD3pDWmVv+8Fy6fhOhZCYiOgRvjqZ8nWzJa";
	string deccryp1 = AES.Decrypt(xx, "123456");
	deccryp1.Dump();

}

public class Dto
{ 
	public Guid Id {get;set;}
	public string Name {get;set;}
	public string Address {get;set;}
}


public static class AES
{
	private const string _SALT = "g46dzQ80";
	private const string _INITVECTOR = "OFRna74m*aze01xY";

	private static byte[] _saltBytes;
	private static byte[] _initVectorBytes;

	static AES()
	{
		_saltBytes = Encoding.UTF8.GetBytes(_SALT);
		_initVectorBytes = Encoding.UTF8.GetBytes(_INITVECTOR);
	}

	/// <summary>
	/// Encrypts a string with AES
	/// </summary>
	/// <param name="plainText">Text to be encrypted</param>
	/// <param name="password">Password to encrypt with</param>   
	/// <param name="salt">Salt to encrypt with</param>    
	/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
	/// <returns>An encrypted string</returns>        
	public static string Encrypt(string plainText, string password, string salt = null, string initialVector = null)
	{
		return Convert.ToBase64String(EncryptToBytes(plainText, password, salt, initialVector));
	}

	/// <summary>
	/// Encrypts a string with AES
	/// </summary>
	/// <param name="plainText">Text to be encrypted</param>
	/// <param name="password">Password to encrypt with</param>   
	/// <param name="salt">Salt to encrypt with</param>    
	/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
	/// <returns>An encrypted string</returns>        
	public static byte[] EncryptToBytes(string plainText, string password, string salt = null, string initialVector = null)
	{
		byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
		return EncryptToBytes(plainTextBytes, password, salt, initialVector);
	}

	/// <summary>
	/// Encrypts a string with AES
	/// </summary>
	/// <param name="plainTextBytes">Bytes to be encrypted</param>
	/// <param name="password">Password to encrypt with</param>   
	/// <param name="salt">Salt to encrypt with</param>    
	/// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
	/// <returns>An encrypted string</returns>        
	public static byte[] EncryptToBytes(byte[] plainTextBytes, string password, string salt = null, string initialVector = null)
	{
		int keySize = 256;

		byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
		byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
		byte[] keyBytes = new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);

		using (RijndaelManaged symmetricKey = new RijndaelManaged())
		{
			symmetricKey.Mode = CipherMode.CBC;

			using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
			{
				using (MemoryStream memStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
					{
						cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
						cryptoStream.FlushFinalBlock();

						return memStream.ToArray();
					}
				}
			}
		}
	}

	/// <summary>  
	/// Decrypts an AES-encrypted string. 
	/// </summary>  
	/// <param name="cipherText">Text to be decrypted</param> 
	/// <param name="password">Password to decrypt with</param> 
	/// <param name="salt">Salt to decrypt with</param> 
	/// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
	/// <returns>A decrypted string</returns>
	public static string Decrypt(string cipherText, string password, string salt = null, string initialVector = null)
	{
		byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(' ', '+'));
		return Decrypt(cipherTextBytes, password, salt, initialVector).TrimEnd('\0');
	}

	/// <summary>  
	/// Decrypts an AES-encrypted string. 
	/// </summary>  
	/// <param name="cipherText">Text to be decrypted</param> 
	/// <param name="password">Password to decrypt with</param> 
	/// <param name="salt">Salt to decrypt with</param> 
	/// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
	/// <returns>A decrypted string</returns>
	public static string Decrypt(byte[] cipherTextBytes, string password, string salt = null, string initialVector = null)
	{
		int keySize = 256;

		byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
		byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
		byte[] keyBytes = new Rfc2898DeriveBytes(password, saltValueBytes).GetBytes(keySize / 8);
		byte[] plainTextBytes = new byte[cipherTextBytes.Length];

		using (RijndaelManaged symmetricKey = new RijndaelManaged())
		{
			symmetricKey.Mode = CipherMode.CBC;

			using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
			{
				using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
				{
					using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
					{
						int byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

						return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
					}
				}
			}
		}
	}
}