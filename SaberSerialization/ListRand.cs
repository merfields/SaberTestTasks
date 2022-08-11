using System.IO;

namespace SaberSerialization
{

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream fileStream)
        {
            Serialization.SerializeList(fileStream, this);
        }

        public void Deserialize(FileStream fileStream)
        {
            Serialization.DeserializeList(fileStream, this);
        }
    }
}
