/*
 * Copyright (c) 2020 Kristopher Gay
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using UnityEngine;

public static class MathUtils
{

	#region Approximately

	/// <summary>
	/// Wrapper around Mathf.Approximately. Use specifically for floats.
	/// </summary>
	/// <param name="a">First float.</param>
	/// <param name="b">Second float.</param>
	/// <returns>True if the values are approximately equal.</returns>
	public static bool Approximately(float a, float b)
	{
		return Mathf.Approximately(a, b);
	}

	/// <summary>
	/// Checks for floating point equality for doubles.
	/// </summary>
	/// <param name="a">The first number to check.</param>
	/// <param name="b">The second number to check.</param>
	/// <returns>True if the values are the same number within the error bounds of double.Epsilon.</returns>
	public static bool Approximately(double a, double b)
	{
		return (a >= b - double.Epsilon && a <= b + double.Epsilon);
	}

	#endregion

	#region Lerps

	/// <summary>
	/// Linearly interpolates between a and b by t.
	/// The parameter t is clamped to the range [0, 1].
	/// </summary>
	/// <param name="a">The start value.</param>
	/// <param name="b">The end value.</param>
	/// <param name="t">The interpolation value.</param>
	/// <returns>The interpolated double result between two doubles.</returns>
	public static double Lerp(double a, double b, double t)
	{
		return MathUtils.LerpUnclamped(a, b, MathUtils.Clamp(t, 0, 1));
	}

	/// <summary>
	/// Linearly interpolates between a and b by t.
	/// </summary>
	/// <param name="a">The start value.</param>
	/// <param name="b">The end value.</param>
	/// <param name="t">The interpolation value.</param>
	/// <returns>The interpolated double result between two doubles.</returns>
	public static double LerpUnclamped(double a, double b, double t)
	{
		return ((1.0 - t) * a + t * b);
	}

	#endregion

	#region Non-Float Clamp

	/// <summary>
	/// Clamping function that doesn't use Mathf. The C# standard library doesn't have one.
	/// This can clamp anything that implements System.IComparable (All numeric types do implement that interface).
	/// </summary>
	/// <param name="val">Value to clamp.</param>
	/// <param name="min">Minium value.</param>
	/// <param name="max">Maximum value.</param>
	/// <typeparam name="T">Type that implements System.IComparable.</typeparam>
	/// <returns>min if the value is less than min, max if the value is more than max. Otherwise it returns val.</returns>
	public static T Clamp<T>(T val, T min, T max) where T : System.IComparable<T>
	{
		if (val.CompareTo(min) < 0)
		{
			return min;
		}
		else if (val.CompareTo(max) > 0)
		{
			return max;
		}
		else 
		{
			return val;
		}
	}

	#endregion

	#region Between

	/// <summary>
	/// Return true if a floating point number is between min and max (inclusive).
	/// </summary>
	/// <param name="value">Value to check</param>
	/// <param name="min">Minimum bounding value.</param>
	/// <param name="max">Maximum bounding value.</param>
	/// <returns>True if value is between min and max; false otherwise.</returns>
	public static bool Between(float value, float min, float max)
	{
		return (value >= min && value <= max);
	}

	/// <summary>
	/// Return true if a double precision number is between min and max (inclusive).
	/// </summary>
	/// <param name="value">Value to check</param>
	/// <param name="min">Minimum bounding value.</param>
	/// <param name="max">Maximum bounding value.</param>
	/// <returns>True if value is between min and max; false otherwise.</returns>
	public static bool Between(double value, double min, double max)
	{
		return (value >= min && value <= max);
	}

	/// <summary>
	/// Return true if an integer number is between min and max (inclusive).
	/// </summary>
	/// <param name="value">Value to check</param>
	/// <param name="min">Minimum bounding value.</param>
	/// <param name="max">Maximum bounding value.</param>
	/// <returns>True if value is between min and max; false otherwise.</returns>
	public static bool Between(int value, int min, int max)
	{
		return (value >= min && value <= max);
	}

	/// <summary>
	/// Return true if a long number is between min and max (inclusive).
	/// </summary>
	/// <param name="value">Value to check</param>
	/// <param name="min">Minimum bounding value.</param>
	/// <param name="max">Maximum bounding value.</param>
	/// <returns>True if value is between min and max; false otherwise.</returns>
	public static bool Between(long value, long min, long max)
	{
		return (value >= min && value <= max);
	}

	#endregion

	#region Map

	/// <summary>
	/// Maps a value within one range to an equivalent value in another range.
	/// </summary>
	/// <param name="value">Value in the original range to derive the mapping from.</param>
	/// <param name="fromLow">Original range minimum.</param>
	/// <param name="fromHigh">Original range maximum.</param>
	/// <param name="toLow">New range minimum.</param>
	/// <param name="toHigh">New range maximum.</param>
	public static float Map(int value, int fromLow, int fromHigh, float toLow, float toHigh)
	{
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}

	/// <summary>
	/// Remaps a value within one range to an equivalent value in another range.
	/// </summary>
	/// <param name="value">Value in the original range to derive the mapping from.</param>
	/// <param name="fromLow">Original range minimum.</param>
	/// <param name="fromHigh">Original range maximum.</param>
	/// <param name="toLow">New range minimum.</param>
	/// <param name="toHigh">New range maximum.</param>
	public static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
	{
		return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
	}

	#endregion

}
