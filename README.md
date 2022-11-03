# Optimus id transformation

A C# class to replicate the PHP "<a href="https://github.com/jenssegers/optimus">Optimus</a>" library.

Based on <a href="https://www.oreilly.com/library/view/art-of-computer/9780321635792/">Donald J Knuth's hash algorithm</a>, this will obfuscate an integer ID into another integer value. 

This can be useful for those who want obfuscated IDs, such as DB primary keys, and want obfuscation but do not want to alter their application to handle string equivalents. 
This class can be dropped in to replace original ints with encoded ints, simply encode and decode where required.

It is similar to <a href="https://github.com/ullmark/hashids.net">HashIds</a>, but will generate integers instead of random strings. It is also super fast.

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

To encode id's, create an instance of the class (totally thread safe, so a singleton is fine)

```c#
Optimus k = new Optimus(prime, inv, rand);
```

Encode a value
```c#
var code = k.Encode(123);
```

To decode the resulting code back to its original value, use the `decode` method:

```c#
var decode = k.Decode(code);
```

For those who really want strings, as your original value is now an obfuscated int, you can stringify it using any technique such as Base64 or UrlEncode.
`EncodeBase64` and `DecodeBase64` methods are provided to show how to do this along with the url-friendly version for your convenience.

## Performance
To show the relative speeds of the 3 methods (pure Knuth hash, hashed and Base64, HashIds library), benchmark code is included. the results are:

```
|            Method |      Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------ |----------:|----------:|----------:|-------:|-------:|----------:|
|          RunKnuth | 0.0041 ns | 0.0000 ns | 0.0000 ns | 0.0000 |      - |         - |
|    RunKnuthBase64 | 0.0688 ns | 0.0004 ns | 0.0003 ns | 0.0000 |      - |         - |
| RunKnuthBase64Url | 0.1481 ns | 0.0010 ns | 0.0010 ns | 0.0001 |      - |         - |
|         RunHashId | 8.2426 ns | 0.0471 ns | 0.0393 ns | 0.0012 | 0.0006 |       7 B |
```

Note the Knuth hasd can be further improved by a factor of 4 by instancing the class once. It is totally thread-safe.

While HashIds is fast - 8ms for 1000 encode/decode pairs, its embarrassed by the pure Knuth hashing at 4 microseconds.
Nearly 2,000 times faster using the superpower that is maths.


## License

The [MIT](https://opensource.org/licenses/MIT) License.