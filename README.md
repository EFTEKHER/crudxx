# Project walkthrough: how the test, service contract, and service work together

This project is a simple .NET example for learning the flow of:

- test project
- service contract
- service implementation
- domain model/entity
- DTOs (request/response objects)

Think of it like a mini backend architecture:

```text
Test project
	-> calls service interface
		-> service implementation runs business logic
			-> converts DTOs to entity objects
				-> returns response DTOs
```

---

## 1. Project structure

The project is organized in layers:

- `Entities` : domain model, business data
- `ServiceContract` : public contract and DTOs
- `Services` : actual business logic
- `CRUDTests` : unit tests using xUnit

### Core files

- `Entities/Country.cs`
- `ServiceContract/ICountriesService.cs`
- `ServiceContract/DTO/CountryAddRequest.cs`
- `ServiceContract/DTO/CountryResponse.cs`
- `Services/CountriesService.cs`
- `CRUDTests/CountriesServiceTest.cs`

---

## 2. The domain model: `Country`

This is the real business object that represents a country in the application.

```csharp
namespace Entities
{
	public class Country
	{
		public Guid CountryId { get; set; }
		public string? CountryName { get; set; }
	}
}
```

### What it means

- `CountryId` is the unique ID of the country
- `CountryName` is the country name

This class is not the UI object and not the database object yet. It is the application/domain entity.

---

## 3. Service contract: the public agreement

The interface defines what the service can do.

```csharp
using ServiceContract.DTO;

namespace ServiceContract
{
	public interface ICountriesService
	{
		CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
		List<CountryResponse> GetAllCountries();
	}
}
```

### Why this matters

This interface is the contract between:

- the test project
- the service implementation

It tells us:

- method name: `AddCountry`
- input type: `CountryAddRequest`
- return type: `CountryResponse`

This is very common in layered architecture because it separates:

- contract definition (`ServiceContract`)
- implementation (`Services`)
- tests (`CRUDTests`)

---

## 4. DTOs: request and response objects

### `CountryAddRequest`

This is the object sent as input when the client wants to add a country.

```csharp
public class CountryAddRequest
{
	public string? CountryName { get; set; }

	public Country ToCountry()
	{
		return new Country
		{
			CountryName = this.CountryName
		};
	}
}
```

### What is happening here?

The request object is not the same as the domain entity.

- `CountryAddRequest` is used to receive input from outside
- `Country` is the internal application entity

The method `ToCountry()` converts a request object into a domain entity.

So this:

```csharp
new CountryAddRequest { CountryName = "England" }
```

becomes:

```csharp
new Country
{
	CountryName = "England"
}
```

### `CountryResponse`

This is what the service returns to the caller.

```csharp
public class CountryResponse
{
	public Guid CountryID { get; set; }
	public string? CountryName { get; set; }
}
```

There is also an extension method:

```csharp
public static class CountryExtensions
{
	public static CountryResponse ToCountryResponse(this Country country)
	{
		return new CountryResponse
		{
			CountryID = country.CountryId,
			CountryName = country.CountryName
		};
	}
}
```

This converts the internal `Country` entity into a response DTO that can be sent outside the service.

### Why use DTOs?

DTOs are used because:

- the client does not need the full internal entity details
- we hide the internal shape of the model
- we can validate input separately
- we keep business logic cleaner

---

## 5. Service implementation: `CountriesService`

This is where the business rules live.

```csharp
public class CountriesService : ICountriesService
{
	private readonly List<Country> _countries;

	public CountriesService()
	{
		_countries = new List<Country>();
	}

	public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
	{
		if (countryAddRequest == null)
		{
			throw new ArgumentNullException(nameof(countryAddRequest));
		}

		if (string.IsNullOrEmpty(countryAddRequest.CountryName))
		{
			throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
		}

		if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
		{
			throw new ArgumentException("Country with the same name already exists.", nameof(countryAddRequest.CountryName));
		}

		Country country = countryAddRequest.ToCountry();
		country.CountryId = Guid.NewGuid();
		_countries.Add(country);

		return country.ToCountryResponse();
	}
}
```

### Step-by-step explanation

#### a) Constructor

