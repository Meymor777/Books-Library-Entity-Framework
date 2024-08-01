using Microsoft.Extensions.Configuration;

namespace BooksLibrary.DB
{
    public class Filter
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public int? MoreThanPages { get; set; }
        public int? LessThanPages { get; set; }
        public DateTime? PublishedBefore { get; set; }
        public DateTime? PublishedAfter { get; set; }

        readonly private IConfiguration config;

        public Filter(IConfiguration config, string parentSection)
        {
            SetPropertiesByConfig(config, parentSection, "Title");
            SetPropertiesByConfig(config, parentSection, "Genre");
            SetPropertiesByConfig(config, parentSection, "Author");
            SetPropertiesByConfig(config, parentSection, "Publisher");
            SetPropertiesByConfig(config, parentSection, "MoreThanPages");
            SetPropertiesByConfig(config, parentSection, "LessThanPages");
            SetPropertiesByConfig(config, parentSection, "PublishedBefore");
            SetPropertiesByConfig(config, parentSection, "PublishedAfter");
        }
        private void SetPropertiesByConfig(IConfiguration config, string parentSection, string section)
        {
            string value = config.GetSection(parentSection).GetSection(section).Value;
            if (value != "")
            {
                Type typeFilter = this.GetType();
                if (typeFilter.GetProperty(section).PropertyType == typeof(int?))
                {
                    if (Int32.TryParse(config.GetSection(parentSection).GetSection(section).Value, out var intValue))
                    {
                        typeFilter.GetProperty(section).SetValue(this, intValue);
                    }
                }
                else if (typeFilter.GetProperty(section).PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(config.GetSection(parentSection).GetSection(section).Value, out var dateTimeValue))
                    {
                        typeFilter.GetProperty(section).SetValue(this, dateTimeValue);
                    }
                }
                else
                {
                    typeFilter.GetProperty(section).SetValue(this, value);
                }
            }
        }


    }
}
