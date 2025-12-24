& {
  $TypeAcceleratorsClass = [psobject].Assembly.GetType('System.Management.Automation.TypeAccelerators')

  $TypeAcceleratorsClass::Add('path', [System.IO.Path])
  $TypeAcceleratorsClass::Add('List', [System.Collections.Generic.List])
}
