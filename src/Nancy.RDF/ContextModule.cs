namespace Nancy.RDF
{
    /// <summary>
    /// Module, which serves JSON-LD contexts
    /// </summary>
    public abstract class ContextModule : NancyModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextModule"/> class.
        /// </summary>
        protected ContextModule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextModule"/> class.
        /// </summary>
        /// <param name="modulePath">A base path for contexts.</param>
        protected ContextModule(string modulePath) : base(modulePath)
        {
        }
    }
}
