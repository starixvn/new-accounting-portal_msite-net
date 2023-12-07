using System.Globalization;
using System.Text.RegularExpressions;
using System.Timers;

namespace Maintenance.MailStation.Logging;

using Fanex.Logging;

public static class RabbitMqLogger
{
    private const int SendingMaxTime = 10;
    private static int sendingMessageCount = SendingMaxTime;
    private static readonly System.Timers.Timer timer = new(10 * 60 * 1000);

    public static void LogError(string errorMessage)
    {
        if (sendingMessageCount > 0)
        {
            sendingMessageCount -= 1;

            if (sendingMessageCount == 0)
            {
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }

            LogService.Error(errorMessage);
        }
    }

    private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
        timer.Stop();
        timer.Elapsed -= Timer_Elapsed;

        sendingMessageCount = SendingMaxTime;
    }
}

public class RabbitMqLogProvider : EasyNetQ.Logging.ILogger
{
    public bool Log(EasyNetQ.Logging.LogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
    {
        var message = LogMessageFormatter.FormatStructuredMessage(messageFunc(), formatParameters);

        if (exception != null)
        {
            message = $"{message} -> {exception}";
        }

        RabbitMqLogger.LogError(message);

        return true;
    }
}

public class RabbitMqLogProvider<TCategory> : RabbitMqLogProvider, EasyNetQ.Logging.ILogger<TCategory>
{
}

internal static class LogMessageFormatter
{
    private static readonly Regex Pattern = new(@"(?<!{){@?(?<arg>[^ :{}]+)(?<format>:[^}]+)?}",
        RegexOptions.Compiled);

    private static string ReplaceFirst(string text, string search, string replace)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        if (pos < 0)
        {
            return text;
        }

        return text[..pos] + replace + text[(pos + search.Length)..];
    }

    public static string FormatStructuredMessage(string targetMessage, object[] formatParameters)
    {
        if (formatParameters == null || formatParameters.Length == 0)
        {
            return targetMessage;
        }

        List<string> processedArguments = null;
        var formattedTargetMessage = targetMessage;

        foreach (Match match in Pattern.Matches(formattedTargetMessage))
        {
            var arg = match.Groups["arg"].Value;

            if (!int.TryParse(arg, out _))
            {
                processedArguments ??= new List<string>(formatParameters.Length);
                var argumentIndex = processedArguments.IndexOf(arg);
                if (argumentIndex == -1)
                {
                    argumentIndex = processedArguments.Count;
                    processedArguments.Add(arg);
                }

                formattedTargetMessage = ReplaceFirst(formattedTargetMessage, match.Value,
                    string.Concat("{", argumentIndex.ToString(), match.Groups["format"].Value, "}"));
            }
        }

        try
        {
            return string.Format(CultureInfo.InvariantCulture, formattedTargetMessage, formatParameters);
        }
        catch (FormatException ex)
        {
            throw new FormatException(
                "The input string '" + formattedTargetMessage + "' could not be formatted using string.Format", ex);
        }
    }
}