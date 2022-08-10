using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AspNetCoreMvcPractice.Helpers
{
    public static class NorthwindImageLinkExtension
    {
        public static HtmlString NorthwindImageLink(this IHtmlHelper helper, int imageId, string text)
        {
            return new HtmlString(string.Format(@"<a href=""images/{0}"">{1}</a>", imageId, text));
        }
    }
}
