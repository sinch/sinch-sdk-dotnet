using System.Collections.Generic;
using System.Linq;

namespace Sinch.Core
{
    public abstract class PropertyMaskQuery
    {
        protected readonly ISet<string> SetFields = new HashSet<string>();

        /// <summary>
        ///     Get the comma separated snake_case list of properties which were directly initialized in this object.
        ///     If, for example, DisplayName and Metadata were set, will return <example>display_name,metadata</example>
        /// </summary>
        /// <returns></returns>
        internal string GetPropertiesMask()
        {
            return string.Join(',', SetFields.Select(StringUtils.ToSnakeCase));
        }
    }
}
