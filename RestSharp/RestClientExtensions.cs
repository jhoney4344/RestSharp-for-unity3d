﻿using System;
using System.Linq;

namespace RestSharp
{
	public static partial class RestClientExtensions
	{
#if FRAMEWORK
		public static IRestResponse Get(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.GET;
			return client.Execute(request);
		}

		public static IRestResponse Post(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.POST;
			return client.Execute(request);
		}

		public static IRestResponse Put(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.PUT;
			return client.Execute(request);
		}

		public static IRestResponse Head(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.HEAD;
			return client.Execute(request);
		}

		public static IRestResponse Options(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.OPTIONS;
			return client.Execute(request);
		}

		public static IRestResponse Patch(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.PATCH;
			return client.Execute(request);
		}

        public static IRestResponse Delete(this IRestClient client, IRestRequest request)
        {
            request.Method = Method.DELETE;
            return client.Execute(request);
        }
#endif

		/// <summary>
		/// Add a parameter to use on every request made with this client instance
		/// </summary>
		/// <param name="restClient">The IRestClient instance</param>
		/// <param name="p">Parameter to add</param>
		/// <returns></returns>
		public static void AddDefaultParameter(this IRestClient restClient, Parameter p)
		{
			if (p.Type == ParameterType.RequestBody)
			{
				throw new NotSupportedException(
					"Cannot set request body from default headers. Use Request.AddBody() instead.");
			}

			restClient.DefaultParameters.Add(p);
		}

        /// <summary>
        /// Removes a parameter from the default parameters that are used on every request made with this client instance
        /// </summary>
        /// <param name="restClient">The IRestClient instance</param>
        /// <param name="name">The name of the parameter that needs to be removed</param>
        /// <returns></returns>
        public static void RemoveDefaultParameter(this IRestClient restClient, string name)
        {
            var parameter = restClient.DefaultParameters.SingleOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (parameter != null)
            {
                restClient.DefaultParameters.Remove(parameter);
            }
        }

		/// <summary>
		/// Adds a HTTP parameter (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
		/// Used on every request made by this client instance
		/// </summary>
		/// <param name="restClient">The IRestClient instance</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <returns>This request</returns>
		public static void AddDefaultParameter(this IRestClient restClient, string name, object value)
		{
			restClient.AddDefaultParameter(new Parameter { Name = name, Value = value, Type = ParameterType.GetOrPost });
		}

		/// <summary>
		/// Adds a parameter to the request. There are four types of parameters:
		///	- GetOrPost: Either a QueryString value or encoded form value based on method
		///	- HttpHeader: Adds the name/value pair to the HTTP request's Headers collection
		///	- UrlSegment: Inserted into URL if there is a matching url token e.g. {AccountId}
		///	- RequestBody: Used by AddBody() (not recommended to use directly)
		/// </summary>
		/// <param name="restClient">The IRestClient instance</param>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <param name="type">The type of parameter to add</param>
		/// <returns>This request</returns>
		public static void AddDefaultParameter(this IRestClient restClient, string name, object value, ParameterType type)
		{
			restClient.AddDefaultParameter(new Parameter { Name = name, Value = value, Type = type });
		}

		/// <summary>
		/// Shortcut to AddDefaultParameter(name, value, HttpHeader) overload
		/// </summary>
		/// <param name="restClient">The IRestClient instance</param>
		/// <param name="name">Name of the header to add</param>
		/// <param name="value">Value of the header to add</param>
		/// <returns></returns>
		public static void AddDefaultHeader(this IRestClient restClient, string name, string value)
		{
			restClient.AddDefaultParameter(name, value, ParameterType.HttpHeader);
		}

		/// <summary>
		/// Shortcut to AddDefaultParameter(name, value, UrlSegment) overload
		/// </summary>
		/// <param name="restClient">The IRestClient instance</param>
		/// <param name="name">Name of the segment to add</param>
		/// <param name="value">Value of the segment to add</param>
		/// <returns></returns>
		public static void AddDefaultUrlSegment(this IRestClient restClient, string name, string value)
		{
			restClient.AddDefaultParameter(name, value, ParameterType.UrlSegment);
		}
	}
}