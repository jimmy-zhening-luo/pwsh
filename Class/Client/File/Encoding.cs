namespace Module.Client.File;

internal enum Encoding
{
  [System.ComponentModel.DataAnnotations.Display(Name = "ANSI")]
  [System.ComponentModel.Description(
    "The encoding for the current culture's ANSI code page"
  )]
  ansi,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-8")]
  [System.ComponentModel.Description(
    "UTF-8 format (no BOM)"
  )]
  utf8,

  [System.ComponentModel.DataAnnotations.Display(Name = "ASCII")]
  [System.ComponentModel.Description(
    "ASCII (7-bit) character set"
  )]
  ascii,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-16 Big-Endian")]
  [System.ComponentModel.Description(
    "UTF-16 (big-endian)"
  )]
  bigendianunicode,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-32 Big-Endian")]
  [System.ComponentModel.Description(
    "UTF-32 (big-endian)"
  )]
  bigendianutf32,

  [System.ComponentModel.DataAnnotations.Display(Name = "OEM")]
  [System.ComponentModel.Description(
    "The default encoding for MS-DOS and console programs"
  )]
  oem,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-16")]
  [System.ComponentModel.Description(
    "UTF-16 (little-endian)"
  )]
  unicode,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-7")]
  [System.ComponentModel.Description(
    "UTF-7"
  )]
  utf7,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-8 BOM")]
  [System.ComponentModel.Description(
    "UTF-8 with Byte Order Mark (BOM)"
  )]
  utf8bom,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-8")]
  [System.ComponentModel.Description(
    "UTF-8 without Byte Order Mark (no BOM)"
  )]
  utf8nobom,

  [System.ComponentModel.DataAnnotations.Display(Name = "UTF-32")]
  [System.ComponentModel.Description(
    "UTF-32 (little-endian)"
  )]
  utf32,
}
