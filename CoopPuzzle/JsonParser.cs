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
            int id = Convert.ToInt32(obj.GetValue("Id"));
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
        public static void WriteJsonToFile(string fileName, List<GameObject> gList)
        {
            JArray enemyArray = new JArray();
            JArray platformArray = new JArray();
            JObject bigobj = new JObject();

            JArray array = new JArray();
            for (int i = 0; i < gList.Count; i++)
            {
                //Ger bara rött för att vi inte har det än
                //if (gList[i] is Enemy)
                //{
                //    JObject obj = CreateObject(gList[i].hitBox);
                //    enemyArray.Add(obj);
                //}
                //else if (gList[i] is Platform)
                //{
                //    JObject obj = CreateObject(gList[i].hitBox);
                //    platformArray.Add(obj);
                //}
                //else if (gList[i] is Player)
                //{
                //    JObject obj = CreateObject(gList[i].hitBox);
                //    bigobj.Add("player", obj);
                //}
            }

            bigobj.Add("enemies", enemyArray);
            bigobj.Add("platforms", platformArray);

            File.WriteAllText(fileName, bigobj.ToString());
        }

        private static JObject CreateObject(Rectangle rect)
        {
            JObject obj = new JObject();
            obj.Add("positionX", rect.X);
            obj.Add("positionY", rect.Y);
            obj.Add("height", rect.Height);
            obj.Add("width", rect.Width);
            obj.Add("Id", 1);
            obj.Add("Type", "static");

            return obj;
        }
    }


}
