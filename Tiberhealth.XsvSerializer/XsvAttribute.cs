namespace Tiberhealth.XsvSerializer
{
    /// <summary>
    /// Attribute to help describe or instruct the Xsv renderer
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class XsvAttribute: Attribute
    {
        /// <summary>
        /// The name of the Xsv Column
        /// </summary>
        public string HeaderName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public XsvAttribute() { }

        /// <summary>
        /// Constructor to include the Header Name
        /// </summary>
        /// <param name="headerName">Name of the Xsv Column Header</param>
        public XsvAttribute(string headerName): this()
        {
            this.HeaderName = headerName;
        }
    }
}
