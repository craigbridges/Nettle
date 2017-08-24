![Alt text](Logo.png "Nettle")

Nettle is a .**NET** **T**emplating **L**anguage **E**ngine designed as a lightweight solution to solving various content rendering problems faced in .NET applications. Typical problems Nettle can be used to solve include:

- Generating web pages for content management systems
- Generating emails or notifications
- Generating XML or CSV exports

Using Nettle is simple, there are just five core concepts to learn:

### Model Bindings
These are essentially the properties contained in the model. A string representation of the properties value will replace the binding place holder. The syntax for using a model binding is:

```
{{Name}}
```

Nested properties are also supported:

```
{{User.Identity.Name}}
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

- @Truncate(Text, Length)
- @Replace(OriginalText, FindText, ReplaceText)
- @HtmlEncode(Text)

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

### Iterators

_For each_ loops are supported with any property or variable that is of type IEnumerable. The syntax for using a _for each_ loop is:

```
{{foreach RoleAssignments}}
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
{{foreach Users}}
	<p>User {{UserName}}</p>

	{{foreach RoleAssignments}}
		<p>Role {{RoleName}}</p>
	{{endfor}}
{{endfor}}
```

### Conditions

_If_ statements are supported with any property or variable that is either of type bool or can be resolved as true or false. The syntax for using an _if_ statement is:

```
{{if Active}}
	<p>Currently Active</p>
{{endif}}
```

Nested _if_ statements are also supported:

```
{{if Active}}
	<p>Currently Active</p>
	
	{{if HasProfile}}
		<p>The user has a profile</p>
	{{endif}}
{{endif}}
```
