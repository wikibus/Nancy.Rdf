Feature: Serializing to Turtle
	Test serializing models to Turtle
	
@Brochure
@Turtle
Scenario: Serialize simple model with blank id to Turtle
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