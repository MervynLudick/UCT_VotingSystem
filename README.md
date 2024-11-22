
# Application Architecture

This document describes the architecture of the application.

```
+-----------------+       +-------+       +--------------------------+
|     Users       |<----->| Auth0 |<----->|        Backend           |
+-----------------+       +-------+       +--------------------------+
                                             |      |       |       |
                                             |      |       |       |
                                             v      v       v       v
                                         +------+ +------+ +------+ +--------------+
                                         | MVC  | | Core | | Data | | Infrastructure |
                                         +------+ +------+ +------+ +--------------+
                                                               |             |
                                  v----------------------------              v
                       +-----------------------+              +----------------------------------+
                       |       Firestore       |              | External Email Validation Service |
                       +-----------------------+              +----------------------------------+
```

## Components Explained

### 1. Users
- Represent application users accessing the system through a browser or API.

### 2. Auth0
- Handles user authentication and issues access tokens.

### 3. Backend
- The central component of the system.
- Comprised of:
  - **MVC**: Handles web requests and serves views.
  - **Core**: 
    - Implements the business logic of the application.
    - Contains interfaces to invert dependencies, ensuring **Core** depends on nothing else.
  - **Data**: 
    - Interfaces with Firestore for database operations.
    - Implements repository patterns based on the interfaces defined in **Core**.
  - **Infrastructure**:
    - Manages cross-cutting concerns like logging, caching, and external services.
    - Communicates with the **External Email Validation Service**.

### 4. Firestore
- A NoSQL database for storing and retrieving application data.
- Accessed via the **Data** layer.

### 5. External Email Validation Service
- Used to validate email addresses.
- Accessed via the **Infrastructure** layer.

## Dependency Inversion Principle in Core
- The **Core** layer depends on nothing.
- It defines interfaces that are implemented by other layers (e.g., **Data**, **Infrastructure**).
- This design inverts the dependencies, allowing **Core** to remain isolated and focused solely on business logic.
- Examples of interfaces include:
  - Repositories for database access (implemented in **Data**).
  - External services like email validation (implemented in **Infrastructure**).

## Flow
1. **Users** authenticate via **Auth0**.
2. **Auth0** communicates with the **Backend** to provide secure access.
3. The **Backend** interacts with:
   - **Firestore** via the **Data** layer.
   - **External Email Validation Service** via the **Infrastructure** layer.
