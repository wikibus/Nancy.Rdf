Feature: Serializing to JSON-LD
	Test serializing models to JSON-LD

@Brochure
@JsonLd
Scenario: Serialize simple model with blank id to JSON-LD
	Given A model with content:
	| Property | Vale |
	| Title    | Jelcz M11 - mały, stary autobus |
	And @context is: 
		"""
		'http://wikibus.org/contexts/brochure.jsonld'
		"""
	When model is serialized
	Then json object should contain key 'title' with value 'Jelcz M11 - mały, stary autobus'
	Then json object should contain key '@context' with value 'http://wikibus.org/contexts/brochure.jsonld'
	
@Brochure
@JsonLd
Scenario: Skip null properties when serializing model to JSON-LD
	Given A model with content:
	| Property    | Vale                            |
	| Title       | Jelcz M11 - mały, stary autobus |
	When model is serialized
	Then json object should not contain key 'description'

@Brochure
@JsonLd
Scenario: Serialize simple model with URI id to JSON-LD
	Given A model with content:
	| Property | Vale                                  |
	| Title    | Jelcz M11 - mały, stary autobus |
	And Model has property Id set to 'http://wikibus.org/brochure/Jelcz_M11'
	And @context is: 
		"""
		'http://wikibus.org/contexts/brochure.jsonld'
		"""
	When model is serialized
	Then json object should contain key '@id' with value 'http://wikibus.org/brochure/Jelcz_M11'