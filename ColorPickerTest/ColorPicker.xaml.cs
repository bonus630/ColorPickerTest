using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.Windows.Media.TextFormatting;

namespace ColorPickerTest
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        ColorManager colorManager;
        public ColorPicker(Corel.Interop.VGCore.Palette palette)
        {
            InitializeComponent();
            colorManager = new ColorManager(palette);
            this.DataContext = colorManager;
            btn_ok.Click += (s, e) => { this.DialogResult = true; };
        }
        public ColorSystem SelectedColor { get { return colorManager.SelectedColor; } }

    }

}
public class RoutedCommand<T> : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public Action<T> action;


    public RoutedCommand(Action<T> action)
    {
        this.action = action;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        action.Invoke((T)parameter);
    }
}
public class ColorManager : INotifyPropertyChanged
{
    private ColorSystem[] colorArray;

    public event PropertyChangedEventHandler PropertyChanged;

    public ColorSystem[] ColorArray { get { return this.colorArray; } }
    public RoutedCommand<ColorSystem> ColorSelected { get { return new RoutedCommand<ColorSystem>(SetSelectedColor); } }

    private ColorSystem selectedColor;
    public ColorSystem SelectedColor
    {
        get
        {
            return selectedColor;
        }
        set
        {
            selectedColor = value;
            NotifyPropertyChanged();
        }
    }
    private void SetSelectedColor(ColorSystem color)
    {
        SelectedColor = color;
    }
    public ColorManager(Corel.Interop.VGCore.Palette palette)
    {
        if (palette == null)
            return;
        colorArray = new ColorSystem[palette.ColorCount];
        for (int i = 1; i < palette.ColorCount; i++)
        {
            Corel.Interop.VGCore.Color color = palette.Color[i];
            colorArray[i - 1] = new ColorSystem(color.HexValue, color.Name, color);
        }

    }
    public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

}
public class ColorSystem
{
    public ColorSystem(string colorHexValue, string corelColorName, Corel.Interop.VGCore.Color corelColor)
    {
        this.colorHexValue = colorHexValue;
        this.corelColorName = corelColorName;
        this.corelColor = corelColor;
    }

    private string colorHexValue;

    public string ColorHexValue
    {
        get { return colorHexValue; }
        set { colorHexValue = value; }
    }
    private string corelColorName;

    public string CorelColorName
    {
        get { return corelColorName; }
        set { corelColorName = value; }
    }
    private Corel.Interop.VGCore.Color corelColor;
    public Corel.Interop.VGCore.Color CorelColor
    {
        get { return corelColor; }
        set { corelColor = value; }
    }


}

