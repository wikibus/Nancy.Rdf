using System;

namespace Nancy.Rdf.Responses
{
    /// <summary>
    /// Wrapper for serialized model
    /// </summary>
    public struct WrappedModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedModel"/> struct.
        /// </summary>
        public WrappedModel(object model, string siteBase)
        {
            this.Model = model;
            this.BaseUrl = new Uri(siteBase, UriKind.Absolute);
        }

        /// <summary>
        /// Gets the base URL from <see cref="NancyContext"/>.
        /// </summary>
        public Uri BaseUrl { get; }

        /// <summary>
        /// Gets the actual serialized model.
        /// </summary>
        public object Model { get; private set; }
    }
}
