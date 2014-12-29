using Nancy.Bootstrapper;
using VDS.RDF;

namespace Nancy.RDF
{
    /// <summary>
    /// Installs components required by Nancy.RDF
    /// </summary>
    public class Installer : Registrations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>
        public Installer()
        {
            Register<INamespaceMapper>(new NamespaceMapper(true));
        }
    }
}
