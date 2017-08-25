![Alt text](Logo.png "Nettle")

Nettle is a .**NET** **T**emplating **L**anguage **E**ngine designed as a lightweight solution to solving various content rendering problems faced in .NET applications. Typical problems Nettle can be used to solve include:

- Generating web pages for content management systems
- Generating emails or notifications
- Generating XML or CSV exports

## Usage

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
The Nettle templating language is simple, there are just six core concepts to learn:

### Comments
Comments allow you to annotate the code and are not rendered:

```
{{! This is a comment, it can span multiple lines}}
```

### Model Bindings
These are essentially the properties contained in the model. A string representation of the properties value will replace the binding place holder. The syntax for using a model binding is:

```
{{Name}}
```

Nested properties are also supported:

```
{{User.Identity.Name}}
```

The dollar sign $ can be used to explicitly denote a property. This is useful when properties are referenced outside of bindings (such as a for loop) where the {{ and }} tags are not supported. 

```
{{$Name}}
```

A single $ can be used to reference the scopes model. This is useful when inside a for loop where the collection is an array of strings.

```
{{foreach $Names}}
	{{$}}
{{endfor}}
```

Which would generate something like:

```
Craig
John
Simon
```

### Functions

Functions can take zero or more parameters, which can be a string literal, number, boolean, property or variable. The syntax for using a function is:

```
{{@Truncate("Hello World!", 5)}}
```

Which would generate:

```
Hello
```

There are various built in functions, these are:

- @FormatDate(Date, Format)
- @GetDate()
- @HtmlEncode(Text)
- @PadLeft(Text, TotalWidth, PaddingChar)
- @PadRight(Text, TotalWidth, PaddingChar)
- @Replace(OriginalText, FindText, ReplaceText)
- @Round(Number, Decimals)
- @ToInt64(Number)
- @Truncate(Text, Length)

### Variables

The value of a property or the result of a function can be assigned to a variable. This gets added to the model and can then be used further down the template. The syntax for creating a variable declaration and assignment is:

```
{{var truncatedText = @Truncate("Here is some text to truncate.", 10)}}

{{truncatedText}}
```

Which would generate:

```
Here is so
```

Nested property paths can also be referenced with variables:

```
{{var today = GetDate()}}

{{today.Day}}
```

In addition to properties and functions, variables can also be assigned a string literal, a number or another variable:

```
{{var pageTitle = "Nettle"}}
{{var vatRate = 20.0}}
{{var heading = pageTitle}}
```

### Iterators

_For each_ loops are supported with any property or variable that is of type IEnumerable. The syntax for using a _for each_ loop is:

```
{{foreach $RoleAssignments}}
	<p>Role {{RoleName}}</p>
{{endfor}}
```

Which would generate something like:

```
<p>Role Admin<p>
<p>Role Customer<p>
```

Nested loops are also supported:

```
{{foreach $Users}}
	<p>User {{UserName}}</p>

	{{foreach $RoleAssignments}}
		<p>Role {{RoleName}}</p>
	{{endfor}}
{{endfor}}
```

### Conditions

_If_ statements are supported with any property or variable that is either of type bool or can be resolved as true or false. The syntax for using an _if_ statement is:

```
{{if $Active}}
	<p>Currently Active</p>
{{endif}}
```

Nested _if_ statements are also supported:

```
{{if $Active}}
	<p>Currently Active</p>
	
	{{if HasProfile}}
		<p>The user has a profile</p>
	{{endif}}
{{endif}}
```
