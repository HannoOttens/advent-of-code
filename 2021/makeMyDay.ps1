param (
    [string]$day = "$(Read-Host 'Enter the folder name. [e.g. day_1]')"
)

function main() {
	New-Item $day -ItemType "directory"
	Set-Location $day
	New-Item "input.txt"
	.\input.txt
	dotnet.exe new console -n a
	dotnet.exe new console -n b
	Set-Location a
	.\Program.cs
}

main