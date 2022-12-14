

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HashidsNet;
using OptimusNet;
// compare with CryptoMute sometime




BenchmarkRunner.Run<Tests>();

//Tests.Collisions();

Tests.SimpleUsage();



namespace OptimusNet
{
	public class Tests
	{
		[Benchmark(OperationsPerInvoke = 1000)]
		public long RunKnuth()
		{
			Optimus k = new Optimus(1580030173, 59260789, 1163945558);
			var code = k.Encode(123);
			var decode = k.Decode(code);

			return decode;
		}


		[Benchmark(OperationsPerInvoke = 1000)]
		public long RunKnuthBase64()
		{
			Optimus k = new Optimus(1580030173, 59260789, 1163945558);
			var code = k.EncodeBase64(123);
			var decode = k.DecodeBase64(code);

			return decode;
		}


		[Benchmark(OperationsPerInvoke = 1000)]
		public long RunKnuthBase64Url()
		{
			Optimus k = new Optimus(1580030173, 59260789, 1163945558);
			var code = k.EncodeBase64Url(123);
			var decode = k.DecodeBase64Url(code);

			return decode;
		}


		[Benchmark(OperationsPerInvoke = 1000)]
		public long RunHashId()
		{
			Hashids h = new Hashids("a salt");
			var code = h.Encode(123);
			var decode = h.DecodeSingle(code);

			return decode;
		}


		/// <summary>
		/// simple example of use. 
		/// </summary>
		public static void SimpleUsage()
		{
			var prime = PrimeTools.GetPrime();	// eg 605132081;
			var inv = PrimeTools.CalcInverse(prime);
			var rand = new Random().Next(1000000, int.MaxValue);

			// example usage
			Optimus k = new Optimus(prime, inv, rand);
			var initial = 123;

			var code = k.Encode(initial);
			var decode = k.Decode(code);

			Console.WriteLine($"inv of prime {prime} is {inv}; using random {rand}, encode {initial} to {code} and back to {decode}");

			var codeb64 = k.EncodeBase64(initial);
			var decodeb64 = k.DecodeBase64(codeb64);

			Console.WriteLine($"inv of prime {prime} is {inv}; using random {rand}, encode {initial} to {codeb64} and back to {decodeb64}");

			var codeb64u = k.EncodeBase64Url(initial);
			var decodeb64u = k.DecodeBase64Url(codeb64);

			Console.WriteLine($"inv of prime {prime} is {inv}; using random {rand}, encode {initial} to {codeb64u} and back to {decodeb64u}");
		}



		/// <summary>
		/// brute force run for all ints, see if encoded values clash
		/// </summary>
		public static void Collisions()
		{
			var prime = PrimeTools.GetPrime();
			var inv = PrimeTools.CalcInverse(prime);

			// example usage
			Optimus k = new Optimus(prime, inv, 1163945558);

			// test for collisions
			Console.WriteLine("Checking collisions");
			var codes = new SortedSet<int>();
			for (int i = 0; i < 100000000 /*int.MaxValue*/; i++)
			{
				if ((i & 1048575) == 0)
					Console.Write('.');

				if (!codes.Add(k.Encode(i)))
					Console.WriteLine("collision for {i}");
			}
			Console.WriteLine("Checked collisions");
		}
	}


}
