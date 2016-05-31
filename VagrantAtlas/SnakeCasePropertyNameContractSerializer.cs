using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace VagrantAtlas
{
    public class SnakeCasePropertyNameContractSerializer : DefaultContractResolver
    {
        /// <summary>
        /// Resolves the key of the dictionary by resolving each part (seperator ".") with
        /// <see cref="M:Newtonsoft.Json.Serialization.DefaultContractResolver.ResolvePropertyName(System.String)" />.
        /// </summary>
        /// <param name="dictionaryKey">Key of the dictionary.</param>
        /// <returns>
        /// Resolved key of the dictionary.
        /// </returns>
        protected override string ResolveDictionaryKey(string dictionaryKey)
        {
            var parts = dictionaryKey.Split('.');

            return string.Join(".", parts.Select(ResolvePropertyName));
        }

        /// <summary>
        /// Resolves the name of the property by returning a snake case representation.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Resolved name of the property.
        /// </returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, "([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
        }
    }
}