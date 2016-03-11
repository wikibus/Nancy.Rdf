Feature: Serializing to RDF
	Test serializing models to RDF
		
@Brochure
@Rdf
Scenario: Serialize simple model to RDF
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
	Then graph should match:
		"""
			ASK WHERE
			{
				<http://wikibus.org/brochure/12345> <http://purl.org/dcterms/title> "Jelcz M11 - mały, stary autobus"
			}
		"""