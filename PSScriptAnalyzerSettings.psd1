@{
  # DOC: https://learn.microsoft.com/powershell/utility-modules/psscriptanalyzer/rules/readme
  IncludeRules = @(
    # "PSAlignAssignmentStatement"
    "PSAvoidAssignmentToAutomaticVariable"
    "PSAvoidDefaultValueForMandatoryParameter"
    "PSAvoidDefaultValueSwitchParameter"
    "PSAvoidExclaimOperator"
    "PSAvoidGlobalAliases"
    "PSAvoidGlobalFunctions"
    "PSAvoidGlobalVars"
    "PSAvoidInvokingEmptyMembers"
    # "PSAvoidLongLines"
    "PSAvoidMultipleTypeAttributes"
    "PSAvoidNullOrEmptyHelpMessageAttribute"
    # "PSAvoidOverwritingBuiltInCmdlets"
    "PSAvoidSemicolonsAsLineTerminators"
    "PSAvoidShouldContinueWithoutForce"
    "PSAvoidTrailingWhitespace"
    "PSAvoidUsingAllowUnencryptedAuthentication"
    "PSAvoidUsingBrokenHashAlgorithms"
    # "PSAvoidUsingCmdletAliases"
    "PSAvoidUsingComputerNameHardcoded"
    "PSAvoidUsingConvertToSecureStringWithPlainText"
    "PSAvoidUsingDeprecatedManifestFields"
    # "PSAvoidUsingDoubleQuotesForConstantString"
    "PSAvoidUsingEmptyCatchBlock"
    "PSAvoidUsingInvokeExpression"
    "PSAvoidUsingPlainTextForPassword"
    # "PSAvoidUsingPositionalParameters"
    "PSAvoidUsingUsernameAndPasswordParams"
    "PSAvoidUsingWMICmdlet"
    "PSAvoidUsingWriteHost"
    "PSMisleadingBacktick"
    "PSMissingModuleManifestField"
    # "PSPlaceCloseBrace" # VSCode
    # "PSPlaceOpenBrace" # VSCode
    "PSPossibleIncorrectComparisonWithNull"
    "PSPossibleIncorrectUsageOfAssignmentOperator"
    "PSPossibleIncorrectUsageOfRedirectionOperator"
    "PSProvideCommentHelp"
    "PSReservedCmdletChar"
    "PSReservedParams"
    # "PSReviewUnusedParameter"
    "PSShouldProcess"
    "PSUseApprovedVerbs"
    "PSUseBOMForUnicodeEncodedFile"
    "PSUseCmdletCorrectly"
    # "PSUseCompatibleCmdlets"
    # "PSUseCompatibleCommands"
    # "PSUseCompatibleSyntax"
    # "PSUseCompatibleTypes"
    # "PSUseConsistentIndentation"
    # "PSUseConsistentWhitespace"
    # "PSUseCorrectCasing"
    # "PSUseDeclaredVarsMoreThanAssignments"
    "PSUseLiteralInitializerForHashtable"
    "PSUseOutputTypeCorrectly"
    "PSUseProcessBlockForPipelineCommand"
    "PSUsePSCredentialType"
    # "PSUseShouldProcessForStateChangingFunctions"
    "PSUseSingularNouns"
    "PSUseSupportsShouldProcess"
    "PSUseToExportFieldsInManifest"
    "PSUseUsingScopeModifierInNewRunspaces"
    "PSUseUTF8EncodingForHelpFile"
  )
  # Rules        = @{
  #   PSUseSingularNouns = @{
  #     NounAllowList = @(
  #       "Data" # default
  #       "Windows" # default
  #     )
  #   }
  # }
}
