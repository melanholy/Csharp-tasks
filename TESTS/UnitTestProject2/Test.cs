using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = new[] { 
                    "a '\\'",                                                                   //0
                    "Let's buy some piece and quite for a change",                        //1
                    "Beer",                                                               //2
                    "Math               is boring. ",                                     //3
                    @"Well my baby's ""gone with the"" wind.",                            //4
                    @"Now I feel 'the ""wind blow"" outside' my door,",                   //5
                    "My baby's gone with the wind.'",                                     //6
                    @"My baby's gone'"" with the wind"".",                                //7
                    @"My baby's gone'w""ith the wind"".",                                 //8
                    "Train roll on, on down the line,",                                   //9
                    "Please ' take me far, aw'ay",                                        //10
                    "Tuesday's gone with 'the wind'.",                                    //11
                    "Tuesday's gone with 'the wind'.",                                    //12
                    "'Beer'",                                                             //13
                    "'Beer",                                                              //14
                    "I\\am",                                                              //15
                    "'Iam\\",                                                             //16
                    @"I am n'in'""e yar""s'dea'd",                                        //17
                    @"a v a""a v""",                                                      //18
                    @"""a\ \""c\""""",                                                    //19
                    @"a\""\""\""\\""vds",                                                 //20
                    "One final(or not) test",                                             //21
                    @"This ""test \\is really the last one" };                            //22
            Assert.AreEqual("\\", Tests.Program.FindSpaces(text[0], 2));
            Assert.AreEqual("s buy some piece and quite for a change", Tests.Program.FindSpaces(text[1], 2));
            Assert.AreEqual("null", Tests.Program.FindSpaces(text[2], 4));
            Assert.AreEqual("is", Tests.Program.FindSpaces(text[3], 2));
            Assert.AreEqual("s gone with the wind.", Tests.Program.FindSpaces(text[4], 4));
            Assert.AreEqual("the wind blow outside", Tests.Program.FindSpaces(text[5], 4));
            Assert.AreEqual("null", Tests.Program.FindSpaces(text[6], 4));
            Assert.AreEqual(" with the wind", Tests.Program.FindSpaces(text[7], 4));
            Assert.AreEqual("w", Tests.Program.FindSpaces(text[8], 4));
            Assert.AreEqual("on", Tests.Program.FindSpaces(text[9], 4));
            Assert.AreEqual("ay", Tests.Program.FindSpaces(text[10], 3));
            Assert.AreEqual("s gone with ", Tests.Program.FindSpaces(text[11], 2));
            Assert.AreEqual(".", Tests.Program.FindSpaces(text[12], 5));
            Assert.AreEqual("Beer", Tests.Program.FindSpaces(text[13], 1));
            Assert.AreEqual("Beer", Tests.Program.FindSpaces(text[14], 1));
            Assert.AreEqual("I\\am", Tests.Program.FindSpaces(text[15], 1));
            Assert.AreEqual(@"Iam\", Tests.Program.FindSpaces(text[16], 1));
            Assert.AreEqual("dea", Tests.Program.FindSpaces(text[17], 7));
            Assert.AreEqual("a", Tests.Program.FindSpaces(text[18], 3));
            Assert.AreEqual(@"a\ ""c""", Tests.Program.FindSpaces(text[19], 1));
            Assert.AreEqual(@"""""\", Tests.Program.FindSpaces(text[20], 2));
            Assert.AreEqual("final(or", Tests.Program.FindSpaces(text[21], 2));
            Assert.AreEqual(@"test \is really the last one", Tests.Program.FindSpaces(text[22], 2));
        }
    }
}
