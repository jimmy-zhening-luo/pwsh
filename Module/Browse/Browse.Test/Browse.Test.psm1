
  process {
    foreach ($computerName in $Name) {
    
          CommonTCPPort {
            switch ($CommonTCPPort) {
              '' { break }
              {
                $null -ne [Browse.TestHostWellKnownPort]::$CommonTCPPort
              } {
                $Destination.CommonTCPPort = [Browse.TestHostWellKnownPort]::$CommonTCPPort
                break
              }
              {
                $CommonTCPPort -as [ushort]
              } {
                $Destination.Port = [ushort]$CommonTCPPort
              }
            }
          }

        Test-NetConnection @Destination -InformationLevel $InformationLevel
      }
    }
  }

  