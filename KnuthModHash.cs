using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace OptimusNet
{
	// https://ericlippert.com/2013/11/14/a-practical-use-of-multiplicative-inverses/
	// obfuscates integer IDs by multipying by a prime and xor masking the result.
	// result is reversible by unmasking then multiplying by the inverse of the prime so (key * INVERSE) & MaxValue == 1
	public class Optimus
	{
		private readonly int _prime, _inverse, _xor;

		public Optimus(int prime, int inverse, int random)
		{
			_prime = prime;
			_inverse = inverse;
			_xor = random;
		}

		public int Encode(int val)
		{
			return ((val * _prime) & int.MaxValue) ^ _xor;
		}

		public int Decode(int code)
		{
			return ((code ^ _xor) * _inverse) & int.MaxValue;
		}


		public string EncodeBase64(int val)
		{
			var plainTextBytes = BitConverter.GetBytes(val);
			return Convert.ToBase64String(plainTextBytes);
		}

		public int DecodeBase64(string val)
		{
			var base64EncodedBytes = Convert.FromBase64String(val);
			return BitConverter.ToInt32(base64EncodedBytes);
		}


		public string EncodeBase64Url(int val)
		{
			var plainTextBytes = BitConverter.GetBytes(val);
			return WebEncoders.Base64UrlEncode(plainTextBytes);

		}

		public int DecodeBase64Url(string val)
		{
			var base64EncodedBytes = WebEncoders.Base64UrlDecode(val);
			return BitConverter.ToInt32(base64EncodedBytes);
		}
	}
}
