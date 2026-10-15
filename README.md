# C# & .NET Knowledge Base — Corrected Study Guide

> **Goal:** Understand C# and .NET deeply enough to explain the ideas, write reliable code, and discuss them in interviews.
>
> **Main target:** **.NET 8 + C# 12**, running on **Linux or Windows**. Modern .NET 9+ differences are labelled. Each code example is a **separate example** unless it explicitly says otherwise. Examples using `public class ...` may be placed below top-level statements in `Program.cs` or in separate files.
>
> **Reading rule:** For every feature ask: **What problem did it solve? → How does it work? → When should I use it? → What could go wrong?**

## Contents

1. [The .NET runtime: from source code to CPU](#1-the-net-runtime-from-source-code-to-cpu)
2. [Values, references, memory, GC, and cleanup](#2-values-references-memory-gc-and-cleanup)
3. [Mutable and immutable objects](#3-mutable-and-immutable-objects)
4. [Generics and collections](#4-generics-and-collections)
5. [Strings and `StringBuilder`](#5-strings-and-stringbuilder)
6. [Operator overloading](#6-operator-overloading)
7. [Delegates, lambdas, and closures](#7-delegates-lambdas-and-closures)
8. [Events](#8-events)
9. [Attributes and custom attributes](#9-attributes-and-custom-attributes)
10. [Reflection](#10-reflection)
11. [Serialization](#11-serialization)
12. [Processes, threads, and concurrency](#12-processes-threads-and-concurrency)
13. [Race conditions, locks, and synchronization](#13-race-conditions-locks-and-synchronization)
14. [Tasks and async/await](#14-tasks-and-asyncawait)
15. [Parallel programming](#15-parallel-programming)
16. [Configuration, options, and secrets](#16-configuration-options-and-secrets)
17. [Logging and Windows-only features](#17-logging-and-windows-only-features)
18. [Cryptography and password storage](#18-cryptography-and-password-storage)
19. [LINQ and query execution](#19-linq-and-query-execution)
20. [Exception handling](#20-exception-handling)
21. [Dependency inversion and dependency injection](#21-dependency-inversion-and-dependency-injection)
22. [EF Core and `DbContext` fundamentals](#22-ef-core-and-dbcontext-fundamentals)
23. [Interview questions and exercises](#23-interview-questions-and-exercises)
24. [Corrections made to the original notes](#24-corrections-made-to-the-original-notes)
25. [Official documentation](#25-official-documentation)

---

## 1. The .NET runtime: from source code to CPU

### The problem
A CPU understands **machine instructions**, not C# source code. We need something to translate and execute our program.

### Core terms

| Term | Plain English | Job |
|---|---|---|
| **C#** | A programming language | Express program logic. |
| **.NET SDK** | Tools for building programs | Compiler, `dotnet build`, templates, CLI. |
| **Assembly** | Usually a compiled `.dll` or `.exe` | Holds code and information describing that code. |
| **IL/CIL** | Intermediate Language | Code emitted by a normal C# compilation. |
| **Metadata** | Description of types and members | Tells .NET what classes, methods, fields, attributes, etc. exist. |
| **CLR** | Common Language Runtime | Runs managed code and supplies services like garbage collection and exception handling. |
| **JIT** | Just-in-time compiler | Turns IL into native instructions for the current CPU. |
| **BCL** | Base Class Library | Ready-made types: `string`, `List<T>`, `Stream`, etc. |

### Normal execution on .NET 8

```text
Program.cs (C# source)
        |
        v
C# compiler (Roslyn)
        |
        v
MyApp.dll (IL + metadata + manifest)
        |
        v
CLR loads and checks types
        |
        v
JIT compiles methods to native instructions
        |
        v
CPU executes instructions
        |
        +---- CLR helps with exceptions, GC, threads, etc.
```

**Important:** This is the normal **JIT-based** execution path, not the only possible path. **Native AOT** can compile an application ahead of time into native code without a JIT at runtime. AOT requires care with dynamic features such as reflection and runtime code generation.

The JIT often starts with quick compilation, then recompiles frequently executed methods with stronger optimizations (**tiered compilation**). It does **not** necessarily compile every method before the program starts.

### Example

```csharp
using System.Reflection;

Console.WriteLine(typeof(int).FullName);  // System.Int32

Assembly assembly = typeof(string).Assembly;
Console.WriteLine(assembly.GetName().Name);
```

`typeof(string)` gets the `Type` that describes `System.String`. Reflection uses runtime type metadata; the C# compiler does not need to examine source code at runtime.

### Common misunderstandings

- **Namespace ≠ assembly:** A namespace groups names. An assembly is a compiled deployment/versioning unit. One namespace can span assemblies.
- **C# ≠ .NET:** Other languages such as F# can target .NET.
- **Managed ≠ magically safe:** Managed code still has bugs, leaks, data races and security vulnerabilities.
- **Everything is not JIT-compiled:** Native AOT and some precompiled runtime paths exist.

**Remember:** `C# → (usually) IL + metadata → CLR → native machine code`.

---

## 2. Values, references, memory, GC, and cleanup

### The problem
Why can modifying one variable affect another variable in some cases but not others?

### Value type vs reference type

A **value type** stores its data as a value. Copying it normally copies the data.

A **reference type** variable stores a reference to an object. Copying the variable copies the **reference**, so both variables can refer to the **same object**.

```csharp
// The classes/structs belong below top-level statements or in separate files.
var firstPoint = new Point(1);
var secondPoint = firstPoint;
secondPoint.X = 99;
Console.WriteLine(firstPoint.X);       // 1: separate value

var firstUser = new User { Name = "Ali" };
var secondUser = firstUser;
secondUser.Name = "Sara";
Console.WriteLine(firstUser.Name);     // Sara: same object

public struct Point
{
    public int X;
    public Point(int x) => X = x;
}

public class User
{
    public string Name { get; set; } = "";
}
```

```text
VALUE TYPE                  REFERENCE TYPE
p1 = [ X:1 ]                a ----+
p2 = [ X:99 ]                     |
                                  v
                            [ User: Sara ]
                            ^
                            |
                       b ---+
```

| Category | Examples | What assignment copies |
|---|---|---|
| Value types | `int`, `bool`, `decimal`, `enum`, `struct`, `(int, int)` | Their value/data |
| Reference types | `class`, `string`, arrays, delegates, interfaces | A reference to an object |

### Stack vs heap: do not memorize the wrong rule

**Wrong rule:** "Every struct lives on the stack; every class lives on the heap."

**Better rule:** The distinction is **value semantics vs reference semantics**, **not** a guaranteed location. Value-type fields can live *inside* heap objects. The runtime can place locals in registers, stack storage, or optimized-away locations. Regular reference-type objects are normally on the managed heap.

### Boxing and unboxing

```csharp
int original = 10;
object boxed = original;       // Boxing: copy int into an object
original = 20;
int restored = (int)boxed;    // Unboxing: check type, copy the value out
Console.WriteLine(restored);  // 10
```

**Why can boxing cost performance?** Usually it allocates an object and creates work for the garbage collector. Generic APIs such as `List<int>` avoid ordinary boxing when storing integers.

### Garbage collection (GC)

The GC manages **managed memory**. Its main idea is reachability:

```text
Live roots (locals, statics, handles, etc.)
       |
       +--> reachable object A --> object B   (keep)

unreachable object C                       (eligible for collection)
```

The GC often uses generations (**Gen 0, 1, 2**) because most new objects die young. "Unreachable" means **eligible** for collection, not "deleted immediately". A long-lived event subscription or static collection can keep objects reachable and cause a memory leak even though the GC works correctly.

### `IDisposable` and `using`: the other kind of cleanup

The GC manages memory but is **not a deterministic way to release resources** such as open files, sockets, database connections or OS handles. A type that owns these may implement `IDisposable` or `IAsyncDisposable`.

```csharp
File.WriteAllText("data.txt", "Example text"); // Create a small demo file
using var stream = File.OpenRead("data.txt");
Console.WriteLine(stream.Length);
// At the end of this scope, Dispose() is called, even if an exception occurs.
```

Conceptually:

```csharp
FileStream stream = File.OpenRead("data.txt"); // Assumes demo file exists
try
{
    Console.WriteLine(stream.Length);
}
finally
{
    stream.Dispose();
}
```

These two snippets are **alternatives**, not meant to be pasted together. Use `await using` for objects that support asynchronous disposal, such as some database connections and streams.

**Important nuance:** Some finalizers can release native resources eventually, but their timing is unpredictable. `Dispose` is the standard explicit cleanup mechanism.

### When to use

- Prefer `class` for entities/services and objects with identity.
- Prefer small, immutable `readonly struct` types for value-like data, where appropriate.
- Use `using`/`await using` around resources that **you own**.
- Do not manually call `GC.Collect()` to "optimize" a normal application.

**Remember:** A value is copied; a reference may be shared. **GC** handles managed memory; **Dispose** handles timely resource cleanup.

---

## 3. Mutable and immutable objects

### The problem
Unexpected changes to shared data cause hard-to-find bugs.

**Mutable** means an object's observable state can change after creation. **Immutable** means it cannot change after creation; an apparent change creates another value/object.

```csharp
string a = "Hi";
string b = a;
a += " there";
Console.WriteLine(b); // Hi. Original string unchanged.

int[] nums = { 1, 2 };
int[] sameArray = nums;
sameArray[0] = 99;
Console.WriteLine(nums[0]); // 99. Array is mutable.
```

### The important exceptions

- `string` is a **reference type** but immutable.
- `Tuple<T1,T2>` has read-only item properties (a tuple can still **contain mutable objects**).
- `(x: 1, y: 2)` is a **`ValueTuple` struct with mutable fields**.
- `record class` with positional parameters has **init-only properties**, but referenced lists/objects can still change.
- A positional `record struct` is mutable by default; `readonly record struct` is designed to prevent instance-state mutation.
- `readonly List<string> names` means the **field cannot be reassigned**; `names.Add(...)` still works.
- `IReadOnlyList<T>` exposes read-only operations; it **does not promise** that the underlying collection never changes.

```csharp
var pair = (Left: 1, Right: 2);
pair.Left = 100;                           // Allowed

var old = new PersonRecord("Ali", 20);
var updated = old with { Age = 21 };       // New record instance
Console.WriteLine(old.Age);                // 20

public record PersonRecord(string Name, int Age);
```

### Why immutability is useful

- Easier reasoning: the data you read cannot silently change through another reference.
- Safer concurrent access when the **entire shared state is genuinely immutable**.
- Stable dictionary keys when equality and hashing agree.

**Caution:** Immutable data alone does not make an entire program thread-safe. Shared variables that point to different immutable objects may still need synchronization when updated.

**Remember:** `readonly`, `init`, `record`, and `IReadOnlyList<T>` have **different guarantees**. Do not automatically call all of them deeply immutable.

---

## 4. Generics and collections

### The problem: using `object` loses type safety

Without generics, a collection can accept unrelated data and failures appear later:

```csharp
System.Collections.ArrayList oldList = new();
oldList.Add(10);
oldList.Add("surprise");
// int x = (int)oldList[1]!;  // Runtime InvalidCastException
```

### The solution: a placeholder type `T`

```csharp
List<int> ages = new() { 18, 23, 30 };
// ages.Add("hello");  // Compile-time error

static T PickFirst<T>(T first, T second) => first;
Console.WriteLine(PickFirst(3, 8));          // 3
Console.WriteLine(PickFirst("a", "b"));      // a
```

### How it works internally

In .NET, generic type arguments are available **at runtime** (generics are *reified*). `List<int>` and `List<string>` are distinct constructed runtime types. In typical JIT execution, value-type instantiations often get specialized code, while many reference-type instantiations can share native code. These are implementation techniques, **not a promise of zero overhead in every generic operation**.

### Constraints

```csharp
public interface IEntity
{
    int Id { get; }
}

public sealed class MemoryStore<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _items = new();
    public void Add(T item) => _items[item.Id] = item;
    public T? Find(int id) => _items.GetValueOrDefault(id);
}
```

`where T : class` restricts T to reference types. `where T : IEntity` requires the interface contract. `where T : new()` requires a public parameterless constructor (and must be last in its constraint list). Other constraints include `struct`, `notnull`, `unmanaged`, and specific base classes/interfaces.

**Note:** This is an **in-memory teaching example**, not a recommendation to build a generic repository around every EF Core entity.

### Collections: choosing a tool

| Type | Main job | Typical cost* |
|---|---|---|
| `T[]` | Fixed-length, indexable sequence | Index O(1) |
| `List<T>` | Growable indexable sequence | Index O(1), linear search O(n) |
| `Dictionary<TKey,TValue>` | Find values by key | Lookup O(1) average |
| `HashSet<T>` | Membership and uniqueness | Contains O(1) average |
| `Queue<T>` | FIFO (first in, first out) | Enqueue/dequeue O(1) amortized |
| `Stack<T>` | LIFO (last in, first out) | Push/pop O(1) amortized |
| `LinkedList<T>` | Linked nodes | Search O(n); edit given node O(1) |
| `SortedDictionary<TKey,TValue>` | Sorted key lookup | O(log n) |
| `ConcurrentDictionary<TKey,TValue>` | Concurrent key/value operations | Thread-safe individual operations |
| `ImmutableList<T>` | Persistent immutable collection | Depends on operation; changes return a new collection |

*Complexity is a typical asymptotic description, not a fixed speed guarantee. Hash tables have collision/resizing costs. `Dictionary` should not be treated as a contractually sorted collection.

### `IEnumerable<T>` is NOT a stored collection

`IEnumerable<T>` means **"you can enumerate a sequence of T values."** It might represent a list, an iterator that produces values lazily, or an EF Core database query. It does not guarantee count, random access, or cheap repeated enumeration.

```csharp
IEnumerable<int> evens = Enumerable.Range(1, 6).Where(x => x % 2 == 0);
foreach (int n in evens) Console.Write(n + " ");  // 2 4 6
```

**Remember:** Generics = reusable + type-checked code. Choose collections based on **operations**. `IEnumerable<T>` = ability to enumerate, not necessarily a list in memory.

---

## 5. Strings and `StringBuilder`

### The problem
Strings are immutable. Repeatedly appending to a growing string can allocate many intermediate strings and repeat copying.

```csharp
string text = "";
for (int i = 0; i < 10_000; i++)
    text += i.ToString();   // Repeated strings and copying
```

### The solution

```csharp
using System.Text;

var builder = new StringBuilder();
for (int i = 0; i < 10_000; i++)
    builder.Append(i);

string text = builder.ToString();
Console.WriteLine(text.Length);
```

`StringBuilder` stores mutable character data (implemented with chunks in .NET) and can grow without rebuilding **one complete immutable string on every append**. `ToString()` produces a string; calling it repeatedly inside the loop loses much of the benefit.

### When NOT to use it

```csharp
string fullName = firstName + " " + lastName;
string csv = string.Join(",", values);
```

For a few strings or a known collection, simple concatenation/interpolation/`Join` is often clearer and efficient. `StringBuilder` itself is **not thread-safe**.

**Remember:** Few parts → simple string operations. Many repeated appends → consider `StringBuilder`.

---

## 6. Operator overloading

### The problem
A domain type such as money or a vector may naturally support `+`, `-`, or comparisons. Operators can improve readability **when their meaning is obvious**.

### A safe-ish small example: money with currency

```csharp
var a = new Money(10m, "USD");
var b = new Money(5m, "USD");
Console.WriteLine(a + b);           // 15 USD

public readonly record struct Money(decimal Amount, string Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Currencies must match.");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public override string ToString() => $"{Amount} {Currency}";
}
```

### Rules and pitfalls

- C# 12 user-defined binary operators are typically `public static` and require an operand of the containing type.
- Some comparison operators come in **required pairs**: `==`/`!=`, `<`/`>`, `<=`/`>=`.
- `&&` and `||` cannot be overloaded directly (custom logical behavior uses other operator rules).
- **In C# 12**, `a += b` uses the corresponding `+` operator and assignment for user-defined types. **C# 14** later introduced explicit user-defined compound-assignment operators.
- If customizing equality on a regular type, keep `==`, `Equals`, and `GetHashCode` consistent.
- **Do not** implicitly turn `Money` into `decimal`: the amount would lose its currency meaning.
- Do not compare monetary amounts in different currencies unless you have an **explicit exchange-rate policy**.

**Remember:** Overloading an operator is not the same as **overriding** a virtual method.

---

## 7. Delegates, lambdas, and closures

### The problem: pass behavior into another method

Sometimes you want a method to accept **what it should do**, not only data.

```csharp
static void ProcessNumbers(IEnumerable<int> numbers, Func<int, bool> condition)
{
    foreach (int number in numbers)
        if (condition(number)) Console.WriteLine(number);
}

ProcessNumbers(new[] { 1, 2, 3, 4 }, n => n % 2 == 0); // 2, 4
```

### What a delegate is

A **delegate** is a type-safe representation of a callable method (and, for instance methods, the target object). The compiler can turn method groups and lambdas into delegate instances.

| Delegate type | Meaning | Example |
|---|---|---|
| `Action<T>` | Takes T, returns nothing | `Action<string> log = Console.WriteLine;` |
| `Func<T, TResult>` | Takes T, returns a result | `Func<int,bool> even = x => x % 2 == 0;` |
| `Predicate<T>` | Takes T, returns `bool` | `Predicate<int> positive = x => x > 0;` |

### Multicast delegates

```csharp
Action actions = () => Console.Write("A");
actions += () => Console.Write("B");
actions(); // AB
```

Delegates are immutable objects. `+=` typically creates a **combined delegate**; it does not mutate an existing delegate instance. Invocation calls handlers **in order**, synchronously by default. If a handler throws, later handlers are not called by normal multicast invocation. If multicast delegates return a value, you receive the **last** return value.

### Closures: capture variables, not a snapshot

```csharp
var wrong = new List<Action>();
for (int i = 0; i < 3; i++)
    wrong.Add(() => Console.Write(i));

foreach (Action action in wrong) action(); // 333

Console.WriteLine();
var right = new List<Action>();
for (int i = 0; i < 3; i++)
{
    int copy = i;
    right.Add(() => Console.Write(copy));
}
foreach (Action action in right) action(); // 012
```

The loop variable `i` is **one captured variable**, updated over time. A new `copy` declared in each iteration gives each lambda its own variable. `foreach` iteration variables have different capture behavior in modern C# and generally do not have this `for` pitfall.

**Remember:** Delegate = callable reference. Lambda = convenient way to write functions. Closure = function + captured surrounding variables.

---

## 8. Events

### The problem
An object wants to announce something without knowing **who** is listening. A public delegate field is too permissive: external code could invoke or overwrite it.

### The solution: `event`

```csharp
var orderService = new OrderService();
EventHandler<OrderPlacedEventArgs> handler = (_, e) =>
    Console.WriteLine($"Send receipt for order {e.OrderId}");

orderService.OrderPlaced += handler;
orderService.PlaceOrder(42);           // Send receipt for order 42
orderService.OrderPlaced -= handler;  // Remove subscription

public sealed class OrderPlacedEventArgs : EventArgs
{
    public int OrderId { get; init; }
}

public sealed class OrderService
{
    public event EventHandler<OrderPlacedEventArgs>? OrderPlaced;

    public void PlaceOrder(int orderId)
    {
        // Actual application: save successfully before announcing.
        OrderPlaced?.Invoke(this, new OrderPlacedEventArgs { OrderId = orderId });
    }
}
```

```text
OrderService (publisher)
       |
       | event: OrderPlaced
       v
  subscribers notified
       |       \
     email    audit
```

### Important rules

- A **field-like event** allows outside code to `+=` and `-=`, not invoke or replace the invocation list.
- Events do not automatically make an operation asynchronous or durable.
- The publisher normally invokes subscribed handlers on the **current thread**. One slow handler can delay the publisher.
- Subscribers can be kept alive by a long-lived publisher. Unsubscribe when needed.
- For microservices, an in-process C# `event` is **not** a replacement for durable messaging (such as a message broker).

**Remember:** A delegate is a callable value; an event gives controlled **subscribe/unsubscribe** access.

---

## 9. Attributes and custom attributes

### The problem
Frameworks need a way to attach information to types and members without hard-coding that information into every method.

An **attribute** is metadata written using brackets, for example `[Obsolete]`, `[Required]`, `[HttpGet]`, or `[JsonIgnore]`.

```csharp
[Obsolete("Use CalculateTotal instead.")]
public static int OldTotal(int a, int b) => a + b;
```

### How attributes work

```text
Source: [SomeAttribute("x")]
         |
         v
Compiler records attribute information in assembly metadata
         |
         +---- compiler may react directly (e.g. [Obsolete])
         |
         +---- a framework may inspect it (e.g. ASP.NET routing)
         |
         +---- your program may read it with reflection
```

**Important nuance:** An attribute is not a magic method call. A framework, compiler, serializer, or your own code must interpret it. When code requests instantiated attribute objects via reflection, the runtime can construct them from the metadata; simply loading the assembly does not instantiate every attribute object.

### Custom attribute with a complete reader

```csharp
using System.Reflection;

var product = new Product { Name = "Very long product name" };
Console.WriteLine(string.Join("; ", Validate(product)));
// Name may contain at most 10 characters.

static List<string> Validate(object instance)
{
    var errors = new List<string>();

    foreach (PropertyInfo property in instance.GetType().GetProperties())
    {
        var limit = property.GetCustomAttribute<MaxTextLengthAttribute>();
        if (limit is null) continue;

        if (property.GetValue(instance) is string value && value.Length > limit.Limit)
            errors.Add($"{property.Name} may contain at most {limit.Limit} characters.");
    }
    return errors;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class MaxTextLengthAttribute : Attribute
{
    public int Limit { get; }
    public MaxTextLengthAttribute(int limit) => Limit = limit;
}

public sealed class Product
{
    [MaxTextLength(10)]
    public string Name { get; set; } = "";
}
```

Step by step:

1. The compiler writes the attribute and its argument (`10`) into metadata.
2. `GetProperties()` discovers properties on the runtime `Product` type.
3. `GetCustomAttribute<MaxTextLengthAttribute>()` obtains the attribute value.
4. **Our `Validate` method**, not the attribute alone, enforces the rule.

### Important built-in attributes

| Attribute | Intended reader/effect |
|---|---|
| `[Obsolete]` | Compiler issues a warning or error on use. |
| `[Conditional("DEBUG")]` | Compiler omits qualifying call sites when the symbol is not defined. |
| `[JsonIgnore]`, `[JsonPropertyName]` | `System.Text.Json` changes JSON behavior. |
| `[Required]`, `[Range]` | Validation components that process DataAnnotations. |
| `[HttpGet]`, `[Authorize]` | ASP.NET Core routing/auth metadata processed by its pipeline. |
| `[Fact]` | xUnit test discovery. |
| `[DataContract]`, `[DataMember]` | Data-contract serializers; not general JSON serialization controls. |
| `[Serializable]`, `[NonSerialized]` | Mainly legacy serialization behavior. **Not** used by `System.Text.Json`. |

`[Conditional]` is special: it mainly affects **call sites**, not whether the method itself exists. It applies to eligible `void` methods and to conditional attribute classes. It differs from `#if DEBUG`, which conditionally includes or excludes source sections.

**Remember:** Attribute = metadata; something must interpret it.

---

## 10. Reflection

### The problem
Normally the compiler knows which class or method you want. But a framework may have to discover types and methods **at runtime** (plugins, dependency injection, test discovery, serializers, etc.).

**Reflection** lets code inspect runtime types and, where supported, create objects and access/invoke members dynamically.

```text
Assembly
  +-- Types
        +-- Constructors
        +-- Properties
        +-- Fields
        +-- Methods
        +-- Events
        +-- Attributes
```

### Example: inspect and invoke a method by its name

```csharp
using System.Reflection;

Type type = typeof(Greeter);
object instance = Activator.CreateInstance(type)!;

MethodInfo? method = type.GetMethod(nameof(Greeter.SayHello));
if (method is null) throw new InvalidOperationException("Method not found.");

object? result = method.Invoke(instance, new object[] { "Sara" });
Console.WriteLine(result);  // Hello, Sara

public sealed class Greeter
{
    public string SayHello(string name) => $"Hello, {name}";
}
```

### Step by step

1. `typeof(Greeter)` returns a `Type` object describing the class.
2. `Activator.CreateInstance` invokes a suitable constructor.
3. `GetMethod` looks up the method in runtime type information.
4. `Invoke` checks/binds arguments and calls the method.

### Downsides

- More runtime checking, allocations, and overhead compared with many direct calls; **do not claim a fixed 10–100× speed difference**. Measure your exact scenario.
- String-based lookups can fail at **runtime**, not compile time.
- Reflecting private implementation details tightly couples you to them.
- **Trimming and Native AOT** may remove members that a reflection-heavy program expects. Some APIs need annotations or source-generated alternatives.
- **Loading a DLL is not a security boundary**; executing untrusted plugin code inside your process can be dangerous.

Use reflection for **framework-level or genuinely dynamic** behavior. Use normal calls, interfaces, and generics for ordinary business code.

**Remember:** Reflection makes code dynamic, at the cost of safety, predictability, and sometimes performance.

---

## 11. Serialization

### The problem
Your C# object exists in process memory, but you need to send data through an API or save it to disk. Other applications do not understand your process's raw object references.

**Serialization:** object/data → transferable format (JSON, XML, bytes).

**Deserialization:** format → new object/data structure.

```text
C# object                       JSON text
Product { Id = 7 }  ----------> { "id": 7 }
                   serialize
Product { Id = 7 }  <---------- { "id": 7 }
                  deserialize
```

### Prefer `System.Text.Json` for ordinary APIs

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

var item = new Product
{
    Id = 7,
    Name = "Notebook",
    Cost = 25m,
    InternalCode = "secret-internal-value"
};

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    WriteIndented = true
};

string json = JsonSerializer.Serialize(item, options);
Console.WriteLine(json);

Product? copy = JsonSerializer.Deserialize<Product>(json, options);
Console.WriteLine(copy?.Name); // Notebook

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [JsonPropertyName("unit_price")]
    public decimal Cost { get; set; }
    [JsonIgnore]
    public string InternalCode { get; set; } = "";
}
```

### How it works

1. A serializer determines which members belong in JSON (via reflection or generated metadata).
2. It reads member values, converts them, applies naming and ignore rules.
3. It writes JSON text/bytes.
4. During deserialization, it parses JSON and creates/populates an object using supported constructors and members.

### Serializer choices

| Technology | Format | When relevant |
|---|---|---|
| `System.Text.Json` | JSON | **Default for modern .NET APIs.** |
| Newtonsoft.Json | JSON | Compatibility or features your project needs. |
| `XmlSerializer` | XML | XML integrations. |
| `DataContractSerializer` | XML | Data-contract XML scenarios. |
| `DataContractJsonSerializer` | JSON | Legacy data-contract JSON scenarios. |
| Protocol Buffers / MessagePack | Binary | APIs or messaging with specific compact-format requirements. |
| `BinaryFormatter` | Legacy binary | **Do not use:** insecure and obsolete. |

**Version note:** On **.NET 9+**, the built-in `BinaryFormatter` implementation throws `PlatformNotSupportedException`. Its API symbols still exist; saying the types were completely deleted is inaccurate. It is unsafe regardless of version.

### Important mistakes

- JSON serialization is **not encryption** and does not hide secrets.
- The default `System.Text.Json` behavior primarily includes public properties; fields need additional configuration/attributes.
- `[Serializable]` and `[NonSerialized]` do not control `System.Text.Json`.
- In APIs, use **request/response DTOs** so you do not accidentally expose password hashes, internal identifiers, or unrelated navigation properties.
- Deserializing an arbitrary type graph may be dangerous: avoid insecure polymorphic type-name settings.
- Be explicit about **date/time zones**, schema changes, required members and circular references.
- Serialization is generally not the correct default method for cloning arbitrary object graphs.

**Remember:** Serialize data deliberately. Do **not** send your database entity graph directly just because you can.

---

## 12. Processes, threads, and concurrency

### The problem
Programs need to handle several activities: serving requests, reading files, performing calculations, and responding to users.

### Three concepts that are NOT synonyms

| Concept | Simple meaning | Example |
|---|---|---|
| **Concurrency** | Multiple operations can make progress during overlapping time periods. | An API handles many pending requests. |
| **Parallelism** | Operations execute at the same instant, typically on different CPU cores. | Multiple cores process separate image chunks. |
| **Asynchrony** | A caller can continue without blocking while an operation is pending. | Waiting for a network response with `await`. |

Concurrency does **not** require multiple CPU cores. Asynchrony does **not** imply another thread. Parallelism is one way to achieve concurrency.

### Process vs thread

```text
Process A                          Process B
+------------------------+         +---------------------+
| Virtual address space  |         | Other address space |
|                        |         |                     |
| Thread 1 -> stack      |         | Thread 1 -> stack   |
| Thread 2 -> stack      |         +---------------------+
| Shared objects/heap    |
+------------------------+
```

A **process** is an OS-managed execution environment with its own virtual address space. A **thread** is an execution path inside a process. Threads in the same process can share memory and must coordinate access to mutable shared state; each thread has its own execution stack and registers.

### ThreadPool vs manually created threads

```csharp
// Illustration: explicit Thread
var thread = new Thread(() => Console.WriteLine("Thread work"));
thread.Start();
thread.Join(); // Block until it finishes

// Common way to offload a CPU calculation:
int result = await Task.Run(() => Enumerable.Range(1, 1000).Sum());
Console.WriteLine(result);
```

`Thread` represents a managed wrapper for an OS thread. The **ThreadPool** reuses worker threads to reduce the cost of frequent creation. `Task.Run` usually schedules work on that pool; it doesn't create a new dedicated thread for each task.

Use explicit `Thread` only for specialized needs such as a dedicated long-lived thread. For normal **asynchronous I/O**, use the API's `Async` methods rather than making threads wait.

**Remember:** Process = isolation boundary; thread = execution path; `Task` = abstraction for completion (next sections).

---

## 13. Race conditions, locks, and synchronization

### The problem: `counter++` is not an atomic transaction

```text
counter = 0
Thread A: reads 0 -------- computes 1 -------- writes 1
Thread B: ------- reads 0 -------- computes 1 ---- writes 1
Final counter = 1, but we wanted 2
```

A **race condition** is a bug where behavior depends on unpredictable timing between operations. Many race conditions involve unsynchronized shared mutable state.

### Demonstration and two correct approaches

```csharp
int unsafeCount = 0;
Parallel.For(0, 100_000, _ => unsafeCount++);
Console.WriteLine(unsafeCount);  // Nondeterministic; may equal 100000 by chance!

int safeCount = 0;
Parallel.For(0, 100_000, _ => Interlocked.Increment(ref safeCount));
Console.WriteLine(safeCount);    // 100000

int protectedCount = 0;
object gate = new();
Parallel.For(0, 100_000, _ =>
{
    lock (gate)
    {
        protectedCount++;
    }
});
Console.WriteLine(protectedCount); // 100000
```

### How `lock` works

Only one thread at a time may enter a critical section guarded by the **same lock object**. On .NET 8 with `lock (gate)` where `gate` is an `object`, the compiler essentially uses `Monitor.Enter` and `Monitor.Exit` with exception-safe cleanup.

```text
Thread A: [enter lock ---- update ---- exit]
Thread B:           [wait ...] [enter --- update --- exit]
```

### Choose the right tool

| Tool | Suitable for |
|---|---|
| `Interlocked` | Simple atomic increments, exchanges, compare-and-swap. |
| `lock` / `Monitor` | Protecting a small set of related fields in one process. |
| `SemaphoreSlim` | Allowing a limited number of operations, including async `WaitAsync`. |
| `ConcurrentDictionary` | Thread-safe dictionary operations. |
| `ReaderWriterLockSlim` | Certain read-heavy in-process workloads (benchmark first). |
| `Mutex` | Mutual exclusion; named mutexes can coordinate processes where supported. |
| `Channel<T>` | Passing work/data safely between producers and consumers. |

**.NET 9+ note:** `System.Threading.Lock` is a dedicated lock type; it is **not available in .NET 8**. For .NET 8, the standard private `object` approach is correct.

### Async concurrency limit

```csharp
using var limiter = new SemaphoreSlim(3);  // At most three at a time

async Task DoWorkAsync(int id)
{
    await limiter.WaitAsync();
    try
    {
        Console.WriteLine($"Start {id}");
        await Task.Delay(100);
    }
    finally
    {
        limiter.Release();
    }
}

await Task.WhenAll(Enumerable.Range(1, 10).Select(DoWorkAsync));
```

### Deadlock

```text
Thread A owns lock X, waits for lock Y.
Thread B owns lock Y, waits for lock X.
Neither can continue.
```

To reduce deadlock risk, acquire multiple locks in a consistent order, keep critical sections short, and avoid callbacks/external calls while holding locks. **Never use `lock(this)` or `lock("literal")`** as a general locking policy: other code may share those lock objects.

**Important:** `await` is **not allowed inside a `lock` statement**. Use `SemaphoreSlim.WaitAsync()` when an async critical section is necessary. Do not assume a `ConcurrentDictionary` makes an entire multi-step business transaction atomic.

**Remember:** Prevent shared mutable state if possible; otherwise synchronize the entire operation that must be consistent.

---

## 14. Tasks and async/await

### The problem: wasting a thread while waiting

A request to PostgreSQL may take time. If a server thread blocks until the database returns, it cannot serve another request during that wait. An asynchronous API can avoid occupying a thread **during the pending I/O**.

### Essential ideas

- `Task` represents an operation that may complete later.
- `Task<T>` additionally provides a result of type `T`.
- `async` allows use of `await` and normally enables a compiler-generated state machine.
- `await` means: **if not finished, arrange to resume this method after completion without blocking the current thread**.
- `Task.Run` schedules **CPU-bound work** on the thread pool; it is not what makes network/database I/O asynchronous.

```text
Request handler on thread pool
         |
         v
await database.QueryAsync()
         |
         +--- operation pending ---> handler suspends; worker can do other work
         |
         v
I/O completes; continuation scheduled
         |
         v
handler resumes, produces response
```

When an awaited operation has **already completed**, execution may continue synchronously without suspension. Before the first incomplete `await`, an `async` method executes normally on the caller's thread.

### Correct HTTP example (reuse `HttpClient`)

```csharp
using System.Net.Http;

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try
{
    string page = await HttpDemo.GetTextAsync("https://example.com", cts.Token);
    Console.WriteLine($"Received {page.Length} characters");
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request canceled or deadline reached.");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP error: {ex.Message}");
}

public static class HttpDemo
{
    // Simple console example. For ASP.NET Core prefer IHttpClientFactory/typed clients.
    private static readonly HttpClient Client = new();

    public static async Task<string> GetTextAsync(string url, CancellationToken ct)
    {
        using HttpResponseMessage response = await Client.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(ct);
    }
}
```

**Why no `new HttpClient()` in every call?** Unnecessary handler/connection churn can harm performance or socket usage. For web apps, use `IHttpClientFactory` or an appropriately configured long-lived client.

### `WhenAll`, `WhenAny`, and timeouts

```csharp
Task<int> first = GetNumberAsync(100, 1);
Task<int> second = GetNumberAsync(200, 2);
int[] values = await Task.WhenAll(first, second);  // [1, 2]

Task<int> winner = await Task.WhenAny(first, second);
int fastest = await winner;   // Observe result / exception of winning task

// In .NET 8, simple timeout when waiting:
try
{
    int value = await GetNumberAsync(3000, 5).WaitAsync(TimeSpan.FromSeconds(1));
}
catch (TimeoutException)
{
    Console.WriteLine("Stopped waiting after 1 second.");
}

static async Task<int> GetNumberAsync(int delayMs, int value)
{
    await Task.Delay(delayMs);
    return value;
}
```

- `WhenAll` waits for **all**, and multiple operations can already be in progress.
- `WhenAny` returns the **completed task**, not directly its value; remaining operations keep running unless separately canceled.
- `WaitAsync(timeout)` stops **waiting** on the task, but it does **not automatically cancel the underlying operation**. Pass a `CancellationToken` to the underlying API if cancellation is required.
- `Task.WhenAll` is concurrency, **not necessarily CPU parallelism**.

### Cancellation and exceptions

A `CancellationToken` is a **request to cancel**, not an operating-system kill switch. The operation must support and observe it. Exceptions in a `Task` surface when awaited. `await` normally propagates the underlying exception rather than the `AggregateException` wrapper commonly observed with `.Wait()`/`.Result`.

### ASP.NET Core vs UI apps

- **ASP.NET Core** does not install a custom `SynchronizationContext` for request processing. After `await`, a continuation may run on a different pool thread.
- **Desktop UI frameworks** often have a synchronization context that resumes work on the UI thread.
- `ConfigureAwait(false)` is mainly a library-writing concern; it does not mean "run on a background thread".

### Common mistakes

- `.Result` / `.Wait()` inside async code: blocks threads; can deadlock in single-threaded synchronization contexts and starve server thread pools.
- `async void` (except necessary event handlers): caller cannot normally await/observe failures.
- Starting a background task in a web request and forgetting it: it may fail silently or outlive request-scoped dependencies.
- Using `Task.Run` to wrap `DbContext.ToListAsync()` or HTTP calls on the server: adds scheduling rather than useful asynchrony.
- Forgetting cancellation/timeout policies and exceptions.

**Remember:** `async` is about non-blocking waiting. `Task.Run` is about scheduling CPU work. A `Task` is **not a thread**.

---

## 15. Parallel programming

### The problem
You have a CPU-heavy calculation and want to use several cores, rather than making one core do all the work sequentially.

### Tools

| Tool | Typical scenario |
|---|---|
| `Parallel.For` / `Parallel.ForEach` | Synchronous CPU-heavy independent iterations. |
| PLINQ (`AsParallel()`) | Parallelizing some in-memory LINQ operations. |
| `Task.Run` / `Task.WhenAll` | Scheduling related CPU work and coordinating completion. |
| `Parallel.ForEachAsync` | Bounded concurrent async operations; suitable for some I/O pipelines. |

### Example with safe aggregation

```csharp
long total = 0;

Parallel.For(1, 1001,
    () => 0L,
    (i, _, localTotal) => localTotal + i,
    localTotal => Interlocked.Add(ref total, localTotal));

Console.WriteLine(total); // 500500
```

Each worker maintains a local sum; merging uses an atomic operation. This avoids unsafely modifying one shared counter for every item.

### Important restrictions

- Parallelism has scheduling/merging overhead. Tiny operations can get slower.
- An `AsParallel()` query need not preserve original ordering unless configured for it.
- More tasks than CPU cores do **not** magically make CPU-heavy work faster.
- Use **asynchronous I/O**, not `Parallel.For` calling blocking HTTP or database APIs.
- Benchmark actual workloads instead of assuming parallel code is faster.

**Remember:** CPU-heavy independent work → consider parallelism. Waiting for external services → prefer true async operations with suitable concurrency limits.

---

## 16. Configuration, options, and secrets

### The problem
You have different database URLs and settings in development and production. You should not modify or recompile source code whenever one setting changes.

### Modern .NET configuration

In an ASP.NET Core application, `WebApplication.CreateBuilder(args)` sets up common configuration sources. In the standard template, later providers generally override earlier ones:

```text
appsettings.json
      ↓ overridden by
appsettings.Development.json (for Development)
      ↓ overridden by
User Secrets (Development, when configured)
      ↓ overridden by
Environment variables
      ↓ overridden by
Command-line arguments
```

The exact precedence depends on which providers you configure, and host settings have their own rules.

### Example: `appsettings.json`

```json
{
  "Database": {
    "CommandTimeoutSeconds": 30
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=appdb;Username=appuser"
  }
}
```

**Do not put a real database password or API key in a committed settings file.** Override a connection string via an environment variable, for example:

```bash
# Linux example; supply the actual secret through your secret-management system.
export ConnectionStrings__DefaultConnection='Host=postgres;Database=appdb;Username=appuser;Password=...'
```

In .NET configuration, double underscore (`__`) in an environment variable maps to a colon (`:`) in hierarchical keys.

### Strongly typed options

The following is an ASP.NET Core application excerpt; the type lives in its own file or below top-level statements.

```csharp
// Program.cs (excerpt, after var builder = WebApplication.CreateBuilder(args);)
builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection("Database"))
    .Validate(x => x.CommandTimeoutSeconds > 0, "Timeout must be positive")
    .ValidateOnStart();
```

```csharp
// DatabaseOptions.cs
public sealed class DatabaseOptions
{
    public int CommandTimeoutSeconds { get; set; } = 30;
}
```

```csharp
// In a service created by dependency injection:
public sealed class SomeService
{
    private readonly DatabaseOptions _settings;

    public SomeService(Microsoft.Extensions.Options.IOptions<DatabaseOptions> options)
        => _settings = options.Value;
}
```

**Options types:** `IOptions<T>` gives standard access to configured values; `IOptionsSnapshot<T>` is scoped and can incorporate configuration changes for new scopes; `IOptionsMonitor<T>` supports updated values and change notifications where the underlying provider supports reloads.

### What about `App.config`?

`App.config` and `System.Configuration.ConfigurationManager` are chiefly **older .NET Framework-style configuration**. They still matter for supporting legacy Windows applications, but `appsettings.json` + `IConfiguration`/options are the usual starting point in modern ASP.NET Core projects.

**Remember:** Configuration is for environment-dependent values; a **secret is not safe just because it is in JSON**. Use environment variables or a managed secrets store; for development consider .NET User Secrets.

---

## 17. Logging and Windows-only features

### Structured logging with `ILogger`

### The problem
`Console.WriteLine` gives no standardized log levels, categories, structured fields, or configurable destinations.

ASP.NET Core has built-in support for `ILogger<T>` via dependency injection:

```csharp
public sealed class ProfileService
{
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(ILogger<ProfileService> logger)
        => _logger = logger;

    public void UpdateProfile(int profileId)
    {
        _logger.LogInformation("Updating profile {ProfileId}", profileId);
        // ... perform business work
    }
}
```

Here `{ProfileId}` is a **structured logging field**, not a request to use string interpolation. Structured logs are easier to search and aggregate.

### Good logging rules

- Use levels intentionally: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`.
- Include useful context such as operation name and correlation/request ID.
- Pass exception objects to logging APIs when reporting failures.
- Do **not** log passwords, API tokens, raw authorization headers, or full sensitive user data.
- Do not catch and silently ignore all exceptions just to keep logs clean.

### Windows Registry — specialized, not portable

The **Windows Registry** stores Windows settings in hives/keys/values (such as `HKEY_CURRENT_USER`, `HKEY_LOCAL_MACHINE`). APIs in `Microsoft.Win32.Registry` are Windows-specific; a Linux backend should **not** use the Registry for application configuration. Reading/writing particular keys may require permissions, and HKLM writes often require elevation, depending on the key and its access control settings.

### Windows Event Log — specialized, not portable

Windows Event Log provides centralized system/application log channels viewable through Event Viewer. The `System.Diagnostics.EventLog` API is Windows-specific. Registering a new event source commonly requires elevated permissions; an installer or administrator should set it up. For a normal cross-platform backend, use `ILogger` and an appropriate provider; logging providers can target files, OpenTelemetry, or Windows Event Log when explicitly configured.

**Why the original Windows chapters are short now:** Their detailed API examples were not useful for the main target environment (**.NET 8 on Linux**). Learn them later if working on Windows-specific software.

**Remember:** Cross-platform apps → `ILogger`, options and environment configuration. Windows Registry/Event Log → only when Windows integration is required.

---

## 18. Cryptography and password storage

### The problem
You may need to protect the confidentiality, integrity, and authenticity of user data. Different cryptographic techniques solve **different problems**.

| Tool | What it does | Good example |
|---|---|---|
| Hash | Produces a one-way fingerprint | SHA-256 for file checksums |
| HMAC | Uses a shared secret to authenticate data | HMAC-SHA256 for message authenticity |
| Symmetric encryption | Encrypts/decrypts using a shared secret key | AES-GCM |
| Asymmetric encryption | Encrypts for someone with a public key; matching private key decrypts | RSA-OAEP for small data/key wrapping |
| Digital signature | Private key signs; public key verifies | RSA-PSS / ECDSA |
| Key agreement | Parties establish shared key material | ECDH |
| Password hashing | Deliberately expensive one-way verification | Identity `PasswordHasher`, Argon2id, PBKDF2 |

**Do not confuse the last four rows:** ECDSA signs but does not encrypt. ECDH performs key agreement but does not itself encrypt application messages.

### Basic hashing

```csharp
using System.Security.Cryptography;
using System.Text;

byte[] data = Encoding.UTF8.GetBytes("hello");
byte[] digest = SHA256.HashData(data);
Console.WriteLine(Convert.ToHexString(digest));
```

A plain hash does **not** establish who supplied the content. If an attacker can change both the file and its published hash, comparing them proves nothing about authenticity.

### HMAC: a keyed message authentication code

```csharp
using System.Security.Cryptography;
using System.Text;

byte[] secretKey = RandomNumberGenerator.GetBytes(32);
byte[] message = Encoding.UTF8.GetBytes("order=100");
byte[] mac = HMACSHA256.HashData(secretKey, message);

byte[] expected = HMACSHA256.HashData(secretKey, message);
bool valid = CryptographicOperations.FixedTimeEquals(mac, expected);
Console.WriteLine(valid); // True
```

All parties able to validate an HMAC need the shared secret; an HMAC is **not a public-key signature**.

### Passwords: use a tested password-hashing component

**Never store plaintext passwords, reversible encrypted passwords, or simple `SHA256(password)` values.** A normal hash is too fast for password storage.

For a new ASP.NET Core sign-in system, prefer **ASP.NET Core Identity's `PasswordHasher<TUser>`** (or an established password-hashing solution whose parameters you maintain) rather than manually designing storage/versioning logic.

```csharp
// Requires ASP.NET Core Identity's password-hashing APIs.
using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<string>();
string userId = "user-123";
string storedHash = hasher.HashPassword(userId, "example-password");

PasswordVerificationResult result = hasher.VerifyHashedPassword(
    userId, storedHash, "example-password");

bool authenticated = result != PasswordVerificationResult.Failed;
```

This is a **password-verification demonstration**, not a complete authentication system. For production authentication, use proper user management, lockout/rate limiting, token/session handling, password reset, and approved identity flows. Identity's hasher handles salts and hash formats for you. PBKDF2/Argon2id/bcrypt are algorithms/families; choosing between implementations depends on operational constraints and correct configuration.

### AES-GCM: authenticated symmetric encryption

```csharp
using System.Security.Cryptography;
using System.Text;

byte[] key = RandomNumberGenerator.GetBytes(32); // AES-256 key
byte[] nonce = RandomNumberGenerator.GetBytes(12);
byte[] plaintext = Encoding.UTF8.GetBytes("private message");
byte[] ciphertext = new byte[plaintext.Length];
byte[] tag = new byte[16];

using (var aes = new AesGcm(key, tagSizeInBytes: 16))
    aes.Encrypt(nonce, plaintext, ciphertext, tag);

byte[] restored = new byte[ciphertext.Length];
using (var aes = new AesGcm(key, tagSizeInBytes: 16))
    aes.Decrypt(nonce, ciphertext, tag, restored);

Console.WriteLine(Encoding.UTF8.GetString(restored)); // private message
```

**Critical:** Never reuse a nonce with the **same key**. Random 96-bit nonces work in many bounded-use designs, but large-scale systems must define explicit nonce-generation, key-rotation, and collision-avoidance limits. Protect the key in a secure store. The authentication tag must be checked (the API checks during decrypt); decryption rejects tampering.

### TLS in practice

Modern HTTPS/TLS typically combines certificates and signatures for authentication, ephemeral key agreement (such as ECDHE) to establish keys, and authenticated symmetric encryption (such as AES-GCM) for actual traffic. It is **not** accurate to describe this as simply "RSA encrypts all HTTP messages".

### Security mistakes to avoid

- MD5, SHA-1 or DES for new security-sensitive designs.
- Treating a password hash and a file hash as interchangeable.
- Homemade cryptographic algorithms or protocols.
- Using `System.Random` for keys/tokens; use `RandomNumberGenerator`.
- Storing encryption keys next to the encrypted data without a separate protection mechanism.
- Treating a checksum as proof of origin.
- Using unauthenticated encryption such as AES-ECB or ordinary CBC without a correct authentication design.

**Remember:** Hash ≠ password hash ≠ HMAC ≠ encryption ≠ signature ≠ key agreement.

---

## 19. LINQ and query execution

### The problem
Without LINQ, filtering, transforming, grouping, and sorting collections requires repeated loops. LINQ provides a composable query style.

LINQ = **Language Integrated Query**. It uses generic types, delegates, extension methods, and (for query providers) expression trees.

### Start with the three most important operators

```csharp
var students = new List<Student>
{
    new("Ali", 20), new("Sara", 25), new("Omar", 17)
};

List<string> adultNames = students
    .Where(s => s.Age >= 18)  // Filter: keep matching students
    .Select(s => s.Name)     // Project: convert Student to string
    .ToList();               // Execute and store results

Console.WriteLine(string.Join(", ", adultNames)); // Ali, Sara

public record Student(string Name, int Age);
```

`Where` takes a predicate (`Func<Student,bool>`). `Select` takes a projection function (`Func<Student,string>`). LINQ's methods are available through extension methods such as those in `Enumerable`.

### Deferred vs immediate execution

```csharp
var numbers = new List<int> { 1, 2, 3 };
IEnumerable<int> query = numbers.Where(n => n > 1); // Define, don't execute yet

numbers.Add(4);
Console.WriteLine(string.Join(",", query)); // 2,3,4

List<int> snapshot = query.ToList();          // Execute NOW
numbers.Add(5);
Console.WriteLine(string.Join(",", snapshot)); // 2,3,4
Console.WriteLine(string.Join(",", query));    // 2,3,4,5
```

**Deferred:** `Where`, `Select`, `OrderBy`, `GroupBy`, etc. produce query objects whose work begins when enumerated. **But not all are streaming**: `OrderBy` must normally buffer/sort its input before yielding its first item. This distinction is **deferred execution vs buffering**, not "everything is lazy one item at a time".

**Immediate/terminal operations:** `ToList`, `ToArray`, `Count`, `Any`, `First`, `Single`, `Sum` execute when called (for relevant LINQ-to-Objects sources).

### `IEnumerable<T>` vs `IQueryable<T>`

```text
IN-MEMORY (LINQ to Objects):
IEnumerable<Student>
  .Where(s => s.Age >= 18)
      --> execute predicate in .NET when enumerated

DATABASE (EF Core):
IQueryable<StudentEntity>
  .Where(s => s.Age >= 18)
      --> expression tree --> SQL translation --> database executes
```

**Important correction:** `IQueryable<T>` does **not guarantee SQL**. It represents a query for some provider. EF Core with a relational provider **attempts** to translate expressions to SQL; unsupported expressions can throw at runtime. Once `AsEnumerable()` or `ToList()` moves processing to memory, later filtering is no longer translated to SQL.

### EF Core example: don't call `ToList` too early

```csharp
// Good (inside an async service with an injected DbContext):
var adults = await db.Students
    .AsNoTracking()
    .Where(s => s.Age >= 18)
    .Select(s => new { s.Id, s.Name })
    .ToListAsync();

// Bad for large tables:
var all = await db.Students.ToListAsync();  // Load everything first
var adultsInMemory = all.Where(s => s.Age >= 18);
```

EF Core generally pushes a supported `Where` filter and `Select` projection into the SQL query. It can do limited client evaluation in the top-level projection; unsupported client-side logic in other query parts generally causes an exception rather than silently loading everything.

### Useful operators

| Method | Purpose | Caveat |
|---|---|---|
| `Where` | Filter | Deferred |
| `Select` | Transform/project | Deferred |
| `OrderBy` | Sort | Deferred but buffering |
| `GroupBy` | Group | EF Core SQL translation depends on query shape |
| `Any` | Test existence | Immediate; often better than `Count() > 0` |
| `FirstOrDefault` | First match or default | Doesn't guarantee exactly one |
| `SingleOrDefault` | Zero or one match | Throws if more than one |
| `Skip` / `Take` | Page/filter range | For DB paging, use deterministic ordering |
| `ToList` / `ToArray` | Materialize | Query executes and results are stored |
| `AsNoTracking` | EF Core read-only query | Avoids change tracking; not a LINQ-to-Objects operator |

**Remember:** `Where` filters, `Select` projects; deferred is not the same as streaming; provider-backed `IQueryable` requires understanding query translation.

---

## 20. Exception handling

### The problem
Operations can fail unexpectedly: files disappear, SQL commands fail, network requests time out, or methods receive invalid arguments.

**Exception** means normal control flow is interrupted and a failure can travel up the call stack until handled.

### Basic example

```csharp
string input = "abc";

try
{
    int age = int.Parse(input);
    Console.WriteLine(age);
}
catch (FormatException)
{
    Console.WriteLine("Enter a valid integer.");
}
finally
{
    Console.WriteLine("Attempt finished.");
}
```

For an expected invalid input, a simpler approach is often **not throwing at all**:

```csharp
if (int.TryParse(input, out int age))
    Console.WriteLine(age);
else
    Console.WriteLine("Invalid integer.");
```

### Rethrowing correctly

```csharp
try
{
    await SaveDataAsync();
}
catch (Exception ex)
{
    logger.LogError(ex, "Data save failed");
    throw;     // Preserves original stack trace
    // NOT: throw ex;
}
```

This is a **service-method excerpt**: `SaveDataAsync` and `logger` are dependencies from the surrounding application.

### What `finally` guarantees

`finally` runs as part of normal managed exception/return flow, but cannot be relied on if the process terminates abruptly or the machine loses power. Dispose critical resources; don't treat `finally` as durable persistence.

### Rules

- Catch **specific exceptions** you can meaningfully handle.
- Let unexpected exceptions propagate to a suitable boundary (e.g. API exception-handling middleware).
- Never silently swallow exceptions just to "make the API succeed".
- Prefer `TryParse`/`TryGetValue` for normal, expected failures.
- Use `throw;` rather than `throw ex;` when rethrowing the same exception.
- Avoid exposing raw exception messages and stack traces to API clients in production.
- Catch `OperationCanceledException` carefully: cancellation is not always the same thing as a network failure.

**Remember:** Exceptions describe failures; input validation describes expected invalid input. Handle errors at the right boundary.

---

## 21. Dependency inversion and dependency injection

These terms are related but **not identical**.

### A. Dependency: one class needs something else

```csharp
// Problem: Business service chooses its own data access implementation.
public sealed class ProfileService
{
    private readonly SqlProfileRepository _repository = new SqlProfileRepository();

    public string GetName(int id) => _repository.FindName(id);
}
```

`ProfileService` **depends on** `SqlProfileRepository`. This can cause problems:

1. Unit tests may need a real database or awkward setup.
2. Changing the storage mechanism requires editing business code.
3. The class creates and controls the lifetime of the dependency.
4. Business logic is directly coupled to a lower-level detail.

### B. Dependency Inversion Principle (DIP): depend on a contract

The principle is: **High-level policy should not be tied to a low-level implementation. Both should depend on an abstraction appropriate to the policy.**

```text
BEFORE:
ProfileService  -------------> PostgreSqlRepository
  (business)                       (database detail)

AFTER:
ProfileService  -------------> IProfileRepository
                                    ^
                                    |
PostgreSqlRepository ---------------+
```

The arrow to `IProfileRepository` means "depends on the contract"; the implementation promises to fulfill that contract.

### C. Dependency Injection (DI): receive the dependency

**DI is a technique** for supplying an object's dependencies externally, often through its constructor. DIP is a **design principle**. DI can help implement DIP, but using a DI container alone does not guarantee a good design.

### Complete minimal ASP.NET Core example

Create a minimal API project with `dotnet new web -n ProfileDemo --framework net8.0`.

**`Program.cs`:**

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register contract --> concrete implementation.
builder.Services.AddScoped<IProfileRepository, InMemoryProfileRepository>();
builder.Services.AddScoped<ProfileService>();

var app = builder.Build();

app.MapGet("/profiles/{id:int}", (int id, ProfileService service) =>
{
    string? name = service.GetName(id);
    return name is null ? Results.NotFound() : Results.Ok(new { Id = id, Name = name });
});

app.Run();
```

**`IProfileRepository.cs`:**

```csharp
public interface IProfileRepository
{
    string? FindName(int id);
}
```

**`InMemoryProfileRepository.cs`:**

```csharp
public sealed class InMemoryProfileRepository : IProfileRepository
{
    public string? FindName(int id) => id == 1 ? "Ali" : null;
}
```

**`ProfileService.cs`:**

```csharp
public sealed class ProfileService
{
    private readonly IProfileRepository _repository;

    public ProfileService(IProfileRepository repository)
    {
        _repository = repository;
    }

    public string? GetName(int id) => _repository.FindName(id);
}
```

### What happens on an HTTP request?

```text
GET /profiles/1
       |
ASP.NET Core DI container creates/resolves ProfileService
       |
Sees constructor parameter: IProfileRepository
       |
Uses registration: IProfileRepository -> InMemoryProfileRepository
       |
Constructs ProfileService(repository)
       |
Calls GetName(1) -> repository.FindName(1)
       |
HTTP 200 { "id": 1, "name": "Ali" }
```

The **container is not the dependency**: `ProfileService` can also be created manually for a unit test:

```csharp
// In a test, supply your own implementation of IProfileRepository:
var service = new ProfileService(new InMemoryProfileRepository());
Console.WriteLine(service.GetName(1)); // Ali
```

### DI lifetimes

| Lifetime | Meaning in ASP.NET Core | Typical use |
|---|---|---|
| `Transient` | New instance each time service is resolved | Lightweight stateless helpers |
| `Scoped` | One instance per scope (normally per HTTP request) | `DbContext`, request-specific services |
| `Singleton` | One instance shared by the app container | Thread-safe global shared services |

**Danger:** A singleton should not directly capture a scoped `DbContext`. The scoped instance could outlive its intended request, and `DbContext` is not thread-safe. A background service should create a scope or use an appropriate factory when it needs scoped dependencies.

### Common mistakes

- Assuming **every class** must have an interface. Create interfaces when they establish a useful boundary.
- Registering a service but then still calling `new SqlProfileRepository()` inside the business class.
- Confusing **DIP**, **DI**, and **IoC container**: principle, technique, infrastructure respectively.
- Using service locator (`IServiceProvider.GetService` everywhere) instead of explicit constructor dependencies.
- Registering mutable non-thread-safe application state as singleton.

**Remember:** **DIP** says what should depend on what; **DI** says how an object receives dependencies.

---

## 22. EF Core and `DbContext` fundamentals

### The problem
You want to work with PostgreSQL from your ASP.NET Core backend. With ADO.NET/Npgsql you write SQL and read rows manually. With **Entity Framework Core**, you model entities in C#, build queries using LINQ, and let a provider translate appropriate queries into SQL.

### How the parts fit together

```text
HTTP Controller / Endpoint
           |
           v
Application or Business Service
           |
           v
     EF Core DbContext
       |          |
       |          +-- ChangeTracker: remembers tracked entity states
       |
       +-- DbSet<TEntity>: query and track entities
                   |
                   v
          LINQ expression tree
                   |
                   v
        EF Core relational provider (Npgsql)
                   |
                   v
              PostgreSQL
```

`DbContext` is more than a connection wrapper: it is primarily a short-lived **unit of work**, coordinating queries, changes, and `SaveChanges()`.

### Entity with a foreign key and a navigation property

If a database table contains `NationalityId`, you usually **do not need to replace the FK with another whole object**. EF Core can model **both** the FK value and the related object:

```csharp
public sealed class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";

    public int NationalityId { get; set; }     // FK column/value
    public Country? Nationality { get; set; } // Navigation to related entity
}

public sealed class Country
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
```

This example chooses a required FK (`int NationalityId`) and a nullable navigation to acknowledge that the related object **might not currently be loaded**. Whether a relationship is *required* in the EF model depends on configuration and nullability conventions. Loading a navigation is not automatic unless you configure/use a loading strategy (eager, explicit, or lazy with extra setup).

### DbContext with **external** configuration

```csharp
using Microsoft.EntityFrameworkCore;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Nationality)
            .WithMany()
            .HasForeignKey(p => p.NationalityId)
            .IsRequired();
    }
}
```

**In `Program.cs`** (ASP.NET Core, once `builder` exists):

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

To use `UseNpgsql`, install the **Npgsql EF Core provider compatible with EF Core 8**, such as its **8.x** major version for a `.NET 8`/EF Core 8 project. The concrete version should be pinned in the project's package configuration.

### Why pass `DbContextOptions<AppDbContext>` into the constructor?

**Without external configuration:** `AppDbContext` might read `appsettings.json`, choose a connection string, and configure PostgreSQL inside `OnConfiguring`. This couples data access directly to config file loading and makes testing/hosting flexibility harder.

**With external configuration:** the application startup decides *which* database/provider/settings to use, and the context receives **prebuilt options** through constructor injection.

```text
Configuration + Host
       |
       v
DbContextOptions<AppDbContext> ----> AppDbContext
  (which provider? connection?)      (entities, queries, changes)
```

This is an example of **separating responsibilities**. Using `OnConfiguring` is not inherently wrong: it remains useful in some console/design-time setups and can be combined with external options. It is simply **not necessary** for a typical DI-configured ASP.NET Core project.

### How `DbSet` and queries work

```csharp
// Inside an async method in a service with an injected `AppDbContext db`:
var people = await db.People
    .AsNoTracking()
    .Where(p => p.NationalityId == 1)
    .Select(p => new PersonDto(p.Id, p.FirstName))
    .ToListAsync();
```

```csharp
// PersonDto.cs
public record PersonDto(int Id, string FirstName);
```

`p.FirstName` refers to the **C# entity property**, not directly to an SQL column name in your code. EF Core maps properties to table columns using conventions, attributes, or Fluent API configuration such as:

```csharp
// Inside OnModelCreating:
modelBuilder.Entity<Person>()
    .Property(p => p.FirstName)
    .HasColumnName("first_name");
```

At runtime, EF Core's provider translates supported query expressions into SQL using the configured mapping. In a relational provider, fetching with `ToListAsync()` usually executes a SQL query.

### Change tracking and saving

```csharp
Person? person = await db.People.FindAsync(1);
if (person is null) return;

person.FirstName = "Updated"; // ChangeTracker can detect change
await db.SaveChangesAsync(); // Database command executed
```

Tracked entities can have states: **Added**, **Unchanged**, **Modified**, **Deleted**, **Detached**. A no-tracking query is useful for read-only projections but does not automatically track changes to entities it returns.

`SaveChanges` normally uses a transaction when needed to make its changes atomic, subject to provider capabilities. For multi-step workflows with more than one save/other external operation, consider explicit transaction boundaries and error-handling design.

### Active Record vs Data Mapper — what is EF Core?

**Active Record** places persistence methods on entities (`person.Save()`, `person.Delete()`). **Data Mapper** separates domain/entity objects from the persistence mechanism. EF Core generally follows **Data Mapper + Unit of Work + Identity Map** ideas through `DbContext` and its tracking system. You do **not** need every entity to call the database itself.

**Preferred starting point:** Entities represent data and domain behavior. EF Core handles persistence. Application services coordinate operations and business rules. This also makes it easier to separate REST DTOs from persistent entities.

### Important rules

- `DbContext` is **not thread-safe**. Do **not** run simultaneous EF Core operations on the same context instance; await each operation before using it again.
- In ASP.NET Core, `AddDbContext` registers the context as **scoped** by default, typically one per request scope.
- Dispose the context (DI normally does it for injected scoped contexts).
- Do not keep one context alive for the entire application or share it across requests.
- Use `AsNoTracking()` for read-only data when tracking is unnecessary.
- Do not load an entire table just to filter it in memory.
- Do not automatically create a generic repository for every entity; **EF Core already provides many repository-like and unit-of-work capabilities**. Add explicit repository interfaces where they improve your architecture/testing.
- Keep connection-string secrets out of committed source/config.

**Remember:** `DbContext` manages a unit of work; `DbSet` is an entry point for entities; LINQ expressions can become SQL; `SaveChanges` persists tracked changes. Your entity does **not** have to contain database access methods.

---

## 23. Interview questions and exercises

### Explain in your own words

1. **C# vs .NET vs CLR:** What is the language, platform, and runtime?
2. **Value vs reference:** Why does assigning a class variable behave differently from assigning a struct variable?
3. **Stack vs heap:** Why is "struct = stack" an unreliable rule?
4. **Boxing:** Why can `object x = 42;` allocate an object?
5. **GC vs Dispose:** Why does GC not replace `using` for files and DB connections?
6. **Immutability:** Is a record containing `List<string>` deeply immutable? Why not?
7. **Generics:** What do constraints like `where T : IEntity` guarantee?
8. **Delegates:** How can a `Func<Student,bool>` change an algorithm's behavior?
9. **Events:** Why can external classes subscribe but not invoke a field-like event?
10. **Attributes:** Who actually interprets `[Required]` or `[HttpGet]`?
11. **Reflection:** When would reflection be justified instead of an interface?
12. **Serialization:** Why must password hashes not appear in API response DTOs?
13. **Concurrency:** Are concurrency, parallelism, and async the same?
14. **Race condition:** Why is `counter++` unsafe for multiple threads?
15. **Deadlock:** What happens if two threads take locks in opposite orders?
16. **Tasks:** Why doesn't `await` necessarily create a new thread?
17. **`WhenAny`:** What happens to the tasks that do **not** finish first?
18. **Configuration:** Why should production passwords not live in `appsettings.json`?
19. **Cryptography:** What is the difference between hashing, HMAC, encryption, and signatures?
20. **LINQ:** When does a deferred query execute? Does `OrderBy` buffer its input?
21. **`IQueryable`:** When can a LINQ expression be translated into SQL?
22. **DI:** What is the difference between a design principle and a container registration?
23. **EF Core:** Why should a `DbContext` normally be scoped and short-lived?
24. **ORM design:** What is the difference between Active Record and Data Mapper?

### Output prediction practice

**1. Value type copy**

```csharp
int a = 5;
int b = a;
b = 20;
Console.WriteLine(a); // 5
```

**2. Shared reference**

```csharp
var a = new List<int> { 1 };
var b = a;
b.Add(2);
Console.WriteLine(a.Count); // 2
```

**3. String immutability**

```csharp
string a = "x";
string b = a;
a += "y";
Console.WriteLine(b); // x
```

**4. Delegate multicast**

```csharp
Action action = () => Console.Write("A");
action += () => Console.Write("B");
action(); // AB
```

**5. Captured `for` loop variable**

```csharp
var actions = new List<Action>();
for (int i = 0; i < 3; i++) actions.Add(() => Console.Write(i));
foreach (var action in actions) action(); // 333
```

**6. LINQ deferred execution**

```csharp
var values = new List<int> { 1, 2 };
var query = values.Where(x => x > 1);
values.Add(3);
Console.WriteLine(query.Count()); // 2 (items 2 and 3)
```

**7. Async can execute before first incomplete await**

```csharp
static async Task WorkAsync()
{
    Console.Write("A");
    await Task.Delay(100);
    Console.Write("B");
}
var work = WorkAsync();
Console.Write("C");
await work;  // Typically ACB for this example
```

**8. Race condition: NO fixed output**

```csharp
int count = 0;
Parallel.For(0, 10_000, _ => count++);
Console.WriteLine(count); // Non-deterministic: may be 10000 OR less
```

### Practical learning roadmap

| Order | Study | Mini-project |
|---|---|---|
| 1 | CLR, types, memory, GC | Explain a struct/class copying demo. |
| 2 | Generics and collections | Implement an in-memory typed store. |
| 3 | Delegates, events, closures | Notification dispatcher. |
| 4 | LINQ | Filter/project JSON records. |
| 5 | Attributes and reflection | Custom property validator. |
| 6 | Serialization | REST request/response DTO converter. |
| 7 | Async and cancellation | HTTP downloader with cancellation and rate limit. |
| 8 | Thread safety | Reproduce and fix a shared counter race. |
| 9 | DI and EF Core | CRUD API with PostgreSQL and proper `DbContext` lifetime. |
| 10 | Configuration, logging, exceptions | Add structured logs and safe error responses to API. |
| 11 | Cryptography basics | Explain password hashing and AES-GCM nonce requirements. |
| Optional | Windows Registry/Event Log | Only if you work on Windows-specific applications. |

---

## 24. Corrections made to the original notes

This guide is a **rewrite** of the uploaded C#/.NET knowledge base, not merely a spelling pass. I kept the main subject areas and removed or reframed misleading statements.

| Original issue | Strict correction |
|---|---|
| Normal compilation presented as the only C# execution path | Described **JIT as standard** and **Native AOT as an alternative**. |
| "Reference type = heap; value type = stack" | Explained **value semantics** independently from memory location. |
| GC implied to clean up file/DB resources | Distinguished **memory reclamation** from **timely disposal**. |
| Tuple/record described as automatically immutable | Differentiated `Tuple`, `ValueTuple`, record class/struct, shallow/deep immutability. |
| Generics framed as guaranteeing performance | Clarified reified generics, typical specialization/sharing and tradeoffs. |
| Operator overload example compared money without checking currencies | Removed the unsafe comparison and implicit conversion that erased currency. |
| Delegates explained as two fundamentally different runtime types | Explained that delegate types derive from `MulticastDelegate`; invocation list size varies. |
| Attributes described as if they execute themselves | Explained **metadata plus an interpreter/reader**. |
| Reflection described as "10–100× slower" in general | Removed the unsupported multiplier; speed depends on usage. |
| `DataContractSerializer` treated as an XML/JSON serializer | Separated XML `DataContractSerializer` from `DataContractJsonSerializer`. |
| BinaryFormatter described as if all symbols were removed in .NET 9 | Clarified that its **in-box implementation throws**; unsafe in all cases. |
| Raw `Thread` and async `Task` insufficiently distinguished | Separated blocking threads, CPU work, and non-blocking I/O. |
| `Task.WhenAny` implicitly treated as complete timeout handling | Explained waiting, result handling, and cancellation of remaining work. |
| New `HttpClient` created per request in the demo | Replaced with a reusable client and noted `IHttpClientFactory`. |
| Race-condition predictions marked as "usually false" | Identified the outcome as **nondeterministic**, including possible success by chance. |
| Windows mutex example used as a portable general solution | Removed platform-sensitive global-name example from cross-platform guidance. |
| Windows Registry/Event Log took substantial space | Condensed to clearly marked **Windows-only** optional reference material. |
| Cryptography grouped RSA, signatures and ECDH together | Explained encryption, signatures, and key agreement as separate mechanisms. |
| Password-hashing example encouraged manual primitives as the default | Recommended tested `PasswordHasher<TUser>` for ASP.NET Core sign-in systems. |
| LINQ deferred execution described as completely lazy | Explained **deferred vs streaming**; `OrderBy` buffers. |
| `IQueryable` described as if it always generates SQL | Clarified that **provider translation is conditional** and can fail. |
| Exception example relied on an undefined `Log` method and a fragile message filter | Replaced with simple functional examples and separate service excerpts. |
| Core backend concepts missing | Added **DIP vs DI**, service lifetimes, **DbContext**, FK/navigation properties, EF Core state tracking, Data Mapper vs Active Record. |

### What this README intentionally does NOT claim

- Every snippet has been compiled on your machine. These have been reviewed for .NET 8/C# 12 syntax, but a .NET SDK/compiler was **not available in the editing environment**; please run the exercises yourself to verify local package/framework configuration.
- Real security code can be copied directly from a study note into production without threat modelling, key lifecycle planning, and appropriate libraries.
- Every LINQ operation translates into SQL, every async task uses a thread, or every `struct` is stack allocated.

---

## 25. Official documentation

Use primary documentation when verifying precise API or version behavior:

- [.NET fundamentals](https://learn.microsoft.com/en-us/dotnet/fundamentals/)
- [C# language documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Garbage collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/)
- [Generics](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/generics)
- [Asynchronous programming with async/await](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [`Task.WhenAny` behavior](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenany)
- [Dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines)
- [HTTP client factory and lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory)
- [Configuration providers](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)
- [EF Core `DbContext` lifetime and threading](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [EF Core client vs server evaluation](https://learn.microsoft.com/en-us/ef/core/querying/client-eval)
- [Deferred vs eager evaluation in LINQ](https://learn.microsoft.com/en-us/dotnet/standard/linq/deferred-execution-lazy-evaluation)
- [System.Text.Json overview](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [BinaryFormatter removal in .NET 9](https://learn.microsoft.com/en-us/dotnet/core/compatibility/serialization/9.0/binaryformatter-removal)
- [AES-GCM tag-size requirement in .NET 8](https://learn.microsoft.com/en-us/dotnet/fundamentals/syslib-diagnostics/syslib0053)
- [Password hashing guidance for ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing)

---

## Quick revision sheet

```text
C# -> compiler -> assembly (IL + metadata) -> CLR/JIT -> machine code (normal path)
Value type = value-copy semantics; reference type = shared-object-reference semantics
GC reclaims unreachable managed objects; Dispose releases owned resources promptly
Immutable != readonly reference; ValueTuple mutable; record immutability is shallow
Generic <T> = typed reusable code; IEnumerable<T> = enumerable sequence
Delegate = callable; lambda = syntax; event = restricted subscription
Attribute = metadata; reflection = runtime type/member inspection
Serialization = data <-> JSON/XML/binary; NOT encryption
Process = OS execution environment; thread = execution path
Concurrency != parallelism != asynchrony
Race condition -> remove shared state OR synchronize (lock/Interlocked/etc.)
Task != thread; await waits without blocking when incomplete; CPU -> Task.Run
WhenAny picks completed task; WaitAsync(timeout) does not cancel underlying work
Config -> IConfiguration/IOptions; do not commit secrets; ILogger for logging
Hash != HMAC != encryption != signature != password hashing
LINQ Where filters, Select projects; deferred != streaming
IEnumerable = enumerable sequence (often LINQ-to-Objects); IQueryable = provider-backed queries
DIP = principle; DI = technique; container = object construction/lifetime tool
DbContext = EF Core short unit of work; not thread-safe; scoped in web apps
Entity can have BOTH ForeignKeyId and RelatedEntity navigation
EF Core Data Mapper-style persistence != Active Record Save() on entities
```

**Study method:** Read one chapter, type its example, predict what will happen, run it, then explain **why** it behaved that way. Understanding beats memorizing API names.
