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
using System.Collections.Generic;
using UnityEngine;

public static class ListUtils
{

	/// <summary>
	/// Does a fisher-yates shuffle on this list.null Randomizing the order of the elements.
	/// The shuffle proceeds from the end of the list down to the start.
	/// </summary>
	/// <param name="operatingList">The list of elements to shuffle.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	public static void Shuffle<T>(this IList<T> operatingList)
	{
		T temp = default(T);
		int j = 0;
		for (int i = operatingList.Count - 1; i > 0; --i)
		{
			j = Random.Range(0, i);
			temp = operatingList[j];
			operatingList[j] = operatingList[i];
			operatingList[i] = temp;
		}
	}

	/// <summary>
	/// Gets a random element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	/// <returns>A random element from the list.</returns>
	public static T RandomElement<T>(this IList<T> operatingList)
	{
		if (operatingList.IsNullOrEmpty())
		{
			throw new System.IndexOutOfRangeException("List has no contents.");
		}

		return operatingList[UnityEngine.Random.Range(0, operatingList.Count)];
	}

	/// <summary>
	/// Gets the first element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	/// <returns>The first item from the specified list.</returns>
	public static T First<T>(this IList<T> operatingList)
	{
		if (operatingList.IsNullOrEmpty())
		{
			throw new System.IndexOutOfRangeException("List has no contents.");
		}
		return operatingList[0];
	}
	
	/// <summary>
	/// Gets the last element from this list.
	/// </summary>
	/// <param name="operatingList">List to draw element from.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	/// <returns>The last item from the specified list.</returns>
	public static T Last<T>(this IList<T> operatingList)
	{
		if (operatingList.IsNullOrEmpty())
		{
			throw new System.IndexOutOfRangeException("List has no contents.");
		}

		return operatingList[operatingList.Count - 1];
	}

	/// <summary>
	/// Check if the list is null or empty.
	/// </summary>
	/// <param name="operatingList">The list to check.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	/// <returns>True if the list is null or empty; False otherwise.</returns>
	public static bool IsNullOrEmpty<T>(this IList<T> operatingList)
	{
		if (operatingList == null || operatingList.Count == 0)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// Checks if a list has a value in a particular index.
	/// </summary>
	/// <param name="list">The list to check.</param>
	/// <param name="index">Index to check.</param>
	/// <typeparam name="T">Type of the list contents.</typeparam>
	/// <returns>True if the list has the requested index; False otherwise.</returns>
	public static bool HasIndex<T>(this IList<T> list, int index)
	{
		return index >= 0 && index < list.Count;
	}

}
