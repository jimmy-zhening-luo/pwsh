namespace Module.Client.File;

public enum Encoding
{
  [System.ComponentModel.Description(
    "Uses the encoding for the for the current culture's ANSI code page. This option was added in PowerShell 7.4."
  )]
  ansi,

  [System.ComponentModel.Description(
    "Encodes in UTF-8 format (no BOM)."
  )]
  utf8,

  [System.ComponentModel.Description(
    "Uses the encoding for the ASCII (7-bit) character set."
  )]
  ascii,

  [System.ComponentModel.Description(
    "Encodes in UTF-16 format using the big-endian byte order."
  )]
  bigendianunicode,

  [System.ComponentModel.Description(
    "Encodes in UTF-32 format using the big-endian byte order."
  )]
  bigendianutf32,

  [System.ComponentModel.Description(
    "Uses the default encoding for MS-DOS and console programs."
  )]
  oem,

  [System.ComponentModel.Description(
    "Encodes in UTF-16 format using the little-endian byte order."
  )]
  unicode,

  [System.ComponentModel.Description(
    "Encodes in UTF-7 format."
  )]
  utf7,

  [System.ComponentModel.Description(
    "Encodes in UTF-8 format with Byte Order Mark (BOM)"
  )]
  utf8BOM,

  [System.ComponentModel.Description(
    "Encodes in UTF-8 format without Byte Order Mark (BOM)"
  )]
  utf8NoBOM,

  [System.ComponentModel.Description(
    "Encodes in UTF-32 format using the little-endian byte order."
  )]
  utf32
}
