using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System;

namespace CoopPuzzle
{
    internal class JsonParser
    {

        static JObject wholeObj;
        static string currentFileName;

        public static void GetJObjectFromFile(string fileName)
        {
            currentFileName = fileName;
            StreamReader file = File.OpenText(currentFileName);
            JsonTextReader reader = new JsonTextReader(file);
            wholeObj = JObject.Load(reader);
        }

        public static Rectangle GetRectangle(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetRectangle(obj);
        }

        public static List<Rectangle> GetRectangleList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<Rectangle> rectList = new List<Rectangle>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                Rectangle rect = GetRectangle(obj);
                rectList.Add(rect);
            }
            return rectList;
        }
        private static Rectangle GetRectangle(JObject obj)
        {
            int x = Convert.ToInt32(obj.GetValue("positionX"));
            int y = Convert.ToInt32(obj.GetValue("positionY"));
            int height = Convert.ToInt32(obj.GetValue("height"));
            int width = Convert.ToInt32(obj.GetValue("width"));

            Rectangle rect = new Rectangle(x, y, width, height);
            return rect;
        }

        public static Vector2 GetPos(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetPos(obj);
        }

        public static List<Vector2> GetPosList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<Vector2> posList = new List<Vector2>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                Vector2 pos = GetPos(obj);
                posList.Add(pos);
            }

            return posList;
        }

        private static Vector2 GetPos(JObject obj)
        {
            Vector2 pos = new Vector2(Convert.ToInt32(obj.GetValue("positionX")), Convert.ToInt32(obj.GetValue("positionY")));
            return pos;
        }

        public static int GetId(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetId(obj);
        }

        public static List<int> GetIdList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<int> idList = new List<int>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                int id = GetId(obj);
                idList.Add(id);
            }

            return idList;
        }
        private static int GetId(JObject obj)
        {
            int id = Convert.ToInt32(obj.GetValue("id"));
            return id;
        }

        public static List<string> GetTypeList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<string> typeList = new List<string>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                string type = GetType(obj);
                typeList.Add(type);
            }

            return typeList;
        }
        public static string GetType(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetType(obj);
        }

        private static string GetType(JObject obj)
        {
            string type = Convert.ToString(obj.GetValue("Type"));
            return type;
        }

        public static List<Vector2> GetTeleVecList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<Vector2> teleVecList = new List<Vector2>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                Vector2 teleVec = GetTeleVec(obj);
                teleVecList.Add(teleVec);
            }

            return teleVecList;
        }
        private static Vector2 GetTeleVec(JObject obj)
        {
            float x = Convert.ToInt32(obj.GetValue("warpX"));
            float y = Convert.ToInt32(obj.GetValue("warpY"));

            Vector2 teleVec = new Vector2(x, y);
            return teleVec;
        }
        public static void WriteJsonToFile(string fileName, List<GameObject> objects, Player[] players)
        {
            JObject player = CreateObject(players[0].Pos);
            JObject otherPlayer = CreateObject(players[1].Pos);

            JArray blockArray = new JArray();
            JArray checkPointArray = new JArray();
            JArray doorArray = new JArray();
            JArray switchArray = new JArray();
            JArray movableArray = new JArray();
            JArray trapArray = new JArray();

            JObject bigobj = new JObject();

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is Block)
                    blockArray.Add(CreateObject((Block)objects[i]));

                if (objects[i] is CheckPoint)
                    checkPointArray.Add(CreateObject(objects[i].hitbox));
                
                if (objects[i] is MovableBlock)
                    movableArray.Add(CreateObject(objects[i].hitbox));

                if (objects[i] is Door)
                    doorArray.Add(CreateObject((Door)objects[i]));

                if (objects[i] is WeighedSwitch)
                    switchArray.Add(CreateObject((WeighedSwitch)objects[i]));

                if (objects[i] is Trap)
                    trapArray.Add(CreateObject(objects[i].Pos));
            }

            bigobj.Add("player", player);
            bigobj.Add("otherPlayer", otherPlayer);
            bigobj.Add("block", blockArray);
            bigobj.Add("checkpoint", checkPointArray);
            bigobj.Add("door", doorArray);
            bigobj.Add("switch", switchArray);
            bigobj.Add("movable", movableArray);
            bigobj.Add("trap", trapArray);

            File.WriteAllText(fileName, bigobj.ToString());
        }

        private static JObject CreateObject(Rectangle rect)
        {
            JObject obj = new JObject();
            obj.Add("positionX", rect.X);
            obj.Add("positionY", rect.Y);
            obj.Add("height", rect.Height);
            obj.Add("width", rect.Width);

            return obj;
        }

        private static JObject CreateObject(Block block)
        {
            JObject obj = new JObject();
            obj.Add("positionX", block.Pos.X);
            obj.Add("positionY", block.Pos.Y);
            obj.Add("height", block.Size.X);
            obj.Add("width", block.Size.Y);

            int id = 0;
            for (int i = 0; i < Assets.colors.Length; i++)
            {
                if (block.Color == Assets.colors[i])
                    id = i;
            }
            obj.Add("id", id);

            return obj;
        }
        private static JObject CreateObject(WeighedSwitch block)
        {
            JObject obj = new JObject();
            obj.Add("positionX", block.Pos.X);
            obj.Add("positionY", block.Pos.Y);
            obj.Add("height", block.Size.X);
            obj.Add("width", block.Size.Y);

            int id = 0;
            for (int i = 0; i < Assets.colors.Length; i++)
            {
                if (block.Color == Assets.colors[i])
                    id = i;
            }
            obj.Add("id", id);

            return obj;
        }

        private static JObject CreateObject(Door door)
        {
            JObject obj = new JObject();
            obj.Add("positionX", door.Pos.X);
            obj.Add("positionY", door.Pos.Y);
            obj.Add("id", door.id);

            return obj;
        }
        private static JObject CreateObject(Vector2 pos)
        {
            JObject obj = new JObject();
            obj.Add("positionX", pos.X);
            obj.Add("positionY", pos.Y);

            return obj;
        }
    }
}
