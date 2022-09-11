﻿using CardBoxCompanyManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CardBoxCompanyManagement.ViewModels;

/// <summary>
/// Performs Paging operations on a given List and Outputs a DataTable
/// </summary>
internal class Paging
{
    private DataTable PagedList = new();
    /// <summary>
    /// Current Page Index Number
    /// </summary>
    public int PageIndex { get; set; }

    //Initialize a DataTable Locally

    /// <summary>
    /// Show the next set of Items based on page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> DataTable </returns>
    public DataTable Next(IList<Company> ListToPage, int RecordsPerPage)
    {
        PageIndex++;
        if (PageIndex >= ListToPage.Count / RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
        }
        PagedList = SetPaging(ListToPage, RecordsPerPage);
        return PagedList;
    }

    /// <summary>
    /// Show the previous set of items base on page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> DataTable </returns>
    public DataTable Previous(IList<Company> ListToPage, int RecordsPerPage)
    {
        PageIndex--;
        if (PageIndex <= 0)
        {
            PageIndex = 0;
        }
        PagedList = SetPaging(ListToPage, RecordsPerPage);
        return PagedList;
    }

    /// <summary>
    /// Show first the set of Items in the page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> DataTable </returns>
    public DataTable First(IList<Company> ListToPage, int RecordsPerPage)
    {
        PageIndex = 0;
        PagedList = SetPaging(ListToPage, RecordsPerPage);
        return PagedList;
    }

    /// <summary>
    /// Show the last set of items in the page index
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> DataTable </returns>
    public DataTable Last(IList<Company> ListToPage, int RecordsPerPage)
    {
        PageIndex = ListToPage.Count / RecordsPerPage;
        PagedList = SetPaging(ListToPage, RecordsPerPage);
        return PagedList;
    }

    /// <summary>
    /// Performs a LINQ Query on the List and returns a DataTable
    /// </summary>
    /// <param name="ListToPage"> </param>
    /// <param name="RecordsPerPage"> </param>
    /// <returns> DataTable </returns>
    public DataTable SetPaging(IList<Company> ListToPage, int RecordsPerPage)
    {
        int PageGroup = PageIndex * RecordsPerPage;

        IList<Company> PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList();
        //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

        return PagedTable(PagedList);
    }

    //If youre paging say 30,000 rows and you know the processors of the users will be slow you can ASync thread both of these to allow the UI to update after they finish and prevent a hang.

    /// <summary>
    /// Internal Method: Performs the Work of converting the Passed in list to a DataTable
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    /// <param name="SourceList"> </param>
    /// <returns> DataTable </returns>
    private DataTable PagedTable<T>(IList<T> SourceList)
    {
        Type columnType = typeof(T);
        DataTable TableToReturn = new();

        foreach (var Column in columnType.GetProperties())
        {
            TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
        }

        foreach (object item in SourceList)
        {
            DataRow ReturnTableRow = TableToReturn.NewRow();
            foreach (var Column in columnType.GetProperties())
            {
                ReturnTableRow[Column.Name] = Column.GetValue(item);
            }
            TableToReturn.Rows.Add(ReturnTableRow);
        }
        return TableToReturn;
    }
}