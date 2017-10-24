![Alt text](Logo.png "Nettle")

Nettle is a .**NET** **T**emplating **L**anguage **E**ngine inspired by Handlebars, designed as a lightweight solution to solving various content rendering problems faced in .NET applications. Typical problems Nettle can be used to solve include:

- Generating web pages for content management systems
- Generating emails or notifications
- Generating administrable XML or CSV exports
- Generating administrable reports or documents

The main difference between Nettle and Handlebars is that Nettle makes use of functions and variables to enable data to be dynamically loaded (and manipulated) into a template. This could be useful in situations where the model passed to the template is very basic, but a requirement means that some additional, related data needs to be displayed. Instead of having to refactor, build and deploy the C# code (which could also create a code smell if the model becomes too cluttered), only the template needs to be edited.

## Usage
Install the NuGet package:
```
Install-Package Nettle
```

The following code will create a Nettle compiler and compile a simple template:
	
```c#
var source = @"Welcome {{Name}}";

var model = new
{
    Name = "John Smith"
};

var compiler = NettleEngine.GetCompiler();
var template = compiler.Compile(source);
var output = template(model);

/* Result:
Welcome John Smith
*/
```

## Nettle Language
See the [documentation section](https://github.com/craigbridges/Nettle/wiki) for more information.
