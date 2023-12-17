# OtdrReader

[![License](https://img.shields.io/badge/license-MIT-blue)](https://github.com/claasd/OtdrReader/blob/main/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/CdIts.OtdrReader)](https://www.nuget.org/packages/CdIts.OtdrReader/)
[![Nuget](https://img.shields.io/nuget/vpre/CdIts.OtdrReader)](https://www.nuget.org/packages/CdIts.OtdrReader/)
[![CI](https://github.com/claasd/OtdrReader/actions/workflows/build.yml/badge.svg)](https://github.com/claasd/caffoa.net/actions/workflows/build.yml)


C#/NET implementation for reading SOR (Standard OTDR Record) V2 files. Currently there is not support for v1 files.

I followed the description of Sidney Li to implement this, he also has implementations in Perl, Python, Ruby and Clojure.
See his [Blog Post](https://morethanfootnotes.blogspot.com/2015/07/the-otdr-optical-time-domain.html) for more information about the SOR file format and his implementations.

To read a SOR file, use of one the static methods of the `OtdrReader` class:

```csharp

var otdrFromFile = await OtdrReader.ReadFileAsync("path/to/file.sor");
var otdrFromStream = await OtdrReader.ReadStreamAsync(stream);
var otdrFromByteArray = OtdrReader.ReadBytes(stream);
```

You can get then get the standard blocks from the otdr object:

```csharp
var version = otdr.Version;
var general = otdr.GetGeneralParameters();
var supplier = otdr.GetSupplierParameters();
var fixedParams = otdr.GetFixedParameters();
var keyEvents = otdr.GetKeyEvents();
var dataPointArray = otdr.GetDataPoints();
```
Currently, the frames only contain the raw data, no further processing is done.

You can get access to non-standard blocks the following way:
```csharp
var existingBlocks = GetBlockNames();
// get the byte array of the block
var block = otdr.GetBlock("NonStandradBlock");
// do something with the non-standard block
```

## License
MIT License

## Acknowledgements
Thanks to Sidney Li for his [Blog Post](https://morethanfootnotes.blogspot.com/2015/07/the-otdr-optical-time-domain.html) that helped me understand the different blocks.