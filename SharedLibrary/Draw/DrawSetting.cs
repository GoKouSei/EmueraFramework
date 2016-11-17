using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeongHun.Common.Config;

namespace YeongHun.EmueraFramework.Draw
{
    public sealed class DrawSetting: LoadableConfig
    {
        public IStringCalculator StringCalculator { get; private set; }

        public DrawSetting(IStringCalculator stringCalculator)
        {
            StringCalculator = stringCalculator;
        }

        protected override void AddParser(ConfigDic config)
        {
            config.AddParser(str =>
            {
                try
                {
                    return new Color(str);
                }
                catch
                {
                    return new Color(int.Parse(str));
                }
            });
        }

        [LoadableProperty(DefaultValue = "10")]
        public int FontSize { get; private set; }
        [LoadableProperty(DefaultValue = "White")]
        public Color TextColor { get; private set; }
        [LoadableProperty(DefaultValue = "Black")]
        public Color BackGroundColor { get; private set; }

        public void SetTextColor(Color color) => TextColor = color;
        public void SetBGColor(Color color) => BackGroundColor = color;
    }
}
