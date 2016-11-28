using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Drawing
{
   public static class Extensions
    {
        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        public static Color FromHsv(this Color color, double h, double S, double V)
        {
            int r = 0, g = 0, b = 0;

            double H = Math.Abs(h) % 361;
            double R, G, B;

            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv + 0.01;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv + 0.01;
                        break;
                    case 2:
                        R = pv + 0.01;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv + 0.01;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv + 0.01;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv + 0.01;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv + 0.01;
                        break;
                    case -1:
                        R = V;
                        G = pv + 0.01;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }

            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Reverse Color RGB
        /// </summary>
        /// <param name="color">Input Color</param>
        /// <returns>Reversed Color</returns>
        public static Color ReverseColor(this Color color)
        {
            int R = 0, G = 0, B = 0;
            R = 255 - color.R;
            G = 255 - color.G;
            B = 255 - color.B;
            return Color.FromArgb(R, G, B);
        }
        /// <summary>
        /// Gets ForeColor by Input Color
        /// </summary>
        /// <param name="color">Input Color</param>
        /// <returns>Fore Color</returns>
        public static Color GetForeColor(this Color color)
        {
            int R = 0, G = 0, B = 0;
            if (color.GetBrightness() > 0.1f)
            {
                R = color.R % 100;
                G = color.G % 100;
                B = color.B % 100;
            }
            else
            {
                R = 100 + (color.R % 127);
                G = 100 + (color.G % 127);
                B = 100 + (color.B % 127);
            }

            return Color.FromArgb(R, G, B);
        }
        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
