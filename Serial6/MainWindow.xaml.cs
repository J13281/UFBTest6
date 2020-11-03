using SharpDX.DirectInput;
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
        private Joystick pad;
        SerialPort serial;

        public MainWindow()
        {
            InitializeComponent();

            buttons = new[] { btn_0, btn_1, btn_2, btn_3, btn_4, btn_5, btn_6, btn_7, btn_8, btn_9, btn_10, btn_11, };

            var dinput = new DirectInput();

            var guid = dinput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices)
                .First().InstanceGuid;

            pad = new Joystick(dinput, guid);

            serial = new SerialPort
            {
                PortName = "COM3",
                BaudRate = 9600
            };
            serial.Open();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        double wait = 120.0 / 1000.0;
        double next = Environment.TickCount;

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Environment.TickCount < next) return;

            next += wait;

            pad.Acquire();
            pad.Poll();

            // var status = m2(ui_to_buttons(buttons));
            var status = m2(pad_to_buttons(pad.GetCurrentState()));
            serial.Write(status, 0, status.Length);
        }


        static bool[] ui_to_buttons(Button[] state)
        {
            return new[] {
                state[0].IsPressed,
                state[1].IsPressed,
                state[2].IsPressed,
                state[3].IsPressed,
                state[4].IsPressed,
                state[5].IsPressed,
                state[6].IsPressed,
                state[7].IsPressed,
                state[8].IsPressed,
                state[9].IsPressed,
                state[10].IsPressed,
                state[11].IsPressed,
            };
        }

        static bool[] pad_to_buttons(JoystickState state)
        {
            return new[] {
                state.PointOfViewControllers[0] == 27000,
                state.PointOfViewControllers[0] == 0,
                state.PointOfViewControllers[0] == 9000,
                state.PointOfViewControllers[0] == 18000,

                state.Buttons[0],
                state.Buttons[3],
                state.Buttons[2],
                state.Buttons[1],

                state.Buttons[4],
                state.Buttons[6],
                state.Buttons[5],
                state.Buttons[7],
            };
        }

        static byte[] m2(bool[] buttons)
        {
            var ans = new byte[] {
                0b00000000,
                0b00100000,
                0b01000000,
            };

            for (int i = 0; i < buttons.Length; i++)
            {
                var x = buttons[i] ? 0 : 1;
                ans[i / 4] |= (byte)(x << i % 4);
            }
            return ans;
        }
    }
}
