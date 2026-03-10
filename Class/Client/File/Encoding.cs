namespace PowerModule.Client.File;

enum Encoding
{
  [System.ComponentModel.Description(
    "The encoding for the current culture's ANSI code page"
  )]
  ansi,

  [System.ComponentModel.Description(
    "UTF-8 format (no BOM)"
  )]
  utf8,

  [System.ComponentModel.Description(
    "ASCII (7-bit) character set"
  )]
  ascii,

  [System.ComponentModel.Description(
    "UTF-16 (big-endian)"
  )]
  bigendianunicode,

  [System.ComponentModel.Description(
    "UTF-32 (big-endian)"
  )]
  bigendianutf32,

  [System.ComponentModel.Description(
    "The default encoding for MS-DOS and console programs"
  )]
  oem,

  [System.ComponentModel.Description(
    "UTF-16 (little-endian)"
  )]
  unicode,

  [System.ComponentModel.Description(
    "UTF-7"
  )]
  utf7,

  [System.ComponentModel.Description(
    "UTF-8 with Byte Order Mark (BOM)"
  )]
  utf8bom,

  [System.ComponentModel.Description(
    "UTF-8 without Byte Order Mark (no BOM)"
  )]
  utf8nobom,

  [System.ComponentModel.Description(
    "UTF-32 (little-endian)"
  )]
  utf32,
}
