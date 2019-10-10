ApiKey=$1
Source=$2

dotnet pack -c Release
dotnet nuget push CleanArchitecture.Requestor/bin/Release/CleanArchitecture.Requestor.*.nupkg -k $ApiKey -s $Source