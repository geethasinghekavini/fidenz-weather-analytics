# Fidenz Weather Analytics

# Author
Kavini Geethasinghe
Developed as part of the Fidenz Technologies Trainee Software Engineer assessment.


## Project Overview

Fidenz Weather Analytics is a full-stack weather analytics application developed as part of the Fidenz Technologies Trainee Software Engineer assessment.

The application retrieves weather data for multiple cities from OpenWeatherMap, calculates a custom Comfort Index score on the backend, ranks the cities based on their comfort score, and displays the results through a responsive web interface.

The application also implements server-side caching and Auth0 authentication and authorization to protect the weather analytics dashboard and API.


## Features

* Reads city codes from `cities.json`
* Processes weather data for multiple cities
* Retrieves weather information from OpenWeatherMap
* Calculates a custom Comfort Index from 0 to 100
* Calculates the Comfort Index on the backend
* Ranks cities from most comfortable to least comfortable
* Displays city name, weather description, temperature, score and rank
* Implements server-side caching
* Caches weather API responses for 5 minutes
* Provides cache debugging functionality
* Auth0 authentication
* JWT-based API authorization
* Protected weather API
* Login and logout functionality
* MFA/email verification configuration through Auth0
* Restricted user signup
* Responsive desktop and mobile interface
* Frontend sorting and filtering
* Dark mode
* Weather data visualization

# Technology Stack

## Frontend

* React
* TypeScript
* Vite
* CSS
* Auth0 React SDK

## Backend

* C#
* ASP.NET Core Web API
* Dependency Injection
* HttpClient
* IMemoryCache
* JWT Bearer Authentication

## External Services

* OpenWeatherMap API
* Auth0

## Development Tools

* Visual Studio
* Git
* GitHub
* Postman
* Swagger

# Application Architecture

The application follows a client-server architecture.

The React frontend is responsible for the user interface, user interaction, authentication flow and displaying the data received from the backend.

The ASP.NET Core Web API is responsible for retrieving weather data, caching responses, calculating the Comfort Index and returning ranked results.

The overall flow is:

User
  |
  v
React Frontend
  |
  | HTTP Request + JWT
  v
ASP.NET Core Web API
  |
  +----> Cache
  |
  +----> OpenWeatherMap API
  |
  v
Weather Data
  |
  v
Comfort Index Calculation
  |
  v
City Ranking
  |
  v
React Frontend


The Comfort Index calculation is kept in a separate service so that the business logic is separated from the controller.


# Prerequisites

The following software is required to run the application:

* .NET SDK
* Node.js
* npm
* Git

The application also requires:

* An OpenWeatherMap API key
* An Auth0 application
* Auth0 configuration values


# Getting Started

## 1. Clone the Repository

Clone the repository from GitHub:
git clone <YOUR_GITHUB_REPOSITORY_URL>

Navigate to the project directory:
cd FidenzWeatherAnalytics



# Backend Setup

## 2. Configure Backend Settings

The backend requires the OpenWeatherMap API key and the required authentication configuration.

Sensitive values should be stored in local configuration or environment variables.

I donot commit API keys, client secrets or other sensitive credentials to GitHub.

## 3. Run the Backend

Navigate to the backend project:
cd WeatherAnalytics.Api

Restore the dependencies:
dotnet restore

Run the API:
dotnet run

The ASP.NET Core API will start using the configured local development URL.

Swagger can be used during development to inspect and test the API endpoints.


# Frontend Setup

## 4. Install Frontend Dependencies

Open another terminal and navigate to the frontend:
cd weather-analytics-ui

Install the required packages:
npm install

## 5. Configure Frontend Environment Variables

Create a local `.env` file inside the frontend project.

Example:env

VITE_AUTH0_DOMAIN=your-auth0-domain
VITE_AUTH0_CLIENT_ID=your-auth0-client-id
VITE_AUTH0_AUDIENCE=your-api-audience


Replace the placeholder values with the appropriate local configuration.

The real `.env` file must not be committed to GitHub.

An `.env.example` file can be included in the repository with placeholder values.

Example:

```env
VITE_AUTH0_DOMAIN=your-auth0-domain
VITE_AUTH0_CLIENT_ID=your-auth0-client-id
VITE_AUTH0_AUDIENCE=your-api-audience
```

## 6. Run the Frontend

Start the Vite development server:
npm run dev

The frontend normally runs at:
http://localhost:5173

# City Code Processing

The application uses `cities.json` as the source of city codes.

The backend reads the city codes and uses them to retrieve weather information from OpenWeatherMap.

The process is:

cities.json
    |
    v
Extract CityCode values
    |
    v
Request weather data
    |
    v
Process weather information
    |
    v
Calculate Comfort Index
    |
    v
Rank cities

The application processes at least 10 cities as required by the assignment.


# OpenWeatherMap Integration

Weather data is retrieved from OpenWeatherMap using the city ID.

The API request follows the OpenWeatherMap weather endpoint format:

GET /data/2.5/weather?id={cityId}&appid={API_KEY}

The application uses weather parameters including:

