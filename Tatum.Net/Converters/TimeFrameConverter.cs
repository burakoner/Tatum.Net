using CryptoExchange.Net.Converters;
using System.Collections.Generic;
using Tatum.Net.Enums;

namespace Tatum.Net.Converters
{
    internal class TimeFrameConverter : BaseConverter<TimeFrame>
    {
        public TimeFrameConverter() : this(true) { }
        public TimeFrameConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TimeFrame, string>> Mapping => new List<KeyValuePair<TimeFrame, string>>
        {
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneMinute, "MIN_1"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.ThreeMinutes, "MIN_3"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.FiveMinutes, "MIN_5"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.FifteenMinutes, "MIN_15"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.ThirtyMinutes, "MIN_30"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneHour, "HOUR_1"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.FourHours, "HOUR_4"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.TwelveHours, "HOUR_12"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneDay, "DAY"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneWeek, "WEEK"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneMonth, "MONTH"),
            new KeyValuePair<TimeFrame, string>(TimeFrame.OneYear, "YEAR"),
        };
    }
}