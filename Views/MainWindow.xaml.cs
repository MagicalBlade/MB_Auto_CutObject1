using MB_Auto_CutObject1.ViewModels;
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
using System.Windows.Shapes;
using Tekla.Structures.Dialog;

namespace MB_Auto_CutObject1.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : PluginWindowBase
    {
        public MainWindowViewModel dataModel;

        public MainWindow(MainWindowViewModel DataModel)
        {
            InitializeComponent();
            dataModel = DataModel;
        }
        //Обновляет свойства формы для загрузки значений поумолчанию
        //Если есть файл standard в папке с плагином с сохраненными значениями свойств, то этот метод не нужен
        /*
        protected override string LoadValuesPath(string FileName)
        {
            dataModel.Height = 55;
            this.Apply();
            return base.LoadValuesPath(FileName);
        }
        */
        private void WPFOkApplyModifyGetOnOffCancel_ApplyClicked(object sender, EventArgs e)
        {
            this.Apply();
        }

        private void WPFOkApplyModifyGetOnOffCancel_CancelClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_GetClicked(object sender, EventArgs e)
        {
            this.Get();
        }

        private void WPFOkApplyModifyGetOnOffCancel_ModifyClicked(object sender, EventArgs e)
        {
            this.Modify();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OkClicked(object sender, EventArgs e)
        {
            this.Apply();
            this.Close();
        }

        private void WPFOkApplyModifyGetOnOffCancel_OnOffClicked(object sender, EventArgs e)
        {
            this.ToggleSelection();
        }

        private void cb_typeCut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    sp_Height.Visibility = Visibility.Visible;
                    sp_Height1.Visibility = Visibility.Visible;
                    sp_Width.Visibility = Visibility.Visible;
                    sp_Width1.Visibility = Visibility.Visible;
                    sp_Width2.Visibility = Visibility.Collapsed;
                    sp_Width3.Visibility = Visibility.Collapsed;
                    sp_Width4.Visibility = Visibility.Collapsed;
                    sp_Radius.Visibility = Visibility.Visible;
                    sp_OffsetH.Visibility = Visibility.Visible;

                    sp_typeChamfer.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    sp_Height.Visibility = Visibility.Visible;
                    sp_Height1.Visibility = Visibility.Visible;
                    sp_Width.Visibility = Visibility.Visible;
                    sp_Width1.Visibility = Visibility.Visible;
                    sp_Width2.Visibility = Visibility.Visible;
                    sp_Width3.Visibility = Visibility.Visible;
                    sp_Width4.Visibility = Visibility.Collapsed;
                    sp_Radius.Visibility = Visibility.Visible;
                    sp_OffsetH.Visibility = Visibility.Visible;

                    sp_typeChamfer.Visibility = Visibility.Visible;
                    break;
                case 2:
                    sp_Height.Visibility = Visibility.Visible;
                    sp_Height1.Visibility = Visibility.Visible;
                    sp_Width.Visibility = Visibility.Visible;
                    sp_Width1.Visibility = Visibility.Visible;
                    sp_Width2.Visibility = Visibility.Visible;
                    sp_Width3.Visibility = Visibility.Visible;
                    sp_Width4.Visibility = Visibility.Visible;
                    sp_Radius.Visibility = Visibility.Visible;
                    sp_OffsetH.Visibility = Visibility.Visible;

                    sp_typeChamfer.Visibility = Visibility.Visible;
                    break;
                case 3:
                    sp_Height.Visibility = Visibility.Visible;
                    sp_Height1.Visibility = Visibility.Visible;
                    sp_Width.Visibility = Visibility.Collapsed;
                    sp_Width1.Visibility = Visibility.Visible;
                    sp_Width2.Visibility = Visibility.Visible;
                    sp_Width3.Visibility = Visibility.Collapsed;
                    sp_Width4.Visibility = Visibility.Collapsed;
                    sp_Radius.Visibility = Visibility.Visible;
                    sp_OffsetH.Visibility = Visibility.Visible;

                    sp_typeChamfer.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void cb_typeChamfer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    sp_DimensionF1.Visibility = Visibility.Visible;
                    sp_DimensionF2.Visibility = Visibility.Collapsed;
                    sp_DimensionF3.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    sp_DimensionF1.Visibility = Visibility.Visible;
                    sp_DimensionF2.Visibility = Visibility.Collapsed;
                    sp_DimensionF3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    sp_DimensionF1.Visibility = Visibility.Visible;
                    sp_DimensionF2.Visibility = Visibility.Visible;
                    sp_DimensionF3.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void cb_autoWH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            switch (listBox.SelectedIndex)
            {
                case 0:
                    sp_Width1.IsEnabled= false;
                    sp_Height.IsEnabled= false;
                    break;
                case 1:
                    sp_Width1.IsEnabled = true;
                    sp_Height.IsEnabled = true;
                    break;
            }
        }
    }
}
