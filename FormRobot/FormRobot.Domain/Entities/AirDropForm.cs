using FormRobot.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormRobot.Domain.Entities
{
    public class AirDropForm
    {
        public UserFormData myFormData { get; set; }

        public string AirDropUrl { get; set; }
        public int AirDropId { get; set; }
        public string AirDropFormHtml
        {
            get
            {
                return HtmlAgilityHelper.GenerateFormData(AirDropUrl, myFormData);
            }
        }
    }
}
