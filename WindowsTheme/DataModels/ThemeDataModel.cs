using Artemis.Core.DataModelExpansions;
using System.Collections.Generic;

namespace WindowsTheme.DataModels
{
    public class ThemeDataModel : DataModel
    {
        public ThemeDataModel()
        {
            
        }

        public enum Theme
        {
            Dark = 0,
            Light = 1
        }

        // Your datamodel can have regular properties and you can annotate them if you'd like
        // [DataModelProperty(Name = "A test string", Description = "It doesn't do much, but it's there.")]
        // public string TemplateDataModelString { get; set; }

        [DataModelProperty(Name = "System Theme", Description = "The current system theme.")]
        public Theme SystemTheme { get; set; }

        [DataModelProperty(Name = "App Theme", Description = "The current apps theme.")]
        public Theme AppTheme { get; set; }

    }
}