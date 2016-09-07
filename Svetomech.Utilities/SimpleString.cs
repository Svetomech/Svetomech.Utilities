using System;
using System.Collections.Generic;
using static System.String;

namespace Svetomech.Utilities
{
  public static class SimpleString
  {
    public static string Pacmanise(this string str, int startIndex, char escapeChar)
    {
      if (IsNullOrWhiteSpace(str))
      {
        throw new ArgumentException(nameof(str));
      }

      string pacmanLine = null;
      foreach (char c in str.Remove(0, startIndex))
      {
        if (c == escapeChar)
        {
          break;
        }
        pacmanLine += c;
      }
      return pacmanLine;
    }

    public static IEnumerable<int> AllIndexesOf(this string str, string ofWhat)
    {
      if (IsNullOrWhiteSpace(str))
      {
        throw new ArgumentException(nameof(str));
      }

      return allIndexesOf(str, ofWhat);
    }


    private static IEnumerable<int> allIndexesOf(string str, string ofWhat)
    {
      for (int index = 0; ; index += ofWhat.Length)
      {
        index = str.IndexOf(ofWhat, index);
        if (-1 == index)
        {
          break;
        }
        yield return index;
      }
    }
  }
}
