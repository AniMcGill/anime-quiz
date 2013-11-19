using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anime_Quiz_3.Classes;
using GameContext;

namespace Anime_Quiz_3.GameMaster
{
    /// <summary>
    /// Interaction logic for BuzzerSetupWindow.xaml
    /// </summary>
    public partial class BuzzerSetupWindow : Window
    {
        string[] comPorts;
        public BuzzerSetupWindow()
        {
            InitializeComponent();
            getPorts();
            getStacks();
        }
        private void getPorts()
        {
            comPorts = SerialPort.GetPortNames();
        }
        private void getStacks()
        {
            foreach (Teams team in App.teams)
            {
                Label label = new Label();
                label.Content = team.Name;
                label.Height = 25;
                teamStack.Children.Add(label);

                ComboBox comboBox = new ComboBox();
                comboBox.Height = 25;
                comboBox.Uid = team.TeamId.ToString();
                comboBox.ItemsSource = comPorts;
                comStack.Children.Add(comboBox);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ComboBox box in comStack.Children.OfType<ComboBox>())
            {
                if (box.SelectedIndex == -1)
                {
                    SoundMessageBox.Show("A buzzer must be assigned to each team", "Fail", Properties.Resources.w_lulu);
                    App.buzzerParams.Clear();
                    return;
                }
                App.buzzerParams.Add((int)Int32.Parse(box.Uid), box.SelectedValue.ToString());
            }
            if (App.buzzerParams.Count.Equals(4))
            {
                this.Close();
            }
            else
                SoundMessageBox.Show("Something went wrong, less than 4 buzzers are saved.", "Fail", Properties.Resources.w_lulu);
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
