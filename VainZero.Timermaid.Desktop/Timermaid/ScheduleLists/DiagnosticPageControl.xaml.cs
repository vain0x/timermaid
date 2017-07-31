using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VainZero.Timermaid.ScheduleLists
{
    /// <summary>
    /// Interaction logic for DiagnosticPageControl.xaml
    /// </summary>
    public partial class DiagnosticPageControl : UserControl
    {
        public DiagnosticPageControl()
        {
            InitializeComponent();

            Loaded += (sender, e) =>
            {
                var page = DataContext as DiagnosticPage;
                if (page == null) return;

                if (page.UpdateCommand.CanExecute(default(object)))
                {
                    page.UpdateCommand.Execute(default(object));
                }
            };
        }
    }
}
