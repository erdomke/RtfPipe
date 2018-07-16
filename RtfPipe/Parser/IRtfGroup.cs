using System;
using System.Collections.Generic;

namespace RtfPipe
{

	public interface IRtfGroup : IRtfElement
	{

		IList<IRtfElement> Contents { get; }

		/// <summary>
		/// Returns the name of the first element if it is a tag, null otherwise.
		/// </summary>
		string Destination { get; }

		/// <summary>
		/// Determines whether the first element is a '\*' tag.
		/// </summary>
		bool IsExtensionDestination { get; }

		/// <summary>
		/// Searches for the first child group which has a tag with the given name
		/// as its first child, e.g. the given destination.
		/// </summary>
		/// <param name="destination">the name of the start tag of the group to search</param>
		/// <returns>the first matching group or null if nothing found</returns>
		/// <exception cref="ArgumentNullException">in case of a null argument</exception>
		IRtfGroup SelectChildGroupWithDestination( string destination );

	}

}

