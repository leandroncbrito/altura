# Altura

---

# Execution Guide

## Introduction

The project involves integration with Trello.

## Pre-requisites

Before running the project, ensure you have the following pre-requisites installed:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- **Premium** Trello account

Note: A standard account has no permission to work with Custom Fields

## Configuration

1. **Obtain ApiKey and Token**:
    - Log in to your Trello account.
    - Create a new workspace.
    - Go to [Trello Power-Ups Admin Portal](https://trello.com/power-ups/admin/) page.
    - Fill in the fields and click on Create button.
    - Click on Generate a new API key.
    - Copy the ApiKey provided.
    - Open the token link next to API Key field
    - Allow the application access to your account.    
    - Copy the generated Token.

2. **Configure Project**:
    - Open the project solution in your preferred IDE.
    - Navigate to the configuration file `appsettings.Development.json` where ApiKkey, Token, and BoardId are stored.
    - Update the configuration file with the values obtained in the previous steps.
    - In order to use the same board and update cards, the Trello.

## Running the Project

1. **Build the Project**:
    - Open a terminal or command prompt.
    - Navigate to the project directory.
    - Run the following command to build the project:
      ```bash
      dotnet build
      ```

2. **Run the Application**:
    - Once the project is built successfully, run the following command to start the application:
      ```bash
      dotnet run --project Altura.Api
      ```

3. **Verify Execution**:
    - Once the application is running, verify that it connects to Trello and performs the desired actions as expected.

## Extracting tender details

1. **Endpoint**:
    - Navigate to http://localhost:5238/swagger/index.html
    - Execute the call to the endpoint POST `/api/tender/parse`
2. **Trello**
    - List and cards should be created while the project reads the .csv file
3. **Stop execution**
    - Press CTRL+C to stop the execution

## Updating the board

1. 1. **Set Up BoardId**:
    - A new board named "Altura" will be created after the first execution of parse endpoint.    - 
    - Obtain the Id of the Trello board in the board's URL after `/b/`. Eg: "https://trello.com/b/ABC123/altura"  => `ABC123`
    - Add this value to configuration settings under Trello property.
