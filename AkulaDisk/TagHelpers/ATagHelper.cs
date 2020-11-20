using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApplication8.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("a",Attributes ="from,to")]
    public class ATagHelper : TagHelper
    {
        public string From { get; set; }
        public string To { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("href", $"{"/?path="+From+To+'/'} ");
            string content = (await output.GetChildContentAsync()).GetContent();
            output.Content.SetHtmlContent(content);
        }
    }
}
