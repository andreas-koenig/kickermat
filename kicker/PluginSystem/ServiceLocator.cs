namespace PluginSystem
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class for registering and locating application-wide services.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// List with all callbacks which are called if a service is registered or updated.
        /// </summary>
        private static readonly List<KeyValuePair<Type, object>> Callbacks = new List<KeyValuePair<Type, object>>();

        /// <summary>
        /// Lock object for accessing the <see cref="Callbacks"/> list.
        /// </summary>
        private static readonly object CallbacksLock = new object();

        /// <summary>
        /// Dictionary which contains the registered services.
        /// </summary>
        private static readonly Dictionary<Type, object> ServiceDictionary = new Dictionary<Type, object>();

        /// <summary>
        /// Lock object for accessing the <see cref="ServiceDictionary"/>.
        /// </summary>
        private static readonly object ServiceDictionaryLock = new object();

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="service">The service.</param>
        public static void RegisterService<TService>(TService service)
            where TService : class
        {
            lock (ServiceDictionaryLock)
            {
                if (ServiceDictionary.ContainsKey(typeof(TService)))
                {
                    ServiceDictionary[typeof(TService)] = service;
                    return;
                }

                ServiceDictionary.Add(typeof(TService), service);
                lock (CallbacksLock)
                {
                    foreach (KeyValuePair<Type, object> callback in Callbacks)
                    {
                        if (callback.Key == typeof(TService))
                        {
                            ((Action<TService>)callback.Value)(service);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public static void UnRegisterService<TService>()
            where TService : class
        {
            if (ServiceDictionary.ContainsKey(typeof(TService)) == false)
            {
                return;
            }

            lock (ServiceDictionaryLock) 
            {
                ServiceDictionary.Remove(typeof(TService));
                lock (CallbacksLock)
                {
                    foreach (KeyValuePair<Type, object> callback in Callbacks)
                    {
                        if (callback.Key == typeof(TService))
                        {
                            ((Action<TService>)callback.Value)(null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Locates the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>The service if it is available, else null.</returns>
        public static TService LocateService<TService>()
            where TService : class
        {
            if (ServiceDictionary.ContainsKey(typeof(TService)))
            {
                return (TService)ServiceDictionary[typeof(TService)];
            }

            return null;
        }

        /// <summary>
        /// Registers the update callback.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="callback">The callback.</param>
        public static void RegisterUpdateCallback<TService>(Action<TService> callback)
        {
            lock (CallbacksLock)
            {
                Callbacks.Add(new KeyValuePair<Type, object>(typeof(TService), callback));
            }            
        }
    }
}