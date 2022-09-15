using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CardBoxCompanyManagement.Application.View.UserControls;

public partial class DefaultCRUDPopup : UserControl
{
    public static readonly DependencyProperty ActionButtonContentProperty = DependencyProperty.Register("ActionButtonContent", typeof(object), typeof(DefaultCRUDPopup));
    public static readonly DependencyProperty ActionButtonCommandProperty = DependencyProperty.Register("ActionButtonCommand", typeof(ICommand), typeof(DefaultCRUDPopup));
    public static readonly DependencyProperty ActionButtonCommandParameterProperty = DependencyProperty.Register("ActionButtonCommandParameter", typeof(object), typeof(DefaultCRUDPopup));

    public DefaultCRUDPopup()
    {
        InitializeComponent();
    }

    public object ActionButtonContent
    {
        get { return GetValue(ActionButtonContentProperty); }
        set { SetValue(ActionButtonContentProperty, value); }
    }
    public ICommand ActionButtonCommand
    {
        get { return (ICommand)GetValue(ActionButtonCommandProperty); }
        set { SetValue(ActionButtonCommandProperty, value); }
    }
    public ICommand ActionButtonCommandParameter
    {
        get { return (ICommand)GetValue(ActionButtonCommandParameterProperty); }
        set { SetValue(ActionButtonCommandParameterProperty, value); }
    }
}