* Temperature
* Humidity
* Wind Speed
* Cloudiness
* Weather Description

The backend processes the response before returning the information to the frontend.

# Comfort Index

The application uses a custom Comfort Index to compare the comfort level of different cities.

The score is a numerical value between: 0 - 100

A higher score represents more comfortable weather conditions.

The Comfort Index is calculated using four weather parameters:

1. Temperature
2. Humidity
3. Wind Speed
4. Cloudiness

The calculation is implemented in the backend using `ComfortIndexService`.

## Temperature Score

I consider approximately 22 celcius to be the ideal temperature for the calculation.

The temperature score is calculated as:

Temperature Score = 100 - |Temperature - 22| x 5


The score is not allowed to become negative.

Therefore, if the calculated value is below zero, it is changed to zero.

## Humidity Score

I consider approximately 50% humidity to be the ideal humidity level.

The humidity score is calculated as:
Humidity Score = 100 - |Humidity - 50| × 2

A larger difference from the ideal humidity results in a lower score.

## Wind Score

I consider approximately 2 m/s to represent a comfortable light wind.

The wind score is calculated as:
Wind Score = 100 - |Wind Speed - 2| × 15

A larger difference from the preferred wind speed reduces the score.


## Cloudiness Score

I consider approximately 30% cloudiness to be a reasonable comfortable level.

The cloudiness score is calculated as:
Cloudiness Score = 100 - |Cloudiness - 30| × 1.2

The score decreases as the cloudiness moves further away from the preferred value.


# Comfort Index Weighting

The individual scores are combined using the following weights:

| Parameter   |   Weight |
| ----------- | -------: |
| Temperature |      40% |
| Humidity    |      30% |
| Wind Speed  |      15% |
| Cloudiness  |      15% |
| **Total**   | **100%** |

The final formula is:
Comfort Index = Temperature Score × 0.40 + Humidity Score × 0.30 + Wind Score × 0.15 + Cloudiness Score × 0.15

The final score is rounded to two decimal places and constrained between 0 and 100.

# Reasoning Behind the Weights

Temperature receives the highest weight of 40% because I consider it the most significant factor affecting general outdoor comfort.

Humidity receives 30% because humidity can strongly affect how comfortable a particular temperature feels.

Wind speed and cloudiness each receive 15% because I consider them secondary factors compared with temperature and humidity.

The total weights add up to 100%, making the resulting score easier to interpret.

The selected ideal values and weights are design assumptions for this application. They are intended to provide a simple and explainable way to compare cities rather than represent a scientifically validated human comfort standard.

# Comfort Index Trade-offs

I chose a simple distance-from-ideal approach because it is easy to understand, implement and modify.

One advantage of this approach is that each parameter has a clear relationship with the final score.

Another advantage is that the weights and ideal values can easily be changed if the requirements change.

However, the model has limitations because weather comfort is subjective and can depend on factors such as location, season, individual preference and other environmental conditions.

The Comfort Index is therefore an application-specific metric rather than a scientifically validated comfort model.

# Backend Calculation

The Comfort Index is calculated on the backend instead of the frontend.

The calculation is implemented in:
ComfortIndexService

This separates the calculation logic from the controller.

Keeping the calculation in the backend provides a single source of truth for the Comfort Index and prevents the frontend from being responsible for the business calculation.

The frontend receives the calculated score from the API and displays it.

# City Ranking

After the Comfort Index is calculated for each city, the cities are sorted by their score in descending order.


Highest Score
     |
     v
Most Comfortable
     |
     v
...
     |
     v
Least Comfortable


The city with the highest Comfort Index receives the highest ranking.

The ranking is calculated using the backend results.


# Server-Side Caching

The application implements server-side caching to reduce unnecessary requests to OpenWeatherMap.

Weather API responses are cached for 5 minutes.

The request flow is:
Weather Request
      |
      v
Check Cache
      |
   ┌──┴──┐
   |     |
  HIT   MISS
   |     |
   |     v
   |  OpenWeatherMap
   |     |
   |     v
   |  Store Response
   |     |
   └──┬──┘
      |
      v
Return Weather Data

## Why Caching Was Used

Without caching, every request could result in another request to OpenWeatherMap.

Caching helps to:

* Reduce external API requests
* Reduce response time for repeated requests
* Reduce unnecessary API usage
* Improve the application's efficiency

The raw weather response is cached for five minutes as required by the assignment

# Cache Implementation

The application uses ASP.NET Core `IMemoryCache`.

`IMemoryCache` stores cached values in the memory of the running backend application.

A cache HIT means the required data is already available in the cache.

A cache MISS means the required data is not available, so the application retrieves fresh data from OpenWeatherMap and stores it in the cache.

The application also includes cache debugging functionality to help verify cache HIT and MISS behaviour during development.


# Cache Trade-offs

`IMemoryCache` was selected because it is simple to implement and suitable for this assignment.

It avoids unnecessary external infrastructure and is sufficient for a single backend instance.

However, in a production environment with multiple backend instances, each instance would have its own memory cache.

