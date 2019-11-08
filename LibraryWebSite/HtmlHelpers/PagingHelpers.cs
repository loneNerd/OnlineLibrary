using System;
using System.Text;
using System.Web.Mvc;
using LibraryWebSite.Models;

namespace LibraryWebSite.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper helper, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sb = new StringBuilder();
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            int start = 0;
            int end = 2;

            if (pageInfo.CurrentPage == 1) { start = 0; end = 2; }
            else if (pageInfo.CurrentPage == pageInfo.TotalPages) { start = -2; end = 0; }
            else  { start = -1; end = 1; }

            for (int i = start; i < pageInfo.TotalPages - end && i <= end; ++i)
            {
                TagBuilder li = new TagBuilder("li");
                li.AddCssClass("page-item");

                TagBuilder a = new TagBuilder("a");
                a.MergeAttribute("href", pageUrl(pageInfo.CurrentPage + i));
                a.InnerHtml = (pageInfo.CurrentPage + i).ToString();
                a.AddCssClass("page-link");

                if ((pageInfo.CurrentPage == pageInfo.TotalPages && i == end) || 
                    (pageInfo.CurrentPage == 1 && i == start) || 
                    (pageInfo.CurrentPage != pageInfo.TotalPages && pageInfo.CurrentPage != 1  && i != start && i != end))
                    li.AddCssClass("active");

                li.InnerHtml = a.ToString();
                sb.Append(li.ToString());
            }

            ul.InnerHtml = sb.ToString();

            return MvcHtmlString.Create(ul.ToString());
        }
    }
}