```csharp
private readonly List<Country> _countries;

public CountriesService()
{
	_countries = new List<Country>();
}
```

This creates an in-memory list to store countries. This is not a real database, just a temporary list in memory.

#### b) Null validation

```csharp
if (countryAddRequest == null)
{
	throw new ArgumentNullException(nameof(countryAddRequest));
}
```

If the caller sends `null`, the service rejects it.

#### c) Name validation

```csharp
if (string.IsNullOrEmpty(countryAddRequest.CountryName))
{
	throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
}
```

This prevents bad data.

#### d) Duplicate check

```csharp
if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
{
	throw new ArgumentException("Country with the same name already exists.", nameof(countryAddRequest.CountryName));
}
```

This ensures no duplicate countries are added.

#### e) Convert request to entity

```csharp
Country country = countryAddRequest.ToCountry();
```

The request DTO is converted into a domain `Country` object.

#### f) Assign ID

```csharp
country.CountryId = Guid.NewGuid();
```

Each country gets a new unique GUID.

#### g) Save to the in-memory list

```csharp
_countries.Add(country);
```

This simulates storage.

#### h) Return response DTO

```csharp
return country.ToCountryResponse();
```

The service returns a `CountryResponse` object, not the internal entity.

---

## 6. Unit tests: `CRUDTests`

The test project verifies the service behavior.

```csharp
public class CountriesServiceTest
{
	private readonly ICountriesService _countriesService;

	public CountriesServiceTest()
	{
		_countriesService = new CountriesService();
	}
```

This shows the pattern:

- create a service instance
- call methods
- assert expected results

### Example: null request test

```csharp
[Fact]
public void AddCountry_NullCountry()
{
	CountryAddRequest? request = null;

	Assert.Throws<ArgumentNullException>(() =>
	{
		_countriesService.AddCountry(request);
	});
}
```

### What is happening?

- Arrange: create a `null` request
- Act: call `_countriesService.AddCountry(request)`
- Assert: verify `ArgumentNullException` is thrown

This is xUnit style testing.

---

## 7. Full flow: AddCountry in one example

Here is the complete lifecycle:

```csharp
CountryAddRequest request = new CountryAddRequest
{
	CountryName = "England"
};
```

Step 1: Test calls service

```csharp
var response = _countriesService.AddCountry(request);
```

Step 2: Service validates data

- request is not null
- name is not empty
- duplicate name is not found

Step 3: Request is converted into entity

```csharp
Country country = request.ToCountry();
```

Step 4: Entity is saved

```csharp
_countries.Add(country);
```

Step 5: Response is returned

```csharp
return country.ToCountryResponse();
```

The result is a `CountryResponse` object like:

```csharp
new CountryResponse
{
	CountryID = some-guid,
	CountryName = "England"
}
```

---

## 8. Why this project is structured this way

This project is a classic layered design:

### Layer 1: Entities
- business model
- no service logic

### Layer 2: ServiceContract
- public interface
- DTO classes
- contract between layers

### Layer 3: Services
- contains business rules
- validates inputs
- manipulates in-memory data

### Layer 4: Tests
- proves the service behavior
- catches mistakes early

---

## 9. Key takeaway

The important idea is this:

- Tests call the service interface
- The service validates input
- The service converts DTOs into domain entities
- The service stores data in memory
- The service returns response DTOs

This is exactly how many real-world .NET applications are structured.

---

## 10. Quick mental model

Imagine a restaurant:

- `CountryAddRequest` = customer order form
- `CountriesService` = kitchen
- `Country` = internal recipe item
- `CountryResponse` = served result to the customer
- `CRUDTests` = quality check team

Every layer has one job.

---

## 11. Final summary

This project teaches a very important .NET pattern:

- interface-driven design
- DTO mapping
- validation in service layer
- unit testing with xUnit
- separation of concerns

If you understand this project, you already understand the foundation of many backend services in C#.

---

## 12. Next step

Try to answer these questions yourself:

1. What is the difference between `CountryAddRequest` and `Country`?
2. Why do we use `ICountriesService` instead of directly using `CountriesService`?
3. Why does the service return `CountryResponse` instead of `Country`?
4. Why are there test methods like `AddCountry_NullCountry`?

When you can answer these, you understand the project deeply.
