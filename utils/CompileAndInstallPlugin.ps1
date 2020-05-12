param(
    [string]$DistributionTool = ".\DistributionTool.exe",
    [string]$StreamDeckPath = "C:\Program Files\Elgato\StreamDeck\StreamDeck.exe",
    [string]$ProjectRoot = "..\src",
    [switch]$Release,
    [string]$UUID = "au.com.mullineaux.lifx"

)
[int]$StreamdeckLoadTimeout = 3


if ($Release) { $Version = "Release" } else { $Version = "Debug" } 
# Stop the StreamDeck & plugin processes
Stop-Process -Name "StreamDeck" -ErrorAction SilentlyContinue
Stop-Process -Name $UUID -ErrorAction SilentlyContinue

# Compile the plugin source
Push-Location "$ProjectRoot\"
& $env:comspec "/c dotnet build --no-incremental"
Pop-Location

# Generate a plugin executable 
# using the StreamDeck distribution tool
$WorkingDir = Get-Item "$ProjectRoot\bin\$Version"
$Installer = Get-Item "$($WorkingDir.FullName)\$UUID.streamDeckPlugin"
If (Test-Path $Installer) { Remove-Item $Installer }
& $env:comspec "/c $DistributionTool --build --input $($WorkingDir.FullName)\$UUID.sdPlugin --output $($WorkingDir.FullName)"



# Remove the previous version of the plugin 
# from the StreamDeck plugin directory
$PluginDir = "$($env:APPDATA)\Elgato\StreamDeck\Plugins\$UUID.sdPlugin"
If (Test-Path $PluginDir) { Remove-Item $PluginDir -Recurse -Force }

# Restart StreamDeck
Start-Process -FilePath $StreamDeckPath 
Start-Sleep -Seconds $StreamdeckLoadTimeout

# Run the plugin installer
Start-Process $Installer


