[![Build status](https://ci.appveyor.com/api/projects/status/t6dgpiamopwugu8v/branch/master?svg=true)](https://ci.appveyor.com/project/kentico/cloud-generators-net/branch/master)
[![Forums](https://img.shields.io/badge/chat-on%20forums-orange.svg)](https://forums.kenticocloud.com)

# Kentico Cloud model generator utility for .NET

This utility generates strongly-typed models based on Content Types in a Kentico Cloud project. The models are supposed to be used together with the [Kentico Cloud Delivery SDK for .NET](https://github.com/Kentico/delivery-sdk-net). Please read the [documentation](https://github.com/Kentico/delivery-sdk-net/wiki/Working-with-Strongly-Typed-Models-(aka-Code-First-Approach)#customizing-the-strong-type-binding-logic) to see all benefits of this approach.


## Get the tool

### Windows

Latest release: [Download](https://github.com/Kentico/cloud-generators-net/releases/latest)

Note: The application is [self-contained](https://www.hanselman.com/blog/SelfcontainedNETCoreApplications.aspx). There's no need to install any version of .NET on your machine.

### Linux, Mac OS and other platforms

* Clone the repository
* Navigate to the `cloud-generators-net\src\CloudModelGenerator` folder
* Run `dotnet build -r <RID>` to build the app
* Run `dotnet publish -c release -r <RID>` to publish the app

See the [list of all RIDs](https://docs.microsoft.com/en-us/dotnet/articles/core/rid-catalog).

## How to use

### Windows

```
CloudModelGenerator.exe --projectid "<projectid>" [--namespace "<custom-namespace>"] [--outputdir "<output-directory>"] [--withtypeprovider] [--structuredmodel] [--filenamesuffix "<suffix>"]
```

### Linux, Mac OS and other platforms
```
dotnet run --projectid "<projectid>" [--namespace "<custom-namespace>"] [--outputdir "<output-directory>"] [--withtypeprovider] [--structuredmodel] [--filenamesuffix "<suffix>"]
```

### Parameters

| Parameter             | Required  | Default value  | Description |
| --------------------- |:---------:|:--------------:|:-----------:|
| `--projectid` | True  | `null` | A GUID that can be found in [Kentico Cloud](https://app.kenticocloud.com) -> API keys -> Project ID |
| `--namespace` | False | `KenticoCloudModels` | A name of the [C# namespace](https://msdn.microsoft.com/en-us/library/z2kcy19k.aspx) |
| `--outputdir` | False | `\.` | An output folder path |
| `--withtypeprovider` | False | `true` | Indicates whether the `CustomTypeProvider` class should be generated (see [Customizing the strong-type binding logic](https://github.com/Kentico/delivery-sdk-net/wiki/Working-with-Strongly-Typed-Models-(aka-Code-First-Approach)#customizing-the-strong-type-binding-logic) for more info) |
| `--structuredmodel` | False | `false` | Generates `IRichTextContent` instead of `string` for rich-text elements. This enables utilizing [structured rich-text rendering](https://github.com/Kentico/delivery-sdk-net/wiki/Structured-Rich-text-rendering) |
| `--filenamesuffix` | False | `null` | Adds a suffix to generated filenames (e.g., News.cs becomes News.Generated.cs) |

These parameters can also be set via the appSettings.json file located in the same directory as the executable file. Command-line parameters always take precedence.


## Example output

```csharp
using System;
using System.Collections.Generic;
using KenticoCloud.Delivery;

namespace KenticoCloudModels
{
    public partial class CompleteContentType
    {
        public string Text { get; set; }
        public string RichText { get; set; }
        public decimal? Number { get; set; }
        public IEnumerable<MultipleChoiceOption> MultipleChoice { get; set; }
        public DateTime? DateTime { get; set; }
        public IEnumerable<Asset> Asset { get; set; }
        public IEnumerable<object> ModularContent { get; set; }
        public IEnumerable<TaxonomyTerm> Taxonomy { get; set; }
        public string UrlSlug { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}
```

## Feedback & Contributing
Check out the [contributing](https://github.com/Kentico/cloud-generators-net/blob/master/CONTRIBUTING.md) page to see the best places to file issues, start discussions and begin contributing.

### Wall of Fame
We would like to express our thanks to the following people who contributed and made the project possible:

- [Dražen Janjiček](https://github.com/djanjicek) - [EXLRT](http://www.exlrt.com/) 
- [Kashif Jamal Soofi](https://github.com/kashifsoofi)
- [Casey Brown](https://github.com/MajorGrits)

Would you like to become a hero too? Pick an [issue](https://github.com/Kentico/cloud-generators-net/issues) and send us a pull request!

