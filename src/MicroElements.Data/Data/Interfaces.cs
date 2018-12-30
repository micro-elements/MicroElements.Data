// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data
{
    using System;
    using System.Collections.Generic;

    public interface IFormatSerializer
    {
        object Deserialize(DataContainer dataContainer);
    }

    public interface IFormatValidator
    {
        IEnumerable<string> Validate(DataContainer dataContainer);
    }

    public interface IFormatConverter
    {
        DataContainer Validate(DataContainer dataContainer);
    }

    public interface IFormatMatcher
    {
        bool Matches(DataContainer dataContainer);
    }

    public interface IModelMapper
    {
        object Map(object model, Type sourceType, Type targetType);
    }

    public interface IModelValidator
    {
        IEnumerable<string> Validate(object model);
    }

    public interface IDataFilter
    {
        bool Matches(MessageData data);
    }

    public interface IDataHandler
    {
        MessageData Process(MessageData data);
    }

}
