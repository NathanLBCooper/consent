using System;
using System.Linq;
using System.Reflection;
using Consent.Domain.Core;
using Consent.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Consent.Storage;
internal static class TypeConverters
{
    public static void Configure(ModelConfigurationBuilder configurationBuilder)
    {
        AutoConfigureIdentifiers(configurationBuilder, typeof(UserId).Assembly);
    }

    private static void AutoConfigureIdentifiers(ModelConfigurationBuilder configurationBuilder, Assembly assembly)
    {
        var identifiersTypes = assembly
            .GetExportedTypes()
            .Where(t => !(t.IsInterface || t.IsAbstract || t.IsGenericTypeDefinition))
            .Where(typeof(IIdentifier).IsAssignableFrom)
            .Where(t => t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null);

        foreach (var type in identifiersTypes)
        {
            var identifierTypeConverter = typeof(IdentifierTypeConverter<>).MakeGenericType(type);

            _ = configurationBuilder.Properties(type).HaveConversion(identifierTypeConverter);
        }
    }
}

public class IdentifierTypeConverter<TIdentifier> : ValueConverter<TIdentifier, int> where TIdentifier : IIdentifier, new()
{
    public IdentifierTypeConverter() : base(v => v.Value, v => new TIdentifier { Value = v })
    {
    }
}
