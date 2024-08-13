# Backend.Api.BackendApi

All URIs are relative to *http://localhost:6420*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ModulesLobbiesScriptsCreateCallPost**](BackendApi.md#moduleslobbiesscriptscreatecallpost) | **POST** /modules/lobbies/scripts/create/call |  |
| [**ModulesLobbiesScriptsDestroyCallPost**](BackendApi.md#moduleslobbiesscriptsdestroycallpost) | **POST** /modules/lobbies/scripts/destroy/call |  |
| [**ModulesLobbiesScriptsFindCallPost**](BackendApi.md#moduleslobbiesscriptsfindcallpost) | **POST** /modules/lobbies/scripts/find/call |  |
| [**ModulesLobbiesScriptsFindOrCreateCallPost**](BackendApi.md#moduleslobbiesscriptsfindorcreatecallpost) | **POST** /modules/lobbies/scripts/find_or_create/call |  |
| [**ModulesLobbiesScriptsJoinCallPost**](BackendApi.md#moduleslobbiesscriptsjoincallpost) | **POST** /modules/lobbies/scripts/join/call |  |
| [**ModulesLobbiesScriptsListCallPost**](BackendApi.md#moduleslobbiesscriptslistcallpost) | **POST** /modules/lobbies/scripts/list/call |  |
| [**ModulesLobbiesScriptsSetLobbyReadyCallPost**](BackendApi.md#moduleslobbiesscriptssetlobbyreadycallpost) | **POST** /modules/lobbies/scripts/set_lobby_ready/call |  |
| [**ModulesLobbiesScriptsSetPlayerConnectedCallPost**](BackendApi.md#moduleslobbiesscriptssetplayerconnectedcallpost) | **POST** /modules/lobbies/scripts/set_player_connected/call |  |
| [**ModulesLobbiesScriptsSetPlayerDisconnectedCallPost**](BackendApi.md#moduleslobbiesscriptssetplayerdisconnectedcallpost) | **POST** /modules/lobbies/scripts/set_player_disconnected/call |  |
| [**ModulesUsersScriptsAuthenticateTokenCallPost**](BackendApi.md#modulesusersscriptsauthenticatetokencallpost) | **POST** /modules/users/scripts/authenticate_token/call |  |
| [**ModulesUsersScriptsFetchCallPost**](BackendApi.md#modulesusersscriptsfetchcallpost) | **POST** /modules/users/scripts/fetch/call |  |

<a id="moduleslobbiesscriptscreatecallpost"></a>
# **ModulesLobbiesScriptsCreateCallPost**
> LobbiesCreateResponse ModulesLobbiesScriptsCreateCallPost (Object body)



Call lobbies.create script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsCreateCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                LobbiesCreateResponse result = apiInstance.ModulesLobbiesScriptsCreateCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsCreateCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsCreateCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<LobbiesCreateResponse> response = apiInstance.ModulesLobbiesScriptsCreateCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsCreateCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**LobbiesCreateResponse**](LobbiesCreateResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptsdestroycallpost"></a>
# **ModulesLobbiesScriptsDestroyCallPost**
> Object ModulesLobbiesScriptsDestroyCallPost (Object body)



Call lobbies.destroy script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsDestroyCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                Object result = apiInstance.ModulesLobbiesScriptsDestroyCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsDestroyCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsDestroyCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Object> response = apiInstance.ModulesLobbiesScriptsDestroyCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsDestroyCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptsfindcallpost"></a>
# **ModulesLobbiesScriptsFindCallPost**
> LobbiesFindResponse ModulesLobbiesScriptsFindCallPost (Object body)



Call lobbies.find script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsFindCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                LobbiesFindResponse result = apiInstance.ModulesLobbiesScriptsFindCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsFindCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsFindCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<LobbiesFindResponse> response = apiInstance.ModulesLobbiesScriptsFindCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsFindCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**LobbiesFindResponse**](LobbiesFindResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptsfindorcreatecallpost"></a>
# **ModulesLobbiesScriptsFindOrCreateCallPost**
> LobbiesFindOrCreateResponse ModulesLobbiesScriptsFindOrCreateCallPost (Object body)



Call lobbies.find_or_create script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsFindOrCreateCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                LobbiesFindOrCreateResponse result = apiInstance.ModulesLobbiesScriptsFindOrCreateCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsFindOrCreateCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsFindOrCreateCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<LobbiesFindOrCreateResponse> response = apiInstance.ModulesLobbiesScriptsFindOrCreateCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsFindOrCreateCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**LobbiesFindOrCreateResponse**](LobbiesFindOrCreateResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptsjoincallpost"></a>
# **ModulesLobbiesScriptsJoinCallPost**
> LobbiesJoinResponse ModulesLobbiesScriptsJoinCallPost (Object body)



Call lobbies.join script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsJoinCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                LobbiesJoinResponse result = apiInstance.ModulesLobbiesScriptsJoinCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsJoinCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsJoinCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<LobbiesJoinResponse> response = apiInstance.ModulesLobbiesScriptsJoinCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsJoinCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**LobbiesJoinResponse**](LobbiesJoinResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptslistcallpost"></a>
# **ModulesLobbiesScriptsListCallPost**
> LobbiesListResponse ModulesLobbiesScriptsListCallPost (Object body)



Call lobbies.list script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsListCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                LobbiesListResponse result = apiInstance.ModulesLobbiesScriptsListCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsListCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsListCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<LobbiesListResponse> response = apiInstance.ModulesLobbiesScriptsListCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsListCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**LobbiesListResponse**](LobbiesListResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptssetlobbyreadycallpost"></a>
# **ModulesLobbiesScriptsSetLobbyReadyCallPost**
> Object ModulesLobbiesScriptsSetLobbyReadyCallPost (Object body)



Call lobbies.set_lobby_ready script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsSetLobbyReadyCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                Object result = apiInstance.ModulesLobbiesScriptsSetLobbyReadyCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetLobbyReadyCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsSetLobbyReadyCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Object> response = apiInstance.ModulesLobbiesScriptsSetLobbyReadyCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetLobbyReadyCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptssetplayerconnectedcallpost"></a>
# **ModulesLobbiesScriptsSetPlayerConnectedCallPost**
> Object ModulesLobbiesScriptsSetPlayerConnectedCallPost (Object body)



Call lobbies.set_player_connected script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsSetPlayerConnectedCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                Object result = apiInstance.ModulesLobbiesScriptsSetPlayerConnectedCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetPlayerConnectedCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsSetPlayerConnectedCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Object> response = apiInstance.ModulesLobbiesScriptsSetPlayerConnectedCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetPlayerConnectedCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="moduleslobbiesscriptssetplayerdisconnectedcallpost"></a>
# **ModulesLobbiesScriptsSetPlayerDisconnectedCallPost**
> Object ModulesLobbiesScriptsSetPlayerDisconnectedCallPost (Object body)



Call lobbies.set_player_disconnected script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesLobbiesScriptsSetPlayerDisconnectedCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                Object result = apiInstance.ModulesLobbiesScriptsSetPlayerDisconnectedCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetPlayerDisconnectedCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesLobbiesScriptsSetPlayerDisconnectedCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<Object> response = apiInstance.ModulesLobbiesScriptsSetPlayerDisconnectedCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesLobbiesScriptsSetPlayerDisconnectedCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

**Object**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="modulesusersscriptsauthenticatetokencallpost"></a>
# **ModulesUsersScriptsAuthenticateTokenCallPost**
> UsersAuthenticateTokenResponse ModulesUsersScriptsAuthenticateTokenCallPost (Object body)



Call users.authenticate_token script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesUsersScriptsAuthenticateTokenCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                UsersAuthenticateTokenResponse result = apiInstance.ModulesUsersScriptsAuthenticateTokenCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesUsersScriptsAuthenticateTokenCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesUsersScriptsAuthenticateTokenCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<UsersAuthenticateTokenResponse> response = apiInstance.ModulesUsersScriptsAuthenticateTokenCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesUsersScriptsAuthenticateTokenCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**UsersAuthenticateTokenResponse**](UsersAuthenticateTokenResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="modulesusersscriptsfetchcallpost"></a>
# **ModulesUsersScriptsFetchCallPost**
> UsersFetchResponse ModulesUsersScriptsFetchCallPost (Object body)



Call users.fetch script.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Backend.Api;
using Backend.Client;
using Backend.Model;

namespace Example
{
    public class ModulesUsersScriptsFetchCallPostExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:6420";
            var apiInstance = new BackendApi(config);
            var body = null;  // Object | 

            try
            {
                UsersFetchResponse result = apiInstance.ModulesUsersScriptsFetchCallPost(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BackendApi.ModulesUsersScriptsFetchCallPost: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModulesUsersScriptsFetchCallPostWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<UsersFetchResponse> response = apiInstance.ModulesUsersScriptsFetchCallPostWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BackendApi.ModulesUsersScriptsFetchCallPostWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Object** |  |  |

### Return type

[**UsersFetchResponse**](UsersFetchResponse.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

