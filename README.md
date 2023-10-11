# GCX Create Chat

## Summary

Creates a Teams chat between the logged in user and a selected user

## Prerequisites

## Version 

![dotnet 6](https://img.shields.io/badge/net6.0-blue.svg)

## API permission

MSGraph

| API / Permissions name    | Type      | Admin consent | Justification                       |
| ------------------------- | --------- | ------------- | ----------------------------------- |
| Chat.Create               | Delegated | Yes           | Create a one-on-one chat            |
| User.Read.All             | Delegated | Yes           | Read logged in user id property     |

Sharepoint

n/a

## App setting

| Name                    | Description                                                                   |
| ----------------------- | ----------------------------------------------------------------------------- |
| AzureWebJobsStorage     | Connection string for the storage acoount                                     |
| clientId                | The application (client) ID of the app registration                           |
| keyVaultUrl             | Address for the key vault                                                     |
| secretName              | Secret name used to authorize the function app                                |
| tenantId                | Id of the Azure tenant that hosts the function app                            |

## Version history

Version|Date|Comments
-------|----|--------
1.0|TBD|Initial release

## Disclaimer

**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**
