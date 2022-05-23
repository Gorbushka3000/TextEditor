using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TextEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            textEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbFontFamily.SelectedItem != null)
                textEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void textEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = textEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = textEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderLine.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = textEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = textEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();

        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if(ofd.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(ofd.FileName, FileMode.Open);
                TextRange range = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(sfd.FileName, FileMode.Create);
                TextRange doc = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
                if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                    doc.Save(fileStream, DataFormats.Rtf);
                else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                    doc.Save(fileStream, DataFormats.Text);
                else
                    doc.Save(fileStream, DataFormats.Xaml);
            }
        }
    }
}
