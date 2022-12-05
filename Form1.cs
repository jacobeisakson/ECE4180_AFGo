using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Diagnostics;
using System.Runtime.InteropServices;


// for COM ports
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;


namespace AFG_GUI_2
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] colorThemes = { "Light", "Dark" };
            toolStripComboBox1.Items.AddRange(colorThemes);

            // Available waveforms
            string[] waveforms = { "DC", "Sine", "Square", "Triangle", "Ramp", "Pulse" };
            comboBox2.Items.AddRange(waveforms);

            // default waveform setup
            numericUpDown1.Value = 5;
            numericUpDown2.Value = 0;
            numericUpDown3.Value = 1;
            numericUpDown4.Value = 50;
            comboBox2.SelectedIndex = 0;

            // COM ports
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            comboBox1.SelectedIndex = 0;
        }


        private void portComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.Close();
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "The selected port could not be opened", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePreview();
        }

        private DateTime startTime;
        private DateTime endTime;

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.BackColor == Color.LightGreen)
            {
                button1.BackColor = Color.FromArgb(164, 9, 229);
                button1.Text = "Generate";

                // time handler
                endTime = DateTime.Now;
                textBox2.Text = endTime.ToString();
            }
            else if (button1.BackColor == Color.FromArgb(164, 9, 229))
            {
                button1.BackColor = Color.FromArgb(175, 1, 26);
                button1.Text = "Start";

                // clear old times
                textBox1.Clear();
                textBox2.Clear();

            }
            else
            {
                button1.BackColor = Color.LightGreen;
                button1.Text = "Stop";

                // time handler
                startTime = DateTime.Now;
                textBox1.Text = startTime.ToString();

            }

        }

        public void updatePreview()
        {
            // ---------------------------------------------------------------------------------------------
            //      DC OFFSET OUTPUT
            // ---------------------------------------------------------------------------------------------
            if (comboBox2.SelectedIndex == 0)
            {
                numericUpDown1.Enabled = false;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = false;  //Frequency off
                numericUpDown4.Enabled = false;  // Duty Cycle off

                // creating square wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;


                for (int i = 0; i < 512; i++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        Offset);
                }


                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";
            }
            // ---------------------------------------------------------------------------------------------
            //      SINE WAVE
            // ---------------------------------------------------------------------------------------------
            else if (comboBox2.SelectedIndex == 1)
            {

                numericUpDown1.Enabled = true;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = true;  //Frequency off
                numericUpDown4.Enabled = false;  // Duty Cycle off

                // creating sine wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;

                for (int i = 0; i < 512; i++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        A * Math.Sin((i * 2 * Math.PI) / 512) + Offset);
                }

                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";
            }


            // ---------------------------------------------------------------------------------------------
            //      SQUARE WAVE
            // ---------------------------------------------------------------------------------------------
            else if (comboBox2.SelectedIndex == 2)
            {
                numericUpDown1.Enabled = true;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = true;  //Frequency off
                numericUpDown4.Enabled = false;  // Duty Cycle off


                // creating square wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;


                for (int i = 0; i < 256; i++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        (A * 1) + Offset);
                }

                for (int x = 256; x < 512; x++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(x * (1 / f) / 511),
                        (A * -1) + Offset);
                }


                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";
            }


            // ---------------------------------------------------------------------------------------------
            //      TRIANGLE WAVE
            // ---------------------------------------------------------------------------------------------
            else if (comboBox2.SelectedIndex == 3)
            {
                numericUpDown1.Enabled = true;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = true;  //Frequency off
                numericUpDown4.Enabled = false;  // Duty Cycle off

                // creating triangle wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;

                double level = 0;

                for (int i = 0; i < 128; i++)
                {
                    level = level + (A / (512 / 4));
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        level + Offset);
                }

                for (int i = 128; i < 384; i++)
                {
                    level = level - (A / (512 / 4));
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        level + Offset);
                }

                for (int i = 384; i < 512; i++)
                {
                    level = level + (A / (512 / 4));
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        level + Offset);
                }


                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";

            }


            // ---------------------------------------------------------------------------------------------
            //      RAMP WAVE
            // ---------------------------------------------------------------------------------------------
            else if (comboBox2.SelectedIndex == 4)
            {
                numericUpDown1.Enabled = true;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = true;  //Frequency off
                numericUpDown4.Enabled = false;  // Duty Cycle off

                // creating ramp wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;

                double level = 0;

                for (int i = 0; i < 512; i++)
                {
                    level = level + (A / 512);
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        level + Offset);
                }


                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";
            }


            // ---------------------------------------------------------------------------------------------
            //      PULSE WAVE
            // ---------------------------------------------------------------------------------------------
            else if (comboBox2.SelectedIndex == 5)
            {
                numericUpDown1.Enabled = true;  // Amplitude off
                numericUpDown2.Enabled = true;  // Offset on
                numericUpDown3.Enabled = true;  //Frequency off
                numericUpDown4.Enabled = true;  // Duty Cycle on


                // creating square wave graph
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                chart1.Titles.Clear();

                chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart1.Titles.Add("Function");

                double A = (double)numericUpDown1.Value;
                double Offset = (double)numericUpDown2.Value;
                double f = (double)numericUpDown3.Value;
                double dutyCycle = (double)numericUpDown4.Value;


                for (int i = 0; i < 512 * (dutyCycle / 100); i++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(i * (1 / f) / 511),
                        (A * 1) + Offset);
                }

                for (int x = (int)(512 * (dutyCycle / 100)); x < 512; x++)
                {
                    chart1.Series["Series1"].Points.AddXY((double)(x * (1 / f) / 511),
                        (A * -1) + Offset);
                }


                // formatting X-Axis
                if (f <= 28)
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.## s";
                }
                else
                {
                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#.##E0 s";
                }

                // formatting Y-Axis
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#.# V";
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            updatePreview();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            updatePreview();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            updatePreview();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            updatePreview();
        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedIndex == 0)
            {
                updateColorTheme(0);
            } else
            {
                updateColorTheme(1);
            }
        }

        private void updateColorTheme(int color)
        {
            if (color == 0)
            {
                // main form colors
                this.BackColor = Color.White;
                this.ForeColor = Color.Black;

                // menu item colors
                toolStripMenuItem1.BackColor = Color.White;
                toolStripMenuItem1.ForeColor = Color.Black;

                toolStripComboBox1.BackColor = Color.White;
                toolStripComboBox1.ForeColor = Color.Black;

                // labels
                label1.BackColor = Color.White;
                label2.BackColor = Color.White;
                label3.BackColor = Color.White;
                label4.BackColor = Color.White;
                label5.BackColor = Color.White;
                label6.BackColor = Color.White;
                label7.BackColor = Color.White;
                label8.BackColor = Color.White;

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label7.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;

                // COM Port
                comboBox1.BackColor = Color.White;
                comboBox1.ForeColor = Color.Black;

                // waveform entries
                comboBox2.BackColor = Color.White;
                comboBox2.ForeColor = Color.Black;

                numericUpDown1.BackColor = Color.White;
                numericUpDown2.BackColor = Color.White;
                numericUpDown3.BackColor = Color.White;
                numericUpDown4.BackColor = Color.White;

                numericUpDown1.ForeColor = Color.Black;
                numericUpDown2.ForeColor = Color.Black;
                numericUpDown3.ForeColor = Color.Black;
                numericUpDown4.ForeColor = Color.Black;

                // time entries
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;

                textBox1.ForeColor = Color.Black;
                textBox2.ForeColor = Color.Black;


                // Start button
                button1.BackColor = Color.FromArgb(164, 9, 229);
                button1.ForeColor = Color.Black;

                // graph
                chart1.BackColor = Color.Silver;
                chart1.ForeColor = Color.Black;

                chart1.BorderlineColor = Color.DodgerBlue;
                chart1.Palette = ChartColorPalette.BrightPastel;

            } else
            {
                this.BackColor = Color.FromArgb(26, 26, 26);
                this.ForeColor = Color.White;

                // menu item colors
                toolStripMenuItem1.BackColor = Color.FromArgb(26, 26, 26);
                toolStripMenuItem1.ForeColor = Color.White;

                toolStripComboBox1.BackColor = Color.FromArgb(26, 26, 26);
                toolStripComboBox1.ForeColor = Color.White;

                // labels
                label1.BackColor = Color.FromArgb(26, 26, 26);
                label2.BackColor = Color.FromArgb(26, 26, 26);
                label3.BackColor = Color.FromArgb(26, 26, 26);
                label4.BackColor = Color.FromArgb(26, 26, 26);
                label5.BackColor = Color.FromArgb(26, 26, 26);
                label6.BackColor = Color.FromArgb(26, 26, 26);
                label7.BackColor = Color.FromArgb(26, 26, 26);
                label8.BackColor = Color.FromArgb(26, 26, 26);

                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
                label3.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                label5.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label7.ForeColor = Color.White;
                label8.ForeColor = Color.White;

                // COM Port
                comboBox1.BackColor = Color.FromArgb(26, 26, 26);
                comboBox1.ForeColor = Color.White;

                // waveform entries
                comboBox2.BackColor = Color.FromArgb(26, 26, 26);
                comboBox2.ForeColor = Color.White;

                numericUpDown1.BackColor = Color.FromArgb(26, 26, 26);
                numericUpDown2.BackColor = Color.FromArgb(26, 26, 26);
                numericUpDown3.BackColor = Color.FromArgb(26, 26, 26);
                numericUpDown4.BackColor = Color.FromArgb(26, 26, 26);

                numericUpDown1.ForeColor = Color.White;
                numericUpDown2.ForeColor = Color.White;
                numericUpDown3.ForeColor = Color.White;
                numericUpDown4.ForeColor = Color.White;

                // time entries
                textBox1.BackColor = Color.FromArgb(26, 26, 26);
                textBox2.BackColor = Color.FromArgb(26, 26, 26);

                textBox1.ForeColor = Color.White;
                textBox2.ForeColor = Color.White;


                // Start button
                button1.BackColor = Color.FromArgb(164, 9, 229);
                button1.ForeColor = Color.Black;

                // graph
                chart1.BackColor = Color.Silver;
                chart1.ForeColor = Color.White;

                chart1.BorderlineColor = Color.DodgerBlue;
                chart1.Palette = ChartColorPalette.BrightPastel;
            }
        }
    }
}
