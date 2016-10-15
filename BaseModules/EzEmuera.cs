using System;
using System.Windows.Forms;
using YeongHun.EmueraFramework;
using YeongHun.EmueraFramework.Function;
using YeongHun.EZTrans;

namespace BaseModules
{
    [ExternType]
    public static class EzTransModule
    {
        public static void EzEmuera(IFramework framework)
        {
            framework.Print("ezEmuera를 초기화 합니다");
            string ezTransPath;
            if (!framework.Config.TryGetValue("ezTransXP_Path", out ezTransPath))
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "ezTransXP 폴더를 선택해주세요";
                    dialog.ShowDialog();
                    ezTransPath = dialog.SelectedPath;
                    framework.Config.SetValue("ezTransXP_Path", ezTransPath);
                }
            }
            int result = TranslateXP.Initialize(ezTransPath);
            if (result != 0)
            {
                throw new Exception("ezEmuera 초기화에 실패했습니다 에러코드 " + result);
            }
            else
            {
                TranslateXP.LoadCache(framework.Root + "Cache.dat");
                TranslateXP.LoadDictionary(framework.Root + "UserDic.xml");
                framework.Print("ezEmuera 로딩 성공");
            }
        }
    }
}
