<img width="1070" alt="Authsignal" src="https://raw.githubusercontent.com/authsignal/identity-server-example/main/.github/images/authsignal.png">

# Authsignal + IdentityServer Example

## Overview

[Duende IdentityServer](https://duendesoftware.com/products/identityserver) is an ASP.NET Core framework for building your own login server in compliance with OpenID Connect and OAuth 2.0 standards.

This example shows how to integrate IdentityServer with Authsignal:

1. Adding MFA to a traditional username & password login flow
2. Allowing passkey login as an secure and user-friendly alternative to username & password

Authsignal can be used to facilitate both MFA on login (step 1) and passwordless login (step 2).

For the full step-by-step guide check out our [docs](https://docs.authsignal.com/integrations/identityserver).

## Running the Application

### Prerequisites
- [Authsignal Account](https://portal.authsignal.com/)
- [.NET SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Authsignal Dotnet SDK](https://github.com/authsignal/authsignal-dotnet)

### Configuration
   - Create an account at [Authsignal](https://portal.authsignal.com/) and grab your Tenant ID, API host and API Key Secret.
   - Add your `AuthsignalSecret` (API Key Secret) to both project's appsettings.json files:
     - src/IdentityServer/appsettings.json
     - src/WebClient/appsettings.json

### Running the Application
1. Clone the repository
   ```
   git clone https://github.com/authsignal/identity-server-example.git
   cd identity-server-example
   ```

2. Start the IdentityServer (authentication server)
   ```
   cd src/IdentityServer
   dotnet run
   ```
   IdentityServer will run on https://localhost:5001

3. In a new terminal, start the WebClient (client application)
   ```
   cd src/WebClient
   dotnet run
   ```
   WebClient will run on https://localhost:5002

4. Open your browser and navigate to https://localhost:5002
   - You will be redirected to the IdentityServer login page
   - Log in with one of the test accounts:
     - Username: `alice`, Password: `alice`
     - Username: `bob`, Password: `bob`

5. After logging in, you'll be presented with MFA options if configured in your Authsignal account

### Troubleshooting
- In development mode, the application is configured to accept untrusted SSL certificates
- If you encounter SSL certificate errors, you might need to trust the development certificates by running:
  ```
  dotnet dev-certs https --trust
  ```

For more detailed integration information, refer to our [official documentation](https://docs.authsignal.com/integrations/identityserver).