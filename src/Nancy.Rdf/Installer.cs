using JsonLD.Entities;
using Nancy.Bootstrapper;
using Nancy.Rdf.Contexts;

namespace Nancy.Rdf
{
    /// <summary>
    /// Installs components required by Nancy.RDF
    /// </summary>
    public class Installer : Registrations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>
        public Installer(ITypeCatalog typeCatalog)
            : base(typeCatalog)
        {
            this.RegisterWithDefault<INamespaceManager>(typeof(DictionaryNamespaceManager));
            this.RegisterWithDefault<IContextPathMapper>(typeof(DefaultContextPathMapper));
            this.Register<IEntitySerializer>(typeof(EntitySerializer));
        }
    }
}
