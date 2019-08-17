using System;

namespace Nancy.Rdf.Sample
{
    public class PersonModule : NancyModule
    {
        public PersonModule()
        {
            Get("person/{id}", _ =>
            {
                return new Person
                {
                    Id = "http://api.guru/person/" + _.id,
                    Name = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1967, 8, 2),
                    Friend = ("http://api.guru/person/" + _.id + 10),
                    Parent = new Person
                    {
                        Name = "Jane"
                    }
                };
            });
        }
    }
}