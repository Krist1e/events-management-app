# Events Management App
This project is a web application designed to manage events, developed using .NET Core and Entity Framework (EF) Core. The application includes a comprehensive Web API for handling various operations related to events and participants.

## How to launch project
### Prerequisites
Docker installed on your machine. If not, please refer to the [official Docker installation guide](https://docs.docker.com/get-docker/).

### Running the Application

1. Open a terminal window.
2. Navigate to the project's root directory.
3. Execute the following command to build and start the services:
   
    ```sh
    docker-compose up --build
    ```

5. Wait for the console output to indicate that all services have started successfully.


### Web API Functionalities

For detailed API documentation open [this file](https://github.com/Krist1e/events-management-app/blob/master/EventsManagementApp.postman_collection.json).

Or refer to Swagger documentation: http://localhost:8080/swagger

#### Event Management

1. **Retrieve All Events**: Get a list of all events.
2. **Retrieve Event by ID**: Get a specific event by its ID.
3. **Add New Event**: Create a new event.
4. **Update Event**: Modify the details of an existing event.
5. **Delete Event**: Remove an event from the system.
6. **Filter Events**: Get a list of events based on specific criteria (date, location, category).
7. **Image Handling**: Add and remove images associated with events.

#### User Management

1. **Register User in Event**: Allow users to register for an event.
2. **Retrieve User by ID**: Get details of a specific user by their ID.
3. **Retrieve Events in which User participates**: Get details about events in which user is participating.
4. **Cancel Registration for Event**: Allow users to cancel their registration for an event.

### Web API Requirements

1. **Policy-Based Authorization**: Implement authorization using refresh and JWT access tokens.
2. **Repository and Unit of Work Patterns**: Ensure proper architecture and separation of concerns using these patterns.
3. **Global Exception Handling Middleware**: Develop middleware to handle exceptions globally across the application.
4. **Pagination**: Implement pagination for endpoints that return lists.
5. **Unit Testing**: Ensure services are covered by unit tests.
