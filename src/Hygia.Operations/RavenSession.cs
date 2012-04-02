namespace Hygia.Operations
{
    using System;
    using System.Collections.Generic;
    using Raven.Client;

    public class RavenSession
    {
         public static IDocumentSession OpenSession(string environmentId,IDocumentStore store)
         {
             string database = String.Empty;

             if (!String.IsNullOrEmpty(environmentId))
             {
                 var environment = Guid.Parse(environmentId);
                 if (!environmentIdToDatabaseLookup.ContainsKey(environment))
                 {
                     //try to reload the environments
                     environmentIdToDatabaseLookup = ReloadEnvironmentLookup(store);
                 }

                 database = environmentIdToDatabaseLookup[environment];
             }


             var session = String.IsNullOrEmpty(database) ? store.OpenSession() : store.OpenSession(database);

             session.Advanced.AllowNonAuthoritativeInformation = false;
             return session;
         }

         public static string EnvironmentToDatabaseLookup(string environmentId)
         {
             if(string.IsNullOrEmpty(environmentId))
                 return string.Empty;

             return environmentIdToDatabaseLookup[Guid.Parse(environmentId)];
         }
         public static IDictionary<Guid, string> ReloadEnvironmentLookup(IDocumentStore currentStore)
         {
             using (var session = currentStore.OpenSession())
             {
                 var lookup = session.Load<DatabaseMappings>("Watchr/DatabaseMappings");

                 if (lookup == null || lookup.Mappings == null)
                 {
                     throw new InvalidOperationException("No database mappings found");
                 }
                     
                 return lookup.Mappings;
             }
         }


        static IDictionary<Guid, string> environmentIdToDatabaseLookup = new Dictionary<Guid, string>();
    }
}