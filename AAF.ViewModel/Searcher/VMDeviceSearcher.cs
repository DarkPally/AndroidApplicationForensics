using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using AAF.Library.Extracter;
using System.Data;
namespace AAF.ViewModel
{
    public class VMDeviceSearcher : VMBase
    {
        public VMDeviceSearcher()
        {
            Path = "data/data/";
        }
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
            if (Path == null)
            {
                State = "请输入根目录";
                return;
            }
            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    searchDevice("*");


                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }
        public DelegateCommand SearchFile
        {
            get
            {
                return searchFile ?? (searchFile = new DelegateCommand(ExecuteSearchFile));
            }
        }

        DelegateCommand searchFile;

        public void ExecuteSearchFile()
        {
            if (Path==null )
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
                    searchDevice(KeyWord);
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

        public DelegateCommand SearchPicture
        {
            get
            {
                return searchPicture ?? (searchPicture = new DelegateCommand(ExecuteSearchPicture));
            }
        }

        DelegateCommand searchPicture;

        public void ExecuteSearchPicture()
        {
            if (Path == null)
            {
                State = "请输入根目录";
                return;
            }
            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {

                    searchDevice("*.jpg");
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }

        void searchDevice(string keyword)
        {
            var fs = new ShellScriptFileExtracter();
            fs.InitConnection();
            var device = fs.Devices.First();
            var xx = fs.SearchFilesVerbose(device, Path, keyword, Library.Extracter.Type.file);
            if (xx.success)
            {
                var t = xx.filesProperty.Select(c => new DeviceSearchBinder()
                {
                    AccessTime = c.accessTime,
                    Size = c.size,
                    ModifyTime = c.modifyTime,
                    FileName = c.path,
                    Type = c.type.ToString()
                }).ToList();
                DataSource = t;

                State = "解析完成！";
            }
            else
            {
                State = "设备连接异常";
            }
        }

        public DelegateCommand CopyFileToPC
        {
            get
            {
                return copyFileToPC ?? (copyFileToPC = new DelegateCommand(ExecuteCopyFileToPC));
            }
        }

        DelegateCommand copyFileToPC;

        public void ExecuteCopyFileToPC()
        {
            if (DataSource == null)
            {
                State = "没有可导出的文件";
                return;
            }
            State = "解析中...";
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string PCRootPath = System.Environment.CurrentDirectory + "\\CopyFiles";
                    var tList=DataSource as List<DeviceSearchBinder>;
                    FileExtracter fe = new ShellScriptFileExtracter();
                    fe.InitConnection();
                    var device=fe.Devices.First();
                    foreach(var it in tList)
                    {
                        var tName = it.FileName.Replace('/', '\\');
                        string tPath;
                        if(tName[0]=='\\')
                        {
                            tPath = PCRootPath + tName;
                        }
                        else
                        {
                            tPath= PCRootPath +'\\'+ tName;
                        }
                        fe.CopyFileFromDevice(device, it.FileName, System.IO.Path.GetDirectoryName(tPath));
                    }
                    State = "解析完成！";

                    System.Diagnostics.Process.Start(PCRootPath);
                }
                catch
                {
                    State = "解析出现异常";
                }

            });
        }
    }
}
