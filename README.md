# 📗 My C# / .NET Knowledge Base

> A reviewed, corrected, and expanded version of my personal C# study notes.
> Goal: understand C# **deeply** (what the CLR is really doing), remember it, pass interviews, and use it in real projects.
> Companion to my JavaScript knowledge base (many ideas, such as closures, async, and immutability, appear in both, so I add 🔁 cross-links).

**Version assumption:** modern .NET (8, 9, 10) and C# 12+. Where something differs in old **.NET Framework**, it is stated.

---

## 🏷️ Legend

| Marker | Meaning |
|---|---|
| 📝 | **From my original notes** (kept, rewritten in simpler English) |
| 🔧 | **Corrected**: my note had a mistake or was misleading |
| ➕ | **Added**: missing concept or explanation |
| 🔁 | Connection to JavaScript |

Template for every topic: **A** What is it? · **B** Why does it exist? · **C** How does it work internally? · **D** Example · **E** Step-by-step · **F** When to use · **G** Common mistakes · **H** Interview questions · **I** Quick revision.
(Small reference topics use a shortened version with the same letters.)

---

## 🔧 Corrections to my original notes (read first)

| # | What my note said | What is actually true |
|---|---|---|
| 1 | `[Serializable]` "allows instances to be serialized… all fields incl. private are included" | True **only for `BinaryFormatter`-style serialization**. `System.Text.Json` **ignores** `[Serializable]`/`[NonSerialized]` (it uses public properties by default). |
| 2 | `BinaryFormatter` "not recommended for security reasons" | Understated. It is **obsolete and dangerous** (deserializing untrusted data can execute attacker-chosen code). In **.NET 9 it was removed** (its APIs throw `PlatformNotSupportedException`). |
| 3 | `[DataContract]`/`[DataMember]` explanation was cut off ("applied to a class to indicate…") | Completed in [Attributes](#41-attributes): `[DataContract]` marks a type as serializable by `DataContractSerializer` (**opt-in**); `[DataMember]` marks each member to include (can be private). |
| 4 | "Immutable types: strings, **tuples**, records" | **Not all tuples.** `System.Tuple<…>` is immutable, but the usual `(int, int)` tuple is `System.ValueTuple`, which is a **mutable struct**. Also: `record` is only *shallowly, by-convention* immutable (init-only props), and `record struct` is mutable unless `readonly`. |
| 5 | "Mutable types: classes, arrays, collections" | Classes are mutable **by default**, but you can design immutable classes. Arrays are always mutable (elements). |
| 6 | Heading "**overriding** operator" | The feature is called operator **overloading**. *Overriding* is for `virtual` methods. |
| 7 | "Logical operators `&&`, `\|\|`, `!` can be overloaded" | `!` yes. **`&&` and `\|\|` cannot be overloaded directly**; they're derived from overloading `&`, `\|`, `true`, `false`. Also comparison operators must be overloaded **in pairs** (`==`/`!=`, `<`/`>`, `<=`/`>=`). |
| 8 | "Single-cast delegates vs multicast delegates" as two *kinds* | In .NET **every delegate is multicast** (derives from `MulticastDelegate`). "Single-cast" just means its invocation list has one method. |
| 9 | Generics "without sacrificing performance" | Correct, but incomplete. *Why:* the CLR generates specialized code per **value type** (no boxing), and generics are **reified** (the type `T` exists at runtime, unlike Java's erasure). |
| 10 | Hashing examples "SHA256, **MD5**"; symmetric "AES, **DES**" | **MD5 and SHA-1 are broken for security** and **DES is insecure**. Use SHA-256/512, AES-GCM. Passwords need a *slow* KDF (PBKDF2/Argon2/bcrypt), not plain SHA-256. |
| 11 | "Asymmetric encryption is often used for secure communication" | Accurate but partial: in practice asymmetric crypto is used for **key exchange and signatures**, then **symmetric** encryption protects the actual data (this is how TLS works). |
| 12 | `App.Config`: "storing API keys" | **Don't store secrets in config files.** Use environment variables, User Secrets (dev), Azure Key Vault etc. Also: in modern .NET the standard is `appsettings.json` + `Microsoft.Extensions.Configuration`. |
| 13 | Registry & Event Log presented as general C# features | Both are **Windows-only** and in modern .NET need the right packages/guards (`Microsoft.Win32.Registry`, `System.Diagnostics.EventLog`). Creating an event *source* requires **admin rights**. |
| 14 | `[Conditional]`: "method call or attribute should be included or omitted" | Precisely: `[Conditional("DEBUG")]` on a **`void` method** makes the **call sites** disappear at compile time if the symbol isn't defined (the method itself is still compiled). It can also be applied to attribute *classes*. |
| 15 | "Mutexes" listed as just one of many locks | `Mutex` is special: it can be **named and shared across processes** (heavier). For in-process locking use `lock`/`Monitor`. |
| 16 | "Task class… run concurrently" | A `Task` is **not a thread**. It's a *promise of a result*; CPU-bound tasks use thread-pool threads, but I/O-bound `async` tasks use **no thread while waiting**. |
| 17 | Typos | `Delagates`→Delegates, `MenberInfo`→`MemberInfo`, `labrary`→library, `systtem`→System, `opject`→object, `ovveriding`→overloading. |

---

## 📚 Table of Contents

**Part 1: Foundations**
- [1.1 .NET Basics: CLR, IL, Assemblies, Metadata ➕](#11-net-basics-clr-il-assemblies-metadata)
- [1.2 Value vs Reference Types, Boxing, GC, `IDisposable` ➕](#12-value-vs-reference-types-boxing-gc-idisposable)

**Part 2: Types and Data**
- [2.1 Mutable vs Immutable Types](#21-mutable-vs-immutable-types)
- [2.2 Generics](#22-generics)
- [2.3 StringBuilder](#23-stringbuilder)
- [2.4 Operator Overloading](#24-operator-overloading)

**Part 3: Delegates and Events**
- [3.1 Delegates](#31-delegates)
- [3.2 Events ➕](#32-events)

**Part 4: Metadata-Driven Programming**
- [4.1 Attributes](#41-attributes)
- [4.2 Custom Attributes](#42-custom-attributes)
- [4.3 Reflection](#43-reflection)

**Part 5: Serialization**
- [5.1 Serialization and Deserialization](#51-serialization-and-deserialization)

**Part 6: Multithreading and Parallel Programming**
- [6.1 Process vs Thread](#61-process-vs-thread)
- [6.2 Thread Synchronization](#62-thread-synchronization)
- [6.3 Task, `async/await`](#63-task-and-asyncawait)
- [6.4 Parallel Programming](#64-parallel-programming)

**Part 7: Configuration, Windows and Logging**
- [7.1 App.Config / appsettings.json](#71-appconfig-and-modern-configuration)
- [7.2 Windows Registry](#72-windows-registry)
- [7.3 Event Log](#73-event-log)

**Part 8: Cryptography**
- [8.1 Cryptography](#81-cryptography)

**Part 9: Important Missing Topics ➕**
- [9.1 LINQ](#91-linq) · [9.2 Exceptions](#92-exception-handling) · [9.3 Collections](#93-collections-cheat-sheet)

**Part 10: Review Tools**
- [10.1 Master Interview Q&A](#101-master-interview-qa) · [10.2 Output Prediction](#102-output-prediction-practice) · [10.3 Roadmap](#103-study-roadmap) · [10.4 Cheat Sheet](#104-one-page-cheat-sheet)

---

# PART 1: Foundations

> ➕ My notes started at advanced topics. These two sections are the base that **Reflection, Attributes, Generics, Serialization and Threading** all depend on.

## 1.1 .NET Basics: CLR, IL, Assemblies, Metadata

### A. What is it?
- **.NET** = the platform: runtime + base class library (BCL) + tools.
- **CLR (Common Language Runtime)** = the **runtime engine** that executes C# programs: loads assemblies, JIT-compiles code, manages memory (GC), enforces type safety, handles threads and exceptions.
- **IL (Intermediate Language / CIL)** = the platform-neutral bytecode that the C# compiler produces.
- **Assembly** = the compiled unit (`.dll` or `.exe`): contains **IL code + metadata + manifest (+ resources)**.
- **Metadata** = a description of every type, member, attribute, and reference inside the assembly. **This is what Reflection reads.** 📝

### B. Why does it exist?
One compiled output (IL) runs on any OS/CPU that has a CLR. Metadata makes assemblies **self-describing**, which enables reflection, serialization, debugging, and tooling.

### C. How it works internally
```text
 C# source (.cs)
     │  Roslyn compiler
     ▼
 Assembly (.dll/.exe) = IL + metadata + manifest
     │  run → CLR loads it
     ▼
 JIT compiler (RyuJIT): IL → native machine code (per method, on first call, tiered)
     ▼
 CPU executes   (+ GC, thread pool, type system services from the CLR)
```
- **JIT, tiered compilation:** methods start quickly (tier 0) and hot ones get re-optimized (tier 1, PGO). 🔁 *Same idea as V8's interpreter + optimizing JIT.*
- **Native AOT** (optional): compile to native code ahead of time (faster startup, no JIT, but **reflection is restricted**).

### D. Example
```csharp
using System.Reflection;

Assembly asm = typeof(string).Assembly;       // the assembly that defines System.String
Console.WriteLine(asm.GetName().Name);        // System.Private.CoreLib
Console.WriteLine(asm.GetTypes().Length > 1000); // True (metadata lists the types)
```

### G. Common mistakes
- Thinking C# compiles directly to machine code (it compiles to **IL**; the JIT does the rest at runtime).
- Confusing **namespace** (logical grouping, in code) with **assembly** (physical file).

### I. Quick revision
C# → IL + metadata in an assembly → CLR JIT-compiles to machine code. Metadata is what makes reflection and serialization possible.

---

## 1.2 Value vs Reference Types, Boxing, GC, `IDisposable`

### A. What is it?
| | **Value types** | **Reference types** |
|---|---|---|
| Declared with | `struct`, `enum`, primitives (`int`, `bool`, `double`, `char`, `decimal`…), `ValueTuple` | `class`, `interface`, `delegate`, `record` (class), `string`, arrays, `object` |
| Variable holds | the **value itself** | a **reference** to an object on the **heap** |
| Assignment | **copies** the value | copies the **reference** (shared object) |
| Default | zeroed (`0`, `false`) | `null` |

### C. Internally
- Value-type **locals** typically live on the **stack** (or in registers); when a value type is a **field of a class**, it lives **inside that heap object**. 🔧 *"Value types = stack, reference types = heap" is a simplification.*
- Reference-type objects live on the **managed heap**; the **GC** (generational: Gen0/1/2 + Large Object Heap) frees unreachable objects. 🔁 *Same reachability idea as JS.*
- **Boxing:** converting a value type to `object`/interface allocates a heap object and **copies** the value into it. **Unboxing** copies it back (with a type check). Boxing = allocation + GC pressure.

### D. Example
```csharp
struct PointS { public int X; }
class  PointC { public int X; }

var s1 = new PointS { X = 1 }; var s2 = s1; s2.X = 99;
var c1 = new PointC { X = 1 }; var c2 = c1; c2.X = 99;
Console.WriteLine(s1.X); // 1   (copy)
Console.WriteLine(c1.X); // 99  (same object)

int i = 10;
object o = i;          // BOXING: heap allocation, copy of 10
i = 20;
Console.WriteLine(o);  // 10 (the box is a separate copy)
int j = (int)o;        // UNBOXING
```

### Deterministic cleanup: `IDisposable` / `using` ➕
The GC frees **memory** but **not unmanaged resources** (files, sockets, DB connections, handles). Types owning such resources implement `IDisposable`.
```csharp
using var stream = new FileStream("a.txt", FileMode.Open);   // Dispose() called at end of scope
// or: using (var s = ...) { ... }
// async: await using var conn = ...;   (IAsyncDisposable)
```
Compiler turns `using` into `try { ... } finally { stream.Dispose(); }`.

### G. Common mistakes
- Unintended boxing in hot paths (`ArrayList`, `object` params, non-generic collections). Use generics (see 2.2).
- **Mutable structs** (copy surprises; see 2.1).
- Forgetting `Dispose` → leaked handles/connections even though GC runs.
- Calling `GC.Collect()` manually (almost never right).

### H. Interview questions
**Q: Struct vs class?** Value vs reference semantics, copying, inheritance (structs can't inherit classes), default values.
**Q: What is boxing and why is it costly?** Wrapping a value type in a heap object; allocation + copy + GC pressure.
**Q: Does the GC call `Dispose`?** No. A finalizer may release resources late; use `using`/`Dispose`.

### I. Quick revision
Value = copy, reference = shared. Boxing allocates. GC frees memory only; `using`/`IDisposable` frees resources.

---

# PART 2: Types and Data

## 2.1 Mutable vs Immutable Types

📝 *From my notes, corrected and expanded.*

### A. What is it?
- **Mutable:** state **can change** after creation (most classes, arrays, `List<T>`, `StringBuilder`).
- **Immutable:** state **can never change**; "changing" produces a **new** object (`string`, `DateTime`, `System.Tuple`, `ImmutableList<T>`, well-designed value objects/records).

### B. Why does it matter?
- **Thread safety:** immutable objects can be shared across threads **without locks** (no one can change them). 📝
- **Predictability:** no surprise side effects, no "who changed this?" bugs.
- Safe as **dictionary keys**, cacheable, easier to reason about/test.
- Cost: modifications allocate new objects (so use `StringBuilder` for heavy string building).

### C. Internally
`string` is a reference type but immutable: every "modification" creates a **new string object**; old ones become garbage.

### D. Example
```csharp
string a = "hi";
string b = a;
a += " there";                 // creates a NEW string; `b` is untouched
Console.WriteLine(b);          // hi

// Immutable class (by design)
public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount, string currency) => (Amount, Currency) = (amount, currency);
    public Money Add(Money o) => new(Amount + o.Amount, Currency);  // returns new instance
}

// Records give "with" copies (init-only properties)
public record Person(string Name, int Age);
var p1 = new Person("Ali", 30);
var p2 = p1 with { Age = 31 };           // new object; p1 unchanged
// p1.Age = 5;  ❌ compile error (init-only)

// Immutable collections (System.Collections.Immutable)
var list = System.Collections.Immutable.ImmutableList.Create(1, 2, 3);
var list2 = list.Add(4);                  // list still has 3 items
```

### E. 🔧 Tuple and record details
```csharp
var vt = (X: 1, Y: 2);        // System.ValueTuple → MUTABLE struct
vt.X = 10;                    // ✅ compiles!
var t = Tuple.Create(1, 2);   // System.Tuple → immutable
// t.Item1 = 5;  ❌

record struct P1(int X);          // mutable by default (positional props are get/set)
readonly record struct P2(int X); // immutable
public record Box(List<int> Items);   // reference is fixed, but the LIST is still mutable ("shallow" immutability)
```

### G. Common mistakes
- Believing `readonly` field / `IReadOnlyList<T>` = immutable. `readonly` only stops **re-assignment of the field**; `IReadOnlyList<T>` is a **read-only view** (the underlying list may still change). Use `ImmutableArray/List` or copy.
- **Mutable structs:** `list[0].X = 5;` on `List<MutableStruct>` doesn't compile; in a `foreach`, you modify a **copy**. Prefer `readonly struct`.
- `str += x` in a big loop (see 2.3).

### H. Interview questions
**Q: Why is `string` immutable?** Thread safety, safe hashing/dictionary keys, string interning, security (paths, connection strings can't be altered after validation).
**Q: Benefits of immutability for concurrency?** No locks needed for reading; no race conditions on state.
**Q: Are records immutable?** Positional `record class` properties are init-only (shallow); not deeply immutable.

### I. Quick revision
Immutable = safe to share, new object per change. `string` is immutable. `ValueTuple` is **mutable**. `readonly` ≠ immutable. Records are shallowly immutable.

🔁 *In JS the same idea:* primitives and strings are immutable; `const` ≠ immutable; `structuredClone`/spread make copies.

---

## 2.2 Generics

📝 *From my notes, expanded.*

### A. What is it?
**Generics** let you write classes/methods/interfaces/delegates with **type parameters** (`T`) that are filled in later, with **compile-time type safety**.

### B. Why does it exist?
Before generics (`ArrayList` of `object`): no type safety (runtime `InvalidCastException`) and **boxing** for value types. Generics give: ✅ **reusability** (one `Stack<T>`, not `IntStack`/`StringStack`), ✅ **type safety**, ✅ **performance** (no boxing/casting). 📝

### C. Internally ➕
- The CLR creates a **closed constructed type** per type argument: `List<int>`, `List<string>`.
- **Value-type arguments** (`List<int>`) → the JIT generates **specialized native code** (no boxing).
- **Reference-type arguments** share one native implementation (all references look alike).
- 🔧 Generics are **reified**: `typeof(List<int>)` exists at runtime (`List<int>` ≠ `List<string>`). *(Java erases types; C# doesn't.)*

### D. Example
```csharp
// Generic class with constraints
public interface IEntity { int Id { get; } }

public class Repository<T> where T : class, IEntity, new()
{
    private readonly Dictionary<int, T> _items = new();
    public void Add(T item) => _items[item.Id] = item;
    public T? Get(int id) => _items.GetValueOrDefault(id);
    public T Create() => new T();         // allowed because of new() constraint
}

// Generic method + type inference
static T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;
Console.WriteLine(Max(3, 7));          // 7   (T inferred as int)
Console.WriteLine(Max("a", "b"));      // b

// default(T), typeof(T)
static T Zero<T>() => default!;        // 0 for int, null for string

// Built-in generic delegates
Func<int, int, int> add = (x, y) => x + y;
```

| Constraint | Meaning |
|---|---|
| `where T : struct` | value type |
| `where T : class` | reference type |
| `where T : new()` | has public parameterless ctor |
| `where T : BaseClass / IFace` | derives/implements |
| `where T : notnull`, `unmanaged`, `Enum`, `Delegate` | more specialized |

**Variance ➕:** `IEnumerable<out T>` (covariant: `IEnumerable<string>` → `IEnumerable<object>` ✅), `Action<in T>` (contravariant). `List<T>` is **invariant**.

### F. When to use
Collections, repositories, result wrappers (`Result<T>`), utility methods working on many types, any place you'd otherwise use `object` + casts.

### G. Common mistakes
- Using `ArrayList`/`Hashtable` (non-generic) → boxing + no type safety.
- Forgetting constraints, then trying `new T()` or `a > b` on `T` (won't compile). 
- Each closed generic type has its **own static fields**: `Counter<int>.Count` ≠ `Counter<string>.Count`.
- Over-abstracting: generics everywhere hurts readability.

### H. Interview questions
**Q: Why generics over `object`?** Compile-time safety, no boxing/casting, reusable.
**Q: Generics in C# vs Java?** C# generics are reified at runtime and specialized for value types; Java erases them.
**Q: What is `where T : class`?** A constraint limiting `T` to reference types.

### I. Quick revision
One definition, many types. Safe + fast (no boxing). Reified. Constraints with `where`. Prefer `List<T>` over `ArrayList`.

---

## 2.3 StringBuilder

📝 *From my notes, expanded.*

### A. What is it?
`System.Text.StringBuilder`: a **mutable** buffer for building strings efficiently. 📝

### B. Why does it exist?
Strings are immutable, so `s += "x"` in a loop allocates a **new string every time**: **O(n²)** copying for n appends, lots of garbage.

### C. Internally
`StringBuilder` keeps an internal **char buffer (a linked chain of chunks)** with a `Capacity`. `Append` writes into spare capacity; when full, it adds a new chunk (no copying of the old text). `ToString()` creates **one** final string.

### D. Example
```csharp
// ❌ Slow: ~100,000 intermediate strings
string s = "";
for (int i = 0; i < 100_000; i++) s += i;

// ✅ Fast
var sb = new StringBuilder(capacity: 1024);   // pre-size if you know roughly how much
for (int i = 0; i < 100_000; i++) sb.Append(i).Append(',');
string result = sb.ToString();

// Operations from my notes 📝
var b = new StringBuilder("Hello");
b.Append(" World");            // Hello World
b.Insert(0, ">> ");            // >> Hello World
b.Replace("World", "C#");      // >> Hello C#
b.Remove(0, 3);                // Hello C#
b.AppendLine("!").AppendFormat("{0:N2}", 3.14159);
Console.WriteLine(b.ToString());
```

### F. When to use
Many appends/inserts, especially in **loops** or with unknown count (reports, CSV/HTML/SQL text generation).

### G. Common mistakes
- Using it for 2–4 concatenations (the compiler already optimizes `a + b + c` into one `string.Concat`; `$"{a} {b}"` is also fine).
- Calling `ToString()` inside the loop (creates strings again).
- Assuming it's thread-safe (it isn't).
- Better tools for known pieces: `string.Join(",", items)`.

### H. Interview questions
**Q: `string` vs `StringBuilder`?** Immutable vs mutable; use `StringBuilder` for repeated modification.
**Q: Why is `+=` in a loop slow?** Each `+=` allocates a new string and copies all characters.

### I. Quick revision
`StringBuilder` = mutable text buffer: `Append/Insert/Replace/Remove/ToString`. Use in loops; not needed for a few concatenations.

---

## 2.4 Operator Overloading

📝 *From my notes (🔧 heading was "overriding"), corrected.*

### A. What is it?
Defining custom behavior for operators (`+ - * / == < ++ …`) on **your own types**, so `a + b` works for them. 📝

### B. Why does it exist?
Readability for math-like types: `Vector`, `Money`, `Fraction`, `Matrix`, `TimeSpan`, `DateTime - DateTime`. 

### C. Rules ➕
- Method must be **`public static`**, use the `operator` keyword, and **at least one parameter must be the containing type**.
- Overloaded **in pairs**: `==`/`!=`, `<`/`>`, `<=`/`>=`. If you overload `==`, also override `Equals` and `GetHashCode` (and ideally implement `IEquatable<T>`).
- `+=`, `-=`… are **automatic** from `+`, `-`…
- 🔧 `&&`/`||` can't be overloaded directly (they use `&`, `|`, `true`, `false`).
- **Can't overload:** `=`, `.`, `?:`, `new`, `is`, `typeof`, `??`.
- Also available: **conversion operators** `implicit`/`explicit`, unary `+ - ! ~ ++ --`, `true/false`, and (C# 11+) `checked` variants.

### D. Example
```csharp
public readonly struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    public Money(decimal amount, string currency) { Amount = amount; Currency = currency; }

    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency) throw new InvalidOperationException("Currency mismatch");
        return new Money(a.Amount + b.Amount, a.Currency);
    }
    public static Money operator -(Money a) => new(-a.Amount, a.Currency);       // unary
    public static Money operator *(Money a, decimal k) => new(a.Amount * k, a.Currency);

    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);
    public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
    public static bool operator <(Money a, Money b) => a.Amount < b.Amount;

    public static implicit operator decimal(Money m) => m.Amount;               // Money → decimal

    public bool Equals(Money o) => Amount == o.Amount && Currency == o.Currency;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount:N2} {Currency}";
}

var total = new Money(10, "USD") + new Money(5, "USD");
total += new Money(1, "USD");            // += works automatically
Console.WriteLine(total);                // 16.00 USD
decimal d = total;                       // implicit conversion
```

### E. Step-by-step (`a + b`)
1. The compiler sees `+` with two `Money` operands.
2. It finds `Money.operator +(Money, Money)` and compiles the expression into a **static method call** `Money.op_Addition(a, b)`.
3. The result type is `Money`.

### G. Common mistakes
- Overloading `==` but not `Equals/GetHashCode` (compiler warning + broken dictionaries).
- Surprising semantics (e.g., `+` that mutates or does I/O).
- Overloading on mutable reference types, so `==` stops meaning reference equality and confuses callers.
- Using `implicit` conversions that lose data (use `explicit`).

### H. Interview questions
**Q: Which operators can't be overloaded?** `=`, `.`, `?:`, `??`, `new`, `is`, `as`, `typeof`; `&&`/`||` indirectly only.
**Q: Overloading vs overriding?** Overloading = same name, different signature (operators/methods). Overriding = replacing a `virtual` method in a derived class.

### I. Quick revision
`public static T operator +(T a, T b)`. Pair comparison operators. Keep `Equals/GetHashCode` consistent. Keep semantics intuitive.

---

# PART 3: Delegates and Events

## 3.1 Delegates

📝 *From my notes (🔧 "Delagates" typo, single/multi-cast clarified), expanded.*

### A. What is it?
A **delegate** is a **type-safe reference to a method** with a specific signature (parameter list + return type). It lets you treat a method as an **object**: store it in a variable, pass it as an argument, return it, invoke it later. 📝

> 🔁 *In JS, functions are first-class values. In C#, delegates are how methods become first-class values.*

### B. Why does it exist?
- **Callbacks** (run my code when something finishes). 📝
- **Event handling** and the **observer pattern**, which **decouples** sender from receiver. 📝
- Passing behavior into algorithms (`List.Sort(comparison)`, LINQ's `Where(predicate)`).

### C. How it works internally ➕
- `delegate` declares a **class** derived from `System.MulticastDelegate`.
- A delegate instance stores **Target** (the object, or `null` for static methods) and **Method** (pointer to the method).
- 🔧 **Every delegate has an invocation list** (so all delegates are multicast). `+=` creates a **new** delegate with a combined list (delegates are **immutable**). Invocation calls each method **in order** on the same thread.
- **Multicast with return values:** you get the return value of the **last** method only. If one handler **throws**, the rest of the chain **doesn't run**.

📝 **Types:**
- **Single-cast:** list has one method.
- **Multi-cast:** list has several; invoking calls all in order.

### D. Example
```csharp
// 1) Declare + use
public delegate int MathOp(int a, int b);

static int Add(int a, int b) => a + b;
static int Mul(int a, int b) => a * b;

MathOp op = Add;                 // reference a method
Console.WriteLine(op(2, 3));     // 5
op = Mul;
Console.WriteLine(op(2, 3));     // 6

// 2) Anonymous method / lambda 📝
MathOp sub = delegate (int a, int b) { return a - b; };
MathOp pow = (a, b) => (int)Math.Pow(a, b);

// 3) Built-in generic delegates (use these instead of declaring your own)
Func<int, int, int> f  = (a, b) => a + b;     // returns a value (last type = return type)
Action<string>      log = msg => Console.WriteLine(msg);   // returns void
Predicate<int>      isEven = n => n % 2 == 0;               // returns bool

// 4) Multicast 📝
Action greet = () => Console.Write("Hello ");
greet += () => Console.Write("World ");
greet += () => Console.WriteLine("!");
greet();                         // Hello World !
Console.WriteLine(greet.GetInvocationList().Length); // 3

// 5) Passing behavior (callback)
static void Process(int[] data, Func<int, bool> filter, Action<int> onMatch)
{
    foreach (var x in data) if (filter(x)) onMatch(x);
}
Process(new[] { 1, 2, 3, 4 }, n => n % 2 == 0, n => Console.WriteLine($"Even: {n}"));

// 6) Safe invocation of possibly-null delegate
Action? maybe = null;
maybe?.Invoke();                 // no NullReferenceException
```

### E. Step-by-step (`greet += ...; greet();`)
1. `greet` points to a delegate with 1 method.
2. `+=` calls `Delegate.Combine`, which creates a **new** delegate holding both methods (old one untouched).
3. `greet()` iterates the invocation list: runs method 1, method 2, method 3, in order.

### F. When to use
Callbacks, strategy pattern, event publishing, LINQ predicates/selectors, `Task.Run(() => …)`, UI handlers.

### G. Common mistakes
```csharp
// 1) Closures capture VARIABLES, not values (🔁 same trap as JS `var` in loops)
var actions = new List<Action>();
for (int i = 0; i < 3; i++) actions.Add(() => Console.Write(i));   // prints 333
foreach (var a in actions) a();
// ✅ for `foreach (var x in ...)` each iteration has its own x (since C# 5), but `for` shares `i`:
for (int i = 0; i < 3; i++) { int copy = i; actions.Add(() => Console.Write(copy)); } // 012

// 2) Return value of a multicast delegate = last handler only
// 3) A throwing handler stops the rest of the chain
// 4) Delegates keep their Target object alive → memory leak if you never unsubscribe (-=)
// 5) Invoking null delegate → NullReferenceException → use ?.Invoke
```

### H. Interview questions
**Q: What is a delegate?** A type-safe method reference; an object that holds a pointer to a method (and its target).
**Q: `Func` vs `Action` vs `Predicate`?** Returns a value / returns void / returns bool.
**Q: What is a multicast delegate?** One that holds several methods and calls them all in order.
**Q: Delegate vs interface?** Delegate = single-method contract, lightweight; interface = multi-member contract.

### I. Quick revision
Delegate = type-safe function pointer object. Use `Func/Action/Predicate` + lambdas. `+=` makes multicast; last return wins; exceptions stop the chain. Beware closures and leaks.

---

## 3.2 Events

➕ *My notes mention events and the observer pattern but not the `event` keyword.*

### A. What is it?
An **event** is a delegate field with **restricted access**: outside code can only **subscribe (`+=`) or unsubscribe (`-=`)**. Only the **declaring class** can **raise** it. 

### B. Why does it exist?
A plain public delegate field lets anyone invoke it or overwrite it (`obj.Handler = null` wipes all subscribers). `event` protects the publisher.

### D. Example (standard .NET pattern)
```csharp
public class OrderEventArgs : EventArgs
{
    public int OrderId { get; init; }
}

public class OrderService                           // PUBLISHER
{
    public event EventHandler<OrderEventArgs>? OrderPlaced;

    public void PlaceOrder(int id)
    {
        Console.WriteLine($"Order {id} saved");
        OnOrderPlaced(new OrderEventArgs { OrderId = id });
    }
    protected virtual void OnOrderPlaced(OrderEventArgs e) => OrderPlaced?.Invoke(this, e);
}

class EmailSender                                    // SUBSCRIBER
{
    public void Handle(object? sender, OrderEventArgs e) => Console.WriteLine($"Email sent for {e.OrderId}");
}

var svc = new OrderService();
var email = new EmailSender();
svc.OrderPlaced += email.Handle;                    // subscribe
svc.OrderPlaced += (s, e) => Console.WriteLine("Audit log");
svc.PlaceOrder(42);
svc.OrderPlaced -= email.Handle;                    // unsubscribe (important!)
```
Output: `Order 42 saved` → `Email sent for 42` → `Audit log`.

### G. Common mistakes
- **Not unsubscribing** → publisher keeps subscriber alive (classic memory leak). 🔁 *Same as forgotten `removeEventListener` in JS.*
- Raising without null check (`OrderPlaced(this, e)` when there are no subscribers → NRE).
- Long-running/throwing handlers block/break the publisher.
- Unsubscribing a **lambda** with a new lambda (different delegate instance; store it in a variable).

### H. Interview questions
**Q: Event vs delegate?** Event restricts external code to `+=`/`-=`; only the owner raises it.
**Q: Why use `EventHandler<T>`?** Standard `(object sender, TEventArgs e)` signature for consistency.

### I. Quick revision
`event` = encapsulated delegate. Publisher raises; subscribers `+=`/`-=`. Always unsubscribe long-lived publishers.

---

# PART 4: Metadata-Driven Programming

> **Attributes** attach metadata → stored in the assembly → **Reflection** reads it at runtime → frameworks (ASP.NET, EF Core, serializers, test runners) change behavior based on it. These three topics form one chain.

## 4.1 Attributes

📝 *From my notes (the `[DataContract]` item was cut off, now completed).*

### A. What is it?
An **attribute** is declarative metadata you attach to code elements (assembly, class, method, property, field, parameter, return value…) using `[Square]` brackets. By itself it does **nothing**; something must **read** it (the compiler, the runtime, or your reflection code).

### B. Why does it exist?
To **describe** code without changing its logic: mark things obsolete, control serialization, configure validation/routing/mapping, conditional compilation.

### C. How it works internally ➕
- The compiler stores each attribute in the assembly **metadata** as: *attribute type + constructor arguments + named property values*.
- The attribute object is **not created** when your program runs. It's **instantiated lazily** only when something asks via reflection (`GetCustomAttribute<T>()`).
- Compiler-recognized attributes (`[Obsolete]`, `[Conditional]`) are acted on **at compile time**.
- Naming: class `ObsoleteAttribute` is used as `[Obsolete]` (the `Attribute` suffix is optional).

### The built-in attributes from my notes 📝

**1. `[Conditional("SYMBOL")]`** : call sites are **removed at compile time** if the symbol isn't defined.
```csharp
using System.Diagnostics;

class Log
{
    [Conditional("DEBUG")]                 // 🔧 only on void methods; the CALLS vanish in Release
    public static void Debug(string msg) => Console.WriteLine($"[DEBUG] {msg}");
}
Log.Debug("only in debug builds");         // compiled away in Release (argument expressions aren't even evaluated)
```
(`#define TRACE_ON` at top of file or `<DefineConstants>` in `.csproj` defines symbols. Different from `#if DEBUG`, which removes the **code**.)

**2. `[Obsolete]`** : warns (or errors) when used.
```csharp
[Obsolete("Use NewMethod() instead.")]            // warning
public void OldMethod() { }
[Obsolete("Removed in v3, use NewMethod().", error: true)]   // compile ERROR
public void VeryOldMethod() { }
```

**3. `[Serializable]`** / **4. `[NonSerialized]`** : (legacy, binary-style serialization) 
```csharp
[Serializable]
class Account
{
    public string Owner;
    [NonSerialized] public string Password;   // 📝 excluded (only for fields, in a [Serializable] class)
}
```
🔧 `System.Text.Json` does **not** use these. It uses public properties and `[JsonIgnore]`/`[JsonPropertyName]`. `[Serializable]` mostly matters for `BinaryFormatter` (**removed in .NET 9**) and some old APIs.

**5. `[DataContract]` and `[DataMember]`** *(completed 🔧)*: used by `DataContractSerializer` (and WCF).
- `[DataContract]` on a **type** = "this type participates in serialization" (**opt-in model**: only members with `[DataMember]` are serialized).
- `[DataMember]` on a **field/property** = include this member (can be **private**); options: `Name`, `Order`, `IsRequired`, `EmitDefaultValue`.
- `[IgnoreDataMember]` excludes a member (used with non-opt-in models).
```csharp
[DataContract]
class Customer
{
    [DataMember(Name = "id", Order = 1)] public int Id { get; set; }
    [DataMember(Order = 2)] private string _secretNote = "x";   // private but serialized
    public string NotSerialized { get; set; } = "";             // no [DataMember] → skipped
}
```

### More attributes you'll meet constantly ➕
| Attribute | Purpose |
|---|---|
| `[Flags]` | enum as bit flags |
| `[CallerMemberName]` / `[CallerFilePath]` | auto-fill caller info into a parameter |
| `[JsonIgnore]`, `[JsonPropertyName("x")]` | System.Text.Json control |
| `[Required]`, `[MaxLength(50)]`, `[Range]` | validation (DataAnnotations) |
| `[HttpGet("/users")]`, `[Authorize]` | ASP.NET Core |
| `[Fact]`, `[Test]`, `[TestMethod]` | unit tests |
| `[DllImport]` / `[LibraryImport]` | P/Invoke (call native code) |
| `[assembly: InternalsVisibleTo("X.Tests")]` | assembly-level attribute |

### G. Common mistakes
- Expecting an attribute to "do something" with no reader code.
- Using `[NonSerialized]` on a **property** (only fields).
- Mixing serializer attribute families (`[JsonIgnore]` is ignored by `DataContractSerializer`, and vice versa).

### H. Interview questions
**Q: What is an attribute?** Declarative metadata stored in the assembly, read through reflection (or the compiler).
**Q: `[Conditional]` vs `#if`?** `[Conditional]` removes **call sites** of a void method; `#if` removes **source code**.

### I. Quick revision
Attributes = metadata only. Stored in assembly. Read with reflection. Know `Conditional`, `Obsolete`, `Serializable/NonSerialized` (legacy), `DataContract/DataMember`.

---

## 4.2 Custom Attributes

📝 *From my notes, expanded with a full working example.*

### A. What is it?
Your own attribute: a class **inheriting `System.Attribute`**, applied with `[...]`, and read at runtime with reflection. 📝

### B. Why does it exist?
To build **declarative frameworks**: validation rules, command/route registration, plugin discovery, mapping configuration, permission checks.

### C. Rules ➕
- Inherit from `System.Attribute`; name usually ends with `Attribute`.
- Constructor parameters = **positional** args; public settable properties = **named** args.
- Parameter types are limited to **compile-time constants** (`int`, `string`, `Type`, enums, arrays of those…).
- Control usage with `[AttributeUsage(AttributeTargets.…, AllowMultiple, Inherited)]`.

### D. Example: a tiny validation framework
```csharp
using System.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MaxLengthAttribute : Attribute
{
    public int Length { get; }
    public string? Message { get; set; }                  // named argument
    public MaxLengthAttribute(int length) => Length = length;   // positional argument
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredFieldAttribute : Attribute { }

public class User
{
    [RequiredField, MaxLength(5, Message = "Name too long")]
    public string? Name { get; set; }

    [MaxLength(10)]
    public string? City { get; set; }
}

public static class Validator
{
    public static List<string> Validate(object obj)
    {
        var errors = new List<string>();
        foreach (PropertyInfo prop in obj.GetType().GetProperties())
        {
            object? value = prop.GetValue(obj);

            if (prop.GetCustomAttribute<RequiredFieldAttribute>() is not null && value is null)
                errors.Add($"{prop.Name} is required");

            if (prop.GetCustomAttribute<MaxLengthAttribute>() is { } max
                && value is string s && s.Length > max.Length)
                errors.Add(max.Message ?? $"{prop.Name} exceeds {max.Length}");
        }
        return errors;
    }
}

var u = new User { Name = "Alexander", City = "Rabat" };
Console.WriteLine(string.Join(", ", Validator.Validate(u)));   // Name too long
```

### E. Step-by-step
1. Compiler writes `[RequiredField, MaxLength(5, Message=…)]` into metadata for `User.Name`.
2. At runtime `GetProperties()` returns `PropertyInfo` objects (via reflection).
3. `GetCustomAttribute<MaxLengthAttribute>()` **now** constructs the attribute object from metadata (`new MaxLengthAttribute(5) { Message = … }`).
4. Our code reads `Length`, compares, and builds the error list.

### F. When to use
Validation, ORM mapping, routing/command discovery, plugin markers, authorization rules, serialization hints, test discovery.

### G. Common mistakes
- Forgetting `[AttributeUsage]` (attribute can then be placed anywhere).
- Reflecting on every call (slow): **cache** results (`static` dictionary) or use **source generators**.
- Putting logic in the attribute class (it should **hold data**; the behavior lives in the code that reads it).

### I. Quick revision
Inherit `Attribute` → apply → read via `GetCustomAttribute`. Constants only. `[AttributeUsage]` restricts targets. Cache reflection.

---

## 4.3 Reflection

📝 *From my notes (🔧 "MenberInfo" = `MemberInfo`), expanded.*

### A. What is it?
**Reflection** = the ability to **inspect and manipulate metadata of types at runtime**: discover types/members in assemblies, create objects, call methods, read/write properties and fields, even private ones. 📝

### B. Why does it exist?
Some programs can't know types at compile time: **plugins, serializers (`System.Text.Json`), dependency injection containers, ORMs, test frameworks, object mappers, ASP.NET model binding**. 

### C. How it works internally ➕
- Reflection reads the **metadata tables** in loaded assemblies (see 1.1) and builds **objects that describe them**:
```text
Assembly ─► Module ─► Type ─► MemberInfo (base class)
                              ├─ FieldInfo
                              ├─ PropertyInfo
                              ├─ MethodInfo
                              ├─ ConstructorInfo
                              ├─ EventInfo
                              └─ (Nested types)
```
- Getting a `Type`: `typeof(T)` (compile-time), `obj.GetType()` (runtime), `Type.GetType("Namespace.Name")`, `assembly.GetTypes()`.
- Invoking via reflection is **slower** (security/type checks, boxing args, no inlining), roughly 10–100× slower than a direct call.

### D. Example
```csharp
using System.Reflection;

public class Person
{
    public string Name { get; set; } = "";
    private int _age = 20;
    public Person() { }
    public Person(string name) => Name = name;
    public string Greet(string other) => $"Hi {other}, I'm {Name}";
    private string Secret() => "hidden";
}

Type t = typeof(Person);

// 1) Inspect
foreach (var p in t.GetProperties()) Console.WriteLine($"Prop: {p.PropertyType.Name} {p.Name}");
foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    Console.WriteLine($"Method: {m.Name}");

// 2) Create instances
object o1 = Activator.CreateInstance(t)!;                           // parameterless ctor
object o2 = Activator.CreateInstance(t, "Sara")!;                   // ctor(string)
ConstructorInfo ctor = t.GetConstructor(new[] { typeof(string) })!;
object o3 = ctor.Invoke(new object[] { "Omar" });

// 3) Get/Set properties dynamically
t.GetProperty("Name")!.SetValue(o1, "Ali");
Console.WriteLine(t.GetProperty("Name")!.GetValue(o1));             // Ali

// 4) Invoke methods
string r = (string)t.GetMethod("Greet")!.Invoke(o1, new object[] { "Bob" })!;
Console.WriteLine(r);                                               // Hi Bob, I'm Ali

// 5) Private members (needs BindingFlags.NonPublic)
var field = t.GetField("_age", BindingFlags.NonPublic | BindingFlags.Instance)!;
field.SetValue(o1, 99);
var secret = t.GetMethod("Secret", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(o1, null);

// 6) Dynamic loading (plugins)
Assembly plugin = Assembly.LoadFrom("MyPlugin.dll");
foreach (Type pt in plugin.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract))
{
    var instance = (IPlugin)Activator.CreateInstance(pt)!;
    instance.Run();
}

// 7) Generic types at runtime
Type listOfInt = typeof(List<>).MakeGenericType(typeof(int));
object list = Activator.CreateInstance(listOfInt)!;
```

### E. Step-by-step (`GetMethod("Greet").Invoke(...)`)
1. `typeof(Person)` → `Type` object built from `Person`'s metadata.
2. `GetMethod("Greet")` searches the method table by name/binding flags → `MethodInfo`.
3. `Invoke(target, args)` validates arg types, **boxes** value-type args into `object[]`, calls the method, and returns a boxed result.

### F. When to use
Writing frameworks/libraries, plugin systems, generic serializers/mappers, test discovery, reading custom attributes, tooling. **Not** for ordinary application logic.

### G. Common mistakes
- Using reflection where **generics, interfaces, or `dynamic`** would do (and be type-safe and faster).
- Reflecting in hot loops; **cache** `PropertyInfo/MethodInfo`; for speed use compiled **expression trees**, delegates (`CreateDelegate`), or **source generators**.
- Breaking **encapsulation** by accessing privates; breaks when internals change.
- **Trimming / Native AOT:** the trimmer can remove code reflection needs → runtime failures. Prefer source-generated alternatives (e.g., `System.Text.Json` source generation) or annotate with `[DynamicallyAccessedMembers]`.
- Security: loading untrusted assemblies = running untrusted code.
- Typos in string names fail at **runtime**, not compile time (`nameof(Person.Name)` helps).

### H. Interview questions
**Q: What is reflection and when would you use it?** Runtime inspection/manipulation of type metadata; for plugins, serializers, DI, ORMs, attribute-driven frameworks.
**Q: Downsides?** Slower, loses compile-time safety, breaks encapsulation, trimming/AOT-unfriendly.
**Q: `typeof` vs `GetType()`?** `typeof(T)` = static type known at compile time; `GetType()` = actual runtime type of the object.
**Q: How do you read a custom attribute?** `member.GetCustomAttribute<T>()`.

### I. Quick revision
Reflection = read metadata at runtime (`Type`, `MethodInfo`, `PropertyInfo`…); create, get/set, invoke, load assemblies. Powerful but slow/unsafe/untrimmable. Cache it or avoid it.

🔁 *JS equivalent:* `Object.keys`, `Reflect`, `Object.getOwnPropertyDescriptors`. In JS everything is dynamic, so it needs fewer special APIs.

---

# PART 5: Serialization

## 5.1 Serialization and Deserialization

📝 *From my notes (typos fixed), expanded.*

### A. What is it?
- **Serialization:** converting an **object (in memory)** into a **format that can be stored or transmitted** (text/bytes). 📝
- **Deserialization:** rebuilding the object from that format.

### B. Why does it exist?
Objects live in process memory as **graphs of references**: you can't write that to disk or a network. You need a **flat, portable representation**: save to files/DB, send over HTTP, cache (Redis), message queues, deep-clone, inter-process communication.

### C. How it works internally ➕
A serializer typically uses **reflection** (or **source-generated code**) to walk the object's members, converts each value to the format (JSON text, XML, binary), and does the reverse by **creating the object and setting members** when deserializing.

| Option | Format | Notes |
|---|---|---|
| **`System.Text.Json`** ✅ | JSON | Built-in, fast, **default choice** in modern .NET; supports source generation (AOT-friendly) |
| **Newtonsoft.Json (Json.NET)** 📝 | JSON | Very flexible, huge feature set, older; still common in legacy |
| **`System.Xml.Serialization.XmlSerializer`** 📝 | XML | Public members + public parameterless ctor; used by SOAP/config-like data |
| **`DataContractSerializer`** | XML/JSON | Opt-in (`[DataContract]`) |
| **`BinaryFormatter`** 📝 | binary | 🔧 **Obsolete & dangerous; removed in .NET 9. Never use.** |
| **MessagePack, protobuf-net, MemoryPack** ➕ | compact binary | Fast and small; schema/attribute based |

### 🔧 Why `BinaryFormatter` is dangerous
During deserialization it **instantiates whatever types the data says** and runs code during that process (constructors, callbacks, property setters). An attacker who controls the bytes can chain existing types into **remote code execution**. You **cannot** make it safe for untrusted data. (Microsoft: "not recommended", obsoleted, then removed.)

### D. Example
```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [JsonPropertyName("unit_price")] public decimal Price { get; set; }
    [JsonIgnore] public string InternalNote { get; set; } = "";
    public DateTime Created { get; set; }
}

var product = new Product { Id = 1, Name = "Pen", Price = 2.5m, InternalNote = "x", Created = DateTime.UtcNow };

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
};

string json = JsonSerializer.Serialize(product, options);
Console.WriteLine(json);
// { "id": 1, "name": "Pen", "unit_price": 2.5, "created": "2026-10-15T10:00:00Z" }

Product? back = JsonSerializer.Deserialize<Product>(json, options);
Console.WriteLine(back!.Name);      // Pen

// Collections / generics / files
var list = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText("products.json"));
await using var fs = File.Create("p.json");
await JsonSerializer.SerializeAsync(fs, product);       // stream (large data)

// XML
var xs = new System.Xml.Serialization.XmlSerializer(typeof(Product));
using var sw = new StringWriter();
xs.Serialize(sw, product);
string xml = sw.ToString();
```

### E. Step-by-step (`JsonSerializer.Serialize(product)`)
1. Serializer inspects `Product`'s public properties (reflection or generated metadata).
2. It skips `[JsonIgnore]` members, applies `[JsonPropertyName]`/naming policy.
3. Each value is written as JSON text (`DateTime` → ISO 8601 string, `decimal` → number).
4. Returns the final string. **Deserialize** reverses it: parse JSON → create `Product` (needs a usable constructor) → set each matched property.

### F. When to use
APIs (JSON), config/files, caching, messaging, persistence, deep copy (via serialize→deserialize; slower but simple).

### G. Common mistakes
- **Never deserialize untrusted data with polymorphic/binary serializers** (`BinaryFormatter`, Newtonsoft `TypeNameHandling.All`).
- `System.Text.Json` serializes **public properties**, **not fields** (unless `IncludeFields = true`); `[Serializable]`/`[NonSerialized]` are ignored. 🔧
- Name casing mismatch (`Name` vs `name`): set `PropertyNameCaseInsensitive = true` or a naming policy.
- **Circular references** → exception; use `ReferenceHandler.IgnoreCycles` / `Preserve` or redesign.
- Missing parameterless/parameterized-constructor support → exception on deserialize.
- Serializing **secrets** (passwords, tokens) accidentally.
- Interfaces/abstract types need polymorphism config (`[JsonDerivedType]`).
- Changing class shape breaks stored data → plan **versioning**.
- `DateTime` time zones: store **UTC** (`DateTimeOffset` helps).

### H. Interview questions
**Q: What is serialization and why is it needed?** Converting objects to a storable/transmittable form and back, since memory object graphs can't be stored or sent directly.
**Q: Why not `BinaryFormatter`?** Deserialization of attacker-controlled data can lead to remote code execution; obsolete and removed in .NET 9.
**Q: `System.Text.Json` vs Newtonsoft?** STJ: built-in, faster, stricter, AOT-friendly; Newtonsoft: more flexible/features, older.

### I. Quick revision
Serialize = object → text/bytes; Deserialize = back. Use `System.Text.Json`. Never `BinaryFormatter`. Watch cycles, casing, secrets, versioning.

🔁 *Compare with JS:* `JSON.stringify/parse` ↔ `JsonSerializer.Serialize/Deserialize`.

---

# PART 6: Multithreading and Parallel Programming

## 6.1 Process vs Thread

📝 *From my notes, expanded.*

### A. What is it?
- **Process** 📝: a running instance of a program with its **own memory space and resources**; the OS isolates processes from each other.
- **Thread** 📝: a unit of **execution inside a process**. Threads of the same process **share the same memory (heap)** and resources, but each has its **own call stack** and registers.

| | Process | Thread |
|---|---|---|
| Memory | Own, isolated | **Shared heap**, own stack (~1 MB default) |
| Creation cost | Heavy | Lighter (but still not free) |
| Communication | IPC (pipes, sockets, memory-mapped files) | Direct shared memory (needs synchronization) |
| Crash | Doesn't affect other processes | An unhandled exception can **kill the whole process** |

### B. Why do we use threads?
- **Responsiveness:** keep the UI alive while working in the background.
- **Throughput:** use multiple CPU cores.
- **Concurrency for I/O:** serve many requests at once.

### Three words people confuse ➕
| Term | Meaning | Example |
|---|---|---|
| **Concurrency** | Dealing with many things *at once* (interleaved) | One cook switching between dishes |
| **Parallelism** | Doing many things *at the same instant* on multiple cores | Many cooks |
| **Asynchrony** | Not blocking while *waiting* (I/O); may use **no thread** while waiting | Put water on to boil, then do other things |

### C. Internally
- The **OS scheduler** assigns threads to CPU cores in time slices (context switching has a cost).
- In .NET, `Thread` maps to an OS thread. The **ThreadPool** keeps reusable worker threads (avoiding the cost of creating threads) and grows/shrinks automatically.
- 🔁 *Contrast with JS:* the JS main thread is single; concurrency comes from the event loop. In C#, you have **real multi-threading**, so you also get **race conditions**.

### D. Example
```csharp
var t = new Thread(() =>
{
    Console.WriteLine($"Worker thread: {Environment.CurrentManagedThreadId}");
}) { IsBackground = true };         // background threads don't keep the process alive
t.Start();
t.Join();                           // wait for it

ThreadPool.QueueUserWorkItem(_ => Console.WriteLine("Pool thread"));   // prefer Task.Run
Console.WriteLine($"Main thread: {Environment.CurrentManagedThreadId}");
```

### G. Common mistakes
- Creating a raw `Thread` per small job (use thread pool/`Task`).
- Updating UI controls from a background thread (WinForms/WPF/MAUI need the UI thread: `Dispatcher.Invoke`/`Control.Invoke`, or `await` back on it).
- Unhandled exceptions on threads crash the app.
- Forgetting `IsBackground` → process won't exit.

### I. Quick revision
Process = isolated memory; thread = execution path sharing process memory (own stack). Concurrency ≠ parallelism ≠ async. Prefer ThreadPool/`Task`.

---

## 6.2 Thread Synchronization

📝 *From my notes (race conditions, locks, mutexes, semaphores, monitors), expanded.*

### A. What is it?
**Thread synchronization** 📝 = coordinating threads so they access **shared resources in a controlled way**.

### B. Why does it exist? (the race condition)
A **race condition** 📝 happens when multiple threads read/modify shared data at the same time and the result depends on **timing**. Even `counter++` is **not atomic**: it's *read → add → write*. 

### C. Internally
```text
counter = 0
Thread A: read 0 ──────────► add 1 ► write 1
Thread B:       read 0 ► add 1 ► write 1          ← lost update! expected 2, got 1
```
Synchronization primitives make a block of code **mutually exclusive** (only one thread inside) or make an operation **atomic**.

| Tool | What it does | Scope |
|---|---|---|
| `lock (obj) { }` / `Monitor` 📝 | Mutual exclusion; `lock` = `Monitor.Enter` + `try/finally Monitor.Exit` | In-process |
| `System.Threading.Lock` ➕ | Newer dedicated lock type (.NET 9+); `lock(myLock)` uses it efficiently | In-process |
| `Interlocked` ➕ | **Atomic** operations (`Increment`, `Add`, `CompareExchange`) with no lock | In-process |
| `Mutex` 📝 | Mutual exclusion that can be **named → across processes** (heavier) | Cross-process |
| `Semaphore` / `SemaphoreSlim` 📝 | Allow **up to N** threads at once; `SemaphoreSlim.WaitAsync()` works with `await` | In-process (`Semaphore` can be named) |
| `ReaderWriterLockSlim` ➕ | Many readers **or** one writer | In-process |
| `ManualResetEvent`, `CountdownEvent`, `Barrier` ➕ | Signaling/coordination | |
| `ConcurrentDictionary`, `ConcurrentQueue`… ➕ | Thread-safe collections | |

### D. Example
```csharp
// ❌ Race condition
int counter = 0;
Parallel.For(0, 100_000, _ => counter++);
Console.WriteLine(counter);                 // often LESS than 100000

// ✅ Fix 1: lock
int c1 = 0; var gate = new object();
Parallel.For(0, 100_000, _ => { lock (gate) { c1++; } });
Console.WriteLine(c1);                      // 100000

// ✅ Fix 2: Interlocked (fastest for simple counters)
int c2 = 0;
Parallel.For(0, 100_000, _ => Interlocked.Increment(ref c2));
Console.WriteLine(c2);                      // 100000

// ✅ Semaphore: at most 3 concurrent downloads (async-friendly)
var limiter = new SemaphoreSlim(3);
async Task DownloadAsync(string url)
{
    await limiter.WaitAsync();
    try { await Task.Delay(500); /* simulate download */ }
    finally { limiter.Release(); }              // ALWAYS in finally
}
await Task.WhenAll(Enumerable.Range(0, 10).Select(i => DownloadAsync($"u{i}")));

// ✅ Mutex across processes: allow only ONE instance of the app
using var mutex = new Mutex(true, "Global\\MyApp_SingleInstance", out bool isFirst);
if (!isFirst) { Console.WriteLine("Already running"); return; }

// ✅ Thread-safe collection
var dict = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
dict.AddOrUpdate("a", 1, (_, old) => old + 1);
```

### E. Step-by-step (`lock`)
1. Thread A reaches `lock(gate)`, acquires it, enters.
2. Thread B reaches `lock(gate)`, finds it taken → **blocks (waits)**.
3. A leaves the block (even via exception, since `finally` releases) → B acquires and enters.
4. Only one thread at a time touches `c1++`, so no lost updates.

### Deadlock ➕
Two threads each hold one lock and wait for the other's → both wait **forever**.
```csharp
// Thread 1: lock(A) { lock(B) { } }      Thread 2: lock(B) { lock(A) { } }   ← deadlock
```
**Prevent:** always acquire locks in the **same global order**, keep lock scope tiny, avoid calling unknown code (callbacks/events) while holding a lock, use timeouts (`Monitor.TryEnter`).

### F. When to use which
- Simple counter/flag → `Interlocked`.
- Protect a few related fields → `lock`.
- Limit concurrency → `SemaphoreSlim`.
- Shared data structure → `Concurrent*` collections or immutable data.
- Single-instance app / cross-process → named `Mutex`.
- **Best strategy: avoid shared mutable state** (immutability, message passing, `Channel<T>`). 🔁

### G. Common mistakes
- `lock(this)`, `lock(typeof(X))`, `lock("string")`: other code can lock the same object → surprise deadlocks. Use a **private readonly object**.
- **`await` inside `lock`** isn't allowed (compile error): use `SemaphoreSlim`.
- Locking too much (kills performance) or too little (still racy).
- Reading shared data without sync "because it's just a read" (torn/stale reads).
- Forgetting to `Release` a semaphore in `finally`.
- Using `Mutex` for in-process locking (slow).

### H. Interview questions
**Q: What is a race condition?** Result depends on thread timing when threads access shared mutable state without sync.
**Q: `lock` vs `Mutex` vs `Semaphore`?** `lock` = in-process exclusive; `Mutex` = exclusive, can be cross-process; `Semaphore` = allows N concurrent.
**Q: What is a deadlock and how do you avoid it?** Circular waiting on locks; consistent lock ordering, short critical sections, timeouts.
**Q: Is `counter++` atomic?** No; use `Interlocked.Increment`.

### I. Quick revision
Shared mutable state + threads = races. Use `lock`/`Interlocked`/`SemaphoreSlim`/`Concurrent*`. Lock on private objects. No `await` in `lock`. Prefer immutability.

---

## 6.3 Task and `async/await`

📝 *My note had only the `Task` definition. Greatly expanded.*

### A. What is it?
- **`Task` / `Task<T>`** 📝 (namespace `System.Threading.Tasks`): represents an **asynchronous operation** (a "promise" of completion/result). 🔧 It is **not a thread**.
- **`async/await`**: syntax to write asynchronous code that **reads like synchronous code**, without blocking threads while waiting.

> 🔁 *`Task<T>` ≈ JS `Promise<T>`. `await` ≈ JS `await`.*

### B. Why does it exist?
Blocking a thread while waiting for I/O (disk, DB, HTTP) wastes it. A server thread blocked 200 ms on a DB call can't serve others; a UI thread blocked = frozen window. `async` frees the thread during the wait.

### C. How it works internally ➕
- **Two different kinds of tasks:**
  1. **CPU-bound** (`Task.Run(() => HeavyMath())`): runs on a **thread-pool thread**.
  2. **I/O-bound** (`await httpClient.GetAsync(...)`): **no thread is used while waiting**; the OS signals completion (I/O completion port), then the continuation runs.
- The compiler rewrites an `async` method into a **state machine**. At each `await`: if the task is already complete → continue synchronously; otherwise → **save state, return to the caller**, and register a **continuation** that resumes the method later.
- **SynchronizationContext:** in UI apps (WinForms/WPF/MAUI), after `await` the continuation returns to the **UI thread** by default (so you can touch controls). ASP.NET Core has **no** sync context (continuation runs on any pool thread). `ConfigureAwait(false)` = "don't resume on the captured context" (use in **library** code).
- Exceptions in a task are **stored in the task** and **rethrown when you `await`** it.

### D. Example
```csharp
// I/O-bound
static async Task<string> GetPageAsync(string url, CancellationToken ct = default)
{
    using var http = new HttpClient();
    HttpResponseMessage res = await http.GetAsync(url, ct);   // thread is free while waiting
    res.EnsureSuccessStatusCode();                             // 🔁 like checking response.ok in JS fetch!
    return await res.Content.ReadAsStringAsync(ct);
}

// CPU-bound
static Task<long> SumAsync(int n) => Task.Run(() =>
{
    long s = 0; for (int i = 1; i <= n; i++) s += i; return s;
});

// Orchestration
var t1 = GetPageAsync("https://example.com");
var t2 = SumAsync(100_000_000);
await Task.WhenAll(t1, t2);                // run together, wait for both (like Promise.all)
Console.WriteLine($"{(await t1).Length} chars, sum={await t2}");

var first = await Task.WhenAny(t1, Task.Delay(2000));   // timeout pattern

// Cancellation
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try { await GetPageAsync("https://slow.example", cts.Token); }
catch (OperationCanceledException) { Console.WriteLine("Cancelled/timeout"); }

// Exceptions
try { await Task.Run(() => throw new InvalidOperationException("boom")); }
catch (InvalidOperationException ex) { Console.WriteLine(ex.Message); }   // unwrapped by await
```

### E. Step-by-step (`await http.GetAsync(url)` in a UI app)
1. UI thread runs the method until `GetAsync`, which starts the request and returns an **incomplete Task**.
2. `await` sees "not done" → saves state, **returns control to the UI** (window stays responsive).
3. Network response arrives → task completes → continuation is queued to the **UI thread** (captured context).
4. Method resumes after the `await` with the result.

### F. When to use
- **I/O** (HTTP, DB, files) → `async/await` with truly async APIs.
- **CPU-heavy** work in UI apps → `Task.Run`. In **ASP.NET Core**, don't wrap I/O in `Task.Run` (pointless).
- Many things at once → `Task.WhenAll`; timeouts → `WhenAny`/`CancellationToken`.

### G. Common mistakes
```csharp
// 1) Sync-over-async DEADLOCK in UI/legacy ASP.NET: blocks the thread the continuation needs
var data = GetPageAsync(url).Result;     // ❌ also .Wait(), .GetAwaiter().GetResult()
var data2 = await GetPageAsync(url);     // ✅ async all the way

// 2) async void: exceptions can't be caught and may crash the app
async void Bad() { throw new Exception(); }      // ❌ (only for event handlers)
async Task Good() { throw new Exception(); }     // ✅ return Task

// 3) Fire and forget: exception lost / unobserved
_ = DoWorkAsync();                       // ❌ unless handled intentionally

// 4) Sequential when you wanted parallel
var a = await FetchA(); var b = await FetchB();           // ❌ ~ sum of times
var ta = FetchA(); var tb = FetchB(); await Task.WhenAll(ta, tb);   // ✅ ~ max of times

// 5) Task.Run for I/O in server code; 6) forgetting to pass CancellationToken
// 7) Using `lock` around await (not allowed) → SemaphoreSlim
// 8) Thinking `async` = new thread (it isn't)
// 9) await in loops with `ForEach`: use `foreach` + await, or Parallel.ForEachAsync
```
- `AggregateException` appears when you use `.Wait()`/`.Result` (it wraps exceptions); `await` unwraps the first one.
- Prefer `ValueTask` only for hot paths that often complete synchronously.

### H. Interview questions
**Q: Is a Task a thread?** No: it's an abstraction for an operation. CPU-bound tasks use thread-pool threads; I/O-bound await uses none while waiting.
**Q: What does `await` do?** If the task isn't finished, it suspends the method (without blocking the thread) and resumes via continuation when it completes.
**Q: Why does `.Result` deadlock?** It blocks the context thread, while the continuation needs that same thread to run.
**Q: `Task.Run` vs `async`?** `Task.Run` offloads CPU work to the pool; `async` is for non-blocking waiting.
**Q: `Task` vs `Thread`?** Thread = OS-level execution unit; Task = higher-level, composable, uses pool, returns results/exceptions.

### I. Quick revision
`Task` ≈ Promise. `await` frees the thread. I/O → async; CPU → `Task.Run`. Async all the way (no `.Result`). No `async void`. `WhenAll` for parallel. Pass `CancellationToken`.

---

## 6.4 Parallel Programming

➕ *Mentioned in my heading ("parallel programming") but no notes existed.*

### A. What is it?
Splitting **CPU-bound** work across **multiple cores** to finish faster.

### C. Tools
| Tool | Use |
|---|---|
| `Parallel.For` / `Parallel.ForEach` | Data parallelism over loops (blocking call) |
| `Parallel.ForEachAsync` | Async work with a degree-of-parallelism limit |
| **PLINQ**: `.AsParallel()` | Parallel LINQ queries |
| `Task.Run` + `Task.WhenAll` | Manual task parallelism |
| `Channel<T>`, `Dataflow` | Producer/consumer pipelines |

### D. Example
```csharp
var numbers = Enumerable.Range(1, 1_000_000).ToArray();

// Parallel loop with thread-local accumulation (no shared state!)
long total = 0;
Parallel.For(0, numbers.Length,
    () => 0L,                                              // thread-local init
    (i, _, local) => local + numbers[i],                   // body
    local => Interlocked.Add(ref total, local));           // merge once per thread

// PLINQ
int evens = numbers.AsParallel().Count(n => n % 2 == 0);

// Async parallel with limit
await Parallel.ForEachAsync(urls, new ParallelOptions { MaxDegreeOfParallelism = 4 },
    async (url, ct) => await DownloadAsync(url));
```

### G. Common mistakes
- Parallelizing **tiny** work (overhead > gain) or **I/O-bound** work with `Parallel.For` (use async).
- **Shared mutable state** without sync (see 6.2).
- Assuming order is preserved (`AsParallel().AsOrdered()` if needed).
- Not measuring: always benchmark (`BenchmarkDotNet`, `Stopwatch`).
- Amdahl's law: speed-up is limited by the sequential part.

### I. Quick revision
CPU-bound → `Parallel.For`/PLINQ; I/O-bound → `async`. Avoid shared state; measure.

---

# PART 7: Configuration, Windows and Logging

## 7.1 App.Config and Modern Configuration

📝 *From my notes, corrected (🔧 secrets, modern .NET).*

### A. What is it?
`App.config` 📝 is an **XML file** used by **.NET Framework** applications to store settings, connection strings and other configuration, **separate from code**. At build time it's copied next to the program as **`YourApp.exe.config`**. (Web apps use `Web.config`.)

### B. Why does it exist?
Change behavior (DB server, feature flags, paths, log level) **without recompiling**. 📝

### C. Internally
`System.Configuration.ConfigurationManager` reads the `.exe.config` at runtime, **caches** the sections, and gives typed access. Sections: `<appSettings>` (key/value), `<connectionStrings>`, custom sections.

### D. Example
```xml
<!-- App.config -->
<configuration>
  <appSettings>
    <add key="PageSize" value="25" />
    <add key="FeatureX" value="true" />
  </appSettings>
  <connectionStrings>
    <add name="Main" connectionString="Server=.;Database=Shop;Trusted_Connection=True;" providerName="System.Data.SqlClient" />
  </connectionStrings>
</configuration>
```
```csharp
using System.Configuration;      // NuGet: System.Configuration.ConfigurationManager (for .NET 5+)

int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"] ?? "10");
bool featureX = bool.TryParse(ConfigurationManager.AppSettings["FeatureX"], out var f) && f;
string cs = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

// Writing (rare; edits the .exe.config; needs write permission)
var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
cfg.AppSettings.Settings["PageSize"].Value = "50";
cfg.Save(ConfigurationSaveMode.Modified);
ConfigurationManager.RefreshSection("appSettings");
```

### 🔧 Modern .NET approach (Core / 5+)
Use **`appsettings.json`** + **`Microsoft.Extensions.Configuration`**, layered sources (later overrides earlier):
```text
appsettings.json → appsettings.{Environment}.json → User Secrets (dev) → Environment variables → Command-line args
```
```json
{ "Shop": { "PageSize": 25, "Features": { "X": true } }, "ConnectionStrings": { "Main": "Server=..." } }
```
```csharp
// Strongly typed options (recommended)
public class ShopOptions { public int PageSize { get; set; } public Features Features { get; set; } = new(); }
public class Features { public bool X { get; set; } }

builder.Services.Configure<ShopOptions>(builder.Configuration.GetSection("Shop"));
// inject: IOptions<ShopOptions> (static), IOptionsSnapshot<> (per request), IOptionsMonitor<> (live reload)
```
Environment selection: `ASPNETCORE_ENVIRONMENT` / `DOTNET_ENVIRONMENT` = Development/Staging/Production. 📝 *(environment-specific configs)*

### F. When to use
Settings that vary per environment: connection strings, URLs, timeouts, feature flags, log levels.

### G. Common mistakes
- 🔧 **Storing secrets** (API keys, passwords) in committed config files. Use **User Secrets** (dev), **environment variables**, **Key Vault/Secrets Manager** (prod).
- Missing key → `null` → `NullReferenceException`/`FormatException`; always provide defaults/validation (`ValidateOnStart`).
- Forgetting the file is only **read once** (cached) unless reloaded.
- Editing `App.config` in `bin/` instead of the project file (overwritten on rebuild).
- Settings that need admin rights to write (Program Files); store *user* settings in `%AppData%`.

### I. Quick revision
`App.config` (Framework) → `ConfigurationManager`. Modern: `appsettings.json` + `IConfiguration` + `IOptions<T>`. **Never commit secrets.**

---

## 7.2 Windows Registry

📝 *From my notes, expanded. 🔧 Windows-only.*

### A. What is it?
A **hierarchical database** of configuration for Windows and installed apps. 📝 Structure: **Hives → Keys (folders) → Values (name + data + type)**.

| Hive | Meaning |
|---|---|
| `HKEY_CURRENT_USER` (HKCU) | Settings for the **current user** (no admin needed to write) |
| `HKEY_LOCAL_MACHINE` (HKLM) | **Machine-wide** settings (**admin** needed to write) |
| `HKEY_CLASSES_ROOT` (HKCR) | File associations/COM |
| `HKEY_USERS`, `HKEY_CURRENT_CONFIG` | Other users / hardware profile |

Value types (`RegistryValueKind`): `String`, `ExpandString`, `MultiString`, `DWord` (int), `QWord` (long), `Binary`.

### B. Why does it exist?
Central store for OS/app settings, file associations, startup programs (`...\Run`), licensing info.

### C. Internally
`Microsoft.Win32.Registry` / `RegistryKey` wrap the Win32 registry APIs. A `RegistryKey` holds an OS **handle**, so it's `IDisposable`. A **32-bit process on 64-bit Windows** is redirected to `WOW6432Node` unless you request `RegistryView.Registry64`.

### D. Example
```csharp
using Microsoft.Win32;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
static void RegistryDemo()
{
    // Create / write (HKCU: no admin)
    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\MyCompany\MyApp"))
    {
        key.SetValue("Theme", "dark", RegistryValueKind.String);
        key.SetValue("Launches", 5, RegistryValueKind.DWord);
    }

    // Read (with default)
    using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\MyCompany\MyApp"))
    {
        string theme = key?.GetValue("Theme", "light") as string ?? "light";
        int launches = (int)(key?.GetValue("Launches", 0) ?? 0);
        Console.WriteLine($"{theme}, {launches}");
        foreach (string name in key?.GetValueNames() ?? Array.Empty<string>()) Console.WriteLine(name);
    }

    // Update/delete (writable: true)
    using (var key = Registry.CurrentUser.OpenSubKey(@"Software\MyCompany\MyApp", writable: true))
        key?.DeleteValue("Launches", throwOnMissingValue: false);

    Registry.CurrentUser.DeleteSubKeyTree(@"Software\MyCompany", throwOnMissingSubKey: false);

    // 64-bit view explicitly
    using var hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
}
```

### F. When to use
Windows-specific integration: file associations, run-at-startup, reading system info, legacy settings. For normal app settings prefer config files / `%AppData%` JSON (portable, safer).

### G. Common mistakes
- 🔧 Using it in cross-platform code (throws `PlatformNotSupportedException` off Windows) → guard with `OperatingSystem.IsWindows()`.
- Writing to **HKLM** without admin → `UnauthorizedAccessException`/`SecurityException`.
- Not disposing keys; not handling `null` from `OpenSubKey`/`GetValue`.
- Casting `GetValue` result blindly (`DWord` → `int`, `QWord` → `long`).
- Editing system keys without a **backup** (can destabilize Windows). 📝
- Forgetting the **32/64-bit view**.
- Storing secrets in plain text.

### I. Quick revision
Registry = Windows hierarchical settings DB (Hives → Keys → Values). Use `Microsoft.Win32`, dispose keys, prefer **HKCU**, careful with HKLM/WOW64, Windows-only.

---

## 7.3 Event Log

📝 *From my notes, expanded. 🔧 Windows-only.*

### A. What is it?
The **Windows Event Log** is a **centralized system log** where the OS and apps record **events (Information, Warning, Error, Audit…)**, viewed in **Event Viewer** (`eventvwr.msc`). 📝

### B. Why does it exist?
One standard place for admins/monitoring tools (SIEM) to diagnose problems across the OS and apps, which is especially useful for **Windows Services** that have no console.

### C. Internally
Logs are named channels (`Application`, `System`, `Security`, or custom). Each app writes through an **event source** name registered to a log. 🔧 **Creating a source requires administrator rights** (usually done at install time), after which writing is allowed. `System.Diagnostics.EventLog` (NuGet package `System.Diagnostics.EventLog` on .NET Core+).

### D. Example
```csharp
using System.Diagnostics;
using System.Runtime.Versioning;

[SupportedOSPlatform("windows")]
static void EventLogDemo()
{
    const string source = "MyApp", log = "Application";

    if (!EventLog.SourceExists(source))                // needs admin to create
        EventLog.CreateEventSource(source, log);

    using var el = new EventLog(log) { Source = source };
    el.WriteEntry("App started",              EventLogEntryType.Information, 1000);
    el.WriteEntry("Disk almost full",         EventLogEntryType.Warning,     2000);
    el.WriteEntry("Unhandled error: ...",     EventLogEntryType.Error,       3000);

    // Read & filter (iterates the log; can be slow on big logs)
    foreach (EventLogEntry e in el.Entries.Cast<EventLogEntry>()
                 .Where(x => x.Source == source && x.EntryType == EventLogEntryType.Error)
                 .TakeLast(10))
        Console.WriteLine($"{e.TimeGenerated:u} [{e.EntryType}] {e.Message}");
}
```

### F. When to use
Windows Services, enterprise Windows apps, auditing/monitoring. 🔧 For modern apps use **`ILogger`** with providers (Serilog/NLog/OpenTelemetry/Application Insights; and the `EventLog` provider if you want Windows Event Log), so logging works everywhere.

### G. Common mistakes
- Writing without a registered source (`SecurityException`).
- Logging **too much** or **sensitive data** (the log is readable by admins).
- Reading `Entries` on huge logs without filtering (slow); use `EventLogQuery`/`EventLogReader` for efficient queries.
- Cross-platform code calling it unguarded.
- Swallowing the logging exception inside your own error handler (the logger should never crash the app).

### I. Quick revision
Event Log = central Windows log (Event Viewer). `EventLog.WriteEntry` with a **source** (admin to create). Windows-only; prefer `ILogger` abstractions for new code.

---

# PART 8: Cryptography

## 8.1 Cryptography

📝 *From my notes, corrected (🔧 weak algorithms, real-world usage) and expanded.*

### A. What is it?
Practice of securing information so only authorized parties can read/trust it. 📝 It provides:
- **Confidentiality**: nobody else can read it (**encryption**).
- **Integrity**: it wasn't changed (**hashes, MACs**).
- **Authenticity**: it really came from who claims (**signatures, MACs**).
- (Non-repudiation: signer can't deny.)

Namespace: `System.Security.Cryptography`. 📝

### B. The 4 families (don't confuse them!)

| Family | Key? | Reversible? | Purpose | Good choices | Avoid |
|---|---|---|---|---|---|
| **Hash** 📝 | none | ❌ one-way | Integrity, fingerprint | **SHA-256/384/512**, SHA-3 | 🔧 **MD5, SHA-1** (collisions broken) |
| **MAC / HMAC** ➕ | secret key | ❌ | Integrity + authenticity | **HMACSHA256** | – |
| **Symmetric** 📝 | **one** shared key | ✅ | Encrypt lots of data | **AES-GCM** (authenticated), ChaCha20-Poly1305 | 🔧 **DES, 3DES**, AES-ECB, unauthenticated CBC |
| **Asymmetric** 📝 | **public + private** pair | ✅ | Key exchange, signatures, small data | **RSA (2048+ with OAEP/PSS)**, **ECDSA/ECDH** | RSA with PKCS#1 v1.5 encryption, <2048 bits |

📝 **Symmetric vs asymmetric:**
| | Symmetric | Asymmetric |
|---|---|---|
| Keys | same key encrypts+decrypts | public encrypts/verifies, private decrypts/signs |
| Speed | **very fast** → large data | **slow** → small data (keys, signatures) |
| Problem | How do we **share the key** safely? | Solves key distribution; needs trust in public keys (certificates) |

> 🔧 **How real systems combine them (e.g., HTTPS/TLS):** use **asymmetric** crypto to authenticate and agree on a key, then use **symmetric** (AES-GCM) to encrypt the actual traffic.

### C. Internally (ideas)
- **Hash:** fixed-size output ("digest"); tiny input change → totally different output; can't be reversed; collisions should be infeasible.
- **AES-GCM:** encrypts **and authenticates**; needs a **unique nonce (IV)** per message (never reuse with the same key!); outputs ciphertext + **tag**.
- **RSA signature:** sign the **hash** of the data with the **private** key; anyone verifies with the **public** key.

### D. Example
```csharp
using System.Security.Cryptography;
using System.Text;

// 1) Hash (integrity). Output is bytes → hex
byte[] data = Encoding.UTF8.GetBytes("hello");
string sha = Convert.ToHexString(SHA256.HashData(data));
Console.WriteLine(sha);   // 2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824

// 2) HMAC (hash + secret key → integrity + authenticity)
byte[] key = RandomNumberGenerator.GetBytes(32);
byte[] mac = HMACSHA256.HashData(key, data);

// 3) Password hashing: SLOW + salted (NOT plain SHA-256!)
byte[] salt = RandomNumberGenerator.GetBytes(16);
byte[] pwHash = Rfc2898DeriveBytes.Pbkdf2("P@ssw0rd", salt, iterations: 600_000, HashAlgorithmName.SHA256, outputLength: 32);
// store: salt + iterations + pwHash. Verify in constant time:
bool ok = CryptographicOperations.FixedTimeEquals(pwHash,
    Rfc2898DeriveBytes.Pbkdf2("P@ssw0rd", salt, 600_000, HashAlgorithmName.SHA256, 32));
// (ASP.NET Core Identity's PasswordHasher or Argon2/bcrypt libraries are even better choices)

// 4) Symmetric authenticated encryption: AES-GCM
static (byte[] nonce, byte[] cipher, byte[] tag) Encrypt(byte[] aesKey, byte[] plain)
{
    byte[] nonce  = RandomNumberGenerator.GetBytes(12);        // unique per message!
    byte[] cipher = new byte[plain.Length];
    byte[] tag    = new byte[16];
    using var aes = new AesGcm(aesKey, tagSizeInBytes: 16);
    aes.Encrypt(nonce, plain, cipher, tag);
    return (nonce, cipher, tag);
}
static byte[] Decrypt(byte[] aesKey, byte[] nonce, byte[] cipher, byte[] tag)
{
    byte[] plain = new byte[cipher.Length];
    using var aes = new AesGcm(aesKey, 16);
    aes.Decrypt(nonce, cipher, tag, plain);                    // throws AuthenticationTagMismatchException if tampered
    return plain;
}
byte[] aesKey = RandomNumberGenerator.GetBytes(32);            // 256-bit
var (n, c, t) = Encrypt(aesKey, Encoding.UTF8.GetBytes("secret message"));
Console.WriteLine(Encoding.UTF8.GetString(Decrypt(aesKey, n, c, t)));  // secret message

// 5) Asymmetric: RSA signature + OAEP encryption
using RSA rsa = RSA.Create(2048);
byte[] sig = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
bool valid = rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);   // true
byte[] enc = rsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);   // encrypt a small thing (a key!)
byte[] dec = rsa.Decrypt(enc, RSAEncryptionPadding.OaepSHA256);
```

### E. Step-by-step (verifying a file download)
1. Publisher computes `SHA256(file)` and publishes the digest.
2. You download the file and compute `SHA256(yourCopy)`.
3. Same digest → file is intact. Different → corrupted/tampered. *(A plain hash proves integrity only if the digest itself came from a trusted source; for authenticity use a signature or HMAC.)*

### F. When to use what
- File/data integrity → SHA-256. Tamper-proof messages between parties sharing a secret → HMAC.
- Store passwords → **PBKDF2/Argon2/bcrypt + unique salt** (never plain hash, never reversible encryption).
- Encrypt data at rest/in transit → AES-GCM (key from a secure store).
- Sign tokens/documents/updates, exchange keys → RSA/ECDSA/ECDH (usually via TLS/certificates).
- Random keys/tokens → `RandomNumberGenerator` (**not** `System.Random`).

### G. Common mistakes
- 🔧 MD5/SHA-1/DES for security; ECB mode; reusing nonce/IV; unauthenticated encryption.
- Calling hashing "encryption" (hashes can't be decrypted).
- **Rolling your own crypto** (algorithms or protocols).
- Hard-coding keys in source/config; weak key derivation from passwords.
- Plain `SHA256(password)` (too fast, brute-forceable; no salt → rainbow tables).
- `==` for comparing MACs/hashes (timing attack) → `CryptographicOperations.FixedTimeEquals`.
- Using `System.Random` for tokens.
- Encrypting large data directly with RSA (slow, size-limited): use hybrid encryption.
- Not disposing `Aes`/`RSA` objects.

### H. Interview questions
**Q: Hashing vs encryption?** Hash is one-way (integrity); encryption is reversible with a key (confidentiality).
**Q: Symmetric vs asymmetric?** Same key vs key pair; symmetric fast for bulk data, asymmetric for key exchange/signatures.
**Q: How do you store passwords?** Salted, slow KDF (PBKDF2/Argon2/bcrypt). Never plain/fast hashes or reversible encryption.
**Q: Why is MD5 unsafe?** Practical collision attacks exist.
**Q: What is a salt? A nonce?** Salt: random per-password value defeating precomputed tables. Nonce/IV: unique per encryption operation.

### I. Quick revision
Hash = one-way integrity (SHA-256). HMAC = keyed integrity. AES-GCM = symmetric encryption. RSA/ECC = signatures + key exchange. Passwords → PBKDF2/Argon2. Never MD5/SHA-1/DES; never roll your own; never reuse nonces.

---

# PART 9: Important Missing Topics (➕ all added)

## 9.1 LINQ

### A–B. What & why
**LINQ (Language Integrated Query)** = a uniform way to query/transform any collection (`IEnumerable<T>`), databases (EF Core → SQL), XML, etc. It is built from **delegates + generics + extension methods** (so it connects directly to Parts 2–3). 🔁 *Like JS `map/filter/reduce`, but richer.*

### C. Internally
- **Deferred execution:** `Where/Select/OrderBy…` just build a pipeline; **nothing runs until you enumerate** (`foreach`, `ToList()`, `Count()`, `First()`).
- On `IEnumerable<T>` the lambdas are **delegates** executed in memory; on `IQueryable<T>` (EF Core) they're **expression trees** translated to SQL.

### D. Example
```csharp
var people = new[] {
    new { Name = "Ali", Age = 30, City = "Rabat" },
    new { Name = "Sara", Age = 22, City = "Casablanca" },
    new { Name = "Omar", Age = 41, City = "Rabat" },
};

var result = people
    .Where(p => p.Age > 25)                // filter
    .OrderByDescending(p => p.Age)         // sort
    .Select(p => new { p.Name, p.City })   // project
    .ToList();                             // EXECUTES here

var byCity = people.GroupBy(p => p.City)
                   .Select(g => new { City = g.Key, Avg = g.Average(x => x.Age) });

bool any = people.Any(p => p.Age > 40);                // true
var first = people.FirstOrDefault(p => p.Name == "X"); // null (no exception)
int total = people.Sum(p => p.Age);

// Query syntax (equivalent)
var q = from p in people where p.Age > 25 orderby p.Name select p.Name;
```

### G. Common mistakes
- **Multiple enumeration** of a deferred query (runs the pipeline each time; may hit the DB twice) → `ToList()` once.
- Forgetting it's lazy: modify the source after defining the query and results change.
- `First()` throws on empty → prefer `FirstOrDefault`; `Single()` throws if ≠1.
- Calling `.ToList()` too early on EF queries (filters then run in memory).
- Heavy LINQ in hot loops (allocations).
- Using `Count() > 0` instead of `Any()`.

### I. Quick revision
LINQ = lazy pipelines of `Where/Select/OrderBy/GroupBy…`. Executes on enumeration. `IEnumerable` = delegates; `IQueryable` = expression trees → SQL.

---

## 9.2 Exception Handling

### A–B. What & why
Exceptions signal errors that can't be handled locally; the runtime **unwinds the call stack** until it finds a matching `catch`.

### D. Example
```csharp
try
{
    int n = int.Parse("abc");
}
catch (FormatException ex) when (ex.Message.Contains("abc"))   // exception filter
{
    Console.WriteLine("Bad number");
}
catch (Exception ex)
{
    Log(ex);
    throw;                       // ✅ rethrow, preserves stack trace  (❌ `throw ex;` resets it)
}
finally
{
    Cleanup();                   // always runs
}

public class InsufficientFundsException : Exception
{
    public decimal Missing { get; }
    public InsufficientFundsException(decimal missing)
        : base($"Missing {missing}") => Missing = missing;
}
```

### G. Common mistakes
- `catch (Exception) { }` (swallowing); `throw ex;` (loses stack); using exceptions for **normal control flow** (slow): prefer `TryParse`, `TryGetValue`.
- Catching too broadly too low in the stack; not using `using`/`finally` for cleanup.
- Throwing `Exception`/`ApplicationException` directly instead of specific types (`ArgumentNullException`, `InvalidOperationException`).
- Async: exceptions surface at `await`; `async void` loses them (see 6.3).

### I. Quick revision
`try/catch/when/finally`, `throw;` not `throw ex;`, specific exception types, `TryXxx` for expected failures.

---

## 9.3 Collections Cheat Sheet

| Type | Ordered? | Lookup | Notes |
|---|---|---|---|
| `T[]` | indexed | O(1) by index | Fixed size |
| `List<T>` | indexed | O(1) index, O(n) search | Dynamic array (the default choice) |
| `Dictionary<K,V>` | no guarantee | **O(1)** by key | Hash table; key needs good `GetHashCode/Equals` |
| `HashSet<T>` | no | O(1) contains | Unique items |
| `Queue<T>` / `Stack<T>` | FIFO / LIFO | – | |
| `LinkedList<T>` | yes | O(n) | Fast insert/remove at known node (rarely best) |
| `SortedDictionary/SortedSet` | by key | O(log n) | Sorted |
| `ConcurrentDictionary`, `ConcurrentQueue`, `Channel<T>` | – | – | Thread-safe |
| `ImmutableList/Array/Dictionary` | – | – | Immutable (see 2.1) |

**Rule:** use generic collections (never `ArrayList`/`Hashtable`), expose `IEnumerable<T>`/`IReadOnlyList<T>` from APIs, and modifying a collection **while iterating** it throws `InvalidOperationException`.

---

# PART 10: Review Tools

## 10.1 Master Interview Q&A

**1. What is a delegate? How is it different from an event?**
Type-safe method reference object. An event is a delegate with restricted access: outsiders can only `+=`/`-=`; only the owner raises it.

**2. Multicast delegate behavior?**
Calls all methods in order; return value is the last one's; an exception stops the chain.

**3. `Func`, `Action`, `Predicate`?**
Return value / void / bool.

**4. Why generics?**
Type safety, reuse, and no boxing; reified in the CLR (unlike Java erasure).

**5. Mutable vs immutable? Why is `string` immutable?**
Immutable can't change after creation (thread-safe, safe keys, predictable). Strings: thread safety, hashing, interning, security.

**6. When use `StringBuilder`?**
Many appends/modifications, especially in loops. Not for a few concatenations.

**7. What is boxing?**
Wrapping a value type into a heap object (allocation + copy); avoid with generics.

**8. Struct vs class?**
Value vs reference semantics, copying, allocation, inheritance rules.

**9. What is reflection and its costs?**
Runtime inspection/manipulation of metadata; slower, loses compile-time safety, trimming/AOT issues.

**10. What are attributes and how are they read?**
Metadata stored in the assembly, read via `GetCustomAttribute` (lazily instantiated).

**11. How do you create a custom attribute?**
Inherit `Attribute`, add `[AttributeUsage]`, apply with `[...]`, read with reflection.

**12. `[Conditional]` vs `#if`?**
Call-site removal of void methods vs source-code removal.

**13. Serialization: what and why?**
Object ↔ storable/transmittable format. Use `System.Text.Json`. Never `BinaryFormatter` (RCE risk; removed in .NET 9).

**14. Process vs thread?**
Isolated memory vs shared memory within a process (own stack).

**15. What is a race condition? How do you fix it?**
Timing-dependent outcome from unsynchronized shared state: `lock`, `Interlocked`, concurrent collections, immutability.

**16. `lock` vs `Mutex` vs `Semaphore`?**
Exclusive in-process / exclusive (can be cross-process) / up to N concurrent.

**17. What is a deadlock?**
Circular wait on locks; prevent with consistent ordering, short locks, timeouts.

**18. Is `Task` a thread? What does `await` do?**
No. `await` suspends the method without blocking the thread and resumes via continuation.

**19. Why does `.Result` deadlock?**
Blocks the context thread that the continuation needs.

**20. `Task.Run` vs `async/await`?**
CPU offload vs non-blocking waiting.

**21. Concurrency vs parallelism vs asynchrony?**
Interleaving / simultaneous execution on cores / not blocking while waiting.

**22. Hash vs encrypt? Symmetric vs asymmetric?**
One-way vs reversible; same key vs key pair; symmetric for bulk data, asymmetric for key exchange/signatures.

**23. How to store passwords?**
Unique salt + slow KDF (PBKDF2/Argon2/bcrypt).

**24. App.config vs appsettings.json?**
Framework XML config via `ConfigurationManager` vs modern layered JSON with `IConfiguration`/`IOptions`.

**25. Why shouldn't you put secrets in config files?**
They end up in source control/builds; use env vars, user secrets, vaults.

**26. `IDisposable` and `using`?**
Deterministic release of unmanaged resources; `using` = try/finally `Dispose()`.

**27. `throw;` vs `throw ex;`?**
`throw;` preserves the original stack trace.

**28. Deferred execution in LINQ?**
Query runs when enumerated, not when defined.

---

## 10.2 Output Prediction Practice

```csharp
// Q1
string a = "x"; string b = a; a += "y";
Console.WriteLine(b);
// → x   (string immutable; a points to a new string)

// Q2
struct S { public int V; }  class C { public int V; }
var s1 = new S { V = 1 }; var s2 = s1; s2.V = 2;
var c1 = new C { V = 1 }; var c2 = c1; c2.V = 2;
Console.WriteLine($"{s1.V} {c1.V}");
// → 1 2

// Q3
int i = 5; object o = i; i = 6;
Console.WriteLine(o);
// → 5   (boxed copy)

// Q4
Action act = () => Console.Write("A");
act += () => Console.Write("B");
act += () => Console.Write("C");
act();
// → ABC

// Q5
var list = new List<Action>();
for (int k = 0; k < 3; k++) list.Add(() => Console.Write(k));
list.ForEach(f => f());
// → 333   (closure captures the variable k)

// Q6
Func<int> f1 = () => 1; f1 += () => 2; f1 += () => 3;
Console.WriteLine(f1());
// → 3   (last return value wins)

// Q7
var t = (X: 1, Y: 2); t.X = 10;
Console.WriteLine(t.X);
// → 10  (ValueTuple is mutable)

// Q8
int counter = 0;
Parallel.For(0, 1000, _ => counter++);
Console.WriteLine(counter == 1000);
// → usually False (race condition)

// Q9
Console.WriteLine("1");
var task = Task.Run(() => Console.WriteLine("2"));
Console.WriteLine("3");
await task;
Console.WriteLine("4");
// → 1 3 2 4  (2 may appear before 3 depending on scheduling, but 4 is always last)

// Q10
static async Task Foo() { Console.WriteLine("A"); await Task.Delay(100); Console.WriteLine("B"); }
var t2 = Foo(); Console.WriteLine("C"); await t2;
// → A C B   (Foo runs synchronously until the first await, then returns to the caller)
```

---

## 10.3 Study Roadmap

| # | Topic | Priority | Practice project |
|---|---|---|---|
| 1 | Value vs reference, boxing, GC, `IDisposable` | ⭐⭐⭐ | Benchmark `ArrayList` vs `List<int>` |
| 2 | Generics + constraints | ⭐⭐⭐ | Generic `Repository<T>` |
| 3 | Delegates, lambdas, events | ⭐⭐⭐⭐ | Pub/sub notification system |
| 4 | LINQ | ⭐⭐⭐⭐ | Query a JSON dataset |
| 5 | Attributes + Reflection | ⭐⭐⭐ | Validation framework (4.2) |
| 6 | Serialization (`System.Text.Json`) | ⭐⭐⭐ | Save/load app state |
| 7 | `async/await`, `Task` | ⭐⭐⭐⭐ | Concurrent downloader with cancellation |
| 8 | Threading + sync | ⭐⭐⭐ | Race condition demo → fix three ways |
| 9 | Configuration + logging | ⭐⭐ | Console app with `appsettings.json` + `ILogger` |
| 10 | Cryptography basics | ⭐⭐ | Password hasher + AES-GCM file encryptor |
| 11 | Windows Registry / Event Log | ⭐ | Small Windows service logging to Event Log |

---

## 10.4 One-Page Cheat Sheet

```text
RUNTIME        C# → IL + metadata (assembly) → CLR JIT → machine code | GC frees memory only
TYPES          struct = copy · class = shared reference · boxing = value → heap object (avoid)
IMMUTABLE      string, System.Tuple, ImmutableX, init-only records (shallow) | ValueTuple = MUTABLE
GENERICS       reified, no boxing, constraints `where T : class, new()` | List<T> not ArrayList
STRINGBUILDER  many appends in loops | `+` fine for a few | not thread-safe
OPERATORS      public static ... operator +(A a, A b) | pairs (==/!=) | no &&/|| directly | override Equals+GetHashCode
DELEGATE       type-safe method ref | Func/Action/Predicate | +=/-= multicast | last return wins | closures capture variables
EVENT          delegate + restricted access | EventHandler<T> | always unsubscribe
ATTRIBUTE      metadata only | read with GetCustomAttribute | [Conditional] [Obsolete] [DataContract] [JsonIgnore]
CUSTOM ATTR    : Attribute + [AttributeUsage] | constants only | cache reflection
REFLECTION     Type/MethodInfo/PropertyInfo/... | slow, runtime errors, breaks AOT/trimming
SERIALIZATION  System.Text.Json ✅ | Newtonsoft | XmlSerializer | BinaryFormatter ❌ (removed .NET 9)
THREADS        process = isolated memory · thread = shared heap, own stack | concurrency ≠ parallelism ≠ async
SYNC           race → lock / Interlocked / SemaphoreSlim / Concurrent* | private lock object | no await in lock
TASK           ≠ thread | I/O: no thread while waiting | CPU: Task.Run | no .Result | no async void | WhenAll
CONFIG         App.config (Framework) → appsettings.json + IOptions (modern) | no secrets in files
REGISTRY/LOG   Windows-only | HKCU no admin, HKLM admin | EventLog needs source (admin) | prefer ILogger
CRYPTO         SHA-256 (hash) · HMAC · AES-GCM · RSA/ECC | passwords: PBKDF2/Argon2 | never MD5/SHA-1/DES
LINQ           lazy until enumerated | beware multiple enumeration | IQueryable → SQL
EXCEPTIONS     throw; (not throw ex;) | specific types | TryXxx for expected failures
```

---

## ✅ How to keep using this file

1. Read **one topic**, close the file, explain it aloud (A → I).
2. Redo the **output predictions** (10.2) weekly; write tiny programs to verify.
3. Add new notes with the same A–I template and the 📝 / ➕ / 🔧 / 🔁 markers.
4. Re-read the **corrections table** often: those are the ideas easiest to remember wrongly.

*Happy coding! 🚀*
