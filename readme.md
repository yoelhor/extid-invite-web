# Add guest users to your directory using MS Graph

You can invite anyone to collaborate with your organization by adding them to your directory as a guest user. This sample, craters and sends an invitation email that contains a redemption link. After the user is created in the directory, the user is added to a security group. 

This example is based on this [GitHub sample](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/1-WebApp-OIDC/1-1-MyOrg) which adds sign-in users to your Web App, leveraging the Microsoft identity platform. We use this sample to let the user sign-in **after** the registration is completed. So, you can test the invitation flow end-to-end. Since the app supports sign-ins, the app settings requires having two Azure App registrations:

1. Allowing users to sign-in, as described [here](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/1-WebApp-OIDC/1-1-MyOrg#step-1-register-the-sample-with-your-azure-ad-tenant).
1. Get and access token to send the MS Graph request, such as user invitation and add a user to security groups. For more information, see [Register an application with the Microsoft identity platform](https://docs.microsoft.com/graph/auth-register-app-v2) and [Get access without a user](https://docs.microsoft.com/en-us/graph/auth-v2-service)

## The code

The sample code has following three steps:

1. Initializes the auth provider using [OAuth 2.0 client credentials grant flow](https://docs.microsoft.com/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow). With the client credentials grant flow, the app is able to get an access token to call the Microsoft Graph API.
1. [Crate invite user](https://docs.microsoft.com/graph/api/invitation-post?view=graph-rest-1.0&tabs=http)
1. Add the invited [user to a security group](https://docs.microsoft.com/graph/api/group-post-members?view=graph-rest-1.0&tabs=http)

## User flow

1. On the home page type an email address and display name, and hit the **Send invite** button. Azure AD will send an invitation email.
1. The invited user has to click on the redemption link sent in the communication in the step above, and go through the interactive redemption process in a browser. Once completed, the invited user becomes an external user in the organization.
1. The user is redirected back to this sample app, to the `MicrosoftIdentity/Account/SignIn` endpoint (such as <http://localhost:5000/MicrosoftIdentity/Account/SignIn>)
1. The sign-in endpoint takes the user back to Azure AD to get an access/id token

## Contents

- [appsettings.json](appsettings.json) - All of the application's settings are contained in a file named appsettings. json. Any changes to the appsettings. json file will require restarting the app to take effect
- [Models/AppSettings.cs](Models/AppSettings.cs) - Application settings model presents the [appsettings.json](appsettings.json)
- [Startup.cs](Startup.cs) - The Startup class configures services and the app's request pipeline. In this example, we configure:
  - The [Microsoft.AspNetCore.Authentication.AzureAD.UI](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.AzureAD.UI/) that allows user to sign-in (after the registration is completed)
  - Load the app settings section and bind to AppSettings model object graph
  - Loads the invite service.  You can use one of the following [InviteSdkService](Services/InviteSdkService) invite class based on MS Graph SDK, or [InviteWithoutSdkService](Services/InviteWithoutSdkService) invite class based on MS Graph REST API calls. You can switch between them:
  
  ```csharp
  services.AddSingleton<IInviteService, InviteWithoutSdkService>();
  //services.AddSingleton<IInviteService, InviteSdkService>();
  ``` 

## Setup

Change the settings according to your enviromant:
- **TenantId** - Enter the domain of your tenant, e.g. contoso.onmicrosoft.com,
- **ClientId** - The application Id used for authentication. Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]
- **CallbackPath** - Such as `/signin-oidc`
- **MSGraphClientID** - The application id of the MS Graph. Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403
- **MSGraphClientSecret** -  The MS Graph application secret,
- **InviteRedirectUrl** - The redirect URI after the user completed the registration, for example: http://localhost:5000/MicrosoftIdentity/Account/SignIn
- **ExternalUsersGroupID** - Azure AD group id where the user will be added to

Run the application, and navigate to <http://localhost:5000/>
