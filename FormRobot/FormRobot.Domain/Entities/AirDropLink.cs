using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormRobot.Domain.Entities
{
    public class AirDropLink
    {
        public int AirDropLinkId { get; set; }
        public string AirDropLinkUrl { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsDeleted { get;  set; }
    }
}
