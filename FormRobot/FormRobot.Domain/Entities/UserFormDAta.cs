using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FormRobot.Domain.Entities
{
    public class UserFormData
    {
        [XmlIgnore]
        public int UserId { get; set; }
        public String EthWalletAddress { get; set; }
        public String BitcointalkProfileURL { get; set; }
        public String TelegramUsername { get; set; }
        public String PersonalEmailAddress { get; set; }
        public String BitcointalkUsername { get; set; }
        public String FacebookName { get; set; }
        public String MediumProfileURL { get; set; }
        public String FacebookProfileURL { get; set; }
        public String TwitterUsername { get; set; }
        public String TwitterProfileURL { get; set; }
        public String YourLanguage { get; set; }
        public String YourSkills { get; set; }
        public String YourHelps { get; set; }
    }
}
