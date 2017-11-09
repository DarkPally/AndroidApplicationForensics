using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AAF.Library.Extracter.Android;

namespace AAF.Library.Extracter
{
    /// <summary>
    /// linux下的7种文件类型
    /// 不过在data目录中应该只有部分类型会存在
    /// </summary>
    public enum Type
    {
        alltype = 'a',
        fne = 'w', // file not exist

        directory = 'd',
        file = 'f',
        link = 'l',
        block = 'b',
        character = 'c',
        socket = 's',
        pipe = 'p'
    }

    /// <summary>
    /// 存放文件详细信息的数据结构
    /// </summary>
    class FileProperty
    {
        public Type type;
        public string path;
        public string size;
        public string modifyTime;
    }

    /// <summary>
    /// 函数调用之后统一返回的结果类型
    /// </summary>
    // enum State { noConnection, copyFail, invalidInput, unexpectedOutput, fileNotExist};
    class Result
    {
        public bool success;
        public string errorMessage;
        // public State state;
        public List<FileProperty> filesProperty;
        public Result()
        {
            filesProperty = new List<FileProperty>();
        }
    }

    /// <summary>
    /// 文件操作的接口
    /// </summary>
    interface FileExtracter
    {
        Result InitConnection();
        Result ListDirecotry(string device, string path);
        Result SearchFiles(string device, string path, string pattern);
        Result CopyFileFromDevice(string device, string devivePath, string pcPath);
        string[] Devices { get; }
    }

    class ShellFileExtracter : FileExtracter
    {
        string[] devices;

        public string[] Devices
        {
            get { return devices; }
        }

        List<FileProperty> ParaseProperties(string[] rawData)
        {
            List<FileProperty> result = new List<FileProperty>();
            foreach(string item in rawData)
            {
                if (item.Contains("No such file or directory")) continue;
                if (item.Contains("Permission denied")) continue;

                FileProperty property = new FileProperty();

                if (item[0] == 'd') property.type = Type.directory;
                if (item[0] == '-') property.type = Type.file;
                if (item[0] == 'l') property.type = Type.link;

                if (property.type == Type.file)
                {
                    string sizePattern = @"\b\d+\b";
                    property.size = Regex.Match(item, sizePattern).ToString();
                }

                string timePattern = @"\d{2}:\d{2}";
                property.modifyTime = Regex.Match(item, timePattern).ToString();

                string pathPattern = @"\b\S+$";
                property.path = Regex.Match(item, pathPattern).ToString().Replace("'", "");

                result.Add(property);
            }
            return result;
        }

        bool IsAccess(string device, string path)
        {
            var result = AdbHelper.ListDataFolder(device, path);
            if (result[0].Contains("Permission denied"))
                throw new Exception("Permission denied, Please Root First");
            else if (result[0].Contains("No such file or directory"))
                throw new Exception("Wrong Path, No such file or directory");
            return true;
        }

        public Result InitConnection()
        {
            Result result = new Result();
            try
            {
                devices = AdbHelper.GetDevices();
                if (devices.Length == 0)
                {
                    throw new Exception("No device detected!");
                }
                else
                {
                    result.success = true;
                    devices = AdbHelper.GetDevices();
                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMessage = ex.ToString();
            }
            return result;
        }

        public Result ListDirecotry(string device, string path)
        {
            Result result = new Result();
            try
            {
                if (IsAccess(device, path))
                {
                    var items = AdbHelper.ListDataFolder(device, path);
                    result.filesProperty = ParaseProperties(items);
                    result.success = true;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMessage = ex.ToString();
            }
            return result;

        }

        public Result SearchFiles(string device, string path, string pattern)
        {
            string searchScript;
            searchScript = System.String.Format(
                                               "find {0} -name \\\"{1}\\\" -type {2}|" +
                                               "while read i;do " +
                                               "echo \\\"$(ls -l \\\"$i\\\")\\\"; " +
                                               "done", path, pattern, (char)Type.file);
            // string scriptName = "/sdcard/utils/search" + path.Replace('/', '_');

            Result result = new Result();
            try
            {
                var items = AdbHelper.RunShell(device, searchScript);
                result.filesProperty = ParaseProperties(items);
                result.success = true;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMessage = ex.ToString();
            }
            return result;
        }

        public Result CopyFileFromDevice(string device, string devicePath, string pcPath)
        {
            Result result = new Result();
            try
            {
                if (IsAccess(device, devicePath))
                {
                    if (!System.IO.Directory.Exists(pcPath))
                        System.IO.Directory.CreateDirectory(pcPath);

                    AdbHelper.CopyFromDevice(device, devicePath, pcPath);
                    result.success = true;
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMessage = ex.ToString();
            }
            return result;
        }
    }
}