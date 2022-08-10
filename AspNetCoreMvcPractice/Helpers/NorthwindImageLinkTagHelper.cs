﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Helpers
{
    public class NorthwindImageLinkTagHelper : TagHelper
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("href", "Images/" + Id);
            output.Content.SetContent(Text);
        }
    }
}
