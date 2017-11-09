using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using AAF.Library.Extracter;
using AAF.Library;

namespace AAF.ViewModel
{
    public class VMExtractor : VMBase
    {

        public DelegateCommand DoWork
        {
            get
            {
                return doWork ?? (doWork = new DelegateCommand(ExecuteWork));
            }
        }

        DelegateCommand doWork;

        public void ExecuteWork()
        {

            if (Path == null || Path == "")
            {
                State = "请输入根目录";
                return;
            }

            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    parseData();
                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

        public DelegateCommand DoWorkFromADB
        {
            get
            {
                return doWorkFromAdb ?? (doWorkFromAdb = new DelegateCommand(ExecuteWorkFromADB));
            }
        }

        DelegateCommand doWorkFromAdb;

        public void ExecuteWorkFromADB()
        {
            
            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Path = System.Environment.CurrentDirectory + "\\temp\\";
                    AAFManager.Instance.ExtractDataFromADB(Path);
                    parseData();
                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }
        void parseData()
        {
            var t = AAFManager.Instance.ParseData(Path);
            DataSource = t.Select(c => new ExtractDataBinder()
            {
                Name = c.Name,
                DisplayName = c.Desc,
                Children = c.TableItems.Where(cc =>
                                cc.Rule.ExtraInfo == null ||
                                !cc.Rule.ExtraInfo.IsHidden
                              ).Select(cc =>
                              new ExtractDataBinder()
                              {
                                  Name = cc.Rule.Name,
                                  DisplayName = cc.Rule.Desc,
                                  DataSource = cc.Table
                              }).ToList(),
                DataSource = c.TableItems.FirstOrDefault() == null ? null : c.TableItems.FirstOrDefault().Table
            }).ToList();
        }
    }
}
