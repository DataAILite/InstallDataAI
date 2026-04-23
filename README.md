# InstallDataAI

A Windows Forms utility for installing and configuring the [DataAI](https://github.com/DataAILite/DataAI) web application on Microsoft IIS.

## Overview

InstallDataAI automates the deployment of DataAI — an open-source AI-powered data reporting and analysis system in memory — onto a Windows server running IIS. It handles IIS site/application creation, virtual directory configuration, and application pool setup so that administrators do not need to configure IIS manually.

## Prerequisites

- Windows 10/11 or Windows Server 2016+
- [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (Windows)
- IIS enabled with ASP.NET support
- Administrator privileges (required for IIS configuration)
- MySQL database server (for DataAI operational and data databases)


## Dependencies

| Dependency | Purpose | License |
|---|---|---|
| .NET 8.0 (Windows Forms) | Application framework and UI | MIT |
| Microsoft.Web.Administration | IIS site and app pool management | MIT |
| System.Data | Database connectivity | MIT |

## Build

Open the project in Visual Studio 2022 or later and build, or use the .NET CLI


## Usage

1. Run `InstallDataAI.exe` as Administrator.
2. Follow the on-screen steps to configure the IIS website path, application pool, and database connection.
3. The installer will create the IIS site/application and deploy the DataAI web files.

> **Note:** Administrator privileges are required because the tool writes to IIS configuration via `Microsoft.Web.Administration`.


## License

This project is part of DataAI and is licensed under the [GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.html).
