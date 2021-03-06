# CloudBuffer Format Specification
CloudBuffers is a binary serialization format derived from FlatBuffers. Like FlatBuffers, CloudBuffers provides a highly-efficient binary format. 

## What’s wrong with FlatBuffers?
Not much! FlatBuffers represents a big step forward in efficiency over incumbent formats like protobuf, which optimizes for size.
However, CloudBuffers specifically aims to modernize parts of FlatBuffers:
-	Support for 56-bit addresses, so buffers can exceed 2GB.
-	Support for variable-sized integers (varints) to allow denser packing of data, all without losing random access.
-	A reduced emphasis on alignment, which reduces payload size.
-	A new vtable format, which preserves random access while reducing payload size.
-	In addition to tables, CloudBuffers canonically supports structs and unions as root types.

On the whole, CloudBuffers offers:
-	Random access
-	Denser packing of data than FlatBuffers
-	Comparable speed to FlatBuffers
-	Support for buffers up to 256TB.
-	A binary format suitable for use as a file format or RPC message format on both cloud scale and embedded devices.

## Structure
A CloudBuffer consists of the following elements:
#### Footer
Metadata about the type of buffer and a pointer to the root object. This is the last item in a CloudBuffer.

#### Tables
Tables are collections of optional fields. Each table has a corresponding VTable, which contains data about the relative offsets and presence of fields within a table. Tables are tolerant of new fields being added, so buffers remain compatible between schema versions.

####	Vectors and Strings
Vectors and strings are a set of contiguous elements with a prefixed length.

####	VTables
A VTable contains data about what fields are set in a table, and how they are laid out. VTables are used to solve 3 problems:
- Schema Extensibility (new fields added in later versions)
- Field presence detection (optional fields set / not set)
- Packing of data within a table

####	Structs
Structs are analogous to C structs. They are fixed size and may only contain structs and scalars. Fields in a struct are accessed by offsets. Structs are not versionable and fields are not optional.

####	Unions
CloudBuffer discriminated unions may contain strings, structs, or tables. A union is a discriminator byte and an offset.

Each of these elements has a contiguous memory layout. CloudBuffers are written front-to-back, with child elements being written before their parents. Therefore, the root element in a CloudBuffer will be the last item in the buffer.

## Primitives
Primitives are the building block of CloudBuffers. CloudBuffer schemas support the typical array of data types:
```
   int8, int16, int32, int64 (two's complement, little-endian)
   bool, uint8, uint16, uint32, uint64 (unsigned, little endian)
   float32, float64 (IEEE-754)
```
While `bool` is a primitive, it is actually encoded as a `uint8` with `true` corresponding to `1` and `false` corresponding to `0`. When decoding, 
a value of `0` corresponds to `false`, and any other value corresponds to `true` (ie, `!= 0`).

## Internal Primitives
One of the main drivers of data size in FlatBuffers is the notion that all internal offsets are fixed-width 32 bit integers. In practice, however, these offsets can usually
be expressed in one byte. CloudBuffers defines a variable-width offset type: `varoffset`. `varoffset` is a signed variable-width integer that uses a modified version of PrefixVarInt encoding:

| width | tag byte | range |
|-------|----------|-------|
|   1   | `x x x x x x 1 s` | `[-63, 63]` |
|   2   | `x x x x x 1 0 s` | `[-8191, 8191]` |
|   3   | `x x x x 1 0 0 s` | `[-(2^20 - 1), 2^20 - 1]` |
|   4   | `x x x 1 0 0 0 s` | `[-(2^27 - 1), 2^27 - 1]` |
|   5   | `x x 1 0 0 0 0 s` | `[-(2^34 - 1), 2^34 - 1]` |
|   6   | `x 1 0 0 0 0 0 s` | `[-(2^41 - 1), 2^41 - 1]` |
|   7   | `1 0 0 0 0 0 0 s` | `[-(2^48 - 1), 2^48 - 1]` |
|   8   | `0 0 0 0 0 0 0 s` | `[-(2^56 - 1), 2^56 - 1]` |

