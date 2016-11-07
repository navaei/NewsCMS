using System;
using System.Collections.Generic;

namespace Mn.NewsCms.WebCore.WebLogic.BaseModel
{
    public class ColumnActionMenu : Object
    {
        public string Title { get; set; }
        public enum ItemType
        {
            ActionLink,
            ScriptCommand
        }
        public class ActionMenuItem
        {
            public ItemType Type { get; set; }
            public string Title { get; set; }
            public string Link { get; set; }
            public ActionMenuItem(ItemType type, string title, string link)
            {
                Type = type;
                Title = title;
                Link = link;
            }
            public ActionMenuItem(string title, string link)
                : this(ItemType.ActionLink, title, link)
            {

            }
        }
        List<ActionMenuItem> Items { get; set; }
        public ColumnActionMenu()
        {
            Items = new List<ActionMenuItem>();
            Title = Resources.Core.Action;
        }
        public ColumnActionMenu(params ActionMenuItem[] items)
            : this()
        {
            Items.AddRange(items);
        }
        public override string ToString()
        {
            string menu = "<div class='k-rtl'><ul class='grid-menu'><li>" + Title + "<ul>";
            foreach (var item in Items)
            {
                if (item.Type == ItemType.ActionLink)
                    menu += string.Format("<li><a href='{1}'>{0}</a></li>", item.Title, item.Link);
                else if (item.Type == ItemType.ScriptCommand)
                    menu += string.Format("<li><a href='\\#' onclick={1}>{0}</a></li>", item.Title, item.Link);
            }
            menu += "</li></ul></div>";
            return menu;
        }
    }
}