using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sinch.Core
{
    /// <summary>
    ///     Tracks which properties on a merge-patch request were explicitly assigned by the caller.
    ///     Used to implement RFC 7396 JSON Merge Patch semantics without wrapper types in the public API:
    ///     <list type="bullet">
    ///         <item>Tracked non-null property → serialized with its value.</item>
    ///         <item>Tracked null property → serialized as <c>null</c> (clears field on server).</item>
    ///         <item>Untracked null property → omitted from the payload entirely.</item>
    ///     </list>
    ///     Properties call <see cref="Track" /> inside their setters; the serializer checks
    ///     <see cref="IsSet" /> via the <see cref="IHasSetTracker" /> interface.
    /// </summary>
    internal sealed class SetTracker
    {
        private HashSet<string>? _properties;

        /// <summary>
        ///     Records that the calling property was explicitly set.
        ///     The <see cref="CallerMemberNameAttribute" /> fills in the property name automatically.
        /// </summary>
        public void Track([CallerMemberName] string? propertyName = null)
        {
            if (propertyName is null) return;
            _properties ??= new HashSet<string>();
            _properties.Add(propertyName);
        }

        /// <summary>Returns <c>true</c> when <paramref name="propertyName" /> was explicitly assigned.</summary>
        public bool IsSet(string propertyName) => _properties?.Contains(propertyName) ?? false;
    }

    /// <summary>
    ///     Marker interface for request objects that use <see cref="SetTracker" /> to control
    ///     which properties are included in the serialized JSON payload.
    /// </summary>
    internal interface IHasSetTracker
    {
        /// <summary>The tracker for this request instance.</summary>
        SetTracker SetTracker { get; }
    }
}
