# Altura

---

## Execution Guide

### Introduction

The Altura project involves integration with Trello, aiming to manage tender details efficiently.

### Pre-requisites

Before running the project, ensure you have the following pre-requisites installed:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- **Premium** Trello account

Please note that a standard Trello account lacks permission to work with Custom Fields.

### Configuration

1. **Obtain API Key and Token**:
    - Log in to your Trello account.
    - Create a new workspace if necessary.
    - Visit the [Trello Power-Ups Admin Portal](https://trello.com/power-ups/admin/) page.
    - Fill in the required fields and click "Create."
    - Click "Generate a new API key."
    - Copy the provided API Key.
    - Next to the API Key field, click on the provided link to obtain a Token.
    - Allow the application access to your Trello account.
    - Copy the generated Token.

2. **Configure Project**:
    - Open the project solution in your preferred IDE.
    - Navigate to the configuration file `appsettings.Development.json`, where the API Key, Token, and BoardId are stored.
    - Update the configuration file with the values obtained in the previous step.

### Running the Project

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

### Extracting Tender Details

1. **Parse Endpoint**:
    - With the project running, navigate to http://localhost:5238/swagger/index.html.
    - Execute a call to the endpoint `POST /api/tender/parse`.

2. **Trello Integration**:
    - Check if lists and cards are created as the application consumes the `.csv` file.

3. **Stopping Execution**:
    - Press `CTRL+C` to stop the application execution.

### Updating the Trello Board

1. **Set Up BoardId**:
    - After the first execution of the parse endpoint, a new board named "Altura" will be created.
    - Obtain the ID of the Trello board from the board's URL after `/b/`. For example, in "https://trello.com/b/ABC123/altura", the ID is `ABC123`.
    - Add this value to the configuration settings under the `Trello` property.     

### Improvements

1. **Background Service**:
    - Introduce a background service to handle the process asynchronously improving overall performance and scalability.
    - This ensures that the application remains responsive and can handle multiple requests concurrently without blocking.

2. **Unit Testing Enhancements**
    - Improve unit test coverage by writing additional test cases to validate various scenarios, including error conditions and edge cases.

3. **Integration Tests**:
    - Develop integration tests to validate end-to-end functionality and interactions between different components of the application. 

4. **Read Files from Amazon S3**:
    - Implement functionality to read files directly from Amazon S3 storage, enabling seamless integration with cloud-based file storage solutions. This would enhance scalability, reliability, and accessibility of file processing operations, especially in distributed or cloud-native architectures