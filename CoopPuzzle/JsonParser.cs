using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System;
using SharpDX.Direct2D1.Effects;

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

        public static int GetRotation(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetRotation(obj);
        }

        public static List<int> GetRotationList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<int> rotationList = new List<int>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                int rotation = GetRotation(obj);
                rotationList.Add(rotation);
            }

            return rotationList;
        }
        private static int GetRotation(JObject obj)
        {
            int rotation = Convert.ToInt32(obj.GetValue("rotation"));
            return rotation;
        }

        public static int GetSkin(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)wholeObj.GetValue(propertyName);
            return GetSkin(obj);
        }

        public static List<int> GetSkinList(string fileName, string propertyName)
        {
            if (wholeObj == null || currentFileName == null || currentFileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            List<int> skinList = new List<int>();
            JArray arrayObj = (JArray)wholeObj.GetValue(propertyName);
            for (int i = 0; i < arrayObj.Count; i++)
            {
                JObject obj = (JObject)arrayObj[i];
                int skin = GetSkin(obj);
                skinList.Add(skin);
            }

            return skinList;
        }
        private static int GetSkin(JObject obj)
        {
            int skin = Convert.ToInt32(obj.GetValue("skin"));
            return skin;
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
        public static void WriteJsonToFile(string fileName, List<GameObject> objects, List<BGTile> bgtiles, Player[] players, List<NPC> npcs)
        {
            JObject bigobj = new JObject();

            JObject player = CreateObject(players[0].Pos);
            JObject otherPlayer = CreateObject(players[1].Pos);

            JArray blockArray = new JArray();
            JArray checkPointArray = new JArray();
            JArray doorArray = new JArray();
            JArray switchArray = new JArray();
            JArray movableArray = new JArray();
            JArray trapArray = new JArray();

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
                    trapArray.Add(CreateObject((Trap)objects[i]));
            }

            bigobj.Add("player", player);
            bigobj.Add("otherPlayer", otherPlayer);
            bigobj.Add("block", blockArray);
            bigobj.Add("checkpoint", checkPointArray);
            bigobj.Add("door", doorArray);
            bigobj.Add("switch", switchArray);
            bigobj.Add("movable", movableArray);
            bigobj.Add("trap", trapArray);

            JArray hiddenNpcArray = new JArray();
            JArray hintNpcArray = new JArray();
            JArray storyNpcArray = new JArray();

            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i] is HiddenNpc)
                    hiddenNpcArray.Add(CreateObject((HiddenNpc)npcs[i]));
                else if (npcs[i] is HintNpc)
                    hintNpcArray.Add(CreateObject((HintNpc)npcs[i]));
                else if (npcs[i] is StoryNpc)
                    storyNpcArray.Add(CreateObject((StoryNpc)npcs[i]));
            }

            bigobj.Add("hiddenNpc", hiddenNpcArray);
            bigobj.Add("hintNpc", hintNpcArray);
            bigobj.Add("storyNpc", storyNpcArray);

            JArray bgArray = new JArray();

            for (int i = 0; i < bgtiles.Count; i++)
                bgArray.Add(CreateObject(bgtiles[i]));

            bigobj.Add("bgTile", bgArray);

            File.WriteAllText(fileName, bigobj.ToString());
            Debug.WriteLine($"Level saved to {fileName}");
        }

        private static JObject CreateObject(BGTile tile)
        {
            JObject obj = new JObject();
            obj.Add("positionX", tile.Pos.X);
            obj.Add("positionY", tile.Pos.Y);
            obj.Add("skin", tile.texNum);
            obj.Add("id", tile.rndSrc);

            return obj;
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
            obj.Add("height", block.Size.Y);
            obj.Add("width", block.Size.X);

            int id = 0;
            for (int i = 0; i < Assets.colors.Length; i++)
            {
                if (block.Color == Assets.colors[i])
                    id = i;
            }
            obj.Add("id", id);

            return obj;
        }
        private static JObject CreateObject(WeighedSwitch ws)
        {
            JObject obj = new JObject();
            obj.Add("positionX", ws.Pos.X);
            obj.Add("positionY", ws.Pos.Y);
            obj.Add("height", ws.Size.Y);
            obj.Add("width", ws.Size.X);
            obj.Add("id", ws.id);

            return obj;
        }

        private static JObject CreateObject(Door door)
        {
            JObject obj = new JObject();
            obj.Add("positionX", door.Pos.X);
            obj.Add("positionY", door.Pos.Y);
            obj.Add("id", door.id);
            obj.Add("rotation", door.rotation);

            return obj;
        }
        private static JObject CreateObject(Vector2 pos)
        {
            JObject obj = new JObject();
            obj.Add("positionX", pos.X);
            obj.Add("positionY", pos.Y);

            return obj;
        }
        private static JObject CreateObject(Trap trap)
        {
            JObject obj = new JObject();
            obj.Add("positionX", trap.Pos.X);
            obj.Add("positionY", trap.Pos.Y);
            obj.Add("id", trap.id);
            return obj;
        }
        private static JObject CreateObject(HiddenNpc npc)
        {
            JObject obj = new JObject();
            obj.Add("positionX", npc.Pos.X);
            obj.Add("positionY", npc.Pos.Y);
            obj.Add("skin", npc.Npc);
            obj.Add("id", npc.TextNum);

            return obj;
        }

        private static JObject CreateObject(HintNpc npc)
        {
            JObject obj = new JObject();
            obj.Add("positionX", npc.Pos.X);
            obj.Add("positionY", npc.Pos.Y);
            obj.Add("id", npc.TextNum);

            return obj;
        }
        private static JObject CreateObject(StoryNpc npc)
        {
            JObject obj = new JObject();
            obj.Add("positionX", npc.Pos.X);
            obj.Add("positionY", npc.Pos.Y);
            obj.Add("id", npc.TextNum);

            return obj;
        }
    }
}
