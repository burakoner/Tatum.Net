using System;
using System.Linq;
using Tatum.Net.Atrributes;
using Tatum.Net.Enums;
using Tatum.Net.WalletObjects;

namespace Tatum.Net.Helpers
{
    public static class TatumExtensions
    {
        #region Null
        public static bool IsNull(this object @this)
        {
            return (@this == null || @this.GetType() == typeof(DBNull));
        }

        public static bool IsNotNull(this object @this)
        {
            return !@this.IsNull();
        }
        #endregion

        #region ToStr
        public static string ToStr(this object @this, bool nullToEmpty = true)
        {
            bool isNull = @this == null ? true : false;
            bool isDBNull = @this != null && @this.GetType() == typeof(DBNull) ? true : false;

            if (isNull)
                return nullToEmpty ? string.Empty : null;
            else if (isDBNull)
                return nullToEmpty ? string.Empty : null;
            else
                return @this?.ToString();
        }
        #endregion

        #region ToNumber
        public static int ToInt(this object @this)
        {
            int result = 0;
            if (!@this.IsNull()) int.TryParse(@this.ToStr(), out result);
            return result;
        }

        public static long ToLong(this object @this)
        {
            long result = 0;
            if (!@this.IsNull()) long.TryParse(@this.ToStr(), out result);
            return result;
        }

        public static double ToDouble(this object @this)
        {
            if (@this == null) return 0.0;

            double result = 0.0;
            double.TryParse(@this.ToStr(), out result);
            return result;
        }
        public static double? ToDoubleNullable(this object @this)
        {
            if (@this == null) return null;

            double result = 0.0;
            double.TryParse(@this.ToStr(), out result);
            return result;
        }

        public static decimal ToDecimal(this object @this)
        {
            if (@this == null) return 0;

            decimal result = 0.0m;
            decimal.TryParse(@this.ToStr(), out result);
            return result;
        }
        public static decimal? ToDecimalNullable(this object @this)
        {
            if (@this == null) return null;

            decimal result = 0.0m;
            decimal.TryParse(@this.ToStr(), out result);
            return result;
        }

        public static float ToFloat(this object @this)
        {
            if (@this == null) return 0;

            float result = 0;
            float.TryParse(@this.ToStr(), out result);
            return result;
        }
        #endregion

        #region DateTime
        public static DateTime FromUnixTimeSeconds(this int unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static DateTime FromUnixTimeSeconds(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTimeSeconds(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }

        public static DateTime FromUnixTimeMilliseconds(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }

        public static long ToUnixTimeMilliseconds(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
        #endregion

        #region IsOneOf
        public static bool IsOneOf(this int @this, params int[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this string @this, params string[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this decimal @this, params decimal[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this AssetType @this, params AssetType[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        public static bool IsOneOf(this BlockchainType @this, params BlockchainType[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        #endregion

        #region Enums
        public static BlockchainOptionsAttribute GetBlockchainOptions(this BlockchainType @this)
        {
            // Get Member Info
            var memberInfo = typeof(BlockchainType).GetMember(@this.ToString()).FirstOrDefault();
            if (memberInfo == null) return null;

            // Get Options
            var options = (BlockchainOptionsAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(BlockchainOptionsAttribute));
            return options;
        }
        #endregion

    }
}
