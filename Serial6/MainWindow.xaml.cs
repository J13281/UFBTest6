using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace Serial6
{
    public partial class MainWindow : Window
    {
        Button[] buttons;
        SerialPort serial;

        public MainWindow()
        {
            InitializeComponent();

            buttons = new[] { btn_0, btn_1, btn_2, btn_3, btn_4, btn_5, btn_6, btn_7, btn_8, btn_9, btn_10, btn_11, };

            serial = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600
            };
            serial.Open();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        double wait = 1.0 / 240.0;
        double next = Environment.TickCount;

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Environment.TickCount < next) return;

            next += wait;

            var status = m2(buttons);
            serial.Write(status, 0, status.Length);
        }

        static byte[] m2(Button[] buttons)
        {
            var ans = new byte[] {
                0b00000000,
                0b00100000,
                0b01000000,
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                var x = buttons[i].IsPressed ? 0 : 1;
                ans[i / 4] |= (byte)(x << i % 4);
            }
            return ans;
        }
    }
}
