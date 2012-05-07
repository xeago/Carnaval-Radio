using System;
using Orchard.Core.Shapes.Localization;
using Orchard.Localization;
using Orchard.Mvc.Html;
using Orchard.Services;

namespace Orchard.Tokens.Providers {
    public class DateTokens : ITokenProvider {
        private readonly IClock _clock;
        private readonly IDateTimeLocalization _dateTimeLocalization;
        private readonly IWorkContextAccessor _workContextAccessor;

        public DateTokens(
            IClock clock, 
            IDateTimeLocalization dateTimeLocalization, 
            IWorkContextAccessor workContextAccessor) {
            _clock = clock;
            _dateTimeLocalization = dateTimeLocalization;
            _workContextAccessor = workContextAccessor;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeContext context) {
            context.For("Date", T("Date/time"), T("Current date/time tokens"))
                .Token("Since", T("Since"), T("Relative to the current date/time."))
                .Token("Local", T("Local"), T("Based on the configured time zone."))
                .Token("ShortDate", T("Short Date"), T("Short date format."))
                .Token("ShortTime", T("Short Time"), T("Short time format."))
                .Token("Long", T("Long Date and Time"), T("Long date and time format."))
                .Token("Format:*", T("Format:<date format>"), T("Optional format specifier (e.g. yyyy/MM/dd). See format strings at <a target=\"_blank\" href=\"http://msdn.microsoft.com/en-us/library/az4se3k1.aspx\">Standard Formats</a> and <a target=\"_blank\" href=\"http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx\">Custom Formats</a>"), "DateTime");
        }

        public void Evaluate(EvaluateContext context) {
            context.For("Date", () => _clock.UtcNow)
                // {Date.Since}
                .Token("Since", DateTimeRelative)
                .Chain("Since", "Date", DateTimeRelative)
                // {Date.Local}
                .Token("Local", d => ConvertToDisplayTime(d))
                .Chain("Local", "Date", d => ConvertToDisplayTime(d))
                // {Date.ShortDate}
                .Token("ShortDate", d => d.ToString(_dateTimeLocalization.ShortDateFormat.Text))
                // {Date.ShortTime}
                .Token("ShortTime", d => d.ToString(_dateTimeLocalization.ShortTimeFormat.Text))
                // {Date.Long}
                .Token("Long", d => d.ToString(_dateTimeLocalization.LongDateTimeFormat.Text))
                // {Date}
                .Token(
                    token => token == String.Empty ? String.Empty : null,
                    (token, d) => d.ToString(_dateTimeLocalization.ShortDateFormat.Text + " " + _dateTimeLocalization.ShortTimeFormat.Text))
                // {Date.Format:<formatstring>}
                .Token(
                    token => token.StartsWith("Format:", StringComparison.OrdinalIgnoreCase) ? token.Substring("Format:".Length) : null,
                    (token, d) => d.ToString(token));
        }

        private string DateTimeRelative(DateTime dateTimeUtc) {
            var time = _clock.UtcNow - dateTimeUtc.ToUniversalTime();

            if (time.TotalDays > 7)
                return dateTimeUtc.ToString(T("'on' MMM d yyyy 'at' h:mm tt").ToString());
            if (time.TotalHours > 24)
                return T.Plural("1 day ago", "{0} days ago", time.Days).ToString();
            if (time.TotalMinutes > 60)
                return T.Plural("1 hour ago", "{0} hours ago", time.Hours).ToString();
            if (time.TotalSeconds > 60)
                return T.Plural("1 minute ago", "{0} minutes ago", time.Minutes).ToString();
            if (time.TotalSeconds > 10)
                return T.Plural("1 second ago", "{0} seconds ago", time.Seconds).ToString();

            return T("a moment ago").ToString();
        }

        /// <summary>
        /// Converts a Coordinated Universal Time (UTC) to the time in the current time zone.
        /// </summary>
        /// <param name="dateTimeUtc">The Coordinated Universal Time (UTC).</param>
        /// <returns>The date and time in the selected time zone. Its System.DateTime.Kind property is System.DateTimeKind.Utc if the current zone is System.TimeZoneInfo.Utc; otherwise, its System.DateTime.Kind property is System.DateTimeKind.Unspecified.</returns>
        private DateTime ConvertToDisplayTime(DateTime dateTimeUtc) {

            // get the time zone for the current request
            var timeZone = _workContextAccessor.GetContext().CurrentTimeZone;

            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, timeZone);
        }
    }
}