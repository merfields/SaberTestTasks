using System;
using System.IO;

namespace SaberSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            ListRand exampleList = new ListRand();

            Console.WriteLine("Enter 1 to Serialize a list");
            Console.WriteLine("Enter 2 to Deserialize a list");
            Console.WriteLine("And press Enter");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    using (FileStream fileStream = new FileStream("examplefile.txt", FileMode.OpenOrCreate))
                    {
                        FillExampleList(exampleList);
                        exampleList.Serialize(fileStream);
                    }
                    break;

                case "2":
                    using (FileStream fileStream = new FileStream("examplefile.txt", FileMode.Open))
                    {
                        exampleList.Deserialize(fileStream);
                        PrintList(exampleList);
                    }
                    break;

                default:
                    Console.WriteLine("Wrong symbol");
                    break;

            }
        }

        private static void FillExampleList(ListRand exampleList)
        {
            ListNode exampleNode1 = new ListNode { Data = "Example data number 1" };
            ListNode exampleNode2 = new ListNode { Data = "каждый охотник желает знать где сидит фазан", Prev = exampleNode1 };
            ListNode exampleNode3 = new ListNode { Data = "12345678", Prev = exampleNode2 };
            ListNode exampleNode4 = new ListNode { Data = "Случайная фраза № 4", Prev = exampleNode3 };
            ListNode exampleNode5 = new ListNode { Data = "вфывфыаывф", Prev = exampleNode4 };

            exampleNode1.Next = exampleNode2;
            exampleNode2.Next = exampleNode3;
            exampleNode3.Next = exampleNode4;
            exampleNode4.Next = exampleNode5;

            exampleList.Head = exampleNode1;
            exampleList.Tail = exampleNode5;

            exampleNode1.Rand = exampleNode5;
            exampleNode2.Rand = exampleNode2;
            exampleNode3.Rand = exampleNode5;
            exampleNode4.Rand = exampleNode1;
            exampleNode5.Rand = exampleNode3;

            exampleList.Count = 5;
        }

        private static void PrintList(ListRand list)
        {
            ListNode currentNode = list.Head;

            Console.WriteLine();
            Console.WriteLine("Printing serialized list:");
            Console.WriteLine();

            while (currentNode != null)
            {
                Console.Write(@"This node data:" + currentNode.Data);
                Console.WriteLine(@"; It's random node data:" + currentNode.Rand.Data);
                Console.WriteLine();

                currentNode = currentNode.Next;
            }
        }

        private static void ClearList(ListRand exampleList)
        {
            exampleList.Head = null;
            exampleList.Tail = null;
            exampleList.Count = 0;
        }


    }
}
