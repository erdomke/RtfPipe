// project    : System Framelet
using System;
using System.Collections;

namespace RtfPipe.Sys
{
  /// <summary>
  /// Some hash utility methods for collections.
  /// </summary>
  internal static class HashTool
  {
    public static int AddHashCode(this int hash, object obj)
    {
      int combinedHash = obj != null ? obj.GetHashCode() : 0;
      if (hash != 0) // perform this check to prevent FxCop warning 'op could overflow'
      {
        combinedHash += hash * 31;
      }
      return combinedHash;
    }

    public static int AddHashCode(int hash, int objHash)
    {
      int combinedHash = objHash;
      if (hash != 0) // perform this check to prevent FxCop warning 'op could overflow'
      {
        combinedHash += hash * 31;
      }
      return combinedHash;
    }

    public static int ComputeHashCode(IEnumerable enumerable)
    {
      int hash = 1;
      if (enumerable == null)
      {
        throw new ArgumentNullException("enumerable");
      }
      foreach (object item in enumerable)
      {
        hash = hash * 31 + (item != null ? item.GetHashCode() : 0);
      }
      return hash;
    }
  }
}

