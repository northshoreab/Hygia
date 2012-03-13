namespace Hygia.FaultManagement
{
    using System;
    using System.Linq;
    using Raven.Client;
    using Raven.Client.Document;

    public static class HiLoGenerator
    {
        static MultiTypeHiLoKeyGenerator generator;

        static MultiTypeHiLoKeyGenerator GetGenerator(IDocumentSession session)
        {
            if(generator == null)
                generator = new MultiTypeHiLoKeyGenerator(session.Advanced.DocumentStore, 32);

            return generator;
        }
        public static long GenerateNumber<T>(IDocumentSession session)
        {
            var key = GetGenerator(session)
                .GenerateDocumentKey(session.Advanced.DocumentStore.Conventions,Activator.CreateInstance<T>());

            return long.Parse(key.Split('/').Last());
        }
    }
}