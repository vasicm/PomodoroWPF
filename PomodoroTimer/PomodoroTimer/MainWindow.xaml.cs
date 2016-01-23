using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean state;
        private int workLength;
        private int breakLength;
        private DispatcherTimer timer;
        private int currentTime;
        private int timerInterval;
        private Dictionary<int, Image> images;
        private int tasksDone;

        public MainWindow()
        {
            InitializeComponent();
            state = true;
            workLength = 25 * 60;
            breakLength = 5 * 60;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            timer.Tick += timerTick;
            timeProgressBar.Value = 0;

            images = new Dictionary<int, Image>();
            images[0] = imgPomodoro1;
            images[1] = imgPomodoro2;
            images[2] = imgPomodoro3;
            images[3] = imgPomodoro4;
            images[4] = imgPomodoro5;
            images[5] = imgPomodoro6;
            images[6] = imgPomodoro7;
            images[7] = imgPomodoro8;

            tasksDone = 0;

            stateTextBlock.Text = "";
            timeTextBlock.Text = "00:00";
            timeProgressBar.Value = 0;
        }

        private void startTimer()
        {
            currentTime = timerInterval;
            timer.Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (state)
            {
                btn.Style = Resources["PomodoroButtonRed"] as Style;
                state = false;
                timer.Start();
                stateTextBlock.Text = "WORK";
                timerInterval = workLength;
                currentTime = timerInterval;
            }
            else
            {
                btn.Style = Resources["PomodoroButtonGreen"] as Style;
                state = true;
                timer.Stop();
                stateTextBlock.Text = "";
                timeTextBlock.Text = "00:00";
                timeProgressBar.Value = 0;
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            currentTime--;

            if (currentTime < 60)
            {
                timeTextBlock.Foreground = Brushes.Red;
                if (currentTime % 2 == 0)
                {
                    timeTextBlock.Visibility = Visibility.Hidden;
                }
                else
                {
                    timeTextBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                timeTextBlock.Foreground = Brushes.OrangeRed;
                timeTextBlock.Visibility = Visibility.Visible;
            }

            timeTextBlock.Text = string.Format("{0}{1}:{2}{3}", (currentTime / 60) / 10, (currentTime/60)%10, (currentTime % 60) / 10, (currentTime % 60) % 10);
            timeProgressBar.Value = 100 * (timerInterval - currentTime) / timerInterval;
            if (currentTime == 0)
            {
                timer.Stop();
                //TODO 
                SoundPlayer p = new SoundPlayer(PomodoroTimer.Properties.Resources.beep2);
                p.Play();

                if (timerInterval == workLength)
                {
                    images[tasksDone].Visibility = Visibility.Visible;
                    tasksDone++;
                    if(tasksDone >= 8)
                    {
                        MessageBox.Show("Congratulations!!!");
                        this.Close();
                    }
                    else { 
                        MessageBox.Show("Take a break!!!");
                        stateTextBlock.Text = "BREAK";
                        timerInterval = breakLength;
                        currentTime = timerInterval;
                    }
                }
                else if (timerInterval == breakLength)
                {
                    MessageBox.Show("Go back to work!!!");
                    stateTextBlock.Text = "WORK";
                    timerInterval = workLength;
                    currentTime = timerInterval;
                }
                
                timer.Start();

            }
        }

        private static int interval = 1000;
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {            
            if (e.Key == Key.A)
            {
                currentTime -= 60;
                if (currentTime < 0)
                {
                    currentTime = 2;
                }
            } else if (e.Key == Key.S)
            {
                interval /= 2;
                if (interval == 0)
                {
                    interval++;
                }
                timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            }
            else if (e.Key == Key.D)
            {
                interval *= 2;
                timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
            }
        }
    }
}
