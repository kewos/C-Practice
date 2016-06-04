using System.Linq;
using System.Xml;
using CSharpNote.Common.Attributes;
using CSharpNote.Core.Implements;

namespace CSharpNote.Data.CSharpPractice.Implement
{
    public class ProductXml : AbstractExecuteModule
    {
        [AopTarget]
        public override void Execute()
        {
            var address = "";
            var xdoc = new XmlDocument();
            // �إ߮ڸ`�I����å[�J XmlDocument �� (�� 0 �h)
            var rootElement = xdoc.CreateElement("objective");
            xdoc.AppendChild(rootElement);
            var file = GetFile();
            var total = file.Count();
            for (int i = 1, j = 1; i <= total; i++, j++)
            {
                //�w�����
                if (i > 65 && i < 77
                    || i > 116 && i < 120)
                {
                    j--;
                    total++;
                    continue;
                }
                var eleChild1 = xdoc.CreateElement("key");
                var attChild1 = xdoc.CreateAttribute("name");
                attChild1.Value = file[j - 1];
                var attChild2 = xdoc.CreateAttribute("value");
                attChild2.Value = i.ToString();
                eleChild1.Attributes.Append(attChild1);
                eleChild1.Attributes.Append(attChild2);
                rootElement.AppendChild(eleChild1);
                // �N�إߪ� XML �`�I�x�s���ɮ�
                xdoc.Save(address);
            }
        }

        private string[] GetFile()
        {
            //excel row data
            var file = @"";
            return file.Split(null).Where(s => s != "").ToArray();
        }
    }
}