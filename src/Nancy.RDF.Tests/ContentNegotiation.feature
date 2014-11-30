Feature: Content Negotiation
	In order to select correct response
	Response processors must be implemented
	
@JsonLd
Scenario: Succesfully process JSON-LD response
	Given requested media range 'application/ld+json'
	When processing model
	Then response should have status 'OK'
	
@Rdf
Scenario: Succesfully process RDF response
	When processing model
	Then response should have status 'OK'