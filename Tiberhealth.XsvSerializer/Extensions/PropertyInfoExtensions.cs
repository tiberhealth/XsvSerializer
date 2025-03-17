namespace Tiberhealth.XsvSerializer.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Gets the name for the Object's column
        /// </summary>
        /// <param name="property">Property to check for Multipart Attribute</param>
        /// <param name="defaultFactory">Factory to build default value (optional)</param>
        /// <returns>The determined name</returns>
        internal static string ObjectColumnName(this PropertyInfo property, Func<string> defaultFactory = null) =>
            (
                property.HasCustomAttribute<XsvAttribute>(out var xsvAttribute) ? xsvAttribute.HeaderName :
                property.HasCustomAttribute<JsonPropertyAttribute>(out var newtonsoftAttribute) ? newtonsoftAttribute.PropertyName :
                property.HasCustomAttribute<JsonPropertyNameAttribute>(out var systemJsonAttribute) ? systemJsonAttribute.Name :
                defaultFactory?.Invoke()
            ) ?? property.Name;

        /// <summary>
        /// Checks the property for a custom attribute
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to find</typeparam>
        /// <param name="property">The property to check</param> 
        /// <param name="attribute">Out variable of the actual attribute</param>
        /// <returns>True/False indicating if the property was found</returns>
        public static bool HasCustomAttribute<TAttribute>(this PropertyInfo property, out TAttribute attribute)
            where TAttribute : Attribute
        {
            attribute = property?.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

        /// <summary>
        /// Determines if a type has a custom attribute
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <param name="attribute">(Out) the resulting instantiated attribute class that was found</param>
        /// <typeparam name="TAttribute">Attribute type that is being checked for in the type</typeparam>
        /// <returns>True/False if the type contained the attribute</returns>
        public static bool HasCustomAttribute<TAttribute>(this Type type, out TAttribute attribute)
            where TAttribute : Attribute
        {
            attribute = type.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

    }
}
