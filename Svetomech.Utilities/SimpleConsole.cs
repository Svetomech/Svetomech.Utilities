using System;
using System.Diagnostics;
using System.Security;
using static System.Console;

namespace Svetomech.Utilities
{
  public static partial class SimpleConsole
  {
    public enum ConsoleTypes
    {
      CMD,
      Powershell,
      Bash,
      None
    }

    public static string ExecuteCommand(string command, ConsoleTypes console)
    {
      ProcessStartInfo procStartInfo = null;
      switch (console)
      {
        case ConsoleTypes.CMD:
          procStartInfo = new ProcessStartInfo("cmd", "/c " + command);
          break;

        case ConsoleTypes.Powershell:
          procStartInfo = new ProcessStartInfo("powershell", "-command " + command);
          break;

        case ConsoleTypes.Bash:
          procStartInfo = new ProcessStartInfo("/bin/bash", "-c " + command);
          break;

        case ConsoleTypes.None:
          return null;

        default:
          throw new ArgumentException(nameof(console));
      }

      procStartInfo.RedirectStandardOutput = true;
      procStartInfo.UseShellExecute = false;
      procStartInfo.CreateNoWindow = true;

      Process proc = new Process();
      proc.StartInfo = procStartInfo;
      proc.Start();

      string result = proc.StandardOutput.ReadToEnd();

      // HACK: Only need it because my server doesn't handle NewLine too well
      result = result.Replace(Environment.NewLine, "\n");

      return result;
    }

    /* HACK (to get the actual value, not an object): new NetworkCredential(String.Empty, PasswordPrompt("Enter a password: ")).Password;
         N.B.! It's really dirty, kills the purpose of using a SecureString and doesn't even work in Mono. Better use Insecure method. */
    public static SecureString PasswordPrompt(string hintMessage)
    {
      var pass = new SecureString();

      passwordPromptBasic(hintMessage, ref pass);

      return pass;
    }

    public static string InsecurePasswordPrompt(string hintMessage)
    {
      string pass = String.Empty;

      passwordPromptBasic(hintMessage, ref pass);

      return pass;
    }

    private static void passwordPromptBasic<T>(string hintMessage, ref T password)
    {
      Clear();
      CursorVisible = true;

      string middlePractical = $"| {hintMessage}";
      string middle = $"{middlePractical} |";
      middle = $"{middlePractical}{Line.GetFilled(' ').Remove(0, middle.Length)} |";

      Write($"#{Line.GetFilled('-').Remove(0, 2)}#");
      Write(middle);
      Write($"#{Line.GetFilled('-').Remove(0, 2)}#");
      SetCursorPosition(middlePractical.Length, CursorTop - 2);

      SecureString passHolder = null;
      string insecurePassHolder = null;
      if (password.GetType() == typeof(SecureString))
      {
        passHolder = new SecureString();
      }
      else
      {
        insecurePassHolder = String.Empty;
      }
      bool useSecureHolder = (passHolder != null);

      int starsCount = 0;
      int middleDiff = middle.Length - middlePractical.Length;

      ConsoleKeyInfo keyInfo;
      while ((keyInfo = ReadKey(true)).Key != ConsoleKey.Enter)
      {
        if (keyInfo.Key != ConsoleKey.Backspace)
        {
          /*if (!((int)ki.Key >= 65 && (int)ki.Key <= 90))
            continue;*/ // <-- stricter, but disallows digits
          if (char.IsControl(keyInfo.KeyChar))
          {
            continue;
          }

          if (starsCount + 1 < middleDiff)
          {
            starsCount++;
          }
          else
          {
            continue;
          }

          if (useSecureHolder)
          {
            passHolder.AppendChar(keyInfo.KeyChar);
          }
          else
          {
            insecurePassHolder += keyInfo.KeyChar;
          }
          

          Write('*');
        }
        else
        {
          if (starsCount - 1 >= 0)
          {
            starsCount--;
          }
          else
          {
            continue;
          }

          if (useSecureHolder)
          {
            passHolder.RemoveAt(passHolder.Length - 1);
          }
          else
          {
            insecurePassHolder.Remove(insecurePassHolder.Length - 1, 1);
          }

          Line.ClearCurrent();
          Write(middlePractical);
          for (int i = 0; i < starsCount; ++i)
          {
            Write('*');
          }
          var pos = new Point(CursorLeft, CursorTop);
          Write($"{Line.GetFilled(' ').Remove(0, middlePractical.Length + starsCount + " |".Length)} |");
          SetCursorPosition(pos.X, pos.Y);
        }

        if (0 == starsCount)
        {
          Beep();
        }
        if (middleDiff - 1 == starsCount)
        {
          SetCursorPosition(CursorLeft - 1, CursorTop);
          Beep();
        }
      }

      Clear();

      password = useSecureHolder ? (T)(object)passHolder : (T)(object)insecurePassHolder;
    }
  }
}
