namespace MicroElements.Data.Filters
{
    public class JsonFilter : IDataFilter
    {
        /// <inheritdoc />
        public bool Matches(IDataContainer data)
        {
            string contentText = data.Content.GetContentText();
            return false;
        }
    }
}
