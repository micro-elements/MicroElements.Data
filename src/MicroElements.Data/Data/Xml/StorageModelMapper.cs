// Copyright (c) MicroElements. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MicroElements.Data.Xml
{
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;
    using MicroElements.Data.Content;
    using MicroElements.Data.Xml.StorageModel;

    /// <summary>
    /// Mapper from <see cref="IDataContainer"/> to <see cref="DataContainerStorageModel"/> and back.
    /// </summary>
    public class StorageModelMapper
    {
        /// <summary>
        /// Singleton instance of mapper.
        /// </summary>
        public static readonly StorageModelMapper Instance = new StorageModelMapper();

        private readonly IMapper _mapper;

        private class DomainToStorageModel : Profile
        {
            public DomainToStorageModel()
            {
                CreateMap<IDataContainer, DataContainerStorageModel>()
                    .ForMember(model => model.Headers, expression => expression.MapFrom(container => container.Headers));
                CreateMap<IDataAttributes, DataAttributesStorageModel>()
                    .ForMember(model => model.DateCreated, expression => expression.MapFrom(content => content.DateCreated.TrimToSeconds()));
                CreateMap<IDataContent, DataContentStorageModel>()
                    .ForMember(model => model.Encoding, expression => expression.MapFrom(content => content.ContentEncoding.WebName))
                    .ForMember(model => model.Text, expression => expression.MapFrom(content => content.GetContentText()));
                CreateMap<Header, HeaderStorageModel>();
            }
        }

        private class StorageModelToDomain : Profile
        {
            public StorageModelToDomain()
            {
                CreateMap<HeaderStorageModel, Header>();
                CreateMap<DataAttributesStorageModel, DataAttributes>()
                    .ConstructUsing((model, context) => new DataAttributes(model.Id, new FormatName(model.FormatName), model.DateCreated));
                CreateMap<DataContentStorageModel, TextContent>()
                    .ConstructUsing(model => new TextContent(model.Text, Encoding.GetEncoding(model.Encoding)));
                CreateMap<DataContainerStorageModel, IDataContainer>()
                    .ConstructUsing((model, context) =>
                        new DataContainer(
                            context.Mapper.Map<DataAttributesStorageModel, DataAttributes>(model.Attributes),
                            context.Mapper.Map<DataContentStorageModel, TextContent>(model.Content),
                            new Headers(context.Mapper.Map<HeaderStorageModel[], IEnumerable<Header>>(model.Headers)),
                            new Properties()));
            }
        }

        private StorageModelMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainToStorageModel>();
                cfg.AddProfile<StorageModelToDomain>();
            });
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public DataContainerStorageModel ToStorageModel(IDataContainer dataContainer)
        {
            return _mapper.Map<IDataContainer, DataContainerStorageModel>(dataContainer);
        }

        public IDataContainer FromStorageModel(DataContainerStorageModel storageModel)
        {
            return _mapper.Map<DataContainerStorageModel, IDataContainer>(storageModel);
        }
    }
}
