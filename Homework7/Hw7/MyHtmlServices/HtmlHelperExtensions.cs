using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var html = new HtmlContentBuilder();
        var model = helper.ViewData.Model;
        var type = helper.ViewData.ModelMetadata.ModelType;
        var properties = type.GetProperties();
        foreach (var property in properties)
        {
            html.AppendHtmlLine("<div>");

            var displayName = GetDisplay(property);
            html.AppendHtmlLine($"<label for=\"{property.Name}\">{displayName}</label><br>");

            var inputField = GetInputField(property, model);
            html.AppendHtmlLine(inputField);

            if (model != null)
            {
                var errorMessage = ValidateModel(model, property);
                html.AppendHtmlLine($"<span>{errorMessage}</span>");
            }

            html.AppendHtmlLine("</div>");
        }
        return html;
    }

    private static string ValidateModel(object model, PropertyInfo property)
    {
        var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
        foreach (var curAttr in validationAttributes)
            if (!curAttr.IsValid(property.GetValue(model)))
                return curAttr.ErrorMessage;
        return "";
    }

    private static string GetInputField(PropertyInfo property, object? model)
    {
        var type = property.PropertyType;
        var strBuilder = new StringBuilder();
        var modelValue = (model != null ? $"value=\"{property.GetValue(model)}\"" : "");
        if (type.IsEnum)
        {
            strBuilder.AppendLine($"<select id=\"{property.Name}\" name=\"{property.Name}\"" + modelValue + ">");

            foreach (var enumVal in Enum.GetValues(type))
                strBuilder.AppendLine($"<option>{enumVal}</option>");

            strBuilder.AppendLine("</select>");
        }
        else
        {
            string inputType = "text";
            if (type == typeof(int))
                inputType = "number";
            strBuilder.AppendLine($"<input id=\"{property.Name}\" type=\"{inputType}\" name=\"{property.Name}\"" + modelValue + "/>");
        }
        return strBuilder.ToString();
    }

    private static string GetDisplay(PropertyInfo property)
    {
        var attribute =  property
            .GetCustomAttributes<DisplayAttribute>()
            .FirstOrDefault();
        if (attribute != null)
            return attribute.Name!;

        var words = Regex.Matches(property.Name, "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)").Select(x => x.Value);

        return string.Join(" ", words);
    }
} 