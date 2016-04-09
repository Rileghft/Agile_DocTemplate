using System;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace MergeDataAndDoc
{
    [TestFixture]
    class TestGenDoc
    {
        [Test]
        public void testGenDoc()
        {
            StringReader dataSr = new StringReader("姓名\t學號\r\n小明\t12345678\r\n小花\t87654321\r\n");
            StringReader templateSr = new StringReader("學生 ${姓名} 學號 ${學號}");
            StringBuilder sBuilder = new StringBuilder();
            StringWriter docWr = new StringWriter(sBuilder);

            Program.GenDoc(dataSr, templateSr, docWr);

            String result = @"學生 小明 學號 12345678
學生 小花 學號 87654321
";
            Assert.That(sBuilder.ToString(), Is.EqualTo(result));
        }

        [Test]
        public void testGenDocIncompleteData()
        {
            StringReader dataSr = new StringReader("姓名\t費用\r\n小明\t100\r\n小花\t\r\n");
            StringReader templateSr = new StringReader(@"學生 ${姓名} 費用 ${費用}");
            StringBuilder sBuilder = new StringBuilder();
            StringWriter docWr = new StringWriter(sBuilder);

            Program.GenDoc(dataSr, templateSr, docWr);

            String result = @"學生 小明 費用 100
學生 小花 費用 
";
            Assert.That(sBuilder.ToString(), Is.EqualTo(result));
        }
    }
}
