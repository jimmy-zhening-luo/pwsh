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
  Rules        = @{
    # PSAvoidUsingCmdletAliases      = @{
    #     AllowList = @(
    #         "cd"
    #     )
    # }
    # PSAvoidUsingPositionalParameters = @{
    #   CommandAllowList = @(
    #     "npm"
    #     "npx"
    #     "wg"
    #     "wga"
    #     "yt"
    #     "yta"
    #   )
    # }
    # done by VSCode too but keeping it on because it catches VSCode not doing it
    # PSUseConsistentIndentation       = @{
    #   Enable              = $true
    #   IndentationSize     = 2
    #   PipelineIndentation = "IncreaseIndentationForFirstPipeline"
    # }
    # defer to VSCode
    # PSUseConsistentWhitespace        = @{
    #   Enable                                  = $true
    #   CheckInnerBrace                         = $true
    #   CheckOpenBrace                          = $true
    #   CheckOpenParen                          = $true
    #   CheckOperator                           = $true
    #   CheckSeparator                          = $true
    #   CheckPipe                               = $true
    #   CheckPipeForRedundantWhitespace         = $true
    #   CheckParameter                          = $false
    #   IgnoreAssignmentOperatorInsideHashTable = $true
    # }
    # defer to VSCode
    # PSUseCorrectCasing               = @{
    #   Enable        = $true
    #   CheckCommands = $true
    #   CheckKeyword  = $true
    #   CheckOperator = $true
    # }
    PSUseSingularNouns = @{
      NounAllowList = @(
        "Data" # default
        "Windows" # default
      )
    }
  }
}
