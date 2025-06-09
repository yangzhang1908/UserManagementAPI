# UserManagementAPI

UserManagementAPI is a simple RESTful API that provides JWT-based authentication and user management functionality. 


This is build with help of Copilot and only for the coursera small project.

-----

# User Management API

The API provides standard CRUD (Create, Read, Update, Delete) operations for user data and incorporates essential middleware for logging, error handling, and authentication.

-----

## Features

  * **User CRUD Operations:**
      * **GET /api/users:** Retrieve a list of all users.
      * **GET /api/users/{id}:** Retrieve a specific user by ID.
      * **POST /api/users:** Add a new user with validation.
      * **PUT /api/users/{id}:** Update an existing user's details with validation.
      * **DELETE /api/users/{id}:** Remove a user by ID.
  * **Data Validation:** Ensures user input meets defined criteria (e.g., required fields, email format) using Data Annotations.
  * **Global Error Handling:** Catches unhandled exceptions and returns consistent, generic JSON error responses, preventing API crashes and sensitive information leaks.
  * **Request/Response Logging:** Logs HTTP method, request path, and response status codes for all incoming and outgoing API traffic, useful for auditing and debugging.
  * **Token-Based Authentication:** Secures API endpoints, allowing access only to requests with a valid `Bearer` token. Invalid requests receive a `401 Unauthorized` response.
  * **Swagger/OpenAPI:** Provides interactive API documentation and a testing interface in development environments.

-----

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

  * .NET SDK (version 6.0 or higher recommended)
  * An IDE like Visual Studio, Visual Studio Code, or JetBrains Rider
  * (Optional) A tool like Postman or Insomnia for API testing

### Installation

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/your-username/UserManagementAPI.git
    cd UserManagementAPI
    ```

    (Note: Replace `your-username` with your actual GitHub username if this were a real repository.)

2.  **Restore dependencies:**

    ```bash
    dotnet restore
    ```

### Running the API

You can run the API from your IDE or the command line.

**From the Command Line:**

```bash
dotnet run
```

This will start the API, typically on `https://localhost:7XXX` (where `7XXX` is a port number assigned by .NET).

**From an IDE (e.g., Visual Studio):**
Open the `UserManagementAPI.sln` file in your IDE and press `F5` or the "Run" button to start the project in debug mode.

Once the API is running, your browser should automatically open to the **Swagger UI** (e.g., `https://localhost:7XXX/swagger`). This interface allows you to explore and test the API endpoints.

-----

## API Endpoints

All endpoints are prefixed with `/api/users`.

| HTTP Method | Path          | Description                         | Authentication Required |
| :---------- | :------------ | :---------------------------------- | :---------------------- |
| `GET`       | `/`           | Get all users                       | Yes                     |
| `GET`       | `/{id}`       | Get a user by ID                    | Yes                     |
| `POST`      | `/`           | Create a new user                   | Yes                     |
| `PUT`       | `/{id}`       | Update an existing user by ID       | Yes                     |
| `DELETE`    | `/{id}`       | Delete a user by ID                 | Yes                     |

**Example `User` Model (JSON structure):**

```json
{
  "id": 0,          // Auto-generated on POST, required on PUT
  "firstName": "string",
  "lastName": "string",
  "email": "user@example.com",
  "department": "string"
}
```

-----

## Middleware Overview

This API incorporates custom middleware components configured in `Program.cs` to handle cross-cutting concerns.

### Logging Middleware

  * **Purpose:** Logs details of incoming requests and outgoing responses for auditing and debugging.
  * **Details:** Captures HTTP method, request path, and final response status code.
  * **Location in Pipeline:** Configured last to capture the final status of the request/response.

### Error Handling Middleware

  * **Purpose:** Catches any unhandled exceptions in the API pipeline.
  * **Details:** Prevents the API from crashing and returns a consistent `500 Internal Server Error` with a generic JSON message (`{"error": "An unexpected error occurred. Please try again later."}`).
  * **Location in Pipeline:** Configured first to ensure it can catch exceptions from all subsequent middleware and controller actions.

## Project Structure

```
UserManagementAPI/
├── Controllers/
│   └── UsersController.cs       # Handles CRUD operations for users
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   ├── RequestLoggingMiddleware.cs
│   └── TokenValidationMiddleware.cs
├── Models/
│   └── User.cs                  # User data model with validation attributes
├── Program.cs                   # Application entry point, service configuration, and middleware pipeline setup
├── appsettings.json             # Application settings
├── UserManagementAPI.csproj     # Project file
└── README.md                    # This file
```

-----

## Testing

You can test the API using Postman, Insomnia, or directly through the Swagger UI.

**Key Test Scenarios:**

  * **Valid User Creation (POST):** Send a correct `User` JSON payload.
  * **Invalid User Creation (POST):** Send payloads with missing required fields, invalid email formats, or excessive string lengths. Expect `400 Bad Request`.
  * **Retrieve All Users (GET /api/users):** Verify the list of users.
  * **Retrieve Existing User (GET /api/users/{id}):** Verify correct user data is returned.
  * **Retrieve Non-Existent User (GET /api/users/999):** Expect `404 Not Found`.
  * **Update Existing User (PUT /api/users/{id}):** Send a valid `User` JSON payload.
  * **Update Non-Existent User (PUT /api/users/999):** Expect `404 Not Found`.
  * **Delete Existing User (DELETE /api/users/{id}):** Verify the user is removed.
  * **Delete Non-Existent User (DELETE /api/users/999):** Expect `404 Not Found`.
  * **Authentication (Bearer Token):**
      * **No Authorization Header:** Expect `401 Unauthorized`.
      * **Invalid Token:** Use `Bearer WRONG_TOKEN`. Expect `401 Unauthorized`.
      * **Valid Token:** Use `Bearer YOUR_SUPER_SECRET_TOKEN`. Expect `200 OK` or other successful responses.
  * **Error Handling:** Temporarily introduce `throw new Exception("Test error");` in a controller method and hit the endpoint to verify the `500 Internal Server Error` response from the middleware.

-----

## Contributing

Contributions are welcome\! Please feel free to open issues or submit pull requests.

-----

## License

This project is licensed under the MIT License - see the `LICENSE` file for details. (Note: You would need to create a `LICENSE` file if you plan to share this project.)

-----

Feel free to customize any section of this README to better reflect specific details or unique aspects of your implementation\!
