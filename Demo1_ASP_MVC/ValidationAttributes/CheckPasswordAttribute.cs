using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text;

namespace Demo1_ASP_MVC.ValidationAttributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CheckPasswordAttribute : ValidationAttribute
    {

        private string _patternMin, _patternMaj, _patternNumb, _patternSymb;

        public bool HasMin { get; private set; }
        public bool HasMaj { get; private set; }
        public bool HasNumb { get; private set; }
        public bool HasSymb { get; private set; }


        public CheckPasswordAttribute()
        {
            _patternMin = @"[a-z]";
            _patternMaj = @"[A-Z]";
            _patternNumb = @"[0-9]";
            _patternSymb = @"(?![a-zA-Z0-9\s])."; // @"[@\-+=#_]+";

            HasMin = false;
            HasMaj = false;
            HasNumb = false;
            HasSymb = false;
        }


        public override bool IsValid(object? value)
        {
            string content = value?.ToString() ?? "";

            HasMin = Regex.IsMatch(content, _patternMin, RegexOptions.None);
            HasMaj = Regex.IsMatch(content, _patternMaj, RegexOptions.None);
            HasNumb = Regex.IsMatch(content, _patternNumb, RegexOptions.None);
            HasSymb = Regex.IsMatch(content, _patternSymb, RegexOptions.None);

            return (HasMin && HasMaj && HasNumb && HasSymb);
        }

        public override string FormatErrorMessage(string name)
        {
            List<string> errors = new List<string>();

            if (!HasMin) errors.Add("de minuscule");
            if (!HasMaj) errors.Add("de majuscule");
            if (!HasNumb) errors.Add(" de chiffre");
            if (!HasSymb) errors.Add("de symbole");

            return $"{name} ne contient pas: {string.Join(", ", errors)}";
        }
    }
}
