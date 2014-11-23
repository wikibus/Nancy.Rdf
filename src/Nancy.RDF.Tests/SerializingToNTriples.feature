Feature: Serializing to NTriples
	Test serializing models to NTriples
	
@Brochure
@NTriples
Scenario: Serialize simple model to NTriples
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