For a distributed production system, a distributed caching solution such as Redis could be considered.

The five-minute cache duration is also a trade-off between data freshness and reducing external API requests.


# Authentication and Authorization

Auth0 is used to provide authentication and authorization.

Authentication determines whether a user is logged in.

Authorization determines whether the authenticated user is allowed to access protected resources.

The authentication flow is:
User
  |
  v
React Application
  |
  v
Auth0
  |
  v
Authenticated User
  |
  v
Access Token
  |
  v
React sends Bearer Token
  |
  v
ASP.NET Core API
  |
  v
JWT Validation
  |
  v
Protected API


# JWT Authentication

The frontend obtains an access token through Auth0.

When requesting a protected backend endpoint, the frontend sends the token using the HTTP Authorization header:

Authorization: Bearer <access-token>

The ASP.NET Core API validates the JWT before allowing access to the protected endpoint.

If the request does not contain a valid authentication token, the API returns:
401 Unauthorized

# Auth0 Configuration

The frontend is configured as an Auth0 Single Page Application.

The local development URL is:
http://localhost:5173

The Auth0 application is configured with the appropriate callback, logout and web-origin URLs for local development.

Example:
Allowed Callback URLs:
http://localhost:5173

Allowed Logout URLs:
http://localhost:5173

Allowed Web Origins:
http://localhost:5173

Public signup is restricted according to the assignment requirements.

MFA/email verification is configured through Auth0.

# Security Considerations

The application uses the following security measures:

* Auth0 for authentication
* JWT Bearer authentication for API authorization
* Protected backend endpoints
* Authorization header for API requests
* No authentication secrets in frontend source code
* Environment variables for local configuration
* `.env` excluded from Git
* Unauthorized requests return HTTP 401

Real API keys and secrets are not committed to the repository.

# Responsive Design

The frontend is designed to support both desktop and mobile screen sizes.

The layout adapts to different screen widths so that weather information, ranking information, controls and visualizations remain usable on smaller devices.


# Frontend Functionality

The frontend provides functionality for:
* Viewing weather information
* Viewing Comfort Index scores
* Viewing city rankings
* Searching/filtering city information
* Sorting weather results
* Viewing weather-related visualizations
* Switching between light and dark modes
* Logging in and logging out

The frontend does not calculate the Comfort Index. It displays the value calculated by the backend.

# Error Handling

The application handles API request failures and invalid requests so that the frontend can provide appropriate feedback to the user.

Authentication failures are handled through the Auth0 authentication flow.

Unauthorized requests to protected backend endpoints return HTTP 401.

# Testing

The application was tested for the main functional requirements.

## Authentication Testing

* Login
* Logout
* Authenticated API request
* Unauthenticated API request
* Unauthorized API request returns HTTP 401

## Weather Testing

* Weather API requests
* Multiple city processing
* Weather information display
* Invalid/error responses

## Comfort Index Testing

* Temperature calculation
* Humidity calculation
* Wind speed calculation
* Cloudiness calculation
* Final score range between 0 and 100
* City ranking

## Caching Testing

* Cache MISS when data is not cached
* Cache HIT when cached data is available
* Five-minute cache duration

## UI Testing

* Desktop layout
* Mobile layout
* Search/filter
* Sorting
* Dark mode
* Weather visualization

# Known Limitations

## Custom Comfort Index

The Comfort Index is a custom metric designed for this assignment and is not a scientifically validated human comfort standard.

## In-Memory Cache

`IMemoryCache` is local to a single backend instance.

A distributed caching solution would be more suitable for a production environment with multiple API instances.

## Weather Data Dependency

The application depends on OpenWeatherMap for weather information. If the external API is unavailable or returns an error, fresh weather data cannot be retrieved.

## Cached Data

Because weather data is cached for five minutes, the displayed information may not always represent the latest available weather conditions.

This is an intentional trade-off between data freshness and reducing external API requests.

## Local Development Configuration

The current application is configured for local development. A production deployment would require production-ready secret management, monitoring and infrastructure configuration.

# Future Improvements

Possible future improvements include:

* Distributed caching using Redis
* Automated unit and integration tests
* More advanced Comfort Index calculations
* Additional weather parameters
* Historical weather data
* More detailed weather analytics
* Improved error reporting
* Production deployment
* CI/CD pipeline
* Additional charts and visualizations

# Running the Application

The application requires both the backend and frontend to be running.

### Backend
cd WeatherAnalytics.Api
dotnet restore
dotnet run

### Frontend
cd weather-analytics-ui
npm install
npm run dev

Then open the frontend URL shown by Vite, normally:
http://localhost:5173

# Environment Variables

The repository must not contain real API keys or secrets.
For local development, create the required `.env` file using the appropriate values.
An example configuration can be provided through `.env.example`:

VITE_AUTH0_DOMAIN=your-auth0-domain
VITE_AUTH0_CLIENT_ID=your-auth0-client-id
VITE_AUTH0_AUDIENCE=your-auth0-audience


The actual `.env` file should remain local and should be included in `.gitignore`.

 
  

