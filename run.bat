@echo off
title Marwadi University - Smart Campus Service Portal
echo Starting Smart Campus Service Portal...
echo (First run downloads NuGet packages, internet needed once.)
start "" http://localhost:5080
dotnet run --launch-profile http
pause
