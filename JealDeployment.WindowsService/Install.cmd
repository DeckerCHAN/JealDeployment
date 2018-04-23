%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe JealDeployment.Service.exe
Net Start JealDeployment
sc config JealDeployment start= auto
PAUSE