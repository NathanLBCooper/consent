﻿using System;
using Consent.Domain.Core;

namespace Consent.Domain.Participants;

/**
 *  Allows segmentation of Participants, eg via language, location etc
 */

public class Tag : IEntity<TagId>
{
    public TagId? Id { get; init; }

    public string Name { get; private init; }
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(Name));
        }
    }

    public string Description { get; private init; }

    public Tag(string name, string description)
    {
        ValidateName(name);
        Name = name;

        Description = description;
    }
}

public readonly record struct TagId(int Value) : IIdentifier;
