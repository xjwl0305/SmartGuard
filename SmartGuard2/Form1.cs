using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartGuard2
{
    
    class TimerState
    {
        public int Counter;
    }
    public partial class Form1 : Form
    {
        private TimerCallback TimerCallback;
        private System.Threading.Timer timer;
        private int counter = 0;
        public void timer_tick(object sender)
        {
            counter++;
            Process[] processes = Process.GetProcessesByName("LogAnalysisApp");
            if (processes.Length == 0)
            {
                //label1.Text = "Not running";
                Process ps = new Process();
                ps.StartInfo.FileName = "LogAnalysisApp";
                ps.StartInfo.WorkingDirectory = @"C:\logFile";
                ps.Start();

            }
            else{
                foreach (Process Pr in processes)
                {
                    // if (!Pr.EnableRaisingEvents) //종료이벤트 수신이 되어 있지 않으면 이벤트를 수신하도록 설정
                    // {
                    //     //Pr.Exited += ProcessExited;
                    //     Pr.EnableRaisingEvents = true; //종료 이벤트 수락   
                    //     Console.WriteLine('3');
                    // }
                    Program.KillErrorProcess(Pr.ProcessName, Pr.Id); //에러창을 강제로 닫기
                }
            }

            //label1.Text = counter.ToString();
        }

        public void click(object sender, EventArgs args)
        {
            TimerCallback = new TimerCallback(this.timer_tick);
            var timerState = new TimerState { Counter = 0 };
            timer = new System.Threading.Timer(timer_tick, timerState, 0, 20000);
            button1.Text = "start!";
            
        }
        
        public Form1()
        {
            InitializeComponent();
            button1.Click += click;
        }
    }
}