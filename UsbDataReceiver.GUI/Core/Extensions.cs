using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using NationalInstruments.Restricted;

namespace UsbDataReceiver.GUI.Core;

internal static class Extensions
{
    
    public static void UpdateList<TSource, TNew, TKey>(
        this IList<TSource> sourceList,
        [NotNull]IEnumerable<TNew> newList,
        [NotNull]Func<TSource, TKey> oldItemSelector,
        [NotNull]Func<TNew, TKey> newItemSelector,
        [NotNull]Func<TNew, TSource> newItemCreator)
    {
        if (sourceList == null || newList == null || oldItemSelector == null || newItemSelector == null || newItemCreator == null)
        {
            throw new ArgumentNullException();
        }

        var sources = newList.ToList();
        // If the lists are equal, do nothing.
        if(sourceList.All(item2 => sources.Any(item1 => Equals(newItemSelector(item1), oldItemSelector(item2)))))
            return;
        
        var existingKeys = sourceList.Select(oldItemSelector).ToList();
        var newKeys = sources.Select(newItemSelector).ToList();

        var keysToRemove = existingKeys.Except(newKeys).ToList();
        var keysToAdd = newKeys.Except(existingKeys).ToList();

        var itemsToRemove = sourceList.Where(item => keysToRemove.Contains(oldItemSelector(item))).ToList();
        var itemsToAdd = sources.Where(item => keysToAdd.Contains(newItemSelector(item))).ToList();
        
        // Remove items from the old list that don't exist in the new list.
        foreach (var item in itemsToRemove)
        {
            sourceList.Remove(item);
        }

        // Add new items to the old list.
        foreach (var newItem in itemsToAdd)
        {
            sourceList.Add(newItemCreator(newItem));
        }
    }
    
    public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
    {
        DependencyObject? parent = VisualTreeHelper.GetParent(child);

        while (parent != null && !(parent is T))
        {
            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent as T;
    }
    
    

    public record Item(int nr, string Value );
    
}