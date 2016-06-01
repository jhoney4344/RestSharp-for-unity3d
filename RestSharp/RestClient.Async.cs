#region License
//   Copyright 2010 John Sheehan
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License. 
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Net;

namespace RestSharp
{
	public partial class RestClient
	{
		/// <summary>
		/// Executes the request and callback asynchronously, authenticating if needed
		/// </summary>
		/// <param name="request">Request to be executed</param>
		/// <param name="callback">Callback function to be executed upon completion providing access to the async handle.</param>
		public virtual RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{

				string method = Enum.GetName(typeof (Method), request.Method);
				switch (request.Method)
				{
						case Method.PATCH:
						case Method.POST:
						case Method.PUT:
							return ExecuteAsync(request, callback, method, DoAsPostAsync);
						default:
							return ExecuteAsync(request, callback, method, DoAsGetAsync);
				}
		}

		/// <summary>
		/// Executes a GET-style request and callback asynchronously, authenticating if needed
		/// </summary>
		/// <param name="request">Request to be executed</param>
		/// <param name="callback">Callback function to be executed upon completion providing access to the async handle.</param>
		/// <param name="httpMethod">The HTTP method to execute</param>
		public virtual RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
		{
			return ExecuteAsync(request, callback, httpMethod, DoAsGetAsync);
		}

		/// <summary>
		/// Executes a POST-style request and callback asynchronously, authenticating if needed
		/// </summary>
		/// <param name="request">Request to be executed</param>
		/// <param name="callback">Callback function to be executed upon completion providing access to the async handle.</param>
		/// <param name="httpMethod">The HTTP method to execute</param>
		public virtual RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
		{
			request.Method = Method.POST;  // Required by RestClient.BuildUri... 
			return ExecuteAsync(request, callback, httpMethod, DoAsPostAsync);
		}

		private RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod, Func<IHttp, Action<HttpResponse>, string, HttpWebRequest> getWebRequest)
		{
			var http = HttpFactory.Create();
			AuthenticateIfNeeded(this, request);

			ConfigureHttp(request, http);

			var asyncHandle = new RestRequestAsyncHandle();

			Action<HttpResponse> response_cb = r => ProcessResponse(request, r, asyncHandle, callback);

			if (UseSynchronizationContext && SynchronizationContext.Current != null)
			{
				var ctx = SynchronizationContext.Current;
				var cb = response_cb;

				response_cb = resp => ctx.Post(s => cb(resp), null);
			}

			asyncHandle.WebRequest = getWebRequest(http, response_cb, httpMethod);
			return asyncHandle;
		}

		private static HttpWebRequest DoAsGetAsync(IHttp http, Action<HttpResponse> response_cb, string method)
		{
			return http.AsGetAsync(response_cb, method);
		}

		private static HttpWebRequest DoAsPostAsync(IHttp http, Action<HttpResponse> response_cb, string method)
		{
			return http.AsPostAsync(response_cb, method);
		}

		private void ProcessResponse(IRestRequest request, HttpResponse httpResponse, RestRequestAsyncHandle asyncHandle, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			var restResponse = ConvertToRestResponse(request, httpResponse);
			callback(restResponse, asyncHandle);
		}
	}
}