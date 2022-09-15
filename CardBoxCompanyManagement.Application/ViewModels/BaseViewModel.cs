﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CardBoxCompanyManagement.ViewModels;

internal class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}