Feature: Serializing to JSON-LD
	Test serializing models to JSON-LD

@Brochure
@JsonLd
Scenario: Should pass through serialized model
	Given A serialized model:
		"""
		{
			'@context': { 
				'title': 'http://purl.org/dcterms/title'
			},
			'@id': 'http://wikibus.org/brochure/12345',
			'title': 'Jelcz M11 - mały, stary autobus'
		}
		"""
	When model is serialized
	Then output stream should equal
		"""
		{
			'@context': { 
				'title': 'http://purl.org/dcterms/title'
			},
			'@id': 'http://wikibus.org/brochure/12345',
			'title': 'Jelcz M11 - mały, stary autobus'
		}
		"""