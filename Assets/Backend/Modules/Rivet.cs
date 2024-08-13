// This file is auto-generated by the Open Game Backend (https://opengb.dev) build system.
// 
// Do not edit this file directly.
//
// Generated at 2024-07-29T07:22:49.267Z

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Backend.Client;
using Backend.Model;

namespace Backend.Modules
{
	/// <summary>
	/// Represents a collection of functions to interact with the API endpoints
	/// </summary>
	public partial class RivetApi : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RivetApi"/> class.
		/// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
		/// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
		/// </summary>
		/// <returns></returns>
		public RivetApi() : this((string)null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RivetApi"/> class.
		/// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
		/// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
		/// </summary>
		/// <param name="basePath">The target service's base path in URL format.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <returns></returns>
		public RivetApi(string basePath)
		{
			this.Configuration = Backend.Client.Configuration.MergeConfigurations(
				Backend.Client.GlobalConfiguration.Instance,
				new Backend.Client.Configuration { BasePath = basePath }
			);
			this.ApiClient = new Backend.Client.ApiClient(this.Configuration.BasePath);
			this.AsynchronousClient = this.ApiClient;
			this.ExceptionFactory = Backend.Client.Configuration.DefaultExceptionFactory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RivetApi"/> class using Configuration object.
		/// **IMPORTANT** This will also create an instance of HttpClient, which is less than ideal.
		/// It's better to reuse the <see href="https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests#issues-with-the-original-httpclient-class-available-in-net">HttpClient and HttpClientHandler</see>.
		/// </summary>
		/// <param name="configuration">An instance of Configuration.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns></returns>
		public RivetApi(Backend.Client.Configuration configuration)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");

			this.Configuration = Backend.Client.Configuration.MergeConfigurations(
				Backend.Client.GlobalConfiguration.Instance,
				configuration
			);
			this.ApiClient = new Backend.Client.ApiClient(this.Configuration.BasePath);
			this.AsynchronousClient = this.ApiClient;
			ExceptionFactory = Backend.Client.Configuration.DefaultExceptionFactory;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RivetApi"/> class
		/// using a Configuration object and client instance.
		/// </summary>
		/// <param name="asyncClient">The client interface for asynchronous API access.</param>
		/// <param name="configuration">The configuration object.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public RivetApi(Backend.Client.IAsynchronousClient asyncClient, Backend.Client.IReadableConfiguration configuration)
		{
			if (asyncClient == null) throw new ArgumentNullException("asyncClient");
			if (configuration == null) throw new ArgumentNullException("configuration");

			this.AsynchronousClient = asyncClient;
			this.Configuration = configuration;
			this.ExceptionFactory = Backend.Client.Configuration.DefaultExceptionFactory;
		}

		/// <summary>
		/// Disposes resources if they were created by us
		/// </summary>
		public void Dispose()
		{
			this.ApiClient?.Dispose();
		}

		/// <summary>
		/// Holds the ApiClient if created
		/// </summary>
		public Backend.Client.ApiClient ApiClient { get; set; } = null;

		/// <summary>
		/// The client for accessing this underlying API asynchronously.
		/// </summary>
		public Backend.Client.IAsynchronousClient AsynchronousClient { get; set; }

		/// <summary>
		/// Gets the base path of the API client.
		/// </summary>
		/// <value>The base path</value>
		public string GetBasePath()
		{
			return this.Configuration.BasePath;
		}

		/// <summary>
		/// Gets or sets the configuration object
		/// </summary>
		/// <value>An instance of the Configuration</value>
		public Backend.Client.IReadableConfiguration Configuration { get; set; }

		private Backend.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

		/// <summary>
		/// Provides a factory method hook for the creation of exceptions.
		/// </summary>
		public Backend.Client.ExceptionFactory ExceptionFactory
		{
			get
			{
				if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
				{
					throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
				}
				return _exceptionFactory;
			}
			set { _exceptionFactory = value; }
		}

/// <summary>
		///  Call rivet.call script.
		/// </summary>
		/// <exception cref="Backend.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="body"> (optional)</param>
		/// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
		/// <returns>Task of Backend.Model.Rivet.CallResponse</returns>
		public async System.Threading.Tasks.Task<Backend.Model.Rivet.CallResponse> Call(Backend.Model.Rivet.CallRequest rivetCallRequest = default(Backend.Model.Rivet.CallRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			var task = CallWithHttpInfo(rivetCallRequest, cancellationToken);
#if UNITY_EDITOR || !UNITY_WEBGL
			Backend.Client.ApiResponse<Backend.Model.Rivet.CallResponse> localVarResponse = await task.ConfigureAwait(false);
#else
			Backend.Client.ApiResponse<Backend.Model.Rivet.CallResponse> localVarResponse = await task;
#endif
			return localVarResponse.Data;
		}

		/// <summary>
		///  Call rivet.call script.
		/// </summary>
		/// <exception cref="Backend.Client.ApiException">Thrown when fails to make API call</exception>
		/// <param name="body"> (optional)</param>
		/// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
		/// <returns>Task of ApiResponse (Backend.Model.Rivet.CallResponse)</returns>
		public async System.Threading.Tasks.Task<Backend.Client.ApiResponse<Backend.Model.Rivet.CallResponse>> CallWithHttpInfo(Backend.Model.Rivet.CallRequest rivetCallRequest = default(Backend.Model.Rivet.CallRequest), System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{

			Backend.Client.RequestOptions localVarRequestOptions = new Backend.Client.RequestOptions();

			string[] _contentTypes = new string[] {
				"application/json"
			};

			// to determine the Accept header
			string[] _accepts = new string[] {
				"application/json"
			};


			var localVarContentType = Backend.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
			if (localVarContentType != null) localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);

			var localVarAccept = Backend.Client.ClientUtils.SelectHeaderAccept(_accepts);
			if (localVarAccept != null) localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);

			localVarRequestOptions.Data = rivetCallRequest;


			// make the HTTP request

			var task = this.AsynchronousClient.PostAsync<Backend.Model.Rivet.CallResponse>("/modules/rivet/scripts/call/call", localVarRequestOptions, this.Configuration, cancellationToken);

#if UNITY_EDITOR || !UNITY_WEBGL
			var localVarResponse = await task.ConfigureAwait(false);
#else
			var localVarResponse = await task;
#endif

			if (this.ExceptionFactory != null)
			{
				Exception _exception = this.ExceptionFactory("Call", localVarResponse);
				if (_exception != null) throw _exception;
			}

			return localVarResponse;
		}


				}
			}
			



