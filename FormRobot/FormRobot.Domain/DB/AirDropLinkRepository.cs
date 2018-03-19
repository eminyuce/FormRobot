using EImece.Domain;
using FormRobot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FormRobot.Domain.DB
{
    public class AirDropLinkRepository
    {
        private static string CacheKeyAllItems = "AirDropLinkCache";

        public static List<AirDropLink> GetAirDropLinksFromCache()
        {
            var items = (List<AirDropLink>)MemoryCache.Default.Get(CacheKeyAllItems);
            if (items == null)
            {
                items = GetAirDropLinks();
                CacheItemPolicy policy = null;
                policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Settings.CacheMediumSeconds);
                MemoryCache.Default.Set(CacheKeyAllItems, items, policy);
            }
            return items;
        }

        public static List<AirDropLink> GetAirDropLinks()
        {
            return DBDirectory.GetAirDropLinks();
        }
        public static int SaveOrUpdateAirDropLink(AirDropLink item)
        {
            RemoveCache();
            return DBDirectory.SaveOrUpdateAirDropLink(item);
        }
        public static AirDropLink GetAirDropLink(int airDropLinkId)
        {
            var item = GetAirDropLinksFromCache().FirstOrDefault(r => r.AirDropLinkId == airDropLinkId);
            if (item != null) return item;
            return DBDirectory.GetAirDropLink(airDropLinkId);
        }
        public static void DeleteAirDropLink(int airDropLinkId)
        {
            RemoveCache();
            DBDirectory.DeleteAirDropLink(airDropLinkId);
        }
        public static void RemoveCache()
        {
            MemoryCache.Default.Remove(CacheKeyAllItems);
        }
    }

}
