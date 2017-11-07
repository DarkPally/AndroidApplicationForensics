using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using AAF.Library.Extractor;
using AAF.Library;

namespace AAF.ViewModel
{
    public class VMExtractor : BindableBase
    {
        object dataSource;
        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;
                    RaisePropertyChanged("DataSource");
                }
            }
        }

        string state = "准备就绪";
        public string State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    RaisePropertyChanged("State");
                }
            }
        }

        public string Path { get; set; }

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
                    var t = AAFManager.Instance.ExtractData(Path);
                    DataSource= t.Select(c => new ExtractDataBinder()
                    {
                        Name = c.Name,
                        DisplayName = c.Desc,
                        Children = c.TableItems.Select(cc =>
                                     new ExtractDataBinder()
                                     {
                                         Name=cc.Rule.Name,
                                         DisplayName=cc.Rule.Desc,
                                         DataSource=cc.Table
                                     }).ToList(),
                        DataSource= c.TableItems.FirstOrDefault()==null?null: c.TableItems.FirstOrDefault().Table
                    }).ToList();
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
                    var t = AAFManager.Instance.ExtractDataFromADB();
                    DataSource = t.Select(c => new ExtractDataBinder()
                    {
                        Name = c.Name,
                        DisplayName = c.Desc,
                        Children = c.TableItems.Select(cc =>
                                     new ExtractDataBinder()
                                     {
                                         Name = cc.Rule.Name,
                                         DisplayName = cc.Rule.Desc,
                                         DataSource = cc.Table
                                     }).ToList(),
                        DataSource = c.TableItems.FirstOrDefault() == null ? null : c.TableItems.FirstOrDefault().Table
                    }).ToList();
                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

    }
}
