using Nancy.Bootstrapper;

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
            Register<INamespaceManager>(new DictionaryNamespaceManager());
        }
    }
}
