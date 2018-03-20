using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Eth Wallet Address")]
        public String EthWalletAddress { get; set; }
        [Display(Name = "Bitcointalk Profile URL")]
        public String BitcointalkProfileURL { get; set; }
        [Display(Name = "Telegram Username")]
        public String TelegramUsername { get; set; }
        [Display(Name = "Email Address")]
        public String PersonalEmailAddress { get; set; }
        [Display(Name = "Bitcointalk Username")]
        public String BitcointalkUsername { get; set; }
        [Display(Name = "Facebook Name")]
        public String FacebookName { get; set; }
        [Display(Name = "Facebook Profile URL")]
        public String FacebookProfileURL { get; set; }
        [Display(Name = "Medium Profile URL")]
        public String MediumProfileURL { get; set; }
        [Display(Name = "Twitter Username")]
        public String TwitterUsername { get; set; }
        [Display(Name = "Twitter Follower Count")]
        public String TwitterFollowerCount { get; set; }
        [Display(Name = "Twitter Profile URL")]
        public String TwitterProfileURL { get; set; }
        [Display(Name = "Reddit Username")]
        public String RedditUsername { get; set; }
        [Display(Name = "Reddit Profile")]
        public String RedditProfile { get; set; }
        [Display(Name = "Your Language")]
        public String YourLanguage { get; set; }
        [Display(Name = "Your Skills")]
        public String YourSkills { get; set; }
        [Display(Name = "Any Helps to coin marketing or development")]
        public String YourHelps { get; set; }
    }
}
