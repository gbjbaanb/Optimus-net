using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimusNet
{

	/// <summary>
	/// Simple helpers to get the seed data for the algorithm.
	/// You need a large prime number, its inverse, and a large random number.
	/// 
	/// Alternatively use 
	///		https://primes.utm.edu/lists/small/millions/ to pick a prime.
	///		https://planetcalc.com/3311/ - to calculate the inverse (prime goes in the number field, 2147483648 in modulo (ie maxval+1))
	///		
	/// </summary>
	public static class PrimeTools
	{
		/// <summary>
		/// gets a random prime between a boundary of min and max possible values (so we don't get small primes or overflow)
		/// </summary>
		/// <returns></returns>
		public static int GetPrime()
		{
			const int minimum = 1000000;
			var r = new Random();
			int p = r.Next(minimum, int.MaxValue - minimum);

			while (!IsPrime(p) && p < int.MaxValue)
				p++;

			return p;
		}

		private static bool IsPrime(int number)
		{
			// quick optimisation, primes are not divisible by 2 or 3
			if (number % 2 == 0 || number % 3 == 0)
				return false;
			else
			{
				// is it divisible by any number between the first next prime (5) and itself.
				int i;
				for (i = 5; i * i <= number; i += 6)
				{
					if (number % i == 0 || number % (i + 2) == 0)
						return false;
				}
				return true;
			}
		}



		/// <summary>
		/// calculate the modulo inverse (not the multiplicative inverse)
		/// online calculator to do the same:
		///		https://planetcalc.com/3311/ - key in number field, 2147483648 in modulo (ie maxval+1)
		/// </summary>
		/// <param name="prime"></param>
		/// <returns>inverse, or -1 on error</returns>
		public static int CalcInverse(int prime)
		{
			long modulo = (long)int.MaxValue + 1;
			long number = prime;

			if (prime < 1) 
				throw new ArgumentOutOfRangeException(nameof(prime));

			long n = number;
			long m = modulo, v = 0, d = 1;
			while (n > 0)
			{
				long t = m / n, x = n;
				n = m % x;
				m = x;
				x = d;
				d = checked(v - t * x); // Just in case
				v = x;
			}
			long result = v % modulo;
			if (result < 0) result += modulo;
			if (number * result % modulo == 1L) return (int)result;

			return -1;
		}
	}
}

