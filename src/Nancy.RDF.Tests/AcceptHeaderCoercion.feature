Feature: Accept Header Coercion
	To be more dev-friendly
	I want default to return RDF from routes by default
	
Scenario: Set default Accept to Turtle
	Given default convention
	When invoke the convention with empty header collection
	Then Accept header should be 'text/turtle', '1.0'
	
Scenario: Set default Accept to declared media type
	Given convention set to return json-ld
	When invoke the convention with empty header collection
	Then Accept header should be 'application/ld+json', '1.0'
	
Scenario: Respect existing Accept when overriden with default
	Given default convention
	When invoke the convention with non-empty header collection
	Then Accept header should be 'text/html', '0.9'

Scenario: Respect existing Accept when overriden selected media type
	Given convention set to return json-ld
	When invoke the convention with non-empty header collection
	Then Accept header should be 'text/html', '0.9'