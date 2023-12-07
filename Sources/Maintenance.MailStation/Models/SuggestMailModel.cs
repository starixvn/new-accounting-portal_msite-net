namespace Maintenance.MailStation.Models;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed. Suppression is OK here.")]
public class SuggestMailModel
{
    public int id { get; set; }

    public string name { get; set; }

    public bool active { get; set; }
}