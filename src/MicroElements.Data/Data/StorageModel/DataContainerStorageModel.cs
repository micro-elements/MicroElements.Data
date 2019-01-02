namespace MicroElements.Data.StorageModel
{
    using System;
    using System.Xml.Serialization;
    using AutoMapper;

    [XmlRoot(ElementName = "DataContainer")]
    public class DataContainerStorageModel
    {
        /// <summary>
        /// Common data attributes.
        /// </summary>
        public DataAttributesStorageModel Attributes { get; set; }

        /// <summary>
        /// Data content.
        /// </summary>
        public DataContentStorageModel Content { get; set; }

        /// <summary>
        /// Data headers.
        /// </summary>
        IHeaders Headers { get; set; }

        /// <summary>
        /// Properties.
        /// </summary>
        IProperties Properties { get; set; }
    }

    /// <summary>
    /// StorageModel for <see cref="IDataAttributes"/>.
    /// </summary>
    public class DataAttributesStorageModel
    {
        public DateTime DateCreated { get; set; }
        public string Id { get; set; }
        public string FormatName { get; set; }
    }

    public class DataContentStorageModel
    {
        public string Encoding { get; set; } = "utf-8";
        public string Text { get; set; }
    }

    public class StorageModelMapper
    {
        public static StorageModelMapper Instance = new StorageModelMapper();

        private readonly IMapper _mapper;

        internal class StorageModelProfile : Profile
        {
            public StorageModelProfile()
            {
                CreateMap<IDataContainer, DataContainerStorageModel>();
                CreateMap<IDataAttributes, DataAttributesStorageModel>()
                    .ForMember(model => model.DateCreated, expression => expression.MapFrom(content => content.DateCreated.TrimToSeconds()));

                CreateMap<IDataContent, DataContentStorageModel>()
                    .ForMember(model => model.Encoding, expression => expression.MapFrom(content => content.ContentEncoding.WebName))
                    .ForMember(model => model.Text, expression => expression.MapFrom(content => content.GetContentText()))
                    ;
            }
        }

        StorageModelMapper()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<StorageModelProfile>(); });
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        public DataContainerStorageModel ToStorageMode(IDataContainer dataContainer)
        {
            return _mapper.Map<IDataContainer, DataContainerStorageModel>(dataContainer);
        }

        public IDataContainer FromStorageMode(DataContainerStorageModel storageModel)
        {
            return _mapper.Map<DataContainerStorageModel, IDataContainer>(storageModel);
        }
    }

    internal static class ConvertUtils
    {
        public static DateTime TrimToSeconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }


}
