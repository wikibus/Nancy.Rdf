Feature: Serializing to Rdf
	Test serializing models to various RDF serializations

@Brochure
@JsonLd
Scenario: Serialize simple model to JSON-LD
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
@Turtle
Scenario: Serialize simple model to Turtle
	Given A model with content:
	| Property    | Vale                            |
	| Title       | Jelcz M11 - mały, stary autobus |
	And @context is:
		"""
		{
			'title': 'http://purl.org/dcterms/title'
		}
		"""
	When model is serialized
	Then graph should match:
		"""
			ASK WHERE
			{
				?res <http://purl.org/dcterms/title> "Jelcz M11 - mały, stary autobus"
			}
		"""