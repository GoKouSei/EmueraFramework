using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Draw
{
    public sealed class DrawSetting: LoadableConfig
    {
        public IStringCalculator StringCalculator { get; private set; }
        public IColorSelector ColorSelector { get; private set; }
        [LoadableProperty(DefaultValue = "10")]
        public int FontSize { get; private set; }
        [LoadableProperty(DefaultValue = "10")]
        public int TextColor { get; private set; }
        public int BackGroundColor { get; private set; }

        public void SetTextColor(int color) => TextColor = color;
        public void SetBGColor(int color) => BackGroundColor = color;
    }
}
