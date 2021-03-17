using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClientCRUD.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverlayIndicator : ContentView
    {
        public OverlayIndicator()
        {
            InitializeComponent();
        }

        public bool VisibleActivityIndicator { get; set; }

        public static readonly BindableProperty VisibleActivityIndicatorProperty = BindableProperty.Create(
                                                         propertyName: "VisibleActivityIndicator",
                                                         returnType: typeof(bool),
                                                         declaringType: typeof(OverlayIndicator),
                                                         defaultValue: true,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         propertyChanged: VisibleActivityIndicatorPropertyChanged);

        private static void VisibleActivityIndicatorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (OverlayIndicator)bindable;
            control.activityIndicator.IsVisible = (bool)newValue;
        }
    }
}