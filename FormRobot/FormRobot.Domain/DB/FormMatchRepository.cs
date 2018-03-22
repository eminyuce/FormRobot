using EImece.Domain;
using FormRobot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FormRobot.Domain.DB
{
    public class FormMatchRepository
    {
        private static string CacheKeyAllItems = "FormMatchCache";

      
        public static List<SelectListItem> GenerateUserDataDropDownItems()
        {
            var key = "UserDataDropDownItems";
            var selectListItems = (List<SelectListItem>)MemoryCache.Default.Get(key);
            if (selectListItems == null)
            {
                selectListItems = new List<SelectListItem>();
                var emptyObj = new UserFormData();
                foreach (var item in emptyObj.GetType().GetProperties())
                {
                    if (!item.Name.Equals("UserId"))
                    {
                        selectListItems.Add(new SelectListItem() { Value = item.Name, Text = item.Name });
                    }
                }
                CacheItemPolicy policy = null;
                policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Settings.CacheMediumSeconds);
                MemoryCache.Default.Set(key, selectListItems, policy);
            }
            return selectListItems;
        }
        public static List<FormMatch> GetFormMatchsFromCache()
        {
            var items = (List<FormMatch>)MemoryCache.Default.Get(CacheKeyAllItems);
            if (items == null)
            {
                items = GetFormMatchs();
                CacheItemPolicy policy = null;
                policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(Settings.CacheMediumSeconds);
                MemoryCache.Default.Set(CacheKeyAllItems, items, policy);
            }
            return items;
        }

        public static List<FormMatch> GetFormMatchs()
        {
            return DBDirectory.GetFormMatchs();
        }
        public static int SaveOrUpdateFormMatch(FormMatch item)
        {
            RemoveCache();
            return DBDirectory.SaveOrUpdateFormMatch(item);
        }
        public static FormMatch GetFormMatch(int formMatchId)
        {
            var item = GetFormMatchsFromCache().FirstOrDefault(r => r.FormMatchId == formMatchId);
            if (item != null) return item;
            return DBDirectory.GetFormMatch(formMatchId);
        }
        public static void DeleteFormMatch(int formMatchId)
        {
            RemoveCache();
            DBDirectory.DeleteFormMatch(formMatchId);
        }
        public static void RemoveCache()
        {
            MemoryCache.Default.Remove(CacheKeyAllItems);
        }
    }

}
