using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System;

namespace NamelistFileCopier
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      edtSourceExt.Text = Properties.Settings.Default.SourceExt;
      edtAsExt.Text = Properties.Settings.Default.AsExt;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (edtSourceExt.Text != string.Empty)
        Properties.Settings.Default.SourceExt = edtSourceExt.Text;
      if (edtAsExt.Text != string.Empty)
        Properties.Settings.Default.AsExt = edtAsExt.Text;
    }

    private void edtClose_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void btnSource_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new FolderBrowserDialog();
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        edtSource.Text = dlg.SelectedPath;
    }

    private void btnAS_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new FolderBrowserDialog();
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        edtAs.Text = dlg.SelectedPath;
    }

    private void btnDestination_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new FolderBrowserDialog();
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        edtDestination.Text = dlg.SelectedPath;
    }

    private void btnCopy_Click(object sender, RoutedEventArgs e)
    {
      if (edtSource.Text == string.Empty)
      {
        System.Windows.MessageBox.Show("Source path cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      if (edtAs.Text == string.Empty)
      {
        System.Windows.MessageBox.Show("As path cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      if (edtDestination.Text == string.Empty)
      {
        System.Windows.MessageBox.Show("Destination path cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      if (edtSourceExt.Text == string.Empty)
      {
        System.Windows.MessageBox.Show("Source extension cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        edtSourceExt.Focus();
        return;
      }
      if (edtAsExt.Text == string.Empty)
      {
        System.Windows.MessageBox.Show("As extension cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        edtAsExt.Focus();
        return;
      }
      var asFiles = Directory.EnumerateFiles(edtAs.Text, "*." + edtAsExt.Text, SearchOption.TopDirectoryOnly);
      var errors = new List<string>();
      foreach (var item in asFiles)
      {
        var fileSource = Path.Combine(edtSource.Text, Path.GetFileNameWithoutExtension(item) + "." + edtSourceExt.Text);
        var fileDestination = Path.Combine(edtDestination.Text, Path.GetFileNameWithoutExtension(item) + "." + edtSourceExt.Text);
        if (File.Exists(fileSource) && !(File.Exists(fileDestination)))
          File.Copy(fileSource, fileDestination);
        else
          errors.Add(Path.GetFileName(item));
      }
      if (errors.Count > 0)
      {
        var message = "Next files were not copied:";
        foreach (var item in errors)
          message += (Environment.NewLine + item);
        System.Windows.MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }
      else
        System.Windows.MessageBox.Show("Done", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
    }
  }
}
