using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace AAF.Library.Searcher
{
    public class XmlHelper
    {
        static public string getXmlPathSingle(XmlNode node)
        {
            var basePath = "/" + node.LocalName;
            if (node.ParentNode == null) return basePath;
            if (node.Attributes.Count>0) //一般来说都是name=""
            {
                return String.Format("{0}[@{1}={2}]", basePath, node.Attributes[0].Name, node.Attributes[0].Value);
            }
            else //基本不会运行
            {
                var t = node.ParentNode.SelectNodes(basePath);
                if (t.Count == 1) return basePath;
                for(int i=0;i<t.Count;++i)
                {
                    if(t[i]==node)
                    {
                        return String.Format("{0}[{1}]", basePath,i);            
                    }
                }
            }
            return basePath;


        }
        static public string getXmlPath(XmlNode node)
        {
            var t = node.ParentNode;
            var path = getXmlPathSingle(node);
            while(t!=null)
            {
                path = getXmlPathSingle(t) + path;
            }
            return path;
        }

        
    }
}
