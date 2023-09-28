using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HammingCodeSimulator
{
    public partial class frmMain : Form
    {
        private int[] bits;
        private int s1;
        private int s2;
        private int s3;
        private int s4;
        private int p;

        private enum State
        {
            Sending = 0,
            Reading = 1,
        }
        private State _state;
        private State state
        {
            get { return _state; }
            set
            {
                _state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }
        private string ErrorAddress;
        private Label ErrorLabel;
        private int ErrorBitIndex;
        private event PropertyChangedEventHandler PropertyChanged;
        public frmMain()
        {
            InitializeComponent();
            bits = new int[16];
            state = State.Sending;
            this.PropertyChanged += FrmMain_PropertyChanged;
            lbdBit3Value.Click += BitValue_Click;
            lbdBit5Value.Click += BitValue_Click;
            lbdBit6Value.Click += BitValue_Click;
            lbdBit7Value.Click += BitValue_Click;
            lbdBit9Value.Click += BitValue_Click;
            lbdBit10Value.Click += BitValue_Click;
            lbdBit11Value.Click += BitValue_Click;
            lbdBit12Value.Click += BitValue_Click;
            lbdBit13Value.Click += BitValue_Click;
            lbdBit14Value.Click += BitValue_Click;
            lbdBit15Value.Click += BitValue_Click;

            lbPValue.TextChanged += LbValue_TextChanged;
            lbS1Value.TextChanged += LbValue_TextChanged;
            lbS2Value.TextChanged += LbValue_TextChanged;
            lbS3Value.TextChanged += LbValue_TextChanged;
            lbS4Value.TextChanged += LbValue_TextChanged;
        }

        private void FrmMain_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(State))
            {
                lbdStateValue.Text = Enum.GetName(typeof(State), state);
                switch (state)
                {
                    case State.Sending:
                        lbdStateValue.ForeColor = Color.DarkGoldenrod;
                        break;
                    case State.Reading:
                        lbdStateValue.ForeColor = Color.Green;
                        break;
                }
            }
        }

        private void LbValue_TextChanged(object sender, EventArgs e)
        {
            var lb = (Label)sender;
            if (lb.Text.Equals("0"))
            {
                lb.ForeColor = Color.Green;
            }
            else
            {
                lb.ForeColor = Color.Red;
            }
        }
        private void BitValue_Click(object sender, EventArgs e)
        {
            ErrorLabel = (Label)sender;
            switch (ErrorLabel.Text)
            {
                case "1":
                    ErrorLabel.Text = "0";
                    break;
                case "0":
                    ErrorLabel.Text = "1";
                    break;
            }

            if (state == State.Reading)
            {
                var index = Convert.ToInt32(ErrorLabel.Tag);
                bits[index] = Convert.ToInt32(ErrorLabel.Text);
                ResetColors();
                RecalculateBits();

                ErrorAddress = $"{s4}{s3}{s2}{s1}";
                Color errorClr = Color.IndianRed;

                switch (ErrorAddress)
                {
                    case "0000":
                        //ResetColors();

                        ErrorBitIndex = 0;

                        break;
                    case "0001":
                        tlpBit1.BackColor = errorClr;
                        ErrorBitIndex = 1;
                        break;
                    case "0010":
                        tlpBit2.BackColor = errorClr;
                        ErrorBitIndex = 2;
                        break;
                    case "0011":
                        tlpBit3.BackColor = errorClr;
                        ErrorBitIndex = 3;
                        break;
                    case "0100":
                        tlpBit4.BackColor = errorClr;
                        ErrorBitIndex = 4;
                        break;
                    case "0101":
                        tlpBit5.BackColor = errorClr;
                        ErrorBitIndex = 5;
                        break;
                    case "0110":
                        tlpBit6.BackColor = errorClr;
                        ErrorBitIndex = 6;
                        break;
                    case "0111":
                        tlpBit7.BackColor = errorClr;
                        ErrorBitIndex = 7;
                        break;
                    case "1000":
                        tlpBit8.BackColor = errorClr;
                        ErrorBitIndex = 8;
                        break;
                    case "1001":
                        tlpBit9.BackColor = errorClr;
                        ErrorBitIndex = 9;
                        break;
                    case "1010":
                        tlpBit10.BackColor = errorClr;
                        ErrorBitIndex = 10;
                        break;
                    case "1011":
                        tlpBit11.BackColor = errorClr;
                        ErrorBitIndex = 11;
                        break;
                    case "1100":
                        tlpBit12.BackColor = errorClr;
                        ErrorBitIndex = 12;
                        break;
                    case "1101":
                        tlpBit13.BackColor = errorClr;
                        ErrorBitIndex = 13;
                        break;
                    case "1110":
                        tlpBit14.BackColor = errorClr;
                        ErrorBitIndex = 14;
                        break;
                    case "1111":
                        tlpBit15.BackColor = errorClr;
                        ErrorBitIndex = 15;
                        break;
                }
                if (btnErrorCase.Text == "One error mode")
                {
                    tmrFixError.Enabled = true;
                }
            }
        }

        private int Xor(params int[] bits2Check)
        {
            if (bits2Check.Where(b => b == 1).ToArray().Count() % 2 == 0)
                return 0;
            else
                return 1;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            state = State.Sending;
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = 0;
            }
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            p = 0;

            lbdBit0Value.Text = "X";
            lbdBit1Value.Text = "X";
            lbdBit2Value.Text = "X";
            lbdBit3Value.Text = "0";
            lbdBit4Value.Text = "X";
            lbdBit5Value.Text = "0";
            lbdBit6Value.Text = "0";
            lbdBit7Value.Text = "0";
            lbdBit8Value.Text = "X";
            lbdBit9Value.Text = "0";
            lbdBit10Value.Text = "0";
            lbdBit11Value.Text = "0";
            lbdBit12Value.Text = "0";
            lbdBit13Value.Text = "0";
            lbdBit14Value.Text = "0";
            lbdBit15Value.Text = "0";

            lbS1Value.Text = s1.ToString();
            lbS2Value.Text = s2.ToString();
            lbS3Value.Text = s3.ToString();
            lbS4Value.Text = s4.ToString();
            lbPValue.Text = p.ToString();

            ResetColors();

            pbFace.Image = Properties.Resources.sleeping_emoji;

            lbdBit0Value.Click -= BitValue_Click;
            lbdBit1Value.Click -= BitValue_Click;
            lbdBit2Value.Click -= BitValue_Click;
            lbdBit4Value.Click -= BitValue_Click;
            lbdBit8Value.Click -= BitValue_Click;
        }
        private void ResetColors()
        {
            tlpBit0.BackColor = SystemColors.ControlLight;
            tlpBit1.BackColor = Color.LightBlue;
            tlpBit2.BackColor = Color.LightBlue;
            tlpBit3.BackColor = Color.OldLace;
            tlpBit4.BackColor = Color.LightBlue;
            tlpBit5.BackColor = Color.OldLace;
            tlpBit6.BackColor = Color.OldLace;
            tlpBit7.BackColor = Color.OldLace;
            tlpBit8.BackColor = Color.LightBlue;
            tlpBit9.BackColor = Color.OldLace;
            tlpBit10.BackColor = Color.OldLace;
            tlpBit11.BackColor = Color.OldLace;
            tlpBit12.BackColor = Color.OldLace;
            tlpBit13.BackColor = Color.OldLace;
            tlpBit14.BackColor = Color.OldLace;
            tlpBit15.BackColor = Color.OldLace;

        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            btnReset.PerformClick();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (state == State.Sending)
            {
                pbFace.Image = Properties.Resources.glad_emoji;
                state = State.Reading;
                lbdBit0Value.Click += BitValue_Click;
                lbdBit1Value.Click += BitValue_Click;
                lbdBit2Value.Click += BitValue_Click;
                lbdBit4Value.Click += BitValue_Click;
                lbdBit8Value.Click += BitValue_Click;
                CalculateBits();
            }
        }
        private void CalculateBits()
        {
            bits[3] = Convert.ToInt32(lbdBit3Value.Text);
            bits[5] = Convert.ToInt32(lbdBit5Value.Text);
            bits[6] = Convert.ToInt32(lbdBit6Value.Text);
            bits[7] = Convert.ToInt32(lbdBit7Value.Text);
            bits[9] = Convert.ToInt32(lbdBit9Value.Text);
            bits[10] = Convert.ToInt32(lbdBit10Value.Text);
            bits[11] = Convert.ToInt32(lbdBit11Value.Text);
            bits[12] = Convert.ToInt32(lbdBit12Value.Text);
            bits[13] = Convert.ToInt32(lbdBit13Value.Text);
            bits[14] = Convert.ToInt32(lbdBit14Value.Text);
            bits[15] = Convert.ToInt32(lbdBit15Value.Text);

            bits[1] = Xor(bits[3], bits[5], bits[7], bits[9], bits[11], bits[13], bits[15]); //3,5,7,9,11,13,15
            bits[2] = Xor(bits[3], bits[6], bits[7], bits[10], bits[11], bits[14], bits[15]); //3,6,7,10,11, 14 , 15
            bits[4] = Xor(bits[5], bits[6], bits[7], bits[12], bits[13], bits[14], bits[15]);
            bits[8] = Xor(bits[9], bits[10], bits[11], bits[12], bits[13], bits[14], bits[15]);
            bits[0] = 0;
            bits[0] = Xor(bits);

            s1 = Xor(bits[1], bits[3], bits[5], bits[7], bits[9], bits[11], bits[13], bits[15]);
            s2 = Xor(bits[2], bits[3], bits[6], bits[7], bits[10], bits[11], bits[14], bits[15]);
            s3 = Xor(bits[4], bits[5], bits[6], bits[7], bits[12], bits[13], bits[14], bits[15]);
            s4 = Xor(bits[8], bits[9], bits[10], bits[11], bits[12], bits[13], bits[14], bits[15]);
            p = Xor(bits);

            lbdBit0Value.Text = bits[0].ToString();
            lbdBit1Value.Text = bits[1].ToString();
            lbdBit2Value.Text = bits[2].ToString();
            lbdBit4Value.Text = bits[4].ToString();
            lbdBit8Value.Text = bits[8].ToString();

            lbS1Value.Text = s1.ToString();
            lbS2Value.Text = s2.ToString();
            lbS3Value.Text = s3.ToString();
            lbS4Value.Text = s4.ToString();

            lbPValue.Text = p.ToString();
        }
        private void RecalculateBits()
        {
            s1 = Xor(bits[1], bits[3], bits[5], bits[7], bits[9], bits[11], bits[13], bits[15]);
            s2 = Xor(bits[2], bits[3], bits[6], bits[7], bits[10], bits[11], bits[14], bits[15]);
            s3 = Xor(bits[4], bits[5], bits[6], bits[7], bits[12], bits[13], bits[14], bits[15]);
            s4 = Xor(bits[8], bits[9], bits[10], bits[11], bits[12], bits[13], bits[14], bits[15]);
            p = Xor(bits);

            if (s1 == 0 && s2 == 0 && s3 == 0 & s4 == 0 && p == 0)
            {
                pbFace.Image = Properties.Resources.happy_emoji;
            }
            else if (s1 == 0 && s2 == 0 && s3 == 0 & s4 == 0 && p == 1)
            {
                pbFace.Image = Properties.Resources.dead_emoji;
                Color errorClr = Color.IndianRed;
                tlpBit1.BackColor = errorClr;
                tlpBit2.BackColor = errorClr;
                tlpBit3.BackColor = errorClr;
                tlpBit4.BackColor = errorClr;
                tlpBit5.BackColor = errorClr;
                tlpBit6.BackColor = errorClr;
                tlpBit7.BackColor = errorClr;
                tlpBit8.BackColor = errorClr;
                tlpBit9.BackColor = errorClr;
                tlpBit10.BackColor = errorClr;
                tlpBit11.BackColor = errorClr;
                tlpBit12.BackColor = errorClr;
                tlpBit13.BackColor = errorClr;
                tlpBit14.BackColor = errorClr;
                tlpBit15.BackColor = errorClr;
            }
            else if ((s1 == 1 || s2 == 1 || s3 == 1 || s4 == 1) && p == 0)
            {
                pbFace.Image = Properties.Resources.dead_emoji;
            }
            else
            {
                pbFace.Image = Properties.Resources.shock_emoji;
            }
            lbdBit0Value.Text = bits[0].ToString();
            lbdBit1Value.Text = bits[1].ToString();
            lbdBit2Value.Text = bits[2].ToString();
            lbdBit4Value.Text = bits[4].ToString();
            lbdBit8Value.Text = bits[8].ToString();

            lbS1Value.Text = s1.ToString();
            lbS2Value.Text = s2.ToString();
            lbS3Value.Text = s3.ToString();
            lbS4Value.Text = s4.ToString();

            lbPValue.Text = p.ToString();
        }

        private void tmrFixError_Tick(object sender, EventArgs e)
        {
            
            tmrFixError.Enabled = false;
            this.Invoke(new Action(() =>
            {
                Color normalColor = Color.OldLace;
                switch (ErrorAddress)
                {
                    case "0000":
                        //ResetColors();
                        break;
                    case "0001":
                        tlpBit1.BackColor = normalColor;
                        break;
                    case "0010":
                        tlpBit2.BackColor = normalColor;
                        break;
                    case "0011":
                        tlpBit3.BackColor = normalColor;
                        break;
                    case "0100":
                        tlpBit4.BackColor = normalColor;
                        break;
                    case "0101":
                        tlpBit5.BackColor = normalColor;
                        break;
                    case "0110":
                        tlpBit6.BackColor = normalColor;
                        break;
                    case "0111":
                        tlpBit7.BackColor = normalColor;
                        break;
                    case "1000":
                        tlpBit8.BackColor = normalColor;
                        break;
                    case "1001":
                        tlpBit9.BackColor = normalColor;
                        break;
                    case "1010":
                        tlpBit10.BackColor = normalColor;
                        break;
                    case "1011":
                        tlpBit11.BackColor = normalColor;
                        break;
                    case "1100":
                        tlpBit12.BackColor = normalColor;
                        break;
                    case "1101":
                        tlpBit13.BackColor = normalColor;
                        break;
                    case "1110":
                        tlpBit14.BackColor = normalColor;
                        break;
                    case "1111":
                        tlpBit15.BackColor = normalColor;
                        break;
                }
                if (ErrorBitIndex != 0)
                {
                    switch (ErrorLabel.Text)
                    {
                        case "1":
                            ErrorLabel.Text = "0";
                            break;
                        case "0":
                            ErrorLabel.Text = "1";
                            break;
                    }
                    switch (bits[ErrorBitIndex])
                    {
                        case 1:
                            bits[ErrorBitIndex] = 0;
                            break;
                        case 0:
                            bits[ErrorBitIndex] = 1;
                            break;
                    }
                    RecalculateBits();
                }
               
              
            }));
           
        }

        private void btnErrorCase_Click(object sender, EventArgs e)
        {
            switch (btnErrorCase.Text)
            {
                case "One error mode":
                    btnErrorCase.Text = "Many errors mode";
                    break;
                case "Many errors mode":
                    btnErrorCase.Text = "One error mode";
                    break;
            }
        }
    }
}