The `x` placeholders in the tag bytes are the least-significant bits of the encoded number. The `s` bit is the sign bit. A value of `1` indicates negative and a value of `0` indicates positive. The total width of the `varoffset` is equal to `TrailingZeroCount(tagByte & 0xF7)`. `TrailingZeroCount` is generally available via hardware intrinsics on modern platforms. `varoffset` is efficient because it can be read as a single `uint64` and stored in a single register.

#### Examples
|   Value     |    Encoding     |
|-------------|-----------------|
|     0       |    `0000 0010` or `0000 0011` |
|    25       | `0110 0110` |
|   -25       | `0110 0111` |

#### Sample Code
This is C# sample code that is mostly branchless for encoding and decoding `varoffset`s:
```c#
// 2^56 - 1 max supported value
private const ulong MaxSupportedValue = (1UL << 56) - 1;

[SkipLocalsInit]
public static ulong Encode(long value)
{
   bool isNegative = value < 0;
   ulong isNegativeArith = Unsafe.As<bool, byte>(ref isNegative);

   // Branchless absolute value.
   ulong absValue;
   {
       long mask = value >> 63;
       absValue = (ulong)((value + mask) ^ mask);
   }

   if (absValue >= MaxSupportedValue)
   {
       throw new OverflowException();
   }

   int bytesNeeded = (int)(((uint)BitOperations.Log2(absValue << 1) / 7) + 1);
   absValue <<= (bytesNeeded + 1);
   absValue |= (1UL << bytesNeeded);
   absValue |= isNegativeArith; // set LSB to 1 to indicate negative.

   return absValue;
}

public static long Decode(ulong value)
{
   const ulong ClearLowestBitMask = ulong.MaxValue << 1;

   bool isNegative = (value & 1UL) != 0;

   int byteCount = BitOperations.TrailingZeroCount(value & ClearLowestBitMask);
   value >>= byteCount + 1;

   if (isNegative)
   {
       return (long)value * -1;
   }

   return (long)value;
}
```

## VTables
A vtable stores information about which fields are present in table, where they are stored in a table. VTables are essential for supporting schema evolution as they allow `V(N)` and `V(N+1)` schemas to interoperate. VTables in CloudBuffers are structured as:
- A header byte indicating the length of the presence mask.
- The presence mask. The presence mask is a bit field up to 8 bytes in length, where each set bit indicates the presence of a particular field. CloudBuffer tables can support up to 64 fields per table.
- A set of offsets, encoded as `uint16` values. The first offset corresponds to the first bit in the presence mask. The number of offsets must be equal to the number of set bits in the presence mask. Each `uint16` represents the offset in the table of that field.

Each table stores a `varoffset` reference to its vtable. This allows multiple tables to point at a common vtable, if the offsets and presence mask are equal. Tables from different objects can also share a vtable, as long as they are equivalent.

### Example 1
```
01 6A 00 00 04 00 08 00 16 00
```
The first byte (`01`) indicates that the presence mask is one byte in length. The presence mask (`6A`) is represented in binary as: `0110 1010`. This indicates that field indexes `1`, `3`, `5`, and `6` are included in the table.

Using the presence mask to decode the vtable, yields this result:

| Table Field Index | Included? | Offset |
|-------------------|-----------|--------|
| 0                 | No        |   -1   |
| 1                 | Yes       |    0   |
| 2                 | No        |   -1   |
| 3                 | Yes       |    4   |
| 4                 | No        |   -1   |
| 5                 | Yes       |    8   |
| 6                 | Yes       |   16   |
| 7                 | No        |   -1   |

The format of the CloudBuffer vtable is quite dense, and needs the population count instruction (`popcnt` in x86 or `popcount` on ARM) to allow random access reads to the vtable. Otherwise, a loop may be used, but the result will be quite slow.
