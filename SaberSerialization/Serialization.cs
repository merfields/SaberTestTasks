using System.Text;
using System.IO;
using System.Collections.Generic;

namespace SaberSerialization
{
    static class Serialization
    {
        //Contains all nodes from the list, paired with it's index in the list, starting from 1
        private static Dictionary<ListNode, int> nodeIndexes = new Dictionary<ListNode, int>();

        //Contains all indexes of nodes referred to by each node in order
        private static List<int> randNodesInOrder = new List<int>();

        public static void SerializeList(FileStream fileStream, ListRand listToSerialize)
        {
            FillNodeIndexDictionary(listToSerialize);
            StringBuilder stringBuilder = new StringBuilder();
            fileStream.SetLength(0);

            ListNode currentNode = listToSerialize.Head;
            while (currentNode != null)
            {
                stringBuilder.Append(@"[Data = " + currentNode.Data + "; Rand = " + nodeIndexes[currentNode.Rand] + "]");
                stringBuilder.AppendLine();
                currentNode = currentNode.Next;
            }

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.Write(stringBuilder.ToString());
            }
        }

        private static void FillNodeIndexDictionary(ListRand list)
        {
            ListNode currentNode = list.Head;
            int index = 1;
            while (currentNode != null)
            {
                nodeIndexes.Add(currentNode, index);
                index++;
                currentNode = currentNode.Next;
            }
        }

        public static void DeserializeList(FileStream fileStream, ListRand targetList)
        {
            List<string> nodesInText = GetLinesFromFile(fileStream);

            GetDataFromText(nodesInText, targetList);
        }

        private static List<string> GetLinesFromFile(FileStream fileStream)
        {
            List<string> nodesInText = new List<string>();
            using (StreamReader streamReader = new StreamReader(fileStream))
            {
                string fileLine;

                while ((fileLine = streamReader.ReadLine()) != null)
                {
                    nodesInText.Add(fileLine);
                }
            }

            return nodesInText;
        }

        private static void GetDataFromText(List<string> nodesInText, ListRand targetList)
        {
            ListNode prevNode = null;
            List<ListNode> decerializedNodes = new List<ListNode>();

            foreach (var nodeInfo in nodesInText)
            {
                ListNode newNode = CreateNodeFromText(nodeInfo);

                GetRandomNodeIndexFromText(nodeInfo);

                if (prevNode != null)
                {
                    prevNode.Next = newNode;
                }

                newNode.Prev = prevNode;
                prevNode = newNode;
                decerializedNodes.Add(newNode);
            }

            ConnectRandomNodes(decerializedNodes);

            targetList.Head = decerializedNodes[0];
            targetList.Tail = decerializedNodes[decerializedNodes.Count - 1];
            targetList.Count = decerializedNodes.Count;
        }

        private static void ConnectRandomNodes(List<ListNode> decerializedNodes)
        {
            int i = 0;
            foreach (var node in decerializedNodes)
            {
                node.Rand = decerializedNodes?[randNodesInOrder[i] - 1];
                i++;
            }
        }

        private static void GetRandomNodeIndexFromText(string nodeInfo)
        {
            int from = nodeInfo.IndexOf("Rand = ") + "Rand = ".Length;
            int to = nodeInfo.IndexOf("]");
            int randIndex = int.Parse(nodeInfo.Substring(from, to - from));

            randNodesInOrder.Add(randIndex);
        }

        private static ListNode CreateNodeFromText(string nodeInfo)
        {
            ListNode createdNode = new ListNode();

            int from = nodeInfo.IndexOf("Data = ") + "Data = ".Length;
            int to = nodeInfo.IndexOf("; ");
            string data = nodeInfo.Substring(from, to - from);

            createdNode.Data = data;

            return createdNode;
        }
    }
}