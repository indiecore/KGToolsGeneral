using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Set of convenience functions that work with unix timestamps.
/// </summary>
public static class TimeUtils {

	#region Constants

	/// <summary>
	/// The epoch date as a DateTime.
	/// </summary>
	public static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	/// <summary>
	/// The number of seconds in a minute.
	/// </summary>
	public const long UNIX_MINUTE = 60L;

	/// <summary>
	/// The number of seconds in an hour.
	/// </summary>
	public const long UNIX_HOUR = 3600L;

	/// <summary>
	/// The number of seconds in a day.
	/// </summary>
	public const long UNIX_DAY = 86400L;

	#endregion

	#region Unix Time Methods

	/// <summary>
	/// Gets the current unix timestamp as an integer.
	/// </summary>
	/// <returns>The time as a unix timestamp.</returns>
	public static int UnixTime32()
	{
		return DateTime.UtcNow.ToUnixTime32();
	}

	/// <summary>
	/// Returns the current unix timestamp in a long value.
	/// </summary>
	/// <returns>The time as a unix timestamp.</returns>
	public static long UnixTime()
	{
		return DateTime.UtcNow.ToUnixTime();
	}

	/// <summary>
	/// Converts a given DateTime into a long value Unix timestamp.
	/// </summary>
	/// <returns>The given DateTime as a long in Unix timestamp format.</returns>
	/// <param name="value">Timestamp to convert.</param>
	public static long ToUnixTime(this DateTime value)
	{
		return (value.ToUniversalTime().Ticks - epoch.Ticks) / TimeSpan.TicksPerSecond;
	}

	/// <summary>
	/// Converts a given DateTime into an integer Unix timestamp.
	/// </summary>
	/// <returns>The given DateTime in Unix timestamp format.</returns>
	/// <param name="value">Timestamp to convert.</param>
	public static int ToUnixTime32(this DateTime value)
	{
		return (int)ToUnixTime(value);
	}

	#endregion

	#region DateTime Functions

	/// <summary>
	/// Converts a Unix Timestamp into a System.DateTime object.
	/// </summary>
	/// <returns> DateTime object representing a time equal to the provided Unix timestamp.</returns>
	/// <param name="value">The timestamp to convert.</param>
	public static DateTime ToDateTime(long value)
	{
		return new DateTime((TimeSpan.TicksPerSecond * value) + epoch.Ticks);
	}

	/// <summary>
	/// Converts an integer Unix Timestamp into a System.DateTime object.
	/// </summary>
	/// <returns>A DateTime object representing a time equal to the provided Unix timestamp.</returns>
	/// <param name="value">The timestamp to convert.</param>
	public static DateTime ToDateTime(int value)
	{
		return new DateTime((TimeSpan.TicksPerSecond * value) + epoch.Ticks);
	}

	#endregion
}
