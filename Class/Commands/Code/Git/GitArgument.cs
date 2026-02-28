namespace Module.Commands.Code.Git;

internal static partial class GitArgument
{
  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?>(?=.*[*=])(?>.+)|-(?>\w|(?>-\w[-\w]*\w)))$"
  )]
  internal static partial System.Text.RegularExpressions.Regex Regex();

  [System.Text.RegularExpressions.GeneratedRegex(
    @"^(?=.)(?>HEAD)?(?<branching>(?>~|\^)?)(?<step>(?>\d{0,10}))$"
  )]
  internal static partial System.Text.RegularExpressions.Regex TreeRegex();
}
