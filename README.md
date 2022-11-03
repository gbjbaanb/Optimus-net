# Optimus id transformation

A C# class to replicate the PHP "<a href="https://github.com/jenssegers/optimus">Optimus</a>" library.

Based on <a href="https://www.oreilly.com/library/view/art-of-computer/9780321635792/">Donald J Knuth's hash algorithm</a>, this will obfuscate an integer ID into another integer value. 
This can be useful for those who want obfuscated IDs, such as DB primary keys, and want obfuscation but do not want to alter their application to handle string equivalents. 
This class can be dropped in to replace original ints with encoded ints, simply encode and decode where required.

It is similar to HashIds, but will generate integers instead of random strings. It is also super fast.

Example of usage:

You need a large prime, its mod inverse, and a large random number. These will then turn the value 123 into a value like 1968939747 or the Base64 string "ewAAAA==".



## Installation

Its a simple class, copy and paste.

## Usage

To get started you will need 3 things;

 - Large prime number lower than `int.MaxValue`
 - The inverse prime so that `(PRIME * INVERSE) & MAXID == 1`
 - A large random integer lower than `int.MaxValue`

Functions are provided to generate these. See the PrimeTools.GetPrime and PrimeTools.CalcInverse static functions that will generate a large random prime number, and calculate its mod inverse.

Save those 3 values as they form thge "key" to encoding and decoding values with an instances of `Optimus(prime, inverse, random)`:

**NOTE**: Make sure that you are using the same constructor values throughout your entire application, or it wil not be able to decode encoded values correctly.

## Encoding and decoding

To encode id's, use the `encode` method:

```c#
Optimus k = new Optimus(prime, inv, rand);
var v = 111111111;
```

Encode a value, v.
```c#
var code = k.Encode(v);
```

To decode the resulting `30938859` back to its original value, use the `decode` method:

```php
var decode = k.Decode(code);
```

For those who really want strings, as your original value is now an obfuscated int, you can stringify it using any technique such as Base64 or UrlEncode.
`EncodeBase64` and `DecodeBase64` methods are provided to show how to do this along with the url-friendly version for your convenience.

## Performance
To show the relative speeds of the 3 methods (pure Knuth hash, hashed and Base64, HashIds library), benchmark code is included. the results for 1000 runs are:

```
|         Method |           Mean |        Error |       StdDev |
|--------------- |---------------:|-------------:|-------------:|
|       RunKnuth |       959.4 ns |      2.69 ns |      2.51 ns |
| RunKnuthBase64 |    65,095.7 ns |    394.60 ns |    369.10 ns |
|      RunHashId | 7,794,771.4 ns | 31,549.37 ns | 27,967.69 ns |
```

While HashIds is fast - 8ms for 1000 encode/decode pairs, its embarrassed by the pure Knuth hashing at less than 1 microsecond. Nearly 8,000 times faster using the superpower that is maths.


## License

The [MIT](https://opensource.org/licenses/MIT) License.