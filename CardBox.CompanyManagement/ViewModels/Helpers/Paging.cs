using System.Collections.Generic;
using System.Linq;

namespace CardBox.CompanyManagement.ViewModels;

/// <summary>
/// Performs Paging operations on a given List and Outputs a DataTable
/// </summary>
internal class Paging<T>
{
    /// <summary>
    /// Current Page Index Number
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Show the next set of Items based on page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> List </returns>
    public List<T> Next(IList<T> ListToPage, int RecordsPerPage)
    {
        PageIndex++;
        if (PageIndex >= ListToPage.Count / RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
        }
        return SetPaging(ListToPage, RecordsPerPage);
    }

    /// <summary>
    /// Show the previous set of items base on page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> List </returns>
    public List<T> Previous(IList<T> ListToPage, int RecordsPerPage)
    {
        PageIndex--;
        if (PageIndex <= 0)
        {
            PageIndex = 0;
        }
        return SetPaging(ListToPage, RecordsPerPage);
    }

    /// <summary>
    /// Show first the set of Items in the page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> List </returns>
    public List<T> First(IList<T> ListToPage, int RecordsPerPage)
    {
        PageIndex = 0;
        return SetPaging(ListToPage, RecordsPerPage);
    }

    /// <summary>
    /// Show the last set of items in the page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> List </returns>
    public List<T> Last(IList<T> ListToPage, int RecordsPerPage)
    {
        //TODO: fix bug with last page
        PageIndex = ListToPage.Count / RecordsPerPage;
        return SetPaging(ListToPage, RecordsPerPage);
    }

    /// <summary>
    /// Performs a LINQ Query on the List and returns a DataTable
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> List </returns>
    public List<T> SetPaging(IList<T> ListToPage, int RecordsPerPage)
    {
        int PageGroup = PageIndex * RecordsPerPage;

        return ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();
    }
}