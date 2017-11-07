using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using AAF.Library.Searcher;
using System.Data;
namespace AAF.ViewModel
{
    public class VMSearcher : BindableBase
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
        public string KeyWord { get; set; }

        public DelegateCommand SearchAll
        {
            get
            {
                return searchAll ?? (searchAll = new DelegateCommand(ExecuteSearchAll));
            }
        }

        DelegateCommand searchAll;

        public void ExecuteSearchAll()
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
                    var fs = new DataSearcher();
                    fs.Init(Path);
                    var xx = fs.SearchAll();
                    DataSource = xx.Items;
                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }
        public DelegateCommand SearchKeyWord
        {
            get
            {
                return searchKeyWord ?? (searchKeyWord = new DelegateCommand(ExecuteSearchKeyWord));
            }
        }

        DelegateCommand searchKeyWord;

        public void ExecuteSearchKeyWord()
        {
            if (Path==null || Path == "")
            {
                State = "请输入根目录";
                return;
            }
            if (KeyWord==null || KeyWord == "")
            {
                State = "请输入关键词";
                return;
            }

            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var fs = new DataSearcher();
                    fs.Init(Path);
                    var xx = fs.SearchStr(KeyWord);

                    DataSource = xx.Items;

                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

        public DelegateCommand SearchKeyWordInPath
        {
            get
            {
                return searchKeyWordInPath ?? (searchKeyWordInPath = new DelegateCommand(ExecuteSearchKeyWordInPath));
            }
        }

        DelegateCommand searchKeyWordInPath;

        public void ExecuteSearchKeyWordInPath()
        {
            if (Path == null || Path == "")
            {
                State = "请输入根目录";
                return;
            }
            if (KeyWord == null || KeyWord == "")
            {
                State = "请输入关键词";
                return;
            }
            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var fs = new DataSearcher();
                    fs.Init(Path);
                    var xx = fs.SearchStrInPath(KeyWord);
                    DataSource = xx.Items;
                    State = "解析完成！";
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

        public DelegateCommand SearchChineseWord
        {
            get
            {
                return searchChineseWord ?? (searchChineseWord = new DelegateCommand(ExecuteSearchChineseWord));
            }
        }

        DelegateCommand searchChineseWord;

        public void ExecuteSearchChineseWord()
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
                    var fs = new DataSearcher();
                    fs.Init(Path);
                    var xx = fs.SearchChineseStrInDB();
                    DataSource = xx.Items;
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
