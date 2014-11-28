Feature: Serializing to JSON-LD
	Test serializing models to JSON-LD

@Brochure
@JsonLd
Scenario: Serialize simple model with blank id
	Given A model with content:
	| Property | Vale |
	| Title    | Jelcz M11 - mały, stary autobus |
	When model is serialized
	Then json object should contain key 'title' with value 'Jelcz M11 - mały, stary autobus'
	Then json object should not contain key '@id'
	
@Brochure
@JsonLd
Scenario: Skip null properties when serializing model
	Given A model with content:
	| Property    | Vale                            |
	| Title       | Jelcz M11 - mały, stary autobus |
	When model is serialized
	Then json object should not contain key 'description'

@Brochure
@JsonLd
Scenario: Serialize simple model with URI id
	Given A model with content:
	| Property | Vale                                  |
	| Title    | Jelcz M11 - mały, stary autobus |
	And Model has property Id set to 'http://wikibus.org/brochure/Jelcz_M11'
	When model is serialized
	Then json object should contain key '@id' with value 'http://wikibus.org/brochure/Jelcz_M11'

@JsonLd
Scenario: Serialize model @types
	Given A model of type 'Nancy.RDF.Tests.Models.TypedModel'
	When model is serialized
	Then @types property should contain
		| Type                               |
		| http://example.org/ontology#Parent |