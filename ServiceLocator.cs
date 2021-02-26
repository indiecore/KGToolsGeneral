using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KGTools.General
{

	/// <summary>
	/// This attribute will indicate that the 
	/// </summary>
	[System.AttributeUsage(System.AttributeTargets.Class, Inherited = true)]
	public class ServiceAttribute : System.Attribute
	{
		
		private string serviceID = string.Empty;
		/// <summary>
		/// The service ID for this service attribute.
		/// </summary>
		public string ServiceID
		{
			get
			{
				return this.serviceID;
			}
		}

		#region Constructor

		/// <summary>
		/// Create a new service attribute for the 
		/// </summary>
		/// <param name="serviceID">The service for the </param>
		public ServiceAttribute(string serviceID)
		{
			this.serviceID = serviceID;
		}

		#endregion

	}


	/// <summary>
	/// Service locator is a class that acts as a collector for non-singleton setup classes we can reference via the class type.
	/// The service locator is attached to a unity object so that it is part of the unity hierarchy.
	/// Only one instance of a service can be registered with the service at a time.
	/// </summary>
	public class ServiceLocator : MonoBehaviour
	{

		#region Data

		private static ServiceLocator instance = null;
		/// <summary>
		/// The static instance of the service.
		/// </summary>
		public static ServiceLocator Instance
		{
			get
			{
				return instance;
			}
		}

		/// <summary>
		/// The dictionary maps the service ID to a specific object that contains the implementation of that object.
		/// </summary>
		private Dictionary<string, object> serviceIDToObject = null;

		/// <summary>
		/// This dictionary maps previously requested types to their Service IDs so that they can be validated without a lookup to their attributes.
		/// </summary>
		private Dictionary<System.Type, string> typeToServiceMap = null;

		/// <summary>
		/// Has this service locator been initialized yet?
		/// </summary>
		private bool initialized = false;

		#endregion

		#region MonoBehaviour

		/// <summary>
		/// Initialize this service locator as soon as the object is set up.
		/// </summary>
		private void Awake()
		{
			this.Initialize();
		}

		#endregion

		#region Class

		/// <summary>
		/// Initialize the service locator.
		/// </summary>
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}

			if (ServiceLocator.instance != null && ServiceLocator.instance != this)
			{
				GameObject.Destroy(this.gameObject);
			}
			else
			{
				ServiceLocator.instance = this;
				GameObject.DontDestroyOnLoad(this);
				this.HandleServiceLocatorSetup();
				this.initialized = true;
			}
		}

		#endregion

		#region Service Location Logic

		/// <summary>
		/// Setup this service locator.
		/// </summary>
		protected virtual void HandleServiceLocatorSetup()
		{
			this.serviceIDToObject = new Dictionary<string, object>();
			this.typeToServiceMap = new Dictionary<System.Type, string>();
		}

		/// <summary>
		/// Binds a service object to the service locator.
		/// </summary>
		/// <param name="serviceInstance">The service instance to bind to the service locator.</param>
		/// <param name="overrideExistingService">
		/// If True and there is an existing instance attached to this locator with the same service ID it will replace the existing service;
		/// otherwise trying to replace an existing service will throw an exception.
		/// </param>
		/// <typeparam name="T">The service type that is requested to bind.</typeparam>
		public static void Bind<T>(T serviceInstance, bool overrideExistingService = false) where T : class
		{
			if (serviceInstance == null)
			{
				throw new System.ArgumentNullException("serviceToBind");
			}

			string serviceKey = null;
			System.Type serviceType = serviceInstance.GetType();
			if (!TryGetServiceID(serviceType, out serviceKey))
			{
				throw new System.ArgumentException(string.Format("Type {0} is not a Service. Add KGTools.ServiceAttribute as an attribute to all service classes.", typeof(T)));
			}

			if (instance.serviceIDToObject.ContainsKey(serviceKey) && !overrideExistingService)
			{
				throw new System.InvalidOperationException(string.Format("Cannot add service of type {0} ServiceLocator already contains a service with the ID: {1}", typeof(T), serviceKey));
			}

			instance.typeToServiceMap.Add(serviceType, serviceKey);
			instance.serviceIDToObject.Add(serviceKey, serviceInstance);
		}

		/// <summary>
		/// Instantes a service object based on a prefab and then binds that object as a child of the service locator.
		/// </summary>
		/// <param name="prefab">The prefab to instantiate and then bind.</param>
		/// <param name="overrideExistingService">
		/// If True and there is an existing instance attached to this locator with the same service ID it will replace the existing service;
		/// otherwise trying to replace an existing service will throw an exception.
		/// </param>
		/// <typeparam name="T">The type of the prefab to bind.</typeparam>
		public static void BindPrefab<T>(T prefab, bool overrideExistingService = false) where T : MonoBehaviour
		{
			T objectInstance = GameObject.Instantiate<T>(prefab, instance.transform);
			ServiceLocator.Bind(objectInstance);
		}

		/// <summary>
		/// Gets a service instance from the service locator.
		/// </summary>
		/// <typeparam name="T">The service type to get the instance of.</typeparam>
		/// <returns>A service of the requested type; null if that service cannot be found.</returns>
		public static T Get<T>() where T : class
		{
			string serviceID = null;
			System.Type serviceType = typeof(T);

			T foundObject = null;

			if (!instance.typeToServiceMap.TryGetValue(serviceType, out serviceID))
			{
				if (!TryGetServiceID(serviceType, out serviceID))
				{
					throw new System.ArgumentException(string.Format("Type {0} is not a Service. Add KGTools.ServiceAttribute as an attribute to all service classes.", typeof(T)));
				}

				// Add the service key to the type to service map.
				instance.typeToServiceMap.Add(serviceType, serviceID);
			}

			// Now that we have a service ID we can check if the service ID is specified.
			if (instance.serviceIDToObject.ContainsKey(serviceID))
			{
				foundObject = instance.serviceIDToObject[serviceID] as T;
			}
			
			if (foundObject == null)
			{
				Debug.LogErrorFormat("ServiceLocator does not contain Service of type: " + serviceType);
			}
			
			return foundObject;
		}

		/// <summary>
		/// Check if the service locator contains a specific type of object.
		/// </summary>
		/// <typeparam name="T">The service type to check for.</typeparam>
		/// <returns>True if the service locator has a service of this type; False otherwise.</returns>
		public static bool Has<T>() where T : class
		{
			string serviceID = null;
			System.Type serviceType = typeof(T);
			if (!instance.typeToServiceMap.TryGetValue(serviceType, out serviceID))
			{
				if (!TryGetServiceID(typeof(T), out serviceID))
				{
					throw new System.ArgumentException(string.Format("Type {0} is not a Service. Add KGTools.ServiceAttribute as an attribute to all service classes.", typeof(T)));
				}

				// Add the service key to the 
				instance.typeToServiceMap.Add(serviceType, serviceID);
			}

			return instance.serviceIDToObject.ContainsKey(serviceID);
		}

		public static void Remove<T>() where T : class
		{
			
		}

		#endregion

		#region Helpers

		/// <summary>
		/// Gets the service ID for a specific type.
		/// </summary>
		/// <param name="serviceID">The service ID for the type.</param>
		/// <returns>True if the type is a service type and contains a service ID; False otherwise.</returns>
		private static bool TryGetServiceID(System.Type serviceType, out string serviceID)
		{
			System.Attribute[] attributes = System.Attribute.GetCustomAttributes(serviceType, true);
			string foundAttribute = string.Empty;
			for (int i = 0; i < attributes.Length; ++i)
			{
				if (attributes[i] is ServiceAttribute)
				{
					serviceID = ((ServiceAttribute)attributes[i]).ServiceID;
					return true;
				}
			}

			serviceID = string.Empty;
			return false;
		}

		#endregion

		#region Object

			/// <summary>
			/// Returns a <see cref="System.String"/> that represents the current <see cref="ServiceLocator"/>.
			/// </summary>
			/// <returns>A <see cref="System.String"/> that represents the current <see cref="ServiceLocator"/>.</returns>
			public override string ToString() {
				System.Text.StringBuilder info = new System.Text.StringBuilder();
				
				info.Append("ServiceLocator: ");
				info.Append(this.serviceIDToObject.Count.ToString());
				info.AppendLine(" Services");
				
				foreach (KeyValuePair<string, object> service in this.serviceIDToObject)
				{
					info.Append(service.Key);
					info.Append(" - ");
					info.AppendLine(service.Value.ToString());
				}

				info.AppendLine("Service Types");
				foreach (KeyValuePair<System.Type, string> service in this.typeToServiceMap) {
					info.Append("Type:" + service.Key);
					info.Append(" - ");
					info.AppendLine("Service ID: " + service.Value.ToString());
				}
				
				return info.ToString();
			}

			#endregion

	}
}
