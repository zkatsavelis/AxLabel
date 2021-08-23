using System.Collections.Generic;

namespace label
{
    public class Config
    {
        public string UserCulture { get; set; }
        public string ModelPrefix { get; set; }
        public string PrimaryFile { get; set; }

        public List<string> LabelFiles { get; set; }
    